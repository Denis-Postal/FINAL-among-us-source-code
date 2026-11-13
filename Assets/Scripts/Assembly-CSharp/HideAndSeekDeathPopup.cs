using System.Collections;
using TMPro;
using UnityEngine;

public class HideAndSeekDeathPopup : MonoBehaviour
{
	[SerializeField]
	private HideAndSeekDeathPopupNameplate nameplate;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private AudioClip sfx;

	[SerializeField]
	private TextMeshPro text;

	public void Show(PlayerControl player, int deathIndex)
	{
		if (AprilFoolsMode.ShouldHorseAround())
		{
			text.enabled = false;
		}
		nameplate.SetPlayer(player);
		Vector3 localPosition = transform.localPosition;
		localPosition.z -= deathIndex;
		transform.localPosition = localPosition;
		StartCoroutine(AnimateCoroutine());
	}

	private void AnimEvent_PlaySound()
	{
		SoundManager.Instance.PlaySoundImmediate(sfx, loop: false);
	}

	private IEnumerator AnimateCoroutine()
	{
		while (animator.GetCurrentAnimatorStateInfo(0).IsName("Show"))
		{
			yield return null;
		}
		Object.Destroy(gameObject);
	}
}
