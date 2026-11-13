using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using UnityEngine;

public class MapPickerMenu : MonoBehaviour
{
	[SerializeField]
	private MapFilterButton[] MapButtons;

	[SerializeField]
	private CreateOptionsPicker Parent;

	[SerializeField]
	private UiElement BackButton;

	private void OnEnable()
	{
		if (AprilFoolsMode.ShouldFlipSkeld())
		{
			MapButtons[0].SetFlipped(flipped: true);
		}
		StartCoroutine(Hack_FixDefaultSelection());
	}

	private IEnumerator Hack_FixDefaultSelection()
	{
		yield return null;
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, MapButtons[0].Button, ((IEnumerable<MapFilterButton>)MapButtons).Select((Func<MapFilterButton, UiElement>)((MapFilterButton m) => m.Button)).ToList());
	}

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public void UpdateMapButtons(int mapid)
	{
		if (Parent.mode == SettingsMode.Host)
		{
			if ((bool)Parent.CrewArea)
			{
				Parent.CrewArea.SetMap(mapid);
			}
			for (int i = 0; i < MapButtons.Length; i++)
			{
				MapFilterButton obj = MapButtons[i];
				obj.SetSelected(obj.MapId == (MapNames)mapid);
			}
		}
		else
		{
			IGameOptions targetOptions = Parent.GetTargetOptions();
			for (int j = 0; j < MapButtons.Length; j++)
			{
				MapFilterButton mapFilterButton = MapButtons[j];
				mapFilterButton.SetSelected(targetOptions.FilterContainsMap((byte)mapFilterButton.MapId));
			}
		}
	}

	public void SetMap(int mapId)
	{
		Parent.SetMap(mapId);
		if (Parent.mode == SettingsMode.Host)
		{
			Close();
		}
	}

	public void Open()
	{
		gameObject.SetActive(value: true);
	}

	public void Close()
	{
		gameObject.SetActive(value: false);
	}
}
