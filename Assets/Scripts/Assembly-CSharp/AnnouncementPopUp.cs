using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using AmongUs.Data;
using Assets.InnerNet;
using Newtonsoft.Json;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class AnnouncementPopUp : MonoBehaviour
{
	public enum AnnounceState
	{
		NotStarted,
		Fetching,
		Failed,
		Success,
		Cached
	}

	private const float PANEL_HEIGHT = 0.8f;

	private const float SCROLL_OFFSET = 2.512f;

	private MainMenuManager mainMenuManager;

	private static AnnounceState UpdateState;

	[SerializeField]
	private GameObject ConnectIcon;

	[SerializeField]
	private AnnouncementPanel AnnouncementPanelPrefab;

	[SerializeField]
	private TextMeshPro Title;

	[SerializeField]
	private TextMeshPro SubTitle;

	[SerializeField]
	private TextMeshPro DateString;

	[SerializeField]
	private TextMeshPro AnnouncementBodyText;

	[SerializeField]
	private GameObject AnnouncementListSlider;

	[SerializeField]
	private Scroller TextScroller;

	[SerializeField]
	private Scroller ListScroller;

	[SerializeField]
	private ManualScrollHelper ManualScrollHelper;

	[SerializeField]
	private AnnouncementPanel ErrorPanel;

	[SerializeField]
	private GameObject ListStateHUD;

	[SerializeField]
	private GameObject ReadingStateHUD;

	[SerializeField]
	private NewsCountButton NewsCountButton;

	private const string READING_STATE_NAME = "Reading";

	private Vector3 panelStartPos = new Vector3(-3.1f, 0.493f, -5f);

	private int newestAnnouncement;

	private List<AnnouncementPanel> visibleAnnouncements = new List<AnnouncementPanel>();

	private AnnouncementPanel selectedPanel;

	private int selectedIndex;

	private Action OnDismissed;

	private readonly Logger logger = new Logger(Logger.Category.UI, "Announcements");

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement ReadingBackButton;

	private List<SelectableHyperLink> selectableHyperLinks = new List<SelectableHyperLink>();

	private bool readingAnnouncement;

	public event Action ActionOnDisable;

	public event Action ActionOnEnable;

	public void SetMainMenuManager(MainMenuManager mainMenu)
	{
		mainMenuManager = mainMenu;
	}

	private static bool IsSuccess(AnnounceState state)
	{
		if (state != AnnounceState.Success)
		{
			return state == AnnounceState.Cached;
		}
		return true;
	}

	private void OnEnable()
	{
		Debug.Log("Announcement Enabling!");
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton);
		SelectableHyperLinkHelper.AddSelectableUiForHyperlinks(selectableHyperLinks, name);
		ActionOnEnable?.Invoke();
	}

	private void OnDisable()
	{
		Debug.Log("Announcement Disabling!");
		ControllerManager.Instance.CloseOverlayMenu("Reading");
		ControllerManager.Instance.CloseOverlayMenu(name);
		ActionOnDisable?.Invoke();
	}

	public static void ClearAnnouncementCache()
	{
		UpdateState = AnnounceState.NotStarted;
	}

	public IEnumerator ShowIfNew(Action onDismissed = null)
	{
		OnDismissed = onDismissed;
		yield return Init(forceOpen: false);
		if (UpdateState == AnnounceState.Success)
		{
			UpdateState = AnnounceState.Cached;
			SetMenu();
		}
		else if (OnDismissed != null)
		{
			OnDismissed();
		}
	}

	public void Show()
	{
		mainMenuManager.ResetScreen();
		gameObject.SetActive(value: true);
		StartCoroutine(Init(forceOpen: true));
		UpdateAnnouncementText(newestAnnouncement, ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick);
	}

	private void SetMenu()
	{
		if (!mainMenuManager.IsMainMenuActive())
		{
			gameObject.SetActive(value: false);
			return;
		}
		gameObject.SetActive(value: true);
		CreateAnnouncementList();
		UpdateAnnouncementText(newestAnnouncement, ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick);
	}

	public void Close()
	{
		DataManager.Player.Save();
		gameObject.SetActive(value: false);
		if (OnDismissed != null)
		{
			OnDismissed();
		}
	}

	private IEnumerator Init(bool forceOpen)
	{
		while (UpdateState == AnnounceState.Fetching)
		{
			yield return null;
		}
		if (UpdateState > AnnounceState.Fetching && DataManager.Player.Announcements.AllAnnouncements.Count > 0)
		{
			CreateAnnouncementList();
			yield break;
		}
		string announcements = BackendEndpoints.Announcements;
		logger.Info("Requesting announcement from: " + announcements);
		Dictionary<string, object> uriParams = new Dictionary<string, object>
		{
			{
				"lang",
				(uint)DataManager.Settings.Language.CurrentLanguage
			},
			{ "id", -1 },
			{
				"console",
				GetAnnouncementTarget()
			}
		};
		string uri = announcements + "?" + HttpUtils.ToUriQuery(uriParams);
		UnityWebRequest request = null;
		bool flag = false;
		int attempts = 3;
		while (!flag && attempts > 0)
		{
			UnityWebRequestAsyncOperation unityWebRequestAsyncOperation;
			try
			{
				request = UnityWebRequest.Get(uri);
				request.SetRequestHeader("X-Platform", Constants.GetPlatformType().ToString());
				if (!string.IsNullOrEmpty(DestroyableSingleton<EOSManager>.Instance.UserIDToken))
				{
					request.SetRequestHeader("Authorization", "Bearer " + DestroyableSingleton<EOSManager>.Instance.UserIDToken);
				}
				logger.Debug("Sent Announcement request");
				unityWebRequestAsyncOperation = request.SendWebRequest();
			}
			catch (Exception exception)
			{
				UpdateState = AnnounceState.Failed;
				Debug.LogException(exception);
				yield break;
			}
			yield return unityWebRequestAsyncOperation;
			flag = request.result == UnityWebRequest.Result.Success;
			attempts--;
		}
		try
		{
			if (request.result == UnityWebRequest.Result.Success)
			{
				ResponseWithTimestamp<AnnouncementList> responseWithTimestamp = JsonConvert.DeserializeObject<ResponseWithTimestamp<AnnouncementList>>(request.downloadHandler.text);
				DestroyableSingleton<EOSManager>.Instance.SetServerTimeStamp(DateTime.ParseExact(responseWithTimestamp.Data.Timestamp, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture).ToUniversalTime().AddSeconds(0f - Time.realtimeSinceStartup));
				DataManager.Player.Announcements.SetAnnouncements(responseWithTimestamp.Data.Attributes.Announcements);
				int number = DataManager.Player.Announcements.AllAnnouncements[0].Number;
				if (DataManager.Player.Announcements.LatestAnnouncementNumber == number)
				{
					logger.Info($"Announcement doesn't seem to be new {DataManager.Player.Announcements.LatestAnnouncementNumber} == {number}");
					UpdateState = AnnounceState.Cached;
				}
				else
				{
					UpdateState = AnnounceState.Success;
				}
				if (forceOpen)
				{
					SetMenu();
				}
				DataManager.Player.Announcements.LatestAnnouncementNumber = number;
				DataManager.Player.Save();
			}
			else
			{
				logger.Error("Failed to get announcement: " + string.Join("\r\n", request.error));
				UpdateState = AnnounceState.Failed;
			}
		}
		catch (Exception exception2)
		{
			UpdateState = AnnounceState.Failed;
			Debug.LogException(exception2);
			yield break;
		}
		NewsCountButton.UpdateCountText();
		yield return null;
	}

	public void ReturnToList()
	{
		TextScroller.ScrollToTop();
		SelectableHyperLinkHelper.DestroyGOs(selectableHyperLinks, "Reading");
		ControllerManager.Instance.CloseOverlayMenu("Reading");
		readingAnnouncement = false;
		ControllerManager.Instance.SetCurrentSelected(selectedPanel.PassiveButton);
	}

	private IEnumerator DelayedUpdateHyperlinkPositions()
	{
		yield return null;
		if (isActiveAndEnabled)
		{
			SelectableHyperLinkHelper.UpdateHyperlinkPositions(AnnouncementBodyText, selectableHyperLinks, name);
		}
	}

	private void ClearAnnouncements()
	{
		foreach (AnnouncementPanel visibleAnnouncement in visibleAnnouncements)
		{
			ControllerManager.Instance.RemoveSelectableUiElement(visibleAnnouncement.PassiveButton);
			UnityEngine.Object.Destroy(visibleAnnouncement.gameObject);
		}
	}

	private void CreateAnnouncementList()
	{
		ClearAnnouncements();
		visibleAnnouncements.Clear();
		Vector3 localPosition = panelStartPos;
		bool flag = true;
		if (DataManager.Player.Announcements.AllAnnouncements.Count == 0)
		{
			ShowError();
			return;
		}
		ErrorPanel.gameObject.SetActive(value: false);
		foreach (Announcement a in DataManager.Player.Announcements.AllAnnouncements)
		{
			if (a.Number > 0)
			{
				AnnouncementPanel panel = UnityEngine.Object.Instantiate(AnnouncementPanelPrefab, AnnouncementListSlider.transform);
				panel.SetUp(a);
				panel.transform.localPosition = localPosition;
				panel.PassiveButton.ClickMask = ListScroller.Hitbox;
				visibleAnnouncements.Add(panel);
				ControllerManager.Instance.AddSelectableUiElement(panel.PassiveButton, flag);
				if (DataManager.Player.Announcements.AnnouncementsRead.Contains(a.Number))
				{
					panel.MarkAsRead();
				}
				if (flag)
				{
					selectedPanel = panel;
					panel.Select();
					flag = false;
					newestAnnouncement = a.Number;
					ControllerManager.Instance.SetCurrentSelected(selectedPanel.PassiveButton);
				}
				panel.PassiveButton.OnClick.AddListener(() =>
				{
					selectedIndex = visibleAnnouncements.IndexOf(panel);
					selectedPanel?.UnSelect();
					selectedPanel = panel;
					selectedPanel.Select();
					UpdateAnnouncementText(a.Number, ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick);
				});
				localPosition.y -= 0.8f;
			}
		}
		ListScroller.SetBoundsMax(0.8f * (float)(DataManager.Player.Announcements.AllAnnouncements.Count + 1) - 2.512f, 0f);
	}

	private void ShowError()
	{
		Title.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameTypeError);
		SubTitle.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.AnnouncementErrorSubtitle);
		DateString.text = string.Empty;
		AnnouncementBodyText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.AnnouncementErrorText);
		ErrorPanel.gameObject.SetActive(value: true);
	}

	private void UpdateAnnouncementText(int id, bool previewOnly = false)
	{
		Announcement? announcement = null;
		int num = DataManager.Player.Announcements.AllAnnouncements.FindIndex((Announcement an) => an.Number == id);
		announcement = ((num == -1) ? new Announcement?(DataManager.Player.Announcements.AllAnnouncements[0]) : new Announcement?(DataManager.Player.Announcements.AllAnnouncements[num]));
		logger.Debug($"Updating announcement text for announcement ID {id}");
		if (!IsSuccess(UpdateState) && (!announcement.HasValue || announcement.Value.Number == 0))
		{
			announcement = DataManager.Player.Announcements.AllAnnouncements[0];
		}
		if (previewOnly)
		{
			Debug.Log("Announcement Setting preview");
			ListStateHUD.SetActive(value: true);
			SelectableHyperLinkHelper.SanitizeLinkText(announcement.Value.Text, out var result);
			AnnouncementBodyText.text = result;
		}
		else
		{
			Debug.Log("Announcement Setting full text");
			ListStateHUD.SetActive(value: false);
			ControllerManager.Instance.CloseOverlayMenu("Reading");
			ControllerManager.Instance.OpenOverlayMenu("Reading", ReadingBackButton);
			ControllerManager.Instance.OpenSpecificMenu("Reading");
			readingAnnouncement = true;
			SelectableHyperLinkHelper.DestroyGOs(selectableHyperLinks, "Reading");
			string text = announcement.Value.Text;
			AnnouncementBodyText.text = SelectableHyperLinkHelper.DecomposeAnnouncementText(AnnouncementBodyText, selectableHyperLinks, "Reading", text);
		}
		Title.text = announcement.Value.Title;
		SubTitle.text = announcement.Value.SubTitle;
		DateString.text = DateTime.Parse(announcement.Value.Date).ToLocalTime().ToString(DestroyableSingleton<TranslationController>.Instance.dateFormats[DataManager.Settings.Language.CurrentLanguage], CultureInfo.InvariantCulture);
		TextScroller.ScrollToTop();
		ManualScrollHelper.enabled = true;
		StartCoroutine(DelayedUpdateHyperlinkPositions());
	}

	private void OnDestroy()
	{
		SelectableHyperLinkHelper.DestroyGOs(selectableHyperLinks, name);
	}

	private void Update()
	{
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			if (ReInput.players.GetPlayer(0).GetButtonDown(11))
			{
				UpdateAnnouncementText(selectedPanel.AnnouncementNumber);
			}
			ReadingStateHUD.SetActive(readingAnnouncement);
			ListStateHUD.SetActive(!readingAnnouncement);
		}
		else if (ListStateHUD.activeSelf || ReadingStateHUD.activeSelf)
		{
			ListStateHUD.SetActive(value: false);
			ReadingStateHUD.SetActive(value: false);
			UpdateAnnouncementText(selectedPanel.AnnouncementNumber);
		}
		TextScroller.SetBoundsMax(Mathf.Abs(AnnouncementBodyText.GetNotDumbRenderedHeight()), 0f);
		ManualScrollHelper.enabled = readingAnnouncement;
	}

	private string GetAnnouncementTarget()
	{
		return "steam";
	}
}
