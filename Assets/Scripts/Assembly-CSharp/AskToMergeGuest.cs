using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class AskToMergeGuest : MonoBehaviour
{
	public PassiveButton GoAheadButton;

	public PassiveButton NotRightNowButton;

	public TextMeshPro InfoText;

	private List<SelectableHyperLink> selectableHyperLinks= new List<SelectableHyperLink>();

	private bool accountIDShow;

	public void OnClose()
	{
		gameObject.SetActive(value: false);
	}

	public void Start()
	{
		InfoText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MergeGuestAccountText, Constants.GetCurrentPlatformName());
		SelectableHyperLinkHelper.AddSelectableUiForHyperlinks(selectableHyperLinks, name);
		OnTextUpdated();
	}

	public void OnTextUpdated()
	{
		InfoText.text = SelectableHyperLinkHelper.DecomposeAnnouncementText(InfoText, selectableHyperLinks, name, InfoText.text);
		SelectableHyperLinkHelper.UpdateHyperlinkPositions(InfoText, selectableHyperLinks, name);
	}

	public AskToMergeGuest()
	{ }
}
