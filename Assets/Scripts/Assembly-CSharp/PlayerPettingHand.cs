using System.Collections;
using PowerTools;
using UnityEngine;

public class PlayerPettingHand : MonoBehaviour
{
	public SpriteRenderer HandSprite;

	public SpriteRenderer hearts;

	public SpriteAnim HandSpriteAnim;

	public AnimationClip PetClip;

	private const int PET_ANIMATION_START_FRAME = 6;

	private const float ZPOS_PETTING = 1E-05f;

	public void StartPet(PetBehaviour pet)
	{
		gameObject.SetActive(value: true);
		transform.position = new Vector3(pet.PettingHandPosition.x, pet.PettingHandPosition.y, pet.transform.position.z - 1E-05f);
		HandSpriteAnim.Play(PetClip);
		StartCoroutine(WaitForPetAnimStart(pet));
	}

	public IEnumerator WaitForPetAnimStart(PetBehaviour pet)
	{
		while (HandSpriteAnim.FrameTime < 6)
		{
			yield return null;
		}
		pet.StartPetAnim();
	}

	public IEnumerator WaitUntilFinishedPetting()
	{
		yield return new WaitForAnimationFinish(HandSpriteAnim, PetClip);
		StopPetting();
	}

	public void StopPetting()
	{
		gameObject.SetActive(value: false);
	}

	public void SetAlpha(float alphaValue)
	{
		HandSprite.color = HandSprite.color.SetAlpha(alphaValue);
		hearts.color = hearts.color.SetAlpha(alphaValue);
	}
}
