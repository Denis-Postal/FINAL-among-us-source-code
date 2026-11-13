using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

internal class SelectableHyperLinkHelper : MonoBehaviour
{
	private static readonly Regex linkReg = new Regex("(\\[http[^>]*?\\])|(\\[\\])");

	public static void SanitizeLinkText(string fullText, out string result)
	{
		result = fullText;
		if (linkReg.IsMatch(fullText))
		{
			result = linkReg.Replace(result, string.Empty);
		}
	}

	public static void ExtractUrl(string fullText, out string url, out string visibleTextPart)
	{
		url = "";
		visibleTextPart = "";
		string value = "<color=#";
		string value2 = "</color>";
		int num = fullText.IndexOf(value);
		int num2 = fullText.IndexOf(value2);
		if (num != -1 && num != -1)
		{
			fullText = fullText[15..num2];
		}
		else if (num != -1 || num != -1)
		{
			Debug.LogError("ERROR: malformed markup tags on URL:[[" + fullText + "]]");
		}
		fullText = fullText.Replace("<nobr>", string.Empty);
		fullText = fullText.Replace("</nobr>", string.Empty);
		value = "<link=";
		value2 = ">";
		string value3 = "</link>";
		num = fullText.IndexOf(value);
		num2 = fullText.IndexOf(value2);
		if (num == -1)
		{
			value = "[";
			value2 = "]";
			value3 = "[";
			num = fullText.IndexOf(value);
			num2 = fullText.IndexOf(value2);
		}
		_ = fullText.Substring(num + value.Length).Length;
		_ = value.Length;
		url = fullText.Substring(num + value.Length, num2 - value.Length);
		string text = fullText.Substring(num2 + 1);
		int num3 = text.IndexOf(value3);
		if (num3 != -1)
		{
			visibleTextPart = text.Substring(0, num3);
		}
		if (url[0] == '"')
		{
			url = url.Substring(1);
		}
		if (url[url.Length - 1] == '"')
		{
			url = url.Substring(0, url.Length - 1);
		}
	}

	public static void DecomposeHyperlinkText(string textToDecompose, List<ITextPart> textConstituents, List<SelectableHyperLink> selectableHyperLinks)
	{
		if (string.IsNullOrEmpty(textToDecompose))
		{
			return;
		}
		selectableHyperLinks.Clear();
		textConstituents.Clear();
		while (textToDecompose.Length > 0)
		{
			ITextPart textPart = null;
			string nonUrlPart = "";
			string urlPart = "";
			GetFirstUrl(textToDecompose, out nonUrlPart, out urlPart);
			if (!string.IsNullOrEmpty(nonUrlPart))
			{
				textPart = new TextPart(nonUrlPart);
				textConstituents.Add(textPart);
			}
			if (!string.IsNullOrEmpty(urlPart))
			{
				SelectableHyperLink component = UnityEngine.Object.Instantiate(Resources.Load("SelectableHyperlink") as GameObject).GetComponent<SelectableHyperLink>();
				selectableHyperLinks.Add(component);
				component.LinkText = urlPart;
				textConstituents.Add(component);
			}
			int startIndex = Math.Min(textToDecompose.Length, nonUrlPart.Length + urlPart.Length + 1);
			textToDecompose = textToDecompose.Substring(startIndex);
		}
	}

	private static void GetFirstUrl(string textToDecompose, out string nonUrlPart, out string urlPart)
	{
		nonUrlPart = "";
		urlPart = "";
		if (!string.IsNullOrEmpty(textToDecompose))
		{
			string value = "<color=";
			string text = "</color>";
			int num = textToDecompose.IndexOf(value);
			int num2 = textToDecompose.IndexOf(text);
			if (num == -1)
			{
				value = "<link=";
				text = "</link>";
				num = textToDecompose.IndexOf(value);
				num2 = textToDecompose.IndexOf(text);
			}
			if (num == -1)
			{
				value = "[http";
				text = "[]";
				num = textToDecompose.IndexOf(value);
				num2 = textToDecompose.IndexOf(text);
			}
			if (num == -1)
			{
				nonUrlPart = textToDecompose;
				return;
			}
			_ = text.Length;
			nonUrlPart = textToDecompose.Substring(0, num);
			urlPart = textToDecompose.Substring(num, num2 + text.Length - num);
		}
	}

	public static void AddSelectableUiForHyperlinks(List<SelectableHyperLink> selectableHyperLinks, string menuName)
	{
		if (selectableHyperLinks.Count <= 0)
		{
			return;
		}
		ControllerUiElementsState menu = ControllerManager.Instance.GetMenu(menuName);
		if (menu == null)
		{
			return;
		}
		foreach (SelectableHyperLink selectableHyperLink in selectableHyperLinks)
		{
			selectableHyperLink.RemoveUiElement(menu);
		}
		foreach (SelectableHyperLink selectableHyperLink2 in selectableHyperLinks)
		{
			selectableHyperLink2.AddUiElement(menu);
		}
		PassiveButton component = selectableHyperLinks[0].GetComponent<PassiveButton>();
		if (ControllerManager.Instance.CurrentUiState.MenuName == menuName)
		{
			ControllerManager.Instance.SetCurrentSelected(component);
		}
	}

	public static void DestroyGOs(List<SelectableHyperLink> selectableHyperLinks, string menuName)
	{
		ControllerUiElementsState menu = ControllerManager.Instance.GetMenu(menuName);
		foreach (SelectableHyperLink selectableHyperLink in selectableHyperLinks)
		{
			if (menu != null)
			{
				selectableHyperLink.RemoveUiElement(menu);
			}
			UnityEngine.Object.Destroy(selectableHyperLink.gameObject);
		}
		selectableHyperLinks.Clear();
	}

	public static void UpdateHyperlinkPositions(TextMeshPro tmp, List<SelectableHyperLink> selectableHyperLinks, string menuName)
	{
		new List<Vector3>(3);
		new List<Vector3>(3);
		try
		{
			tmp.ForceMeshUpdate();
			tmp.UpdateVertexData();
			List<SelectableHyperLink> list = new List<SelectableHyperLink>(3);
			list.Clear();
			List<string> list2 = new List<string>();
			int num = 0;
			foreach (SelectableHyperLink selectableHyperLink in selectableHyperLinks)
			{
				if ((bool)selectableHyperLink)
				{
					int num2 = 0;
					if (tmp.GetLinkPosition(num, out var bottomLeft, out var topRight))
					{
						list2.Add(selectableHyperLink.TaglessLinkText);
						if (num2 == 0)
						{
							Vector3 localPosition = (topRight + bottomLeft) * 0.5f;
							Vector3 localScale = topRight - bottomLeft + new Vector3(0.4f, 0.1f, 0f);
							localPosition.z = selectableHyperLink.transform.localPosition.z;
							selectableHyperLink.transform.localPosition = localPosition;
							selectableHyperLink.transform.localScale = localScale;
						}
						else
						{
							list.Add(selectableHyperLink);
						}
					}
					else
					{
						Debug.LogError("Failed to find text '" + selectableHyperLink.TaglessLinkText + "'");
					}
				}
				num++;
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Failed to update announcement hyperlink positions");
			Debug.LogException(exception);
		}
	}

	public static string DecomposeAnnouncementText(TextMeshPro tmp, List<SelectableHyperLink> selectableHyperLinks, string menuName, string text)
	{
		DestroyGOs(selectableHyperLinks, menuName);
		List<ITextPart> textConstituents = new List<ITextPart>();
		DecomposeHyperlinkText(text, textConstituents, selectableHyperLinks);
		int num = 1;
		foreach (SelectableHyperLink selectableHyperLink in selectableHyperLinks)
		{
			selectableHyperLink.transform.SetParent(tmp.transform, worldPositionStays: false);
			selectableHyperLink.SetSelectionCallback(() =>
			{
				tmp.text = RecomposeAnnouncementText(textConstituents);
			});
			num++;
		}
		AddSelectableUiForHyperlinks(selectableHyperLinks, menuName);
		return RecomposeAnnouncementText(textConstituents);
	}

	public static string RecomposeAnnouncementText(List<ITextPart> textConstituents)
	{
		string text = "";
		foreach (ITextPart textConstituent in textConstituents)
		{
			text += textConstituent.GetText();
		}
		text = text.Replace("<link", "<nobr><link");
		return text.Replace("</link>", "</nobr></link>");
	}
}
