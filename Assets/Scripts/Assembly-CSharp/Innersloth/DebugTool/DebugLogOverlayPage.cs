using System.Collections;
using System.Collections.Generic;
using System.Text;
using PolyAndCode.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public class DebugLogOverlayPage : DebugToolOverlayPage, IRecyclableScrollRectDataSource
	{
		private readonly Dictionary<LogType, DebugLogFilterButton> logButtons = new Dictionary<LogType, DebugLogFilterButton>();

		[SerializeField]
		private RecyclableScrollRect logScrollList;

		[SerializeField]
		private RectTransform idealLogPosition;

		[SerializeField]
		private DebugLogOverlayPageListSelector listSelector;

		[SerializeField]
		[Header("Bottom Section")]
		private Button clearButton;

		[SerializeField]
		private Button copyButton;

		[SerializeField]
		private DebugLogFilterButton logsButton;

		[SerializeField]
		private DebugLogFilterButton warningsButton;

		[SerializeField]
		private DebugLogFilterButton errorsButton;

		[SerializeField]
		[Header("Log Details")]
		private GameObject detailParent;

		[SerializeField]
		private TextMeshProUGUI detailsText;

		[SerializeField]
		private GameObject notListeningWarning;

		private string detailsToCopy;

		private readonly List<DebugLogMessage> logMessageList = new List<DebugLogMessage>();

		private bool initialized;

		private int selectedLogIndex;

		public override string Name => "Debug Logs";

		public override bool ConsumingNavActions => false;

		private void Awake()
		{
			listSelector.OnSelected += OnListSelected;
			listSelector.OnDeselected += OnListDeselected;
			clearButton.onClick.AddListener(ClearLogs);
			copyButton.onClick.AddListener(CopyLogOrLogs);
		}

		private void Start()
		{
			EventSystem.current.SetSelectedGameObject(listSelector.gameObject);
		}

		private void Update()
		{
			foreach (IDebugToolInputProvider inputProvider in InputProviders)
			{
				int num = selectedLogIndex;
				if (inputProvider.GetNavDown())
				{
					selectedLogIndex = Mathf.Clamp(selectedLogIndex + 1, 0, logMessageList.Count - 1);
				}
				else if (inputProvider.GetNavUp())
				{
					selectedLogIndex = Mathf.Clamp(selectedLogIndex - 1, 0, logMessageList.Count - 1);
				}
				if (selectedLogIndex != num)
				{
					HighlightLog(selectedLogIndex);
					if (detailParent.gameObject.activeSelf)
					{
						ShowLogDetails(logMessageList[selectedLogIndex]);
						FadeOtherLogs(selectedLogIndex);
					}
					UpdateScrollRectPosition(num, selectedLogIndex);
					break;
				}
				if (inputProvider.GetConfirm() && (listSelector.IsSelected || listSelector.IsChildSelected()))
				{
					HighlightLog(selectedLogIndex);
					FadeOtherLogs(selectedLogIndex);
					ShowLogDetails(logMessageList[selectedLogIndex]);
					break;
				}
				if (inputProvider.GetCancel())
				{
					HideDetails();
					break;
				}
				if (inputProvider.GetCopy())
				{
					CopyLogOrLogs();
					break;
				}
				if (inputProvider.GetClear())
				{
					ClearLogs();
					break;
				}
			}
		}

		private void UpdateScrollRectPosition(int originalSelectedIndex, int newlySelectedIndex)
		{
			float num = (float)(newlySelectedIndex - originalSelectedIndex) * logScrollList.PrototypeCell.rect.height;
			if (num < 0f)
			{
				num = Mathf.Clamp(num, 0f - logScrollList.content.anchoredPosition.y, 0f);
			}
			if (num > 0f)
			{
				num = Mathf.Clamp(num, 0f, logScrollList.viewport.rect.height / 2f - logScrollList.content.anchoredPosition.y);
			}
			Vector3[] array = new Vector3[4];
			idealLogPosition.GetWorldCorners(array);
			Rect rect = new Rect(array[0], array[2] - array[0]);
			DebugLogMessageLine debugLogMessageLine = FindLogObject(newlySelectedIndex);
			Vector3[] array2 = new Vector3[4];
			((RectTransform)debugLogMessageLine.transform).GetWorldCorners(array2);
			Rect other = new Rect(array2[0], array2[2] - array2[0]);
			if (Mathf.Abs(num) > 0.1f && (newlySelectedIndex >= originalSelectedIndex || rect.Overlaps(other) || !(other.yMax < rect.yMin)) && (newlySelectedIndex <= originalSelectedIndex || rect.Overlaps(other) || !(other.yMin > rect.yMax)))
			{
				logScrollList.content.anchoredPosition += new Vector2(0f, num);
				logScrollList.OnValueChangedListener(Vector2.zero);
			}
		}

		private void OnListSelected()
		{
			HighlightLog(selectedLogIndex);
		}

		private void OnListDeselected()
		{
			UnhighlightLog();
		}

		public override void Show()
		{
			EventSystem.current.SetSelectedGameObject(listSelector.gameObject);
			bool flag = false;
			if (!initialized)
			{
				logButtons[LogType.Log] = logsButton;
				logButtons[LogType.Warning] = warningsButton;
				logButtons[LogType.Error] = errorsButton;
				logsButton.Initialize(() =>
				{
					Filter(LogType.Log);
				});
				warningsButton.Initialize(() =>
				{
					Filter(LogType.Warning);
				});
				errorsButton.Initialize(() =>
				{
					Filter(LogType.Error);
				});
				logScrollList.Initialize(this);
				initialized = true;
				flag = true;
			}
			HideDetails();
			RefreshLogList();
			StartCoroutine(CoScrollToBottom());
			if (!flag)
			{
				logScrollList.ReloadData();
			}
			notListeningWarning.SetActive(!DebugTool.ListeningToLogs);
		}

		public override void OnBackButtonPressed()
		{
			HideDetails();
		}

		public int GetItemCount()
		{
			return logMessageList.Count;
		}

		public void SetCell(ICell cell, int index)
		{
			(cell as DebugLogMessageLine)?.Initialize(logMessageList[index], (int idx, DebugLogMessage log) =>
			{
				FadeOtherLogs(index);
				HighlightLog(idx);
				ShowLogDetails(log);
			}, index);
		}

		private void HideDetails()
		{
			detailParent.SetActive(value: false);
			detailsToCopy = null;
			DebugUIEvents.OnDetailsToggledVisible(obj: false);
			UnfadeLogs();
		}

		private IEnumerator CoScrollToBottom()
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			logScrollList.ScrollToBottom();
			HighlightLog(logMessageList.Count - 1);
		}

		private void RefreshLogList()
		{
			IEnumerator<DebugLogMessage> logs = DebugTool.Logs;
			logMessageList.Clear();
			Dictionary<LogType, int> dictionary = new Dictionary<LogType, int>();
			Dictionary<LogType, bool> dictionary2 = new Dictionary<LogType, bool>
			{
				{
					LogType.Log,
					logButtons[LogType.Log].Enabled
				},
				{
					LogType.Warning,
					logButtons[LogType.Warning].Enabled
				},
				{
					LogType.Error,
					logButtons[LogType.Error].Enabled
				},
				{
					LogType.Exception,
					logButtons[LogType.Error].Enabled
				}
			};
			while (logs.MoveNext())
			{
				DebugLogMessage current = logs.Current;
				if (!dictionary.ContainsKey(current.LogType))
				{
					dictionary[current.LogType] = 0;
				}
				dictionary[current.LogType]++;
				dictionary2.TryGetValue(current.LogType, out var value);
				if (value)
				{
					logMessageList.Add(current);
				}
			}
			SetFilterButtonCounts(dictionary);
		}

		private void SetFilterButtonCounts(Dictionary<LogType, int> logTypeCounts)
		{
			foreach (KeyValuePair<LogType, DebugLogFilterButton> logButton in logButtons)
			{
				logTypeCounts.TryGetValue(logButton.Key, out var value);
				if (logButton.Key == LogType.Error)
				{
					logTypeCounts.TryGetValue(LogType.Exception, out var value2);
					value += value2;
				}
				logButton.Value.SetCount(value);
			}
		}

		private void Filter(LogType logType)
		{
			DebugLogFilterButton debugLogFilterButton = logButtons[logType];
			debugLogFilterButton.ToggleEnabled(!debugLogFilterButton.Enabled);
			RefreshLogList();
			logScrollList.ReloadData();
		}

		private void CopyLogOrLogs()
		{
			if (detailsToCopy != null)
			{
				GUIUtility.systemCopyBuffer = detailsToCopy;
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerator<DebugLogMessage> logs = DebugTool.Logs;
			while (logs.MoveNext())
			{
				DebugLogMessage current = logs.Current;
				stringBuilder.AppendLine(current.LogString);
				stringBuilder.AppendLine(current.Stacktrace);
			}
			GUIUtility.systemCopyBuffer = stringBuilder.ToString();
		}

		private void ClearLogs()
		{
			DebugTool.ClearLogs();
			RefreshLogList();
			logScrollList.ReloadData();
			HideDetails();
		}

		private void ShowLogDetails(DebugLogMessage debugLogMsg)
		{
			detailParent.SetActive(value: true);
			string sourceText = debugLogMsg.LogString + "\n" + debugLogMsg.Stacktrace;
			detailsText.SetText(sourceText);
			detailsToCopy = sourceText;
			DebugUIEvents.OnDetailsToggledVisible(obj: true);
		}

		private void UnhighlightLog()
		{
			DebugLogMessageLine[] componentsInChildren = logScrollList.GetComponentsInChildren<DebugLogMessageLine>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ToggleHighlight(highlighted: false);
			}
		}

		private void HighlightLog(int i)
		{
			if (initialized)
			{
				selectedLogIndex = i;
				IEnumerator<ICell> cells = logScrollList.GetCells();
				while (cells.MoveNext())
				{
					DebugLogMessageLine debugLogMessageLine = cells.Current as DebugLogMessageLine;
					debugLogMessageLine.ToggleHighlight(i == debugLogMessageLine.Index);
				}
			}
		}

		private void FadeOtherLogs(int index)
		{
			IEnumerator<ICell> cells = logScrollList.GetCells();
			while (cells.MoveNext())
			{
				DebugLogMessageLine debugLogMessageLine = cells.Current as DebugLogMessageLine;
				debugLogMessageLine.ToggleFade(index != debugLogMessageLine.Index);
			}
		}

		private DebugLogMessageLine FindLogObject(int index)
		{
			IEnumerator<ICell> cells = logScrollList.GetCells();
			while (cells.MoveNext())
			{
				DebugLogMessageLine debugLogMessageLine = cells.Current as DebugLogMessageLine;
				if (debugLogMessageLine.Index == index)
				{
					return debugLogMessageLine;
				}
			}
			return null;
		}

		private void UnfadeLogs()
		{
			IEnumerator<ICell> cells = logScrollList.GetCells();
			while (cells.MoveNext())
			{
				(cells.Current as DebugLogMessageLine).ToggleFade(faded: false);
			}
		}
	}
}
