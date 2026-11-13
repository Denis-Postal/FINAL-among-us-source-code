using AmongUs.GameOptions;
using TMPro;
using UnityEngine;

public class ToggleOption : OptionBehaviour
{
	public TextMeshPro TitleText;

	public SpriteRenderer CheckMark;

	private bool oldValue;

	private BoolOptionNames boolOptionName;

	public override void SetUpFromData(BaseGameSetting data, int maskLayer)
	{
		base.SetUpFromData(data, maskLayer);
		CheckboxGameSetting checkboxGameSetting = data as CheckboxGameSetting;
		if (!(checkboxGameSetting == null))
		{
			Title = checkboxGameSetting.Title;
			boolOptionName = checkboxGameSetting.OptionName;
		}
	}

	private void Start()
	{
		Initialize();
	}

	public override void Initialize()
	{
		TitleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(Title);
		CheckMark.enabled = GameOptionsManager.Instance.CurrentGameOptions.GetValue(data) == 1f;
	}

	private void FixedUpdate()
	{
		bool flag = GetBool();
		if (oldValue != flag)
		{
			oldValue = flag;
			CheckMark.enabled = flag;
		}
	}

	public void Toggle()
	{
		CheckMark.enabled = !CheckMark.enabled;
		UpdateValue();
		OnValueChanged(this);
	}

	public override bool GetBool()
	{
		return CheckMark.enabled;
	}

	private void UpdateValue()
	{
		if (boolOptionName != BoolOptionNames.Invalid)
		{
			GameOptionsManager.Instance.CurrentGameOptions.SetBool(boolOptionName, GetBool());
		}
		else
		{
			Debug.LogError("Could not update value of " + Title);
		}
	}
}
