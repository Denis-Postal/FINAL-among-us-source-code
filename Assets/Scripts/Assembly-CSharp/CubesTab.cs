using System.Linq;
using AmongUs.Data;
using Innersloth.Assets;
using TMPro;
using UnityEngine;

public class CubesTab : InventoryTab
{
	public CosmicubeMenu cosmicubeMenu;

	public GameObject viewButton;

	public PassiveButton activateButton;

	public TextMeshPro activateText;

	public TextMeshPro itemName;

	public SpriteRenderer podIcon;

	public SpriteRenderer maskArea;

	private CosmicubeData currentCube;

	private void Awake()
	{
		NumRowsVisible = 2;
	}

	public override void OnEnable()
	{
		base.OnEnable();
		maskArea.material.SetInt(PlayerMaterial.MaskLayer, 50);
		PlayerPreview.gameObject.SetActive(value: false);
		CosmicubeData[] unlockedCubes = DestroyableSingleton<CosmicubeManager>.Instance.GetUnlockedCubes();
		unlockedCubes = (from x in unlockedCubes
			orderby x.ProdId == DataManager.Player.Store.ActiveCosmicube descending, DestroyableSingleton<CosmicubeManager>.Instance.IsCompleted(x), DestroyableSingleton<CosmicubeManager>.Instance.GetCompletionProgress(x) descending, x.GetDateCreated() descending
			select x).ToArray();
		for (int num = 0; num < unlockedCubes.Length; num++)
		{
			CosmicubeData cube = unlockedCubes[num];
			float y = YStart - (float)(num / NumPerRow) * YOffset;
			ColorChip chip = Object.Instantiate(ColorTabPrefab, scroller.Inner);
			chip.transform.localPosition = new Vector3(-0.29f, y, -1f);
			chip.Button.ClickMask = scroller.Hitbox;
			chip.Button.OnClick.AddListener(() =>
			{
				SelectCube(chip, cube);
			});
			(chip as CubeChip).SetUp(cube, 16 + num);
			ColorChips.Add(chip);
		}
		if (unlockedCubes.Length != 0)
		{
			GetDefaultSelectable().PlayerEquippedForeground.SetActive(value: true);
			SelectCube(ColorChips[0], unlockedCubes[0]);
		}
		SetScrollerBounds();
	}

	private void Update()
	{
		viewButton.SetActive(currentCube != null);
		activateButton.gameObject.SetActive(currentCube != null);
		CosmicubeData cubeDataByID = DestroyableSingleton<CosmicubeManager>.Instance.GetCubeDataByID(DataManager.Player.Store.ActiveCosmicube);
		for (int i = 0; i < ColorChips.Count; i++)
		{
			ColorChip colorChip = ColorChips[i];
			colorChip.PlayerEquippedForeground.SetActive(cubeDataByID.ProdId == (colorChip as CubeChip).ProductId);
			colorChip.SelectionHighlight.gameObject.SetActive(currentCube.ProdId == (colorChip as CubeChip).ProductId);
		}
	}

	private void SelectCube(ColorChip chip, CosmicubeData cube)
	{
		currentCube = cube;
		bool flag = cube.ProdId == DataManager.Player.Store.ActiveCosmicube;
		activateText.text = (flag ? DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Deactivate) : DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Activate));
		itemName.text = currentCube.GetCosmicubeName();
		StartCoroutine(this.CoLoadAssetAsync(cube.podIconRef, (Sprite sprite) =>
		{
			podIcon.sprite = sprite;
		}));
		activateButton.SetButtonEnableState(!DestroyableSingleton<CosmicubeManager>.Instance.IsCompleted(cube) | flag);
		for (int num = 0; num < ColorChips.Count; num++)
		{
			ColorChip colorChip = ColorChips[num];
			colorChip.PlayerEquippedForeground.SetActive((chip == colorChip) & flag);
		}
	}

	public override ColorChip GetDefaultSelectable()
	{
		if (ColorChips.Count > 0)
		{
			return ColorChips[0];
		}
		return null;
	}

	public void ViewCube()
	{
		cosmicubeMenu.Show(currentCube.ProdId);
		gameObject.SetActive(value: false);
	}

	public override void ClickEquip()
	{
		if (currentCube.ProdId == DataManager.Player.Store.ActiveCosmicube)
		{
			DeactivateCube();
		}
		else
		{
			ActivateCube();
		}
	}

	public override bool IsSelectedItemEquipped()
	{
		return true;
	}

	private void ActivateCube()
	{
		CosmicubeData cubeDataByID = DestroyableSingleton<CosmicubeManager>.Instance.GetCubeDataByID(currentCube.ProdId);
		if (!DestroyableSingleton<CosmicubeManager>.Instance.IsCompleted(cubeDataByID))
		{
			DataManager.Player.Store.ActiveCosmicube = currentCube.ProdId;
			PlayerStorageManager.CloudPlayerPrefs playerPrefs = DestroyableSingleton<PlayerStorageManager>.Instance.PlayerPrefs;
			playerPrefs.ActivePodType = currentCube.podId;
			DestroyableSingleton<PlayerStorageManager>.Instance.SaveChanges(playerPrefs);
			if (AmongUsClient.Instance.AmConnected)
			{
				AmongUsClient.Instance.SetActivePodType(playerPrefs.ActivePodType);
			}
			activateText.text = ((currentCube.ProdId == DataManager.Player.Store.ActiveCosmicube) ? DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Deactivate) : DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Activate));
			Logger.GlobalInstance.Info("PlayerCustomizationMenu::Activated cube " + currentCube.ProdId);
		}
	}

	private void DeactivateCube()
	{
		CosmicubeData cubeDataByID = DestroyableSingleton<CosmicubeManager>.Instance.GetCubeDataByID(DataManager.Player.Store.ActiveCosmicube);
		activateButton.SetButtonEnableState(!DestroyableSingleton<CosmicubeManager>.Instance.IsCompleted(cubeDataByID));
		DataManager.Player.Store.ActiveCosmicube = "";
		DataManager.Player.Save();
		PlayerStorageManager.CloudPlayerPrefs playerPrefs = DestroyableSingleton<PlayerStorageManager>.Instance.PlayerPrefs;
		playerPrefs.ActivePodType = "";
		DestroyableSingleton<PlayerStorageManager>.Instance.SaveChanges(playerPrefs);
		if (AmongUsClient.Instance.AmConnected)
		{
			AmongUsClient.Instance.SetActivePodType(playerPrefs.ActivePodType);
		}
		activateText.text = ((currentCube.ProdId == DataManager.Player.Store.ActiveCosmicube) ? DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Deactivate) : DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Activate));
		Logger.GlobalInstance.Info("PlayerCustomizationMenu::Deactivated cube " + currentCube.ProdId);
	}
}
