using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using CsvHelper;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
	public enum CreditType
	{
		TEXT,
		IMAGE
	}

	[Serializable]
	public struct FormatStruct
	{
		public string format;

		public CreditType creditType;

		public List<GameObject> formatPrefabs;
	}

	[Serializable]
	public struct CreditStruct
	{
		public string format;

		public string[] columns;
	}

	private Action m_OnFinish;

	public int NumberOfContentColumns = 3;

	public GameObject creditPanelPrefab;

	public GameObject creditMainPanel;

	public float creditScrollSpeed;

	public float initialDelay = 2f;

	public float startPos = 230f;

	private float remainingDelay;

	private RectTransform creditsRect;

	private float defaultCreditsScrollSpeed;

	private float navCreditsScrollSpeed = 100f;

	private Player player;

	private bool paused;

	public List<FormatStruct> formats;

	public List<CreditStruct> credits;

	public event Action OnFinish
	{
		add
		{
			m_OnFinish = (Action)System.Delegate.Combine(m_OnFinish, value);
		}
		remove
		{
			m_OnFinish = (Action)System.Delegate.Remove(m_OnFinish, value);
		}
	}

	private void Awake()
	{
		creditsRect = creditMainPanel.GetComponent<RectTransform>();
		LoadCredits();
		defaultCreditsScrollSpeed = creditScrollSpeed;
		player = ReInput.players.GetPlayer(0);
	}

	private void Start()
	{
		remainingDelay = initialDelay;
		Debug.Log("First Credit: " + credits[0].columns[0]);
		_ = base.gameObject;
		for (int i = 0; i < credits.Count; i++)
		{
			FormatStruct format = GetFormat(credits[i].format);
			GameObject gameObject = UnityEngine.Object.Instantiate(creditPanelPrefab, creditMainPanel.transform);
			for (int j = 0; j < format.formatPrefabs.Count; j++)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(format.formatPrefabs[j], gameObject.transform);
				if (format.creditType == CreditType.TEXT)
				{
					string text = credits[i].columns[j].Trim();
					if (text.IsNullOrWhiteSpace())
					{
						gameObject2.GetComponent<TextMeshProUGUI>().text = string.Empty;
					}
					else
					{
						gameObject2.GetComponent<TextMeshProUGUI>().text += text;
					}
				}
				else if (format.creditType == CreditType.IMAGE)
				{
					gameObject2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Credits/" + credits[i].columns[j]);
					gameObject2.GetComponent<Image>().preserveAspect = true;
					HorizontalLayoutGroup componentInParent = gameObject2.GetComponentInParent<HorizontalLayoutGroup>();
					componentInParent.childControlHeight = false;
					componentInParent.padding.bottom = 0;
				}
			}
		}
	}

	private unsafe void OnEnable()
	{
		creditMainPanel.transform.localPosition = new Vector3(creditMainPanel.transform.localPosition.x, startPos, creditMainPanel.transform.localPosition.z);
		remainingDelay = initialDelay;
	}

	private void OnDisable()
	{
	}

	private void PauseUnpauseCreditsScroll()
	{
		paused = !paused;
	}

	private void Update()
	{
		if (player.GetAxis(10) != 0f)
		{
			creditScrollSpeed = (0f - player.GetAxis(10)) * navCreditsScrollSpeed;
		}
		else if (player.GetButton(44) || player.GetButton(42))
		{
			creditScrollSpeed = (player.GetButton(44) ? (0f - navCreditsScrollSpeed) : navCreditsScrollSpeed);
		}
		else if (Input.touchCount > 0 || Input.GetMouseButton(0))
		{
			creditScrollSpeed = navCreditsScrollSpeed;
		}
		else
		{
			creditScrollSpeed = defaultCreditsScrollSpeed;
		}
	}

	private unsafe void FixedUpdate()
	{
		if (paused)
		{
			return;
		}
		if (remainingDelay > 0f)
		{
			remainingDelay -= Time.unscaledDeltaTime;
			return;
		}
		float y = Mathf.Clamp(creditMainPanel.transform.localPosition.y + creditScrollSpeed, 0f, creditsRect.rect.height);
		creditMainPanel.transform.localPosition = new Vector2(creditMainPanel.transform.localPosition.x, y);
		if (creditsRect.localPosition.y >= creditsRect.rect.height && m_OnFinish != null)
		{
			m_OnFinish();
		}
	}

	private void GenerateCreditEntry(CreditStruct credit)
	{
		if (string.IsNullOrEmpty(GetFormat(credit.format).format))
		{
			Debug.LogError("Credit entry format (" + credit.format + ") is missing from formats");
		}
	}

	public void ClearCredits()
	{
		Debug.Log("CLearing Credits");
		credits = new List<CreditStruct>();
	}

	public unsafe void AddCredit(CreditStruct credit)
	{
		credits.Add(credit);
	}

	public unsafe void AddFormat(string formatName)
	{
		for (int i = 0; i < formats.Count; i++)
		{
			if (formats[i].format == formatName)
			{
				formatName = "";
				break;
			}
		}
		if (!string.IsNullOrEmpty(formatName))
		{
			FormatStruct item = new FormatStruct
			{
				format = formatName
			};
			formats.Add(item);
		}
	}

	public unsafe FormatStruct GetFormat(string formatName)
	{
		foreach (FormatStruct format in formats)
		{
			if (format.format == formatName)
			{
				return format;
			}
		}
		return new FormatStruct
		{
			format = ""
		};
	}

	private unsafe void LoadCredits()
	{
		CsvReader csvReader = new CsvReader(new StringReader(DestroyableSingleton<ReferenceDataManager>.Instance.Refdata.credits.ToString()));
		ClearCredits();
		while (csvReader.Read())
		{
			CreditStruct credit = default;
			string formatName = (credit.format = csvReader.GetField<string>("Format"));
			credit.columns = new string[NumberOfContentColumns];
			for (int i = 1; i <= NumberOfContentColumns; i++)
			{
				string text = "";
				text = csvReader.GetField<string>("Column" + i);
				credit.columns[i - 1] = text;
			}
			AddCredit(credit);
			AddFormat(formatName);
		}
	}
}
