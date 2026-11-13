using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class VendingMinigame : Minigame
{
	public static readonly string[] Letters = new string[3] { "a", "b", "c" };

	public TextMeshPro NumberText;

	public SpriteRenderer TargetImage;

	public string enteredCode = string.Empty;

	private bool animating;

	private bool done;

	private string targetCode;

	public SpriteRenderer AcceptButton;

	public VendingSlot[] Slots;

	public Sprite[] Drinks;

	public Sprite[] DrawnDrinks;

	public AudioClip Ambience;

	public AudioClip Button;

	public AudioClip Error;

	public AudioClip SliderOpen;

	public AudioClip DrinkShake;

	public AudioClip DrinkLand;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	// Offline remake: the Vending sheet packs the real drink/button sprites into the same pixels as the "vending_front" panel,
	// so drawing the front panel as-is paints sample drinks and keypad buttons over every slot. Draw a copy with transparent windows instead.
	private static readonly RectInt[] FrontWindows = new RectInt[]
	{
		new RectInt(13, 432, 88, 157), new RectInt(122, 432, 88, 157), new RectInt(230, 432, 88, 157), new RectInt(339, 432, 88, 157),
		new RectInt(13, 239, 88, 157), new RectInt(122, 239, 88, 157), new RectInt(230, 239, 88, 157), new RectInt(339, 239, 88, 157),
		new RectInt(13, 45, 88, 157), new RectInt(122, 45, 88, 157), new RectInt(230, 45, 88, 157), new RectInt(339, 45, 88, 157)
	};

	private void CleanFrontPlaceholders()
	{
		foreach (SpriteRenderer r in GetComponentsInChildren<SpriteRenderer>(true))
		{
			Sprite src = r.sprite;
			if (src == null || src.name != "vending_front" || src.texture == null || !src.texture.isReadable)
			{
				continue;
			}
			Rect rc = src.textureRect;
			int w = (int)rc.width;
			int h = (int)rc.height;
			Color[] px = src.texture.GetPixels((int)rc.x, (int)rc.y, w, h);
			foreach (RectInt win in FrontWindows)
			{
				for (int y = win.y; y < win.y + win.height && y < h; y++)
				{
					for (int x = win.x; x < win.x + win.width && x < w; x++)
					{
						px[y * w + x].a = 0f;
					}
				}
			}
			Texture2D clean = new Texture2D(w, h, TextureFormat.RGBA32, mipChain: false);
			clean.filterMode = src.texture.filterMode;
			clean.wrapMode = TextureWrapMode.Clamp;
			clean.SetPixels(px);
			clean.Apply();
			r.sprite = Sprite.Create(clean, new Rect(0f, 0f, w, h), new Vector2(src.pivot.x / w, src.pivot.y / h), src.pixelsPerUnit);
		}
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		CleanFrontPlaceholders();
		int num = Drinks.RandomIdx();
		TargetImage.sprite = DrawnDrinks[num];
		for (int i = 0; i < Drinks.Length; i++)
		{
			Sprite sprite = Drinks[i];
			int slotId;
			while (!PickARandomSlot(sprite, out slotId))
			{
			}
			Slots[slotId].DrinkImage.enabled = true;
			Slots[slotId].DrinkImage.sprite = sprite;
			if (num == i)
			{
				targetCode = SlotIdToString(slotId);
			}
		}
		NumberText.text = string.Empty;
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultButtonSelected, ControllerSelectable);
	}

	private static int StringToSlotId(string code)
	{
		if (int.TryParse(code[0].ToString(), out var _) || Letters.Any(code.EndsWith))
		{
			return -1;
		}
		int num = Letters.IndexOf(code.StartsWith);
		return int.Parse(code[1].ToString()) - 1 + num * 4;
	}

	private static string SlotIdToString(int slotId)
	{
		int num = slotId % 4 + 1;
		int num2 = slotId / 4;
		return Letters[num2] + num;
	}

	private bool PickARandomSlot(Sprite drink, out int slotId)
	{
		slotId = Slots.RandomIdx();
		return !Slots[slotId].DrinkImage.enabled;
	}

	public void EnterDigit(string s)
	{
		if (animating || done)
		{
			return;
		}
		if (enteredCode.Length >= 2)
		{
			StartCoroutine(BlinkAccept());
			return;
		}
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(Button, loop: false).pitch = FloatRange.Next(0.8f, 1.2f);
		}
		enteredCode += s;
		NumberText.text = enteredCode;
	}

	public void ClearDigits()
	{
		if (!animating)
		{
			enteredCode = string.Empty;
			NumberText.text = string.Empty;
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(Button, loop: false).pitch = FloatRange.Next(0.8f, 1.2f);
			}
		}
	}

	public void AcceptDigits()
	{
		if (!animating && enteredCode.Length == 2)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(Button, loop: false).pitch = FloatRange.Next(0.8f, 1.2f);
			}
			StartCoroutine(Animate());
		}
	}

	private IEnumerator BlinkAccept()
	{
		int i = 0;
		while (i < 5)
		{
			AcceptButton.color = Color.gray;
			yield return null;
			yield return null;
			AcceptButton.color = Color.white;
			yield return null;
			yield return null;
			int num = i + 1;
			i = num;
		}
	}

	private IEnumerator Animate()
	{
		animating = true;
		int slotId = StringToSlotId(enteredCode);
		if (slotId >= 0 && Slots[slotId].DrinkImage.enabled)
		{
			yield return Effects.All(CoBlinkVend(), Slots[slotId].CoBuy(SliderOpen, DrinkShake, DrinkLand));
			if (targetCode == enteredCode)
			{
				done = true;
				MyNormTask.NextStep();
				StartCoroutine(CoStartClose());
			}
			yield return Slots[slotId].CloseSlider(SliderOpen);
		}
		else
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(Error, loop: false);
			}
			WaitForSeconds wait = new WaitForSeconds(0.1f);
			NumberText.text = "XXXXXXXX";
			yield return wait;
			NumberText.text = string.Empty;
			yield return wait;
			NumberText.text = "XXXXXXXX";
			yield return wait;
		}
		enteredCode = string.Empty;
		NumberText.text = enteredCode;
		animating = false;
	}

	private IEnumerator CoBlinkVend()
	{
		int i = 0;
		while (i < 5)
		{
			NumberText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Vending);
			yield return Effects.Wait(0.1f);
			NumberText.text = string.Empty;
			yield return Effects.Wait(0.1f);
			int num = i + 1;
			i = num;
		}
	}
}
