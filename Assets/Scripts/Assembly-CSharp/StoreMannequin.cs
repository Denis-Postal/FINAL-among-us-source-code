using Object = UnityEngine.Object;
using Cpp2ILInjected;
using UnityEngine;

public class StoreMannequin : MonoBehaviour
{
	[SerializeField]
	private HatParent hat;

	[SerializeField]
	private VisorLayer visor;

	[SerializeField]
	private SpriteRenderer skin;

	[SerializeField]
	private SpriteRenderer pet;

	[SerializeField]
	private Material defaultMaterial;

	[SerializeField]
	private Material playerMaterial;

	public void WearThing(IBuyable product, PlayerMaterial.MaskType maskType, int outfitColorOverride = -1)
	{
		int num = ((outfitColorOverride >= 0) ? outfitColorOverride : 2);
		if (product is HatData hatData)
		{
			hat.gameObject.SetActive(value: true);
			visor.gameObject.SetActive(value: false);
			skin.gameObject.SetActive(value: false);
			pet.gameObject.SetActive(value: false);
			hat.SetHat(hatData, num);
			hat.SetMaskType(maskType);
		}
		else if (product is VisorData data)
		{
			hat.gameObject.SetActive(value: false);
			visor.gameObject.SetActive(value: true);
			skin.gameObject.SetActive(value: false);
			pet.gameObject.SetActive(value: false);
			visor.SetVisor(data, num);
			visor.SetMaskType(maskType);
		}
		else if (product is SkinData skinData)
		{
			hat.gameObject.SetActive(value: false);
			visor.gameObject.SetActive(value: false);
			skin.gameObject.SetActive(value: true);
			pet.gameObject.SetActive(value: false);
			PlayerControl.SetSkinImageAsync(skinData, num, skin);
		}
		else if (product is PetData petData)
		{
			hat.gameObject.SetActive(value: false);
			visor.gameObject.SetActive(value: false);
			skin.gameObject.SetActive(value: false);
			pet.gameObject.SetActive(value: true);
			petData.SetPreview(pet, num);
			pet.material = new Material(petData.PreviewCrewmateColor ? playerMaterial : defaultMaterial);
			PlayerMaterial.SetColors(num, pet);
		}
	}
}
