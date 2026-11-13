using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class LobbyNotificationMessage : MonoBehaviour
{
	[SerializeField]
	public SpriteRenderer Icon;

	[SerializeField]
	public TextMeshPro Text;

	public float alphaTimer;

	public float showDuration = 8f;

	private float fadeDuration = 1f;

	private Action onDestroy;

	private Color textColor;

	private List<SupportedLangs> languagesToBold= new List<SupportedLangs>
	{
		SupportedLangs.Russian,
		SupportedLangs.Japanese,
		SupportedLangs.SChinese,
		SupportedLangs.TChinese,
		SupportedLangs.Korean
	};

	private unsafe void Update()
	{
		if (alphaTimer > 0f)
		{
			alphaTimer -= Time.deltaTime;
			textColor.a = Mathf.Clamp(alphaTimer / fadeDuration, 0f, 1f);
			Text.color = textColor;
			Icon.color = textColor;
			if (alphaTimer <= 0f)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}

	private void OnDestroy()
	{
		onDestroy?.Invoke();
	}

	public unsafe void SetUp(string item, Sprite icon, Color textColor, Action onDestroy)
	{
		this.textColor = textColor;
		this.onDestroy = onDestroy;
		Text.text = item;
		Text.color = this.textColor;
		Icon.sprite = icon;
		Icon.color = this.textColor;
		alphaTimer = showDuration;
		if (languagesToBold.Contains(DataManager.Settings.Language.CurrentLanguage))
		{
			Text.fontStyle = FontStyles.Bold;
		}
	}

	public unsafe void UpdateMessage(string item)
	{
		Text.text = item;
		Text.color = textColor;
		Icon.color = textColor;
		alphaTimer = showDuration;
	}

	public LobbyNotificationMessage()
	{ }
}
