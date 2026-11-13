using System;
using AmongUs.GameOptions;
using TMPro;
using UnityEngine;

public class NumberOption : OptionBehaviour
{
	public TextMeshPro TitleText;

	public TextMeshPro ValueText;

	public float Value = 1f;

	private float oldValue = float.MaxValue;

	public float Increment;

	public FloatRange ValidRange = new FloatRange(0f, 2f);

	public string FormatString = "0.0";

	public bool ZeroIsInfinity;

	public NumberSuffixes SuffixType = NumberSuffixes.Multiplier;

	private FloatOptionNames floatOptionName;

	private Int32OptionNames intOptionName;

	[SerializeField]
	private GameOptionButton PlusBtn;

	[SerializeField]
	private GameOptionButton MinusBtn;

	public override void SetUpFromData(BaseGameSetting data, int maskLayer)
	{
		base.SetUpFromData(data, maskLayer);
		if (data.Type == OptionTypes.Float)
		{
			FloatGameSetting floatGameSetting = data as FloatGameSetting;
			if (!(floatGameSetting == null))
			{
				Title = floatGameSetting.Title;
				Value = floatGameSetting.Value;
				Increment = floatGameSetting.Increment;
				ValidRange = floatGameSetting.ValidRange;
				FormatString = floatGameSetting.FormatString;
				ZeroIsInfinity = floatGameSetting.ZeroIsInfinity;
				SuffixType = floatGameSetting.SuffixType;
				floatOptionName = floatGameSetting.OptionName;
			}
		}
		else if (data.Type == OptionTypes.Int)
		{
			IntGameSetting intGameSetting = data as IntGameSetting;
			if (!(intGameSetting == null))
			{
				Title = intGameSetting.Title;
				Value = intGameSetting.Value;
				Increment = intGameSetting.Increment;
				if (data.Title == StringNames.GameNumImpostors)
				{
					// Comment out the current implementation:
					// GameOptionsManager.Instance.CurrentGameOptions.TryGetInt(Int32OptionNames.MaxPlayers, out var value);
					// GameOptionsManager.Instance.CurrentGameOptions.TryGetIntArray(Int32ArrayOptionNames.MaxImpostors, out var value2);
					// ValidRange = new FloatRange(intGameSetting.ValidRange.min, value2[value]);

					GameOptionsManager.Instance.CurrentGameOptions.TryGetInt(Int32OptionNames.MaxPlayers, out var value);
					if (GameOptionsManager.Instance.CurrentGameOptions.TryGetIntArray(Int32ArrayOptionNames.MaxImpostors, out var value2) && value2 != null && value < value2.Length)
					{
						ValidRange = new FloatRange(intGameSetting.ValidRange.min, value2[value]);
					}
					else
					{
						int fallbackMax = (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek) ? 15 : 5;
						ValidRange = new FloatRange(intGameSetting.ValidRange.min, fallbackMax);
					}
				}
				else
				{
					ValidRange = new FloatRange(intGameSetting.ValidRange.min, intGameSetting.ValidRange.max);
				}
				FormatString = intGameSetting.FormatString;
				ZeroIsInfinity = intGameSetting.ZeroIsInfinity;
				SuffixType = intGameSetting.SuffixType;
				intOptionName = intGameSetting.OptionName;
			}
		}
		else
		{
			Debug.LogError("Could not set up NumberOption for " + data.Title);
		}
	}

	private void Start()
	{
		Initialize();
	}

	public override void Initialize()
	{
		TitleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(Title);
		if ((bool)data)
		{
			GameOptionsManager optionsManager = GameOptionsManager.Instance;
			IGameOptions options = optionsManager?.currentGameOptions ?? optionsManager?.currentHostOptions;
			if (options == null)
			{
				Debug.LogError($"NumberOption '{Title}' has no active or host game options; keeping its configured default value.", this);
				RestoreConfiguredValue();
			}
			else
			{
				try
				{
					Value = options.GetValue(data);
				}
				catch (Exception exception)
				{
					Debug.LogError($"NumberOption '{Title}' could not read the active game options; keeping its configured default value. {exception}", this);
					RestoreConfiguredValue();
				}
			}
		}
		AdjustButtonsActiveState();
	}

	private void RestoreConfiguredValue()
	{
		if (data is IntGameSetting intSetting)
		{
			Value = intSetting.Value;
		}
		else if (data is FloatGameSetting floatSetting)
		{
			Value = floatSetting.Value;
		}
	}

	private void FixedUpdate()
	{
		if (data != null && ValueText != null && oldValue != Value)
		{
			oldValue = Value;
			ValueText.text = data.GetValueString(Value);
		}
	}

	public void Increase()
	{
		if (data != null && data.Title == StringNames.GameNumImpostors)
		{
			GameOptionsManager.Instance.CurrentGameOptions.TryGetInt(Int32OptionNames.MaxPlayers, out var maxP);
			if (GameOptionsManager.Instance.CurrentGameOptions.TryGetIntArray(Int32ArrayOptionNames.MaxImpostors, out var maxImpArr) && maxImpArr != null && maxP < maxImpArr.Length)
			{
				ValidRange = new FloatRange(ValidRange.min, maxImpArr[maxP]);
			}
		}
		if (Value != ValidRange.max)
		{
			Value = ValidRange.Clamp(Value + Increment);
			UpdateValue();
			OnValueChanged(this);
			AdjustButtonsActiveState();
		}
	}

	public void Decrease()
	{
		if (data != null && data.Title == StringNames.GameNumImpostors)
		{
			GameOptionsManager.Instance.CurrentGameOptions.TryGetInt(Int32OptionNames.MaxPlayers, out var maxP);
			if (GameOptionsManager.Instance.CurrentGameOptions.TryGetIntArray(Int32ArrayOptionNames.MaxImpostors, out var maxImpArr) && maxImpArr != null && maxP < maxImpArr.Length)
			{
				ValidRange = new FloatRange(ValidRange.min, maxImpArr[maxP]);
			}
		}
		if (Value != ValidRange.min)
		{
			Value = ValidRange.Clamp(Value - Increment);
			UpdateValue();
			OnValueChanged(this);
			AdjustButtonsActiveState();
		}
	}

	public override float GetFloat()
	{
		return Value;
	}

	public override int GetInt()
	{
		return (int)Value;
	}

	private void UpdateValue()
	{
		if (floatOptionName != FloatOptionNames.Invalid)
		{
			GameOptionsManager.Instance.CurrentGameOptions.SetFloat(floatOptionName, GetFloat());
		}
		else if (intOptionName != Int32OptionNames.Invalid)
		{
			GameOptionsManager.Instance.CurrentGameOptions.SetInt(intOptionName, GetInt());
		}
		else
		{
			Debug.LogError("Could not update value of " + Title);
		}
	}

	private void AdjustButtonsActiveState()
	{
		if (Value == ValidRange.min)
		{
			MinusBtn.SetInteractable(interactable: false);
			PlusBtn.SetInteractable(interactable: true);
		}
		else if (Value == ValidRange.max)
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
