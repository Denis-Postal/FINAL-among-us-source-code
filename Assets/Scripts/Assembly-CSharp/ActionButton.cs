using TMPro;
using UnityEngine;

public abstract class ActionButton : MonoBehaviour
{
	[SerializeField]
	protected SpriteRenderer graphic;

	[SerializeField]
	protected SpriteRenderer usesRemainingSprite;

	[SerializeField]
	protected TextMeshPro usesRemainingText;

	[SerializeField]
	protected TextMeshPro buttonLabelText;

	[SerializeField]
	protected TextMeshPro cooldownTimerText;

	[SerializeField]
	private ActionMapGlyphDisplay glyph;

	protected bool isCoolingDown;

	protected readonly Logger logger = new Logger(Logger.Category.UI, "ActionButton");

	public bool canInteract = true;

	private Vector3 position;

	private RewiredConstsEnum.Action originalAction;

	public bool IsOnCooldown => isCoolingDown;

	private void Start()
	{
		position = graphic.transform.localPosition;
		if (glyph != null)
		{
			originalAction = glyph.actionToDisplayMappedGlyphFor;
		}
	}

	public void SetCustomGlyph(RewiredConstsEnum.Action newAction)
	{
		glyph.actionToDisplayMappedGlyphFor = newAction;
		glyph.UpdateGlyphDisplay();
	}

	public void RestoreGlyph()
	{
		glyph.actionToDisplayMappedGlyphFor = originalAction;
		glyph.UpdateGlyphDisplay();
	}

	public abstract void DoClick();

	public bool CanInteract()
	{
		return canInteract;
	}

	public void SetCoolDown(float timer, float maxTimer)
	{
		float num = Mathf.Clamp(timer / maxTimer, 0f, 1f);
		isCoolingDown = num > 0f;
		SetCooldownFill(num);
		if (isCoolingDown)
		{
			cooldownTimerText.text = Mathf.CeilToInt(timer).ToString();
			cooldownTimerText.gameObject.SetActive(value: true);
		}
		else
		{
			cooldownTimerText.gameObject.SetActive(value: false);
		}
	}

	public void SetUsesRemaining(int num)
	{
		usesRemainingText.gameObject.SetActive(value: true);
		usesRemainingText.text = num.ToString();
		usesRemainingSprite.gameObject.SetActive(value: true);
	}

	public void SetInfiniteUses()
	{
		usesRemainingText.gameObject.SetActive(value: false);
		usesRemainingSprite.gameObject.SetActive(value: false);
	}

	public void SetFillUp(float timer, float maxTimer)
	{
		float num = Mathf.Clamp((maxTimer - timer) / maxTimer, 0f, 1f);
		isCoolingDown = num > 0f;
		if (isCoolingDown && timer < 3f)
		{
			graphic.transform.localPosition = position + (Vector3)Random.insideUnitCircle * 0.05f;
			cooldownTimerText.text = Mathf.CeilToInt(timer).ToString();
			cooldownTimerText.gameObject.SetActive(value: true);
		}
		else
		{
			graphic.transform.localPosition = position;
		}
		SetCooldownFill(num);
	}

	public void SetEnabled()
	{
		canInteract = true;
		graphic.color = Palette.EnabledColor;
		buttonLabelText.color = Palette.EnabledColor;
		graphic.material.SetFloat("_Desat", 0f);
		glyph.SetColor(Palette.EnabledColor);
	}

	public void SetDisabled()
	{
		canInteract = false;
		buttonLabelText.color = Palette.DisabledClear;
		graphic.color = Palette.DisabledClear;
		graphic.material.SetFloat("_Desat", 1f);
		glyph.SetColor(Palette.DisabledClear);
	}

	public void ToggleVisible(bool visible)
	{
		if (visible)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	public virtual void Hide()
	{
		gameObject.SetActive(value: false);
	}

	public virtual void Show()
	{
		gameObject.SetActive(value: true);
	}

	public void SetCooldownFill(float percentCool)
	{
		graphic.material.SetFloat("_Percent", percentCool);
	}

	public void OverrideText(string text)
	{
		buttonLabelText.text = text;
	}

	public void OverrideColor(Color color)
	{
		graphic.color = color;
	}

	protected void ResetCoolDown()
	{
		isCoolingDown = false;
		SetCooldownFill(1f);
		cooldownTimerText.gameObject.SetActive(value: false);
	}
}
