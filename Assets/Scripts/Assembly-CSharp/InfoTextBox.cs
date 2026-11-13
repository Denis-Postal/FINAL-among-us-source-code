using Object = UnityEngine.Object;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoTextBox : MonoBehaviour
{
	public SpriteRenderer background;

	public bool isConfirmWindow;

	public bool AutoSetOneButton = true;

	public TextMeshPro titleTexxt;

	public TextMeshPro bodyText;

	public TextMeshPro button1Text;

	public TextMeshPro button2Text;

	public PassiveButton button1;

	public PassiveButton button2;

	public Transform button1Trans;

	public Transform button2Trans;

	private List<SelectableHyperLink> selectableHyperLinks= new List<SelectableHyperLink>();

	public unsafe void Awake()
	{
		if (AutoSetOneButton)
		{
			SetOneButton();
		}
		if (isConfirmWindow)
		{
			SetConfirmWindow();
		}
	}

	public void Close()
	{
		GetComponent<TransitionOpen>().Close();
	}

	public unsafe void SetConfirmWindow()
	{
		background.color = Color.green;
	}

	public unsafe void SetTwoButtons()
	{
		button2Trans.gameObject.SetActive(value: true);
		button1Trans.localPosition = new Vector2(2f, button1Trans.localPosition.y);
		button2Trans.localPosition = new Vector2(-2f, button2Trans.localPosition.y);
	}

	public unsafe void SetOneButton()
	{
		button2Trans.gameObject.SetActive(value: false);
		button1Trans.localPosition = new Vector2(0f, button1Trans.localPosition.y);
	}

	public void Update()
	{
		GameObject gameObject = base.gameObject;
		string menuName = gameObject.name;
		if (!ControllerManager.Instance.IsMenuActiveAtAll(menuName))
		{
			ControllerNavMenu component = GetComponent<ControllerNavMenu>();
			Debug.Log("EOS: InfoTextBox overlay menu wasn't active, opening it...");
			component.OpenMenu(force: true);
		}
	}

	public void SetText(string text)
	{
		bodyText.text = text;
		SelectableHyperLinkHelper.AddSelectableUiForHyperlinks(selectableHyperLinks, name);
		OnTextUpdated();
	}

	public void OnTextUpdated()
	{
		bodyText.text = SelectableHyperLinkHelper.DecomposeAnnouncementText(bodyText, selectableHyperLinks, name, bodyText.text);
		SelectableHyperLinkHelper.UpdateHyperlinkPositions(bodyText, selectableHyperLinks, name);
	}

	public InfoTextBox()
	{ }
}
