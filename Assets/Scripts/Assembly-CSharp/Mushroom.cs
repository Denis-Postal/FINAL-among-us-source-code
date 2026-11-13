using System.Collections;
using PowerTools;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
	private const float TRIGGER_WAIT_TIME = 0.5f;

	private const string SFX_ACTIVATE_NAME = "SporeActivate";

	private const string SFX_SPAWN_NAME = "MushroomSpawn";

	private const float VIBRATION_INTENSITY = 0.5f;

	private const float VIBRATION_FALLOFF_DISTANCE = 3.5f;

	private const float VIBRATION_DURATION = 0.25f;

	private static readonly Collider2D[] Hits = new Collider2D[15];

	[SerializeField]
	private Collider2D mushroomCollider;

	[SerializeField]
	private SpriteRenderer mushroom;

	[SerializeField]
	private SpriteRenderer spores;

	[SerializeField]
	private GameObject sporeMask;

	[SerializeField]
	[Tooltip("Use Fix Mushroom IDs to auto-set this value for all mushrooms on the map.")]
	[Header("Metadata")]
	private int id;

	[Header("Mushroom Animations")]
	[SerializeField]
	private SpriteAnim mushroomAnimator;

	[SerializeField]
	private AnimationClip mushroomIdle;

	[SerializeField]
	private AnimationClip mushroomAppear;

	[SerializeField]
	private AnimationClip mushroomSteppedOn;

	[Header("Spore Cloud Animations")]
	[SerializeField]
	private SpriteAnim sporeCloudAnimator;

	[SerializeField]
	private SpriteAnim sporeCloudMaskAnimator;

	[SerializeField]
	private AnimationClip sporeCloudIdle;

	[SerializeField]
	private AnimationClip sporeCloudAppear;

	[SerializeField]
	private AnimationClip sporeCloudDisappear;

	[Header("Timings")]
	[SerializeField]
	private float secondsBetweenSporeReleases = 17f;

	[SerializeField]
	private float secondsSporeIsActive = 5f;

	[SerializeField]
	private float secondsSporeReappearsFor = 2f;

	[SerializeField]
	private float secondsSporeFade = 1f;

	[Header("Audio")]
	[SerializeField]
	private AudioClip spawnSound;

	[SerializeField]
	private AudioClip activateSporeSound;

	private ContactFilter2D filter;

	private Vector3 origPosition;

	public int Id => id;

	private void Awake()
	{
		origPosition = transform.position;
		filter = new ContactFilter2D
		{
			useLayerMask = true,
			layerMask = Constants.LivingPlayersOnlyMask
		};
		ResetState();
	}

	private void FixedUpdate()
	{
		if (mushroomCollider.enabled && (bool)PlayerControl.LocalPlayer && mushroomCollider.IsTouching(PlayerControl.LocalPlayer.Collider))
		{
			StartSporeTrigger(PlayerControl.LocalPlayer);
		}
	}

	public void StartSporeTrigger(PlayerControl player)
	{
		mushroomCollider.enabled = false;
		StartCoroutine(WaitForSpores());
		player.CmdCheckSporeTrigger(this);
	}

	public void TriggerSpores()
	{
		StopAllCoroutines();
		transform.position = origPosition;
		StartCoroutine(CoReleaseSpores());
	}

	private IEnumerator WaitForSpores()
	{
		yield return Effects.Shake(transform, 0.5f, 0.02f, taper: true);
		mushroomCollider.enabled = true;
	}

	private void ResetState()
	{
		transform.position = origPosition;
		mushroom.enabled = true;
		mushroomAnimator.Play(mushroomIdle);
		mushroomCollider.enabled = true;
		spores.enabled = false;
		sporeMask.SetActive(value: false);
	}

	private IEnumerator CoReleaseSpores()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlayDynamicSound("SporeActivate", activateSporeSound, loop: false, SoundDynamics, SoundManager.Instance.SfxChannel);
			VibrationManager.Vibrate(0.5f, transform.position, 3.5f, 0.25f);
		}
		mushroomCollider.enabled = false;
		mushroomAnimator.Play(mushroomSteppedOn);
		yield return new WaitUntil(() => mushroomAnimator.FrameTime > 2);
		sporeMask.SetActive(value: true);
		spores.enabled = true;
		sporeCloudAnimator.Play(sporeCloudAppear);
		yield return new WaitForAnimationFinish(sporeCloudMaskAnimator, sporeCloudAppear);
		sporeCloudAnimator.Play(sporeCloudIdle);
		sporeCloudMaskAnimator.Play(sporeCloudIdle);
		yield return Effects.All(Effects.Sequence(new WaitForAnimationFinish(mushroomAnimator, mushroomSteppedOn, restartIfAlreadyPlaying: false), Effects.Action(() =>
		{
			mushroom.enabled = false;
		})), Effects.Wait(secondsSporeIsActive));
		sporeCloudAnimator.Play(sporeCloudDisappear);
		yield return new WaitForAnimationFinish(sporeCloudMaskAnimator, sporeCloudDisappear);
		spores.enabled = false;
		sporeMask.SetActive(value: false);
		yield return Effects.Wait(secondsBetweenSporeReleases - secondsSporeFade - secondsSporeIsActive - secondsSporeReappearsFor);
		mushroom.enabled = true;
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlayDynamicSound("MushroomSpawn", spawnSound, loop: false, SoundDynamics, SoundManager.Instance.SfxChannel);
			VibrationManager.Vibrate(0.5f, transform.position, 3.5f, 0.25f);
		}
		yield return new WaitForAnimationFinish(mushroomAnimator, mushroomAppear);
		ResetState();
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
		source.volume = SoundManager.GetSoundVolume(transform.position, truePosition);
	}
}
