using System.Collections;
using UnityEngine;

public class MeetingCalledAnimation : OverlayAnimation
{
	public AspectPosition emergencyText;

	public PoolablePlayer playerParts;

	public AudioClip Stinger;

	public float StingerVolume = 0.6f;

	public AnimationCurve CrewmateSlide;

	[SerializeField]
	private GameObject emergencyParent;

	[SerializeField]
	private GameObject emergencyClassicParent;

	[SerializeField]
	private PoolablePlayer classicPlayerParts;

	[SerializeField]
	private GameObject classicFlame;

	public void Initialize(NetworkedPlayerInfo.PlayerOutfit outfit)
	{
		if (AprilFoolsMode.ShouldClassicMode())
		{
			classicPlayerParts.UpdateFromPlayerOutfit(outfit, PlayerMaterial.MaskType.None, isDead: false, includePet: false);
		}
		else
		{
			playerParts.UpdateFromPlayerOutfit(outfit, PlayerMaterial.MaskType.None, isDead: false, includePet: false);
		}
	}

	public override IEnumerator CoShow(KillOverlay parent)
	{
		gameObject.SetActive(value: true);
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(Stinger, loop: false).volume = StingerVolume;
		}
		if (AprilFoolsMode.ShouldClassicMode())
		{
			emergencyParent.SetActive(value: false);
			emergencyClassicParent.SetActive(value: true);
			classicPlayerParts.gameObject.SetActive(value: false);
			classicFlame.transform.localScale = new Vector3(1.3f, 0.6f, 1.3f);
			classicFlame.transform.localEulerAngles = new Vector3(0f, 0f, 25f);
			yield return Effects.Wait(1f / 12f);
			classicFlame.transform.localScale = new Vector3(1.3f, 0.8f, 1.3f);
			classicFlame.transform.localEulerAngles = new Vector3(0f, 0f, -15f);
			yield return Effects.Wait(1f / 12f);
			classicFlame.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
			classicFlame.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			classicPlayerParts.gameObject.SetActive(value: true);
			yield return new WaitForSeconds(2.5f);
			yield return Effects.Lerp(0.1f, (float t) =>
			{
				Vector3 localScale = emergencyClassicParent.transform.localScale;
				emergencyClassicParent.transform.localScale = new Vector3(localScale.x, 1f - t, localScale.z);
			});
		}
		else
		{
			emergencyParent.SetActive(value: true);
			emergencyClassicParent.SetActive(value: false);
			AspectPosition playerTransform = playerParts.GetComponent<AspectPosition>();
			_ = playerTransform.DistanceFromEdge;
			yield return Effects.All(Effects.Lerp(2.5f, (float t) =>
			{
				emergencyText.SetNormalizedX(CrewmateSlide.Evaluate(t), 0.5f);
			}), Effects.Lerp(2.5f, (float t) =>
			{
				playerTransform.SetNormalizedX(CrewmateSlide.Evaluate(t), 0.5f);
			}));
		}
		gameObject.SetActive(value: false);
	}
}
