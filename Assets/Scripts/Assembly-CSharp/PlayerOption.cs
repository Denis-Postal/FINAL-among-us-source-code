using System.Collections.Generic;
using AmongUs.GameOptions;
using TMPro;
using UnityEngine;

public class PlayerOption : OptionBehaviour
{
	public TextMeshPro TitleText;

	public TextMeshPro ValueText;

	[SerializeField]
	private TextMeshPro PlusTxt;

	[SerializeField]
	private TextMeshPro MinusTxt;

	[SerializeField]
	private TextMeshPro OptionUnavailableTxt;

	[SerializeField]
	private GameOptionButton PlusBtn;

	[SerializeField]
	private GameOptionButton MinusBtn;

	[SerializeField]
	private GameObject ValueBox;

	private List<NetworkedPlayerInfo> Values;

	public int Value = -1;

	private int oldValue = -1;

	private int playerIndex = -1;

	private Int32OptionNames optionName;

	public override void SetUpFromData(BaseGameSetting data, int maskLayer)
	{
		base.SetUpFromData(data, maskLayer);
		PlayerSelectionGameSetting playerSelectionGameSetting = data as PlayerSelectionGameSetting;
		if (!(playerSelectionGameSetting == null))
		{
			optionName = playerSelectionGameSetting.OptionName;
		}
	}

	public void OnEnable()
	{
		Values = new List<NetworkedPlayerInfo>(GameData.Instance.AllPlayers);
		Values.Sort((NetworkedPlayerInfo a, NetworkedPlayerInfo b) => a.PlayerId.CompareTo(b.PlayerId));
		Value = GameOptionsManager.Instance.GameHostOptions.GetInt(Int32OptionNames.ImpostorPlayerID);
		playerIndex = Values.FindIndex((NetworkedPlayerInfo p) => p.PlayerId == Value);
		TitleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(Title);
		SetValueText();
		AdjustButtonsActiveState();
	}

	private void SetValueText()
	{
		bool isGamePublic = AmongUsClient.Instance.IsGamePublic;
		if (playerIndex < 0)
		{
			ValueText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RoundRobin);
		}
		else
		{
			ValueText.text = Values[playerIndex].PlayerName;
		}
		ValueText.gameObject.SetActive(!isGamePublic);
		ValueBox.gameObject.SetActive(!isGamePublic);
		PlusBtn.gameObject.SetActive(!isGamePublic);
		MinusBtn.gameObject.SetActive(!isGamePublic);
		OptionUnavailableTxt.gameObject.SetActive(isGamePublic);
	}

	private void FixedUpdate()
	{
		if (GameData.Instance.AllPlayers.Count != Values.Count)
		{
			Values = new List<NetworkedPlayerInfo>(GameData.Instance.AllPlayers);
			Values.Sort((NetworkedPlayerInfo a, NetworkedPlayerInfo b) => a.PlayerId.CompareTo(b.PlayerId));
			playerIndex = Values.FindIndex((NetworkedPlayerInfo p) => p.PlayerId == Value);
			SetValueText();
		}
		CheckValueChanged();
	}

	public void Increase()
	{
		if (playerIndex < Values.Count - 1)
		{
			UpdatePlayerIndex(Mathf.Clamp(playerIndex + 1, -1, Values.Count - 1));
			CheckValueChanged();
			OnValueChanged(this);
			AdjustButtonsActiveState();
		}
	}

	public void Decrease()
	{
		if (playerIndex >= 0)
		{
			UpdatePlayerIndex(Mathf.Clamp(playerIndex - 1, -1, Values.Count - 1));
			CheckValueChanged();
			OnValueChanged(this);
			AdjustButtonsActiveState();
		}
	}

	public override int GetInt()
	{
		if (AmongUsClient.Instance.IsGamePublic)
		{
			return -1;
		}
		return Value;
	}

	private void UpdatePlayerIndex(int index)
	{
		if (index >= 0 && index < Values.Count)
		{
			playerIndex = index;
			Value = Values[index].PlayerId;
		}
		else
		{
			playerIndex = -1;
			Value = -1;
		}
	}

	private void CheckValueChanged()
	{
		if (oldValue != Value)
		{
			oldValue = Value;
			SetValueText();
			UpdateValue();
		}
	}

	private void UpdateValue()
	{
		if (optionName != Int32OptionNames.Invalid)
		{
			GameOptionsManager.Instance.CurrentGameOptions.SetInt(optionName, GetInt());
		}
		else
		{
			Debug.LogError("Could not update value of " + Title);
		}
	}

	private void AdjustButtonsActiveState()
	{
		if (Value < 0)
		{
			MinusBtn.SetInteractable(interactable: false);
			PlusBtn.SetInteractable(interactable: true);
		}
		else if (Value == Values.Count - 1)
		{
			MinusBtn.SetInteractable(interactable: true);
			PlusBtn.SetInteractable(interactable: false);
		}
		else
		{
			MinusBtn.SetInteractable(interactable: true);
			PlusBtn.SetInteractable(interactable: true);
		}
	}
}
