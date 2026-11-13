using System.Collections;
using AmongUs.GameOptions;
using Hazel;
using UnityEngine;

public class MovingPlatformBehaviour : MonoBehaviour, ISystemType
{
	public Vector3 LeftPosition;

	public Vector3 RightPosition;

	public Vector3 DisabledPosition;

	public Vector3 LeftUsePosition;

	public Vector3 RightUsePosition;

	public AudioClip MovingSound;

	private bool IsLeft;

	private PlayerControl Target;

	private byte useId;

	public bool InUse => Target;

	public bool IsDirty { get; private set; }

	public void Use()
	{
		PlayerControl.LocalPlayer.RpcUsePlatform();
	}

	public void Use(PlayerControl player)
	{
		Vector3 vector = transform.position - player.transform.position;
		if (!player.Data.IsDead && !player.Data.Disconnected && !Target && !(vector.magnitude > 3f))
		{
			IsDirty = true;
			StartCoroutine(UsePlatform(player));
		}
	}

	public void Start()
	{
		SetSide(isLeft: true);
	}

	private void SetSide(bool isLeft)
	{
		IsLeft = isLeft;
		transform.localPosition = (IsLeft ? LeftPosition : RightPosition);
		if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek && DisabledPosition != Vector3.zero)
		{
			transform.localPosition = DisabledPosition;
		}
		IsDirty = true;
	}

	private void SetTarget(uint playerNetId, bool isLeft)
	{
		if ((bool)Target)
		{
			MeetingCalled();
		}
		PlayerControl playerControl = AmongUsClient.Instance.FindObjectByNetId<PlayerControl>(playerNetId);
		if (!playerControl)
		{
			SetSide(isLeft);
		}
		else
		{
			StartCoroutine(UsePlatform(playerControl));
		}
	}

	private IEnumerator UsePlatform(PlayerControl target)
	{
		float platformTime = Time.time;
		float totalTime = Time.time;
		Target = target;
		target.MyPhysics.ResetMoveState();
		if (target.AmOwner)
		{
			PlayerControl.HideCursorTemporarily();
		}
		target.moveable = false;
		target.NetTransform.SetPaused(isPaused: true);
		target.NetTransform.ClearPositionQueues();
		target.SetKinematic(b: true);
		target.inMovingPlat = true;
		target.ForceKillTimerContinue = true;
		Vector3 position = (IsLeft ? LeftUsePosition : RightUsePosition);
		Vector3 position2 = ((!IsLeft) ? LeftUsePosition : RightUsePosition);
		Vector3 sourcePos = (IsLeft ? LeftPosition : RightPosition);
		Vector3 targetPos = ((!IsLeft) ? LeftPosition : RightPosition);
		Vector3 vector = transform.parent.TransformPoint(position);
		Vector3 worldUseTargetPos = transform.parent.TransformPoint(position2);
		Vector3 worldSourcePos = transform.parent.TransformPoint(sourcePos);
		Vector3 worldTargetPos = transform.parent.TransformPoint(targetPos);
		yield return target.MyPhysics.WalkPlayerTo(vector);
		GameServerUtils.DebugTime("walkToUseSeconds", ref platformTime, Time.time);
		yield return target.MyPhysics.WalkPlayerTo(worldSourcePos);
		GameServerUtils.DebugTime("walkToSourceSeconds", ref platformTime, Time.time);
		yield return Effects.Wait(0.1f);
		GameServerUtils.DebugTime("waitSeconds", ref platformTime, Time.time);
		worldSourcePos -= (Vector3)target.Collider.offset;
		worldTargetPos -= (Vector3)target.Collider.offset;
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlayDynamicSound("PlatformMoving", MovingSound, loop: true, SoundDynamics, SoundManager.Instance.SfxChannel);
		}
		IsLeft = !IsLeft;
		yield return Effects.All(Effects.Slide2D(transform, sourcePos, targetPos, target.MyPhysics.Speed), Effects.Slide2DWorld(target.transform, worldSourcePos, worldTargetPos, target.MyPhysics.Speed));
		GameServerUtils.DebugTime("platformTravelSeconds", ref platformTime, Time.time);
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.StopNamedSound("PlatformMoving");
		}
		if (target == null)
		{
			ResetPlatform();
			yield break;
		}
		yield return target.MyPhysics.WalkPlayerTo(worldUseTargetPos);
		GameServerUtils.DebugTime("walkFromSeconds", ref platformTime, Time.time);
		GameServerUtils.DebugTime("totalPlatformSeconds (compare this with server log output)", ref totalTime, Time.time);
		target.SetPetPosition(target.transform.position);
		target.inMovingPlat = false;
		target.moveable = true;
		target.ForceKillTimerContinue = false;
		target.NetTransform.SetPaused(isPaused: false);
		target.SetKinematic(b: false);
		target.NetTransform.Halt();
		yield return Effects.Wait(0.1f);
		Target = null;
	}

	private void ResetPlatform()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.StopNamedSound("PlatformMoving");
		}
		Target = null;
	}

	private void SoundDynamics(AudioSource source, float dt)
	{
		if (!PlayerControl.LocalPlayer)
		{
			source.volume = 0f;
			return;
		}
		Vector2 vector = transform.position;
		Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
		float num = Vector2.Distance(vector, truePosition);
		if (num > 6f)
		{
			source.volume = 0f;
			return;
		}
		float b = 1f - num / 6f;
		source.volume = Mathf.Lerp(source.volume, b, dt);
		VibrationManager.Vibrate(0.15f, vector, 6f);
	}

	public void MeetingCalled()
	{
		StopAllCoroutines();
		VibrationManager.ClearAllVibration();
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.StopNamedSound("PlatformMoving");
		}
		if ((bool)Target)
		{
			Target.MyPhysics.enabled = true;
			Target.Collider.enabled = true;
			Target.moveable = true;
			Target.NetTransform.enabled = true;
			Target.SetKinematic(b: false);
			Target.inMovingPlat = false;
			Target = null;
		}
		SetSide(IsLeft);
	}

	public void Deteriorate(float deltaTime)
	{
	}

	public void UpdateSystem(PlayerControl player, MessageReader msgReader)
	{
	}

	public void MarkClean()
	{
		IsDirty = false;
	}

	public void Serialize(MessageWriter writer, bool initialState)
	{
		useId++;
		writer.Write(useId);
		writer.Write((uint)(((int?)Target?.NetId) ?? (-1)));
		writer.Write(IsLeft);
	}

	public void Deserialize(MessageReader reader, bool initialState)
	{
		if (initialState)
		{
			useId = reader.ReadByte();
			SetTarget(reader.ReadUInt32(), reader.ReadBoolean());
			return;
		}
		byte newSid = reader.ReadByte();
		if (NetHelpers.SidGreaterThan(newSid, useId))
		{
			useId = newSid;
			SetTarget(reader.ReadUInt32(), reader.ReadBoolean());
		}
	}
}
