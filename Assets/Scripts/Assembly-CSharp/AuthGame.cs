using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AuthGame : Minigame
{
	public TextMeshPro TargetText;

	public TextMeshPro NumberText;

	public TextMeshPro OtherStatusText;

	public int number;

	public string numString = string.Empty;

	private bool animating;

	private HqHudSystemType system;

	public SpriteRenderer OurLight;

	public SpriteRenderer TheirLight;

	public SpriteRenderer TimeBar;

	public AudioClip ButtonSound;

	public AudioClip AcceptSound;

	public AudioClip RejectSound;

	private int OtherConsoleId;

	private bool evenColor;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public override void Begin(PlayerTask task)
	{
		OtherConsoleId = (ConsoleId + 1) % 2;
		base.Begin(task);
		system = ShipStatus.Instance.Systems[SystemTypes.Comms] as HqHudSystemType;
		ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Comms, (byte)(0x40 | ConsoleId));
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultButtonSelected, ControllerSelectable);
	}

	public override void Close()
	{
		base.Close();
		ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Comms, (byte)(0x20 | ConsoleId));
	}

	public void Update()
	{
		evenColor = (int)Time.time * 2 % 2 == 0;
		Vector3 localScale = TimeBar.transform.localScale;
		localScale.x = system.PercentActive;
		TimeBar.transform.localScale = localScale;
		if (system.PercentActive < 0.25f)
		{
			TimeBar.color = new Color(1f, 0.45f, 0.25f);
		}
		else if ((double)system.PercentActive < 0.5)
		{
			TimeBar.color = Color.yellow;
		}
		else
		{
			TimeBar.color = Color.white;
		}
		TargetText.text = system.TargetNumber.ToString("D5");
		if (system.IsConsoleOkay(ConsoleId))
		{
			OurLight.color = Color.green;
		}
		else
		{
			OurLight.color = (evenColor ? Color.white : Color.yellow);
		}
		if (amClosing == CloseState.None && !system.IsActive)
		{
			StartCoroutine(CoStartClose());
		}
		if (system.IsConsoleOkay(OtherConsoleId))
		{
			TheirLight.color = Color.green;
			StringNames id = ((OtherConsoleId == 1) ? StringNames.AuthOfficeOkay : StringNames.AuthCommsOkay);
			OtherStatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(id);
		}
		else if (system.IsConsoleActive(OtherConsoleId))
		{
			TheirLight.color = (evenColor ? Color.white : Color.yellow);
			StringNames id2 = ((OtherConsoleId == 1) ? StringNames.AuthOfficeActive : StringNames.AuthCommsActive);
			OtherStatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(id2);
		}
		else
		{
			TheirLight.color = Color.red;
			StringNames id3 = ((OtherConsoleId == 1) ? StringNames.AuthOfficeNotActive : StringNames.AuthCommsNotActive);
			OtherStatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(id3);
		}
	}

	public void ClickNumber(int i)
	{
		if (!animating && NumberText.text.Length < 5)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(ButtonSound, loop: false);
			}
			numString += i;
			number = number * 10 + i;
			NumberText.text = numString;
		}
	}

	public void ClearEntry()
	{
		if (!animating)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(ButtonSound, loop: false);
			}
			number = 0;
			numString = string.Empty;
			NumberText.text = string.Empty;
		}
	}

	public void Enter()
	{
		if (!animating)
		{
			StartCoroutine(Animate());
		}
	}

	private IEnumerator Animate()
	{
		animating = true;
		WaitForSeconds wait = new WaitForSeconds(0.1f);
		yield return wait;
		NumberText.text = string.Empty;
		yield return wait;
		if (system.TargetNumber == number)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(AcceptSound, loop: false);
			}
			ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Comms, (byte)(0x10 | ConsoleId));
			try
			{
				((SabotageTask)MyTask).MarkContributed();
			}
			catch
			{
			}
			NumberText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.OK);
			yield return wait;
			NumberText.text = string.Empty;
			yield return wait;
			NumberText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.OK);
			yield return wait;
			NumberText.text = string.Empty;
			yield return wait;
			NumberText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.OK);
		}
		else
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(RejectSound, loop: false);
			}
			NumberText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Bad);
			yield return wait;
			NumberText.text = string.Empty;
			yield return wait;
			NumberText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Bad);
			yield return wait;
			numString = string.Empty;
			number = 0;
			NumberText.text = numString;
		}
		number = 0;
		numString = string.Empty;
		NumberText.text = string.Empty;
		animating = false;
	}
}
