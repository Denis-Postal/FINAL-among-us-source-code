using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RegionMenu : MonoBehaviour
{
	public ObjectPoolBehavior ButtonPool;

	public TextMeshPro RegionText;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	private UiElement defaultButtonSelected;

	private List<UiElement> controllerSelectable;

	private void Awake()
	{
		defaultButtonSelected = null;
		controllerSelectable = new List<UiElement>();
	}

	public void OnEnable()
	{
		controllerSelectable.Clear();
		int num = 0;
		foreach (IRegionInfo item in DestroyableSingleton<ServerManager>.Instance.AvailableRegions.OrderBy((IRegionInfo s) => (uint)ServerManager.DefaultRegions.IndexOf((IRegionInfo d) => d.Name.Equals(s.Name))))
		{
			IRegionInfo region = item;
			ServerListButton serverListButton = ButtonPool.Get<ServerListButton>();
			serverListButton.transform.localPosition = new Vector3(0f, 2f - 0.5f * (float)num, 0f);
			serverListButton.SetTextTranslationId(item.TranslateName, item.Name);
			serverListButton.Text.text = DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(item.TranslateName, item.Name);
			serverListButton.Text.ForceMeshUpdate();
			serverListButton.Button.OnClick.RemoveAllListeners();
			serverListButton.Button.OnClick.AddListener(() =>
			{
				ChooseOption(region);
			});
			serverListButton.SetSelected(DestroyableSingleton<ServerManager>.Instance.CurrentRegion.Equals(item));
			if (DestroyableSingleton<ServerManager>.Instance.CurrentRegion.Equals(item))
			{
				defaultButtonSelected = serverListButton.Button;
			}
			controllerSelectable.Add(serverListButton.Button);
			num++;
		}
		if (defaultButtonSelected == null && controllerSelectable.Count > 0)
		{
			defaultButtonSelected = controllerSelectable[0];
		}
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, defaultButtonSelected, controllerSelectable);
		num++;
	}

	private void OpenCustomRegion()
	{
		throw new NotImplementedException();
	}

	public void OnDisable()
	{
		ButtonPool.ReclaimAll();
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public void Open()
	{
		gameObject.SetActive(value: true);
	}

	public void ChooseOption(IRegionInfo region)
	{
		DestroyableSingleton<ServerManager>.Instance.SetRegion(region);
		RegionText.text = DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(region.TranslateName, region.Name);
		Close();
	}

	public void Close()
	{
		gameObject.SetActive(value: false);
	}
}
