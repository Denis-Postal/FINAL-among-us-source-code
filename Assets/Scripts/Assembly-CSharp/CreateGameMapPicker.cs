using System;
using System.Collections.Generic;
using AmongUs.GameOptions;

public class CreateGameMapPicker : GameOptionsMapPicker
{
	private Action<int> toolTipAction;

	public override void Initialize(int maskLayer)
	{
		selectedMapId = GameOptionsManager.Instance.GameHostOptions.MapId;
		SetupMapButtons(maskLayer);
	}

	protected override void UpdateValue()
	{
		if (optionName != ByteOptionNames.Invalid)
		{
			IGameOptions gameHostOptions = GameOptionsManager.Instance.GameHostOptions;
			gameHostOptions.SetByte(ByteOptionNames.MapId, (byte)GetInt());
			GameOptionsManager.Instance.GameHostOptions = gameHostOptions;
		}
	}

	public void SetupMouseOver(Action<int> action)
	{
		foreach (MapSelectButton mapButton in mapButtons)
		{
			mapButton.Button.OnClick.AddListener(() =>
			{
				action(mapButton.MapID);
			});
			mapButton.Button.OnMouseOver.AddListener(() =>
			{
				action(mapButton.MapID);
			});
			mapButton.Button.OnMouseOut.AddListener(() =>
			{
				action(-1);
			});
		}
	}

	public int GetSelectedID()
	{
		return selectedMapId;
	}

	public List<MapSelectButton> GetMapButtons()
	{
		return mapButtons;
	}
}
