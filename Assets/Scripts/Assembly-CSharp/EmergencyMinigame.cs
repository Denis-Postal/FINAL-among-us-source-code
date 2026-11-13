using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class EmergencyMinigame : Minigame
{
	public SpriteRenderer ClosedLid;

	public SpriteRenderer OpenLid;

	public Transform meetingButton;

	public TextMeshPro StatusText;

	public TextMeshPro NumberText;

	public bool ButtonActive = true;

	public AudioClip ButtonSound;

	private int state;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public const int MinEmergencyTime = 15;

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		Update();
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultButtonSelected);
	}

	public void Update()
	{
		if (ShipStatus.Instance.Timer < 15f || ShipStatus.Instance.EmergencyCooldown > 0f)
		{
			int b = Mathf.CeilToInt(15f - ShipStatus.Instance.Timer);
			b = Mathf.Max(Mathf.CeilToInt(ShipStatus.Instance.EmergencyCooldown), b);
			ButtonActive = false;
			StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.EmergencyNotReady);
			NumberText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SecondsAbbv, b);
			ClosedLid.gameObject.SetActive(value: true);
			OpenLid.gameObject.SetActive(value: false);
		}
		else if (!PlayerControl.LocalPlayer.myTasks.Any(PlayerTask.TaskIsEmergency))
		{
			if (state != 1)
			{
				state = 1;
				int remainingEmergencies = PlayerControl.LocalPlayer.RemainingEmergencies;
				StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.EmergencyCount, PlayerControl.LocalPlayer.Data.PlayerName);
				NumberText.text = remainingEmergencies.ToString();
				ButtonActive = remainingEmergencies > 0;
				ClosedLid.gameObject.SetActive(!ButtonActive);
				OpenLid.gameObject.SetActive(ButtonActive);
			}
		}
		else if (state != 2)
		{
			state = 2;
			ButtonActive = false;
			StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.EmergencyDuringCrisis);
			NumberText.text = string.Empty;
			ClosedLid.gameObject.SetActive(value: true);
			OpenLid.gameObject.SetActive(value: false);
		}
	}

	public void CallMeeting()
	{
		if (!PlayerControl.LocalPlayer.myTasks.Any(PlayerTask.TaskIsEmergency) && PlayerControl.LocalPlayer.RemainingEmergencies > 0 && ButtonActive)
		{
			StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.EmergencyRequested);
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(ButtonSound, loop: false);
			}
			PlayerControl.LocalPlayer.CmdReportDeadBody(null);
			ButtonActive = false;
			VibrationManager.Vibrate(1f, 1f, 0.2f);
		}
	}

	private float easeOutElastic(float t)
	{
		float num = 0.3f;
		return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - num / 4f) * (MathF.PI * 2f) / num) + 1f;
	}

	protected override IEnumerator CoAnimateOpen()
	{
		for (float timer = 0f; timer < 0.2f; timer += Time.deltaTime)
		{
			float t = timer / 0.2f;
			transform.localPosition = new Vector3(0f, Mathf.SmoothStep(-8f, 0f, t), -50f);
			yield return null;
		}
		transform.localPosition = new Vector3(0f, 0f, -50f);
		Vector3 meetingPos = meetingButton.localPosition;
		for (float timer = 0f; timer < 0.1f; timer += Time.deltaTime)
		{
			float num = timer / 0.1f;
			meetingPos.y = Mathf.Sin(MathF.PI * num) * 1f / (num * 5f + 4f) - 0.882f;
			meetingButton.localPosition = meetingPos;
			yield return null;
		}
		meetingPos.y = -0.882f;
		meetingButton.localPosition = meetingPos;
	}
}
