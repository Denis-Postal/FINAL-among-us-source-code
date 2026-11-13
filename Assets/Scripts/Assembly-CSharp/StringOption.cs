using AmongUs.GameOptions;
using TMPro;
using UnityEngine;

public class StringOption : OptionBehaviour
{
	public TextMeshPro TitleText;

	public TextMeshPro ValueText;

	public StringNames[] Values;

	public int Value;

	private int oldValue = -1;

	private Int32OptionNames stringOptionName;

	[SerializeField]
	private GameOptionButton PlusBtn;

	[SerializeField]
	private GameOptionButton MinusBtn;

	public override void SetUpFromData(BaseGameSetting data, int maskLayer)
	{
		base.SetUpFromData(data, maskLayer);
		StringGameSetting stringGameSetting = data as StringGameSetting;
		if (!(stringGameSetting == null))
		{
			Title = stringGameSetting.Title;
			Value = stringGameSetting.Index;
			Values = stringGameSetting.Values;
			stringOptionName = stringGameSetting.OptionName;
		}
	}

	private void Start()
	{
		Initialize();
	}

	public override void Initialize()
	{
		Value = (int)GameOptionsManager.Instance.CurrentGameOptions.GetValue(data);
		TitleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(Title);
		ValueText.text = DestroyableSingleton<TranslationController>.Instance.GetString(Values[Value]);
		AdjustButtonsActiveState();
	}

	private void FixedUpdate()
	{
		if (oldValue != Value)
		{
			oldValue = Value;
			ValueText.text = DestroyableSingleton<TranslationController>.Instance.GetString(Values[Value]);
		}
	}

	public void Increase()
	{
		Value = Mathf.Clamp(Value + 1, 0, Values.Length - 1);
		UpdateValue();
		OnValueChanged(this);
		AdjustButtonsActiveState();
	}

	public void Decrease()
	{
		Value = Mathf.Clamp(Value - 1, 0, Values.Length - 1);
		UpdateValue();
		OnValueChanged(this);
		AdjustButtonsActiveState();
	}

	public override int GetInt()
	{
		return Value;
	}

	private void UpdateValue()
	{
		if (stringOptionName != Int32OptionNames.Invalid)
		{
			GameOptionsManager.Instance.CurrentGameOptions.SetInt(stringOptionName, GetInt());
		}
		else
		{
			Debug.LogError("Could not update value of " + Title);
		}
	}

	private void AdjustButtonsActiveState()
	{
		if (Value == 0)
		{
			MinusBtn.SetInteractable(interactable: false);
			PlusBtn.SetInteractable(interactable: true);
		}
		else if (Value == Values.Length - 1)
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
