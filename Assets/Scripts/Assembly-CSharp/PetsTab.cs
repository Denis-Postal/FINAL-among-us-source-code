using AmongUs.Data;
using UnityEngine;

public class PetsTab : InventoryTab
{
	private string petId;

	private bool currentPetIsEquipped;

	private bool initialized;

	public override void OnEnable()
	{
		base.OnEnable();
		initialized = false;
		PetData[] unlockedPets = DestroyableSingleton<HatManager>.Instance.GetUnlockedPets();
		int num = 0;
		foreach (PetData pet in unlockedPets)
		{
			float x = XRange.Lerp((float)(num % NumPerRow) / ((float)NumPerRow - 1f));
			float y = YStart - (float)(num / NumPerRow) * YOffset;
			ColorChip chip = Object.Instantiate(ColorTabPrefab, scroller.Inner);
			chip.transform.localPosition = new Vector3(x, y, -1f);
			chip.PlayerEquippedForeground.SetActive(pet.ProdId == DataManager.Player.Customization.Pet);
			chip.SelectionHighlight.gameObject.SetActive(value: false);
			if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Keyboard)
			{
				chip.Button.OnMouseOver.AddListener(() =>
				{
					SelectPet(chip, pet);
				});
				chip.Button.OnMouseOut.AddListener(() =>
				{
					SelectPet(chip, DestroyableSingleton<HatManager>.Instance.GetPetById(DataManager.Player.Customization.Pet));
				});
				chip.Button.OnClick.AddListener(() =>
				{
					ClickEquip();
				});
			}
			else
			{
				chip.Button.OnClick.AddListener(() =>
				{
					SelectPet(chip, pet);
				});
			}
			chip.Button.ClickMask = scroller.Hitbox;
			chip.Tag = pet;
			UpdateMaterials(chip.Inner.FrontLayer, pet);
			pet.SetPreview(chip.Inner.FrontLayer, GetDisplayColor());
			ColorChips.Add(chip);
			num++;
			if (!DestroyableSingleton<HatManager>.Instance.CheckLongModeValidCosmetic(pet.ProdId, PlayerPreview.GetIgnoreLongMode()))
			{
				chip.SetUnavailable();
			}
		}
		petId = DataManager.Player.Customization.Pet;
		currentPetIsEquipped = true;
		SetScrollerBounds();
		initialized = true;
	}

	public void Update()
	{
		PetData petById = DestroyableSingleton<HatManager>.Instance.GetPetById(DataManager.Player.Customization.Pet);
		PetData petById2 = DestroyableSingleton<HatManager>.Instance.GetPetById(petId);
		currentPetIsEquipped = DataManager.Player.Customization.Pet == petId;
		for (int i = 0; i < ColorChips.Count; i++)
		{
			ColorChip colorChip = ColorChips[i];
			colorChip.PlayerEquippedForeground.SetActive(petById == (PetData)colorChip.Tag);
			colorChip.SelectionHighlight.gameObject.SetActive(petById2 == (PetData)colorChip.Tag);
		}
	}

	private void SelectPet(ColorChip sender, PetData pet)
	{
		if (initialized)
		{
			petId = pet.ProdId;
			int displayColor = GetDisplayColor();
			PlayerPreview.SetPetIdle(pet, displayColor);
			PlayerCustomizationMenu.Instance.SetItemName(pet.GetItemName());
		}
	}

	public override void ClickEquip()
	{
		if (DestroyableSingleton<HatManager>.Instance.CheckLongModeValidCosmetic(petId, PlayerPreview.GetIgnoreLongMode()))
		{
			base.ClickEquip();
			DataManager.Player.Customization.Pet = petId;
			if (HasLocalPlayer())
			{
				PlayerControl.LocalPlayer.RpcSetPet(petId);
			}
			Logger.GlobalInstance.Info("PlayerCustomizationMenu::Equipped pet " + petId);
		}
	}

	public override ColorChip GetDefaultSelectable()
	{
		PetData[] unlockedPets = DestroyableSingleton<HatManager>.Instance.GetUnlockedPets();
		for (int i = 0; i < ColorChips.Count; i++)
		{
			PetData petData = unlockedPets[0];
			ColorChip result = ColorChips[i];
			if (petData.ProdId == DataManager.Player.Customization.Pet)
			{
				PlayerCustomizationMenu.Instance.SetItemName(petData.GetItemName());
				return result;
			}
		}
		return ColorChips[0];
	}

	public override bool IsSelectedItemEquipped()
	{
		return currentPetIsEquipped;
	}

	public override string GetCurrentProdID()
	{
		return petId;
	}
}
