using System.Collections;
using System.Collections.Generic;
using InnerNet;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReportReasonScreen : MonoBehaviour
{
	public BanMenu Parent;

	public ButtonRolloverHandler[] Buttons;

	public List<UiElement> uiButtons;

	public TextMeshPro NameText;

	public SpriteRenderer PlayerIcon;

	private ReportReasons? currentReason;

	public ReportNotification ConfirmScreen;

	public bool isFriendListReport;

	public PassiveUiElement closeButton;

	public PassiveUiElement badNameButton;

	private string playerName;

	private int colorId;

	private readonly string MENU_NAME = "ReportMenu";

	public void Show(string playerName, int colorId, bool fromFriends = false)
	{
		Vector3 position = transform.position;
		if (!fromFriends)
		{
			Vector3 vector = DestroyableSingleton<HudManager>.Instance.UICamera.transform.TransformPoint(Vector3.zero);
			position.x = vector.x;
			position.y = vector.y;
			transform.position = position;
		}
		this.playerName = playerName;
		this.colorId = colorId;
		NameText.text = playerName;
		if (!fromFriends)
		{
			PlayerMaterial.SetColors(colorId, PlayerIcon);
		}
		else
		{
			PlayerIcon.gameObject.SetActive(value: false);
		}
		currentReason = null;
		gameObject.SetActive(value: true);
		if (isFriendListReport)
		{
			StartCoroutine(AsignControllerMenuCo());
		}
		for (int i = 0; i < Buttons.Length; i++)
		{
			Buttons[i].ChangeOutColor(Color.white);
		}
	}

	private IEnumerator AsignControllerMenuCo()
	{
		yield return new WaitForEndOfFrame();
		ControllerManager.Instance.OpenOverlayMenu(MENU_NAME, closeButton, badNameButton, uiButtons);
	}

	public void SelectReason(ReportReason reason)
	{
		if (reason.Reason == ReportReasons.None)
		{
			currentReason = null;
		}
		else
		{
			currentReason = reason.Reason;
		}
		for (int i = 0; i < Buttons.Length; i++)
		{
			Buttons[i].ChangeOutColor((Buttons[i] == reason.button) ? Color.green : Color.white);
		}
	}

	public void Submit()
	{
		if (currentReason.HasValue)
		{
			if (isFriendListReport)
			{
				DestroyableSingleton<FriendsListManager>.Instance.GetReportInfo().reason = currentReason.Value;
				DestroyableSingleton<FriendsListManager>.Instance.StartReport();
			}
			else
			{
				Parent.ReportPlayer(currentReason.Value);
			}
			if (SceneManager.GetActiveScene().name == "OnlineGame")
			{
				Object.Instantiate(ConfirmScreen, DestroyableSingleton<HudManager>.Instance.gameObject.transform);
			}
			else
			{
				Object.Instantiate(ConfirmScreen, DestroyableSingleton<FriendsListManager>.Instance.gameObject.transform).GetComponent<AspectPosition>().DistanceFromEdge.z = -900f;
			}
			Hide();
		}
	}

	public void Hide()
	{
		if (isFriendListReport)
		{
			ControllerManager.Instance.CloseOverlayMenu(MENU_NAME);
		}
		gameObject.SetActive(value: false);
	}
}
