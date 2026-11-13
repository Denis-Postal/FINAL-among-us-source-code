using System;
using System.IO;
using PolyAndCode.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public class DebugLogMessageLine : MonoBehaviour, ICell
	{
		[SerializeField]
		private Button expandButton;

		[SerializeField]
		private Image fader;

		[SerializeField]
		private TextMeshProUGUI logLine;

		[SerializeField]
		private Image background;

		[SerializeField]
		private Image warningLogIcon;

		[SerializeField]
		private Image regularLogIcon;

		[SerializeField]
		private Image errorLogIcon;

		[SerializeField]
		private Color fadeColor;

		[SerializeField]
		private Color evenColor;

		[SerializeField]
		private Color oddColor;

		[SerializeField]
		private Color highlightColor;

		public int Index { get; private set; }

		public void Initialize(DebugLogMessage debugLog, Action<int, DebugLogMessage> onClick, int index)
		{
			using (StringReader stringReader = new StringReader(debugLog.LogString))
			{
				logLine.SetText(stringReader.ReadLine());
			}
			expandButton.onClick.RemoveAllListeners();
			expandButton.onClick.AddListener(() =>
			{
				onClick(index, debugLog);
			});
			background.color = ((index % 2 == 0) ? evenColor : oddColor);
			Index = index;
			ToggleFade(faded: false);
			errorLogIcon.gameObject.SetActive(value: false);
			regularLogIcon.gameObject.SetActive(value: false);
			warningLogIcon.gameObject.SetActive(value: false);
			switch (debugLog.LogType)
			{
			case LogType.Warning:
				warningLogIcon.gameObject.SetActive(value: true);
				break;
			case LogType.Log:
				regularLogIcon.gameObject.SetActive(value: true);
				break;
			default:
				errorLogIcon.gameObject.SetActive(value: true);
				break;
			}
		}

		public void ToggleFade(bool faded)
		{
			fader.color = (faded ? fadeColor : Color.clear);
		}

		public void ToggleHighlight(bool highlighted)
		{
			if (highlighted)
			{
				background.color = highlightColor;
			}
			else
			{
				background.color = ((Index % 2 == 0) ? evenColor : oddColor);
			}
		}
	}
}
