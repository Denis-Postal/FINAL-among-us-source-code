using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineBehaviour : MonoBehaviour
{
	public const float MAX_USE_DISTANCE = 3f;

	private const string SFX_NAME = "ZiplineTravel_";

	private const float PET_HIDE_SPEED = 2f;

	private const float TOGGLE_PET_INVISIBLE_SECONDS = 0.5f;

	private const float TOGGLE_PET_VISIBLE_SECONDS = 0f;

	[SerializeField]
	[Header("Positions")]
	private Transform handleTop;

	[SerializeField]
	private Transform handleBottom;

	[SerializeField]
	private Transform landingPositionBottom;

	[SerializeField]
	private Transform landingPositionTop;

	[SerializeField]
	private Transform dropPositionBottom;

	[SerializeField]
	private Transform dropPositionTop;

	[Header("Travel time")]
	[SerializeField]
	private float upTravelTime = 2f;

	[SerializeField]
	private float downTravelTime = 2f;

	[Header("Animation Settings")]
	[SerializeField]
	private float timeJump = 0.2f;

	[SerializeField]
	private AnimationCurve jumpZiplineCurve;

	[SerializeField]
	private AnimationCurve jumpZiplineCurveBottom;

	[SerializeField]
	private AnimationCurve jumpZiplineHandCurve;

	[Header("Hand Settings")]
	[SerializeField]
	private ObjectPoolBehavior handPool;

	[SerializeField]
	private Transform upHandPosition;

	[SerializeField]
	private Transform downHandPosition;

	[SerializeField]
	[Header("Audio")]
	private AudioClip attachSound;

	[SerializeField]
	private AudioClip detachSound;

	[SerializeField]
	private AudioClip upSound;

	[SerializeField]
	private AudioClip downSound;

	[SerializeField]
	private AudioClip downLoopSound;

	private readonly Logger logger = new Logger(Logger.Category.Gameplay, "ZiplineBehaviour");

	private Dictionary<byte, Coroutine> playerIdWalkCoroutines;

	private Dictionary<byte, Coroutine> playerIdUseZiplineCoroutines;

	private Dictionary<byte, HandZiplinePoolable> playerIdHands;

	private Dictionary<byte, IEnumerator> soundLoopCoroutines;

	private ZiplineConsole lastUsedConsole;

	private void Awake()
	{
		playerIdWalkCoroutines = new Dictionary<byte, Coroutine>();
		playerIdUseZiplineCoroutines = new Dictionary<byte, Coroutine>();
		playerIdHands = new Dictionary<byte, HandZiplinePoolable>();
		soundLoopCoroutines = new Dictionary<byte, IEnumerator>();
	}

	public void Use(bool fromTop, ZiplineConsole console)
	{
		lastUsedConsole = console;
		PreUseZipline(PlayerControl.LocalPlayer, fromTop);
	}

	public void OnMeetingCalled()
	{
		StopAllZiplineSounds();
		StopAllCoroutines();
		ResetZiplineMeeting();
		playerIdWalkCoroutines.Clear();
		playerIdUseZiplineCoroutines.Clear();
		playerIdHands.Clear();
	}

	public void Use(PlayerControl player, bool fromTop)
	{
		if (!player.Data.IsDead && !player.Data.Disconnected)
		{
			Transform start;
			Transform end;
			Transform landing;
			if (fromTop)
			{
				start = handleTop;
				end = handleBottom;
				landing = landingPositionBottom;
			}
			else
			{
				start = handleBottom;
				end = handleTop;
				landing = landingPositionTop;
			}
			StopAllCoroutinesForPlayer(player);
			playerIdUseZiplineCoroutines[player.PlayerId] = StartCoroutine(CoUseZipline(player, start, end, landing, fromTop));
		}
	}

	public Vector3 GetHandlePos(bool fromTop)
	{
		if (!fromTop)
		{
			return handleBottom.position;
		}
		return handleTop.position;
	}

	public void CancelZiplineUseForPlayer(PlayerControl player)
	{
		StopAllCoroutinesForPlayer(player);
		if (playerIdHands.TryGetValue(player.PlayerId, out var value))
		{
			handPool.Reclaim(value);
			playerIdHands.Remove(player.PlayerId);
		}
		ResetTarget(player);
		player.SetPetPosition(player.transform.position);
		player.cosmetics.TogglePetVisible(visible: true);
	}

	private void StopAllCoroutinesForPlayer(PlayerControl player)
	{
		if (playerIdWalkCoroutines.TryGetValue(player.PlayerId, out var value))
		{
			if (value != null)
			{
				StopCoroutine(value);
			}
			playerIdWalkCoroutines.Remove(player.PlayerId);
		}
		if (playerIdUseZiplineCoroutines.TryGetValue(player.PlayerId, out var value2))
		{
			if (value2 != null)
			{
				StopCoroutine(value2);
			}
			playerIdUseZiplineCoroutines.Remove(player.PlayerId);
		}
	}

	private void PreUseZipline(PlayerControl player, bool fromTop)
	{
		float lastTime = Time.time;
		if (!playerIdWalkCoroutines.ContainsKey(player.PlayerId))
		{
			Transform transform = ((!fromTop) ? handleBottom : handleTop);
			PreparePlayerForZipline(player, gotPermission: false);
			Coroutine value = StartCoroutine(CoWalkPlayerToZiplineWhileWaiting(player, transform.position));
			playerIdWalkCoroutines.Add(player.PlayerId, value);
			GameServerUtils.DebugTime("walkToSeconds", ref lastTime, Time.time);
			PlayerControl.LocalPlayer.CmdCheckUseZipline(player, this, fromTop);
		}
		else
		{
			logger.Warning($"{player} was already walking towards the zipline.");
		}
	}

	private IEnumerator CoUseZipline(PlayerControl player, Transform start, Transform end, Transform landing, bool fromTop)
	{
		float ziplineTime = Time.time;
		float totalTime = Time.time;
		logger.Info($"[{Time.time}] CoUseZipline: Animating player {player.PlayerId} going across zipline");
		int superAbort = 100;
		while (player.isKilling)
		{
			superAbort--;
			if (superAbort < 0)
			{
				yield break;
			}
			yield return null;
		}
		PreparePlayerForZipline(player, gotPermission: true);
		yield return CoWalkPlayerToZipline(player, start.position);
		GameServerUtils.DebugTime("walkToSeconds", ref ziplineTime, Time.time);
		HandZiplinePoolable currentHand = GetHand();
		playerIdHands[player.PlayerId] = currentHand;
		currentHand.SetPlayerColor(player.CurrentOutfit, PlayerMaterial.MaskType.None, player.cosmetics.GetPhantomRoleAlpha());
		PlaySound(attachSound, start.position, player);
		yield return CoAnimatePlayerJumpingOnToZipline(player, fromTop, currentHand);
		GameServerUtils.DebugTime("jumpToSeconds", ref ziplineTime, Time.time);
		StartSound(player, fromTop);
		yield return CoAnimateZiplineAndPlayer(player, end.position, fromTop, currentHand);
		GameServerUtils.DebugTime("travelSeconds", ref ziplineTime, Time.time);
		StopZiplineSound(player);
		PlaySound(detachSound, end.position, player);
		yield return CoAlightPlayerFromZipline(player, landing.position, fromTop, currentHand);
		GameServerUtils.DebugTime("jumpFromSeconds + walkFrom + wait", ref ziplineTime, Time.time);
		GameServerUtils.DebugTime("totalZiplineSeconds (compare this with server log output)", ref totalTime, Time.time);
		if ((bool)lastUsedConsole && player.AmOwner)
		{
			lastUsedConsole.SetDestinationCooldown();
		}
		player.NetTransform.Halt();
		ResetTarget(player);
		ResetZipline();
	}

	private IEnumerator CoAnimatePlayerJumpingOnToZipline(PlayerControl player, bool fromTop, HandZiplinePoolable hand)
	{
		hand.gameObject.SetActive(value: true);
		if (!player.Visible)
		{
			hand.Hide();
		}
		AnimationCurve curve;
		if (fromTop)
		{
			hand.StartDownAnimation();
			hand.transform.position = upHandPosition.position;
			curve = jumpZiplineCurve;
		}
		else
		{
			hand.StartUpAnimation();
			hand.transform.position = downHandPosition.position;
			curve = jumpZiplineCurveBottom;
		}
		player.cosmetics.UpdateBounceHatZipline();
		player.cosmetics.AnimateSkinJump();
		float ziplineTime = Time.time;
		yield return Effects.All(Effects.CurvePositionY(player.transform, curve, timeJump), Effects.CurvePositionY(hand.transform, jumpZiplineHandCurve, timeJump), player.MyPhysics.Animations.CoPlayJumpAnimation());
		GameServerUtils.DebugTime("effectsAllSeconds", ref ziplineTime, Time.time);
		yield return Effects.Wait(0.1f);
		player.MyPhysics.enabled = false;
	}

	private void PreparePlayerForZipline(PlayerControl player, bool gotPermission)
	{
		player.MyPhysics.ResetMoveState();
		if (player.AmOwner)
		{
			PlayerControl.HideCursorTemporarily();
		}
		player.moveable = false;
		player.ForceKillTimerContinue = true;
		player.NetTransform.enabled = false;
		player.SetKinematic(b: true);
		player.NetTransform.ClearPositionQueues();
		if (gotPermission)
		{
			player.inMovingPlat = true;
		}
	}

	private IEnumerator CoTogglePetVisibility(PlayerControl player, float delaySeconds, bool shouldMakeVisible)
	{
		yield return new WaitForSeconds(delaySeconds);
		player.cosmetics.TogglePetVisible(shouldMakeVisible);
	}

	private IEnumerator CoWalkPlayerToZipline(PlayerControl player, Vector3 handleStartPosition)
	{
		if (player.cosmetics.HasPetEquipped())
		{
			StartCoroutine(CoTogglePetVisibility(player, 0.5f, shouldMakeVisible: false));
		}
		yield return player.MyPhysics.WalkPlayerTo(handleStartPosition, 0.001f, 1f, ignoreColliderOffset: true);
	}

	private IEnumerator CoWalkPlayerToZiplineWhileWaiting(PlayerControl player, Vector3 handleStartPosition)
	{
		if (player.cosmetics.HasPetEquipped())
		{
			StartCoroutine(CoTogglePetVisibility(player, 0.5f, shouldMakeVisible: false));
		}
		yield return player.MyPhysics.WalkPlayerTo(handleStartPosition, 0.001f, 1f, ignoreColliderOffset: true);
		StartCoroutine(CoTogglePetVisibility(player, 0f, shouldMakeVisible: true));
		ResetTarget(player);
		player.SetPetPosition(player.transform.position);
		player.cosmetics.TogglePetVisible(visible: true);
		if (playerIdWalkCoroutines.ContainsKey(player.PlayerId))
		{
			playerIdWalkCoroutines.Remove(player.PlayerId);
		}
	}

	private IEnumerator CoAnimateZiplineAndPlayer(PlayerControl player, Vector3 handleEndPosition, bool fromTop, HandZiplinePoolable hand)
	{
		float travelSeconds;
		if (fromTop)
		{
			travelSeconds = downTravelTime;
			handleEndPosition = dropPositionBottom.position;
		}
		else
		{
			travelSeconds = upTravelTime;
			handleEndPosition = dropPositionTop.position;
		}
		Vector3 zero = Vector3.zero;
		Vector3 startPos = player.transform.position;
		Vector3 handOffset = hand.transform.position - startPos;
		for (float time = 0f; time < travelSeconds; time += Time.deltaTime)
		{
			float t = time / travelSeconds;
			if (player != null)
			{
				zero.x = Mathf.SmoothStep(startPos.x, handleEndPosition.x, t);
				zero.y = Mathf.SmoothStep(startPos.y, handleEndPosition.y, t);
				zero.z = player.transform.localPosition.z;
				player.transform.position = zero;
				zero += handOffset;
				zero.z = hand.transform.position.z;
				hand.transform.position = zero;
				yield return null;
				continue;
			}
			Debug.LogWarning("Player transform was destroyed while moving");
			break;
		}
	}

	private IEnumerator CoAlightPlayerFromZipline(PlayerControl player, Vector3 landingPosition, bool fromTop, HandZiplinePoolable hand)
	{
		player.MyPhysics.enabled = true;
		float ziplineTime = Time.time;
		Vector3 vector = transform.TransformPoint(landingPosition);
		if (fromTop)
		{
			hand.StartDownOutroAnimation();
		}
		else
		{
			hand.StartUpOutroAnimation();
		}
		GameServerUtils.DebugTime("jumpFromSeconds", ref ziplineTime, Time.time);
		yield return player.MyPhysics.WalkPlayerTo(vector);
		GameServerUtils.DebugTime("walkFromSeconds", ref ziplineTime, Time.time);
		player.SetPetPosition(player.transform.position);
		StartCoroutine(CoTogglePetVisibility(player, 0f, shouldMakeVisible: true));
		yield return Effects.Wait(0.1f);
	}

	private void StartSound(PlayerControl player, bool fromTop)
	{
		if (player.Visible)
		{
			StopZiplineSound(player);
			if (fromTop)
			{
				soundLoopCoroutines[player.PlayerId] = CoDownSound(player);
				StartCoroutine(soundLoopCoroutines[player.PlayerId]);
			}
			else if (ShouldPlaySound())
			{
				SoundManager.Instance.PlayDynamicSound("ZiplineTravel_" + player.PlayerId, upSound, loop: true, SoundDynamics, SoundManager.Instance.SfxChannel);
			}
		}
	}

	private void StopZiplineSound(PlayerControl player)
	{
		if (soundLoopCoroutines.ContainsKey(player.PlayerId) && soundLoopCoroutines[player.PlayerId] != null)
		{
			StopCoroutine(soundLoopCoroutines[player.PlayerId]);
			soundLoopCoroutines[player.PlayerId] = null;
		}
		string text = "ZiplineTravel_" + player.PlayerId;
		if (SoundManager.Instance.HasNamedSound(text))
		{
			SoundManager.Instance.StopNamedSound(text);
		}
	}

	private void StopAllZiplineSounds()
	{
		foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
		{
			StopZiplineSound(allPlayerControl);
		}
	}

	private IEnumerator CoDownSound(PlayerControl player)
	{
		if (ShouldPlaySound())
		{
			SoundManager.Instance.PlayDynamicSound("ZiplineTravel_" + player.PlayerId, downSound, loop: false, SoundDynamics, SoundManager.Instance.SfxChannel);
			yield return new WaitForSeconds(downSound.length - 0.05f);
			SoundManager.Instance.PlayDynamicSound("ZiplineTravel_" + player.PlayerId, downLoopSound, loop: true, SoundDynamics, SoundManager.Instance.SfxChannel);
		}
	}

	private void SoundDynamics(AudioSource source, float dt)
	{
		if (!PlayerControl.LocalPlayer)
		{
			source.volume = 0f;
			return;
		}
		source.volume = 1f;
		Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
		Vector2 closestPointOnLine = GetClosestPointOnLine(handleBottom.position, handleTop.position, truePosition);
		float num = Vector2.Distance(truePosition, closestPointOnLine);
		source.volume = SoundManager.GetSoundVolume(closestPointOnLine, truePosition);
		if (num < 0.25f)
		{
			VibrationManager.Vibrate(0.15f, 0.15f);
		}
	}

	private bool ShouldPlaySound()
	{
		return Constants.ShouldPlaySfx();
	}

	private void PlaySound(AudioClip sound, Vector2 soundPosition, PlayerControl player)
	{
		if (ShouldPlaySound() && player.Visible)
		{
			float soundVolume = SoundManager.GetSoundVolume(soundPosition, PlayerControl.LocalPlayer.GetTruePosition());
			if (!(soundVolume <= 0f))
			{
				SoundManager.Instance.PlaySoundImmediate(sound, loop: false, soundVolume);
			}
		}
	}

	private Vector2 GetClosestPointOnLine(Vector2 lineStart, Vector2 lineEnd, Vector2 position)
	{
		Vector2 vector = lineEnd - lineStart;
		float magnitude = vector.magnitude;
		vector.Normalize();
		float value = Vector2.Dot(position - lineStart, vector);
		value = Mathf.Clamp(value, 0f, magnitude);
		return lineStart + vector * value;
	}

	private void ResetZipline()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.StopNamedSound("ZiplineTravel_");
		}
		VibrationManager.ClearAllVibration();
	}

	private void ResetZiplineMeeting()
	{
		handPool.ReclaimAll();
		ResetZipline();
	}

	private void ResetTarget(PlayerControl player)
	{
		if (player != null)
		{
			player.inMovingPlat = false;
			player.moveable = true;
			player.NetTransform.enabled = true;
			player.SetKinematic(b: false);
			player.ForceKillTimerContinue = false;
		}
	}

	private HandZiplinePoolable GetHand()
	{
		return handPool.Get<HandZiplinePoolable>();
	}

	private void OnDrawGizmos()
	{
		if (!(handleTop == null) && !(handleBottom == null))
		{
			Color color = Gizmos.color;
			Gizmos.color = Color.red;
			Gizmos.DrawLine(handleTop.position, handleBottom.position);
			Gizmos.color = color;
		}
	}
}
