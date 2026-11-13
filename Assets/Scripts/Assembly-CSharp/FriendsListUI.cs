using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AmongUs.Data;
using Assets.InnerNet;
using InnerNet;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FriendsListUI : MonoBehaviour
{
	[Serializable]
	public class FriendsListTabButton
	{
		public FriendsListTab tab;

		public GameObject tabObject;

		public List<SpriteRenderer> selectedSprites;

		public bool selectable = true;
	}

	[Serializable]
	public enum FriendsListTab
	{
		LobbyPlayers,
		RecentPlayers,
		AmongUsFriends,
		Blocked,
		Notifications,
		AddFriend
	}

	public static FriendsListUI Instance;

	public TextMeshPro FriendCodeText;

	public SpriteRenderer FriendCodeHiddenIcon;

	public GameObject FriendCodeHideToggleObject;

	public FriendsListTabButton[] Tabs;

	public GameObject NotifArea;

	public GameObject BlockedArea;

	public GameObject FriendArea;

	public GameObject PlatformFriendArea;

	public GameObject LobbyPlayerArea;

	public GameObject RecentlyPlayedArea;

	public GameObject LobbyPlayersTab;

	public GameObject LobbyPlayersInactiveTab;

	public GameObject PlatformFriendsButton;

	public FriendRequestBar FriendRequestBar;

	public BlockedPlayerBar BlockedPlayerBar;

	public OnlineFriendBar OnlineFriendBar;

	public OnlineFriendBar PlatformFriendBar;

	public LobbyPlayerBar LobbyPlayerBar;

	public LobbyInviteBar LobbyInviteBar;

	public Scroller FriendsScroller;

	public Scroller PlatformFriendsScroller;

	public Scroller BlockedScroller;

	public Scroller NotifScroller;

	public Scroller LobbyScroller;

	public Scroller RecentlyPlayedScroller;

	public SpriteRenderer InactiveAllFriends;

	public SpriteRenderer InactivePlatformFriends;

	public TextBoxTMP AddFriendArea;

	public List<FriendsListGuestWarning> guestAccountWarnings;

	public float YStart;

	public float YOffset;

	public SpriteRenderer ViewRequestsButton;

	public TextMeshPro ViewRequestsText;

	public Color NewRequestColor;

	public Color NoRequestsColor;

	public GameObject AddFriendObjects;

	public ReportReasonScreen reportScreen;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	public GameObject glyphL;

	public GameObject glyphR;

	private int selectedTab;

	private List<LobbyPlayerBar> lobbyBars;

	private List<FriendsListBar> friendBars;

	private List<FriendsListBar> platformFriendBars;

	private List<FriendsListBar> notifBars;

	private string friendCodeHiddenText = "XXXXXXXXXX";

	private bool viewingAllFriends = true;

	private string currentSceneName;

	private float addFriendCooldown;

	public FriendsListTab CurrentTab => Tabs[selectedTab].tab;

	public bool IsOpen => gameObject.activeSelf;

	public void Open()
	{
		if (gameObject.activeSelf || currentSceneName == "")
		{
			Close();
			return;
		}
		FriendsListBar[] componentsInChildren = GetComponentsInChildren<FriendsListBar>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != null)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
			}
		}
		currentSceneName = SceneManager.GetActiveScene().name;
		UpdateFriendCodeUI();
		if ((DestroyableSingleton<HudManager>.InstanceExists && DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening) || ShipStatus.Instance != null)
		{
			return;
		}
		friendBars = new List<FriendsListBar>();
		lobbyBars = new List<LobbyPlayerBar>();
		notifBars = new List<FriendsListBar>();
		platformFriendBars = new List<FriendsListBar>();
		viewingAllFriends = true;
		gameObject.SetActive(value: true);
		guestAccountWarnings.ForEach((FriendsListGuestWarning t) =>
		{
			t.gameObject.SetActive(value: false);
		});
		ViewRequestsButton.color = NoRequestsColor;
		ViewRequestsText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NoNewRequests);
		StartCoroutine(DestroyableSingleton<FriendsListManager>.Instance.RefreshFriendsList(() =>
		{
			ClearNotifs();
			if (DestroyableSingleton<EOSManager>.Instance.IsFriendsListAllowed())
			{
				AddFriendObjects.SetActive(value: true);
				RefreshBlockedPlayers();
				RefreshFriends();
				RefreshNotifications();
			}
			else
			{
				AddFriendObjects.SetActive(value: false);
				guestAccountWarnings.ForEach((FriendsListGuestWarning g) =>
				{
					g.SetUp();
				});
			}
			RefreshRecentlyPlayed();
			RefreshPlatformFriends();
			foreach (FriendsListBar friendBar in friendBars)
			{
				foreach (PassiveButton item in friendBar.ControllerSelectable)
				{
					ControllerManager.Instance.AddSelectableUiElement(item);
				}
			}
			foreach (FriendsListBar platformFriendBar in platformFriendBars)
			{
				foreach (PassiveButton item2 in platformFriendBar.ControllerSelectable)
				{
					ControllerManager.Instance.AddSelectableUiElement(item2);
				}
			}
			foreach (FriendsListBar notifBar in notifBars)
			{
				foreach (PassiveButton item3 in notifBar.ControllerSelectable)
				{
					ControllerManager.Instance.AddSelectableUiElement(item3);
				}
			}
			foreach (LobbyPlayerBar lobbyBar in lobbyBars)
			{
				foreach (PassiveButton item4 in lobbyBar.ControllerSelectable)
				{
					ControllerManager.Instance.AddSelectableUiElement(item4);
				}
			}
			ControllerManager.Instance.PickTopSelectable();
		}));
		if (currentSceneName == "OnlineGame")
		{
			RefreshLobbyPlayers();
			LobbyPlayersTab.SetActive(value: true);
			LobbyPlayersInactiveTab.SetActive(value: false);
			OpenTab(0);
		}
		else
		{
			LobbyPlayersInactiveTab.SetActive(value: true);
			LobbyPlayersTab.SetActive(value: false);
			OpenTab(2);
		}
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultButtonSelected, ControllerSelectable);
	}

	public void OpenTab(int tabType)
	{
		CloseTab();
		FriendsListTabButton friendsListTabButton = Tabs.First((FriendsListTabButton t) => t.tab == (FriendsListTab)tabType);
		friendsListTabButton.tabObject.SetActive(value: true);
		Logger.GlobalInstance.Info("FriendsListUI::Opening friends list tab " + friendsListTabButton.tabObject.name);
		selectedTab = tabType;
		if (friendsListTabButton.selectedSprites != null)
		{
			friendsListTabButton.selectedSprites.ForEach((SpriteRenderer s) =>
			{
				s.enabled = true;
			});
		}
		ControllerManager.Instance.ClearDestroyedSelectableUiElements();
	}

	public void Close(bool force = false)
	{
		if (!force && ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick && (Tabs[selectedTab].tab == FriendsListTab.AddFriend || Tabs[selectedTab].tab == FriendsListTab.Notifications))
		{
			OpenTab(2);
		}
		else
		{
			gameObject.SetActive(value: false);
		}
	}

	public virtual void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	private void Update()
	{
		if (ShipStatus.Instance != null)
		{
			Close();
		}
		else
		{
			if (ControllerManager.Instance.CurrentUiState.MenuName != name)
			{
				return;
			}
			if (addFriendCooldown > 0f)
			{
				addFriendCooldown -= Time.unscaledDeltaTime;
			}
			Player player = ReInput.players.GetPlayer(0);
			if (!Tabs[selectedTab].selectable)
			{
				return;
			}
			int num = ((!LobbyPlayersTab.activeSelf) ? 1 : 0);
			if (selectedTab > num && player.GetButtonDown(35))
			{
				CloseTab();
				selectedTab--;
				OpenTab(selectedTab);
			}
			if (selectedTab < Tabs.Length - 1 && player.GetButtonDown(34) && Tabs[selectedTab + 1].selectable)
			{
				CloseTab();
				selectedTab++;
				OpenTab(selectedTab);
			}
			if (PlatformFriendsButton.activeSelf && selectedTab == 2 && player.GetButtonDown(29))
			{
				if (viewingAllFriends)
				{
					ViewPlatformFriends();
				}
				else
				{
					ViewAllFriends();
				}
			}
		}
	}

	private void CloseTab()
	{
		if (Tabs[selectedTab] == null)
		{
			return;
		}
		if (Tabs[selectedTab].selectedSprites != null)
		{
			Tabs[selectedTab].selectedSprites.ForEach((SpriteRenderer s) =>
			{
				s.enabled = false;
			});
		}
		Tabs[selectedTab].tabObject.SetActive(value: false);
	}

	public void AddFriend()
	{
		if (AddFriendArea.text.Trim() == string.Empty || addFriendCooldown > 0f)
		{
			return;
		}
		addFriendCooldown = 1f;
		DestroyableSingleton<FriendsListManager>.Instance.SendFriendRequestByUsername(AddFriendArea.text.Trim(), (ResponseState cb, Response<ResponseFriendsListRequest> response) =>
		{
			if (cb == ResponseState.Success)
			{
				DestroyableSingleton<FriendsListManager>.Instance.FriendListNotification.SetUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FriendRequestSent), FriendListNotification.IconType.Sent);
			}
			else if (response == null)
			{
				DestroyableSingleton<FriendsListManager>.Instance.FriendListNotification.SetUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FriendRequestSentFailed), FriendListNotification.IconType.Failed);
			}
			else if (response.Errors != null && response.Errors.Length != 0 && DestroyableSingleton<FriendsListManager>.Instance.ErrorCodes.ContainsKey(response.Errors[0].Detail))
			{
				DestroyableSingleton<FriendsListManager>.Instance.FriendListNotification.SetUp(DestroyableSingleton<TranslationController>.Instance.GetString(DestroyableSingleton<FriendsListManager>.Instance.ErrorCodes[response.Errors[0].Detail]), FriendListNotification.IconType.Failed);
			}
			else
			{
				DestroyableSingleton<FriendsListManager>.Instance.FriendListNotification.SetUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FriendRequestSentFailed), FriendListNotification.IconType.Failed);
			}
		});
	}

	public void RefreshFriends()
	{
		for (int i = 0; i < DestroyableSingleton<FriendsListManager>.Instance.Friends.Count; i++)
		{
			float y = YStart - (float)i * YOffset;
			string friendPuid = DestroyableSingleton<FriendsListManager>.Instance.Friends[i].FriendPuid;
			OnlineFriendBar onlineFriendBar = UnityEngine.Object.Instantiate(OnlineFriendBar, FriendArea.transform);
			onlineFriendBar.SetUp(friendPuid, this, DestroyableSingleton<FriendsListManager>.Instance.Friends[i].FriendCode, DataManager.Player.Friends.GetCachedName(friendPuid));
			onlineFriendBar.transform.localPosition = new Vector3(-0.29f, y, -1f);
			onlineFriendBar.Buttons.ForEach((PassiveButton b) =>
			{
				b.ClickMask = FriendsScroller.Hitbox;
			});
			onlineFriendBar.GetAndSetPlatform();
			friendBars.Add(onlineFriendBar);
		}
		foreach (FriendsListBar friendBar in friendBars)
		{
			foreach (PassiveButton item in friendBar.ControllerSelectable)
			{
				ControllerManager.Instance.AddSelectableUiElement(item);
			}
		}
		FriendsScroller.SetYBoundsMax(0f - (YStart - (float)DestroyableSingleton<FriendsListManager>.Instance.Friends.Count * YOffset));
	}

	public void RefreshPlatformFriends()
	{
		for (int i = 0; i < DestroyableSingleton<FriendsListManager>.Instance.PlatformFriends.Count; i++)
		{
			float y = YStart - (float)i * YOffset;
			OnlineFriendBar onlineFriendBar = UnityEngine.Object.Instantiate(PlatformFriendBar, PlatformFriendArea.transform);
			onlineFriendBar.SetUp(null, this, DestroyableSingleton<FriendsListManager>.Instance.PlatformFriends[i].Username, null);
			onlineFriendBar.PlatformFriendData = DestroyableSingleton<FriendsListManager>.Instance.PlatformFriends[i];
			onlineFriendBar.transform.localPosition = new Vector3(-0.29f, y, -1f);
			onlineFriendBar.Buttons.ForEach((PassiveButton b) =>
			{
				b.ClickMask = PlatformFriendsScroller.Hitbox;
			});
			onlineFriendBar.SetPlatform(Constants.GetPlatformType(), "");
			platformFriendBars.Add(onlineFriendBar);
		}
		foreach (FriendsListBar platformFriendBar in platformFriendBars)
		{
			foreach (PassiveButton item in platformFriendBar.ControllerSelectable)
			{
				ControllerManager.Instance.AddSelectableUiElement(item);
			}
		}
		PlatformFriendsScroller.SetYBoundsMax(0f - (YStart - (float)DestroyableSingleton<FriendsListManager>.Instance.PlatformFriends.Count * YOffset));
	}

	public void RefreshBlockedPlayers()
	{
		for (int i = 0; i < DestroyableSingleton<FriendsListManager>.Instance.BlockedPlayers.Count; i++)
		{
			float y = YStart - (float)i * YOffset;
			BlockedPlayerBar blockedPlayerBar = UnityEngine.Object.Instantiate(BlockedPlayerBar, BlockedArea.transform);
			blockedPlayerBar.SetUp(DestroyableSingleton<FriendsListManager>.Instance.BlockedPlayers[i].FriendPuid, this, DestroyableSingleton<FriendsListManager>.Instance.BlockedPlayers[i].FriendCode, null);
			blockedPlayerBar.transform.localPosition = new Vector3(-0.29f, y, -1f);
			blockedPlayerBar.Buttons.ForEach((PassiveButton b) =>
			{
				b.ClickMask = BlockedScroller.Hitbox;
			});
			blockedPlayerBar.GetAndSetPlatform();
			friendBars.Add(blockedPlayerBar);
		}
		BlockedScroller.SetYBoundsMax(0f - (YStart - (float)DestroyableSingleton<FriendsListManager>.Instance.BlockedPlayers.Count * YOffset));
	}

	private void ClearNotifs()
	{
		foreach (FriendsListBar notifBar in notifBars)
		{
			if (notifBar == null)
			{
				continue;
			}
			foreach (PassiveButton item in notifBar.ControllerSelectable)
			{
				ControllerManager.Instance.RemoveSelectableUiElement(item);
			}
			UnityEngine.Object.DestroyImmediate(notifBar.gameObject);
		}
		notifBars.Clear();
	}

	public void RefreshNotifications()
	{
		ClearNotifs();
		if (!DestroyableSingleton<EOSManager>.Instance.IsFriendsListAllowed())
		{
			return;
		}
		for (int i = 0; i < DestroyableSingleton<FriendsListManager>.Instance.ReceivedRequests.Count; i++)
		{
			float y = YStart - (float)i * YOffset;
			FriendRequestBar friendRequestBar = UnityEngine.Object.Instantiate(FriendRequestBar, NotifArea.transform);
			friendRequestBar.SetUp(DestroyableSingleton<FriendsListManager>.Instance.ReceivedRequests[i].SenderId, this, DestroyableSingleton<FriendsListManager>.Instance.ReceivedRequests[i].SenderFriendCode, null);
			friendRequestBar.transform.localPosition = new Vector3(-0.29f, y, -1f);
			friendRequestBar.Buttons.ForEach((PassiveButton b) =>
			{
				b.ClickMask = NotifScroller.Hitbox;
			});
			notifBars.Add(friendRequestBar);
		}
		for (int num = 0; num < DestroyableSingleton<FriendsListManager>.Instance.ReceivedLobbyInvites.Count; num++)
		{
			float y2 = YStart - (float)(num + DestroyableSingleton<FriendsListManager>.Instance.ReceivedRequests.Count) * YOffset;
			LobbyInviteBar lobbyInviteBar = UnityEngine.Object.Instantiate(LobbyInviteBar, NotifArea.transform);
			lobbyInviteBar.SetUp(DestroyableSingleton<FriendsListManager>.Instance.ReceivedLobbyInvites[num].SenderId, this, DestroyableSingleton<FriendsListManager>.Instance.ReceivedLobbyInvites[num].RoomCode, DestroyableSingleton<FriendsListManager>.Instance.ReceivedLobbyInvites[num].SenderFriendCode, null);
			lobbyInviteBar.transform.localPosition = new Vector3(-0.29f, y2, -1f);
			lobbyInviteBar.Buttons.ForEach((PassiveButton b) =>
			{
				b.ClickMask = NotifScroller.Hitbox;
			});
			notifBars.Add(lobbyInviteBar);
		}
		int num2 = DestroyableSingleton<FriendsListManager>.Instance.ReceivedLobbyInvites.Count + DestroyableSingleton<FriendsListManager>.Instance.ReceivedRequests.Count;
		if (num2 > 0)
		{
			ViewRequestsButton.color = NewRequestColor;
			ViewRequestsText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NewRequests, num2);
		}
		else
		{
			ViewRequestsButton.color = NoRequestsColor;
			ViewRequestsText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NoNewRequests);
		}
		NotifScroller.SetYBoundsMax(0f - (YStart - (float)num2 * YOffset));
		foreach (FriendsListBar notifBar in notifBars)
		{
			foreach (PassiveButton item in notifBar.ControllerSelectable)
			{
				ControllerManager.Instance.AddSelectableUiElement(item);
			}
		}
	}

	public void RefreshLobbyPlayers()
	{
		if (GameData.Instance == null || GameData.Instance.AllPlayers.Count == 0)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < GameData.Instance.AllPlayers.Count; i++)
		{
			NetworkedPlayerInfo player = GameData.Instance.AllPlayers[i];
			if (player.PlayerId != PlayerControl.LocalPlayer.PlayerId)
			{
				float y = YStart - (float)num * YOffset;
				LobbyPlayerBar lobbyPlayerBar = UnityEngine.Object.Instantiate(LobbyPlayerBar, LobbyPlayerArea.transform);
				FriendsListManager.RecentPlayedWithPlayer recentPlayer = new FriendsListManager.RecentPlayedWithPlayer(player, AmongUsClient.Instance.GameId, $"{(uint)IPAddress.Parse(AmongUsClient.Instance.GetNetworkAddress()).Address}:{AmongUsClient.Instance.GetNetworkPort()}");
				lobbyPlayerBar.SetRecentPlayer(recentPlayer);
				lobbyPlayerBar.SetUp(player.Puid, this, player.FriendCode, player.PlayerName);
				lobbyPlayerBar.transform.localPosition = new Vector3(-0.29f, y, -1f);
				lobbyPlayerBar.Buttons.ForEach((PassiveButton b) =>
				{
					b.ClickMask = LobbyScroller.Hitbox;
				});
				PlayerControl playerControl = PlayerControl.AllPlayerControls.Find((PlayerControl pc) => pc.PlayerId == player.PlayerId);
				if ((bool)playerControl)
				{
					ClientData client = AmongUsClient.Instance.GetClient(playerControl.OwnerId);
					lobbyPlayerBar.SetInfoFromClient(client);
				}
				else
				{
					lobbyPlayerBar.SetInfoFromClient(null);
				}
				lobbyBars.Add(lobbyPlayerBar);
				num++;
			}
		}
		foreach (LobbyPlayerBar lobbyBar in lobbyBars)
		{
			foreach (PassiveButton item in lobbyBar.ControllerSelectable)
			{
				ControllerManager.Instance.AddSelectableUiElement(item);
			}
		}
		LobbyScroller.SetYBoundsMax(0f - (YStart - (float)GameData.Instance.AllPlayers.Count * YOffset));
	}

	public void RefreshRecentlyPlayed()
	{
		int num = 0;
		for (int i = 0; i < DestroyableSingleton<FriendsListManager>.Instance.RecentlyPlayedWith.Count; i++)
		{
			FriendsListManager.RecentPlayedWithPlayer recentPlayedWithPlayer = DestroyableSingleton<FriendsListManager>.Instance.RecentlyPlayedWith[i];
			if (!(recentPlayedWithPlayer.PlayerName == DataManager.Player.Customization.Name))
			{
				float y = YStart - (float)num * YOffset;
				LobbyPlayerBar lobbyPlayerBar = UnityEngine.Object.Instantiate(LobbyPlayerBar, RecentlyPlayedArea.transform);
				lobbyPlayerBar.SetRecentPlayer(recentPlayedWithPlayer);
				lobbyPlayerBar.SetUp(recentPlayedWithPlayer.Puid, this, recentPlayedWithPlayer.FriendCode, recentPlayedWithPlayer.PlayerName);
				lobbyPlayerBar.transform.localPosition = new Vector3(-0.29f, y, -1f);
				lobbyPlayerBar.Buttons.ForEach((PassiveButton b) =>
				{
					b.ClickMask = RecentlyPlayedScroller.Hitbox;
				});
				lobbyPlayerBar.GetAndSetPlatform();
				lobbyBars.Add(lobbyPlayerBar);
				num++;
			}
		}
		RecentlyPlayedScroller.SetYBoundsMax(0f - (YStart - (float)DestroyableSingleton<FriendsListManager>.Instance.RecentlyPlayedWith.Count * YOffset));
	}

	public void UpdatedReportedPlayers()
	{
		if (lobbyBars == null)
		{
			return;
		}
		foreach (LobbyPlayerBar lobbyBar in lobbyBars)
		{
			lobbyBar.CheckAlreadyReported();
		}
	}

	public void UpdateFriendBars()
	{
		foreach (FriendsListBar friendBar in friendBars)
		{
			if (friendBar == null)
			{
				continue;
			}
			foreach (PassiveButton item in friendBar.ControllerSelectable)
			{
				ControllerManager.Instance.RemoveSelectableUiElement(item);
			}
			UnityEngine.Object.DestroyImmediate(friendBar.gameObject);
		}
		friendBars.Clear();
		RefreshBlockedPlayers();
		RefreshFriends();
	}

	public void GiveFocus()
	{
		AddFriendArea.GiveFocus();
	}

	public void ToggleFriendCode()
	{
		if (DataManager.Player.Account.LoginStatus == EOSManager.AccountLoginStatus.LoggedIn)
		{
			DestroyableSingleton<FriendsListManager>.Instance.FriendCodeHidden = !DestroyableSingleton<FriendsListManager>.Instance.FriendCodeHidden;
			UpdateFriendCodeUI();
		}
	}

	public void UpdateFriendCodeUI()
	{
		if (DestroyableSingleton<EOSManager>.Instance.IsFriendsListAllowed())
		{
			FriendCodeHideToggleObject.SetActive(value: true);
			FriendCodeText.enabled = true;
			FriendCodeHiddenIcon.enabled = DestroyableSingleton<FriendsListManager>.Instance.FriendCodeHidden;
			if (DestroyableSingleton<FriendsListManager>.Instance.FriendCodeHidden)
			{
				FriendCodeText.text = friendCodeHiddenText;
			}
			else
			{
				FriendCodeText.text = DestroyableSingleton<EOSManager>.Instance.FriendCode;
			}
		}
		else
		{
			FriendCodeText.enabled = false;
			FriendCodeHideToggleObject.SetActive(value: false);
		}
	}

	public void ViewAllFriends()
	{
		InactiveAllFriends.enabled = false;
		InactivePlatformFriends.enabled = true;
		viewingAllFriends = true;
		PlatformFriendArea.SetActive(value: false);
		FriendArea.SetActive(value: true);
		FriendsScroller.gameObject.SetActive(value: true);
		PlatformFriendsScroller.gameObject.SetActive(value: false);
		if (!ControllerManager.Instance.CurrentUiState.CurrentSelection.isActiveAndEnabled && friendBars.Count > 0 && friendBars[0].Buttons.Count > 0)
		{
			ControllerManager.Instance.SetCurrentSelected(friendBars[0].Buttons[0]);
		}
	}

	public void ViewPlatformFriends()
	{
		InactiveAllFriends.enabled = true;
		InactivePlatformFriends.enabled = false;
		viewingAllFriends = false;
		PlatformFriendArea.SetActive(value: true);
		FriendArea.SetActive(value: false);
		FriendsScroller.gameObject.SetActive(value: false);
		PlatformFriendsScroller.gameObject.SetActive(value: true);
		if (!ControllerManager.Instance.CurrentUiState.CurrentSelection.isActiveAndEnabled && platformFriendBars.Count > 0 && platformFriendBars[0].Buttons.Count > 0)
		{
			ControllerManager.Instance.SetCurrentSelected(platformFriendBars[0].Buttons[0]);
		}
	}

	public void ResendGuardianEmail()
	{
		DestroyableSingleton<EOSManager>.Instance.ResendGuardianEmail();
	}

	public void SortSamePlatformToTop(List<FriendsListBar> friendBars)
	{
		Constants.GetPlatformType();
		friendBars.Sort(ComparePreferSamePlatform);
		int num = 0;
		for (int i = 0; i < friendBars.Count; i++)
		{
			float y = YStart - (float)num * YOffset;
			friendBars[num].transform.localPosition = new Vector3(-0.29f, y, -1f);
			num++;
		}
		static int ComparePreferSamePlatform(FriendsListBar a, FriendsListBar b)
		{
			bool flag = a.PlatformFriendData != null;
			bool flag2 = b.PlatformFriendData != null;
			if (flag)
			{
				if (flag2)
				{
					return 0;
				}
				return -1;
			}
			if (flag2)
			{
				return 1;
			}
			return 0;
		}
	}
}
