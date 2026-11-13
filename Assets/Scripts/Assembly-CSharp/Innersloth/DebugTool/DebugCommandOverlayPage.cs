using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public class DebugCommandOverlayPage : DebugToolOverlayPage
	{
		[Header("List")]
		[SerializeField]
		private DebugCommandButton commandButtonPrefab;

		[SerializeField]
		private Transform commandButtonParent;

		[SerializeField]
		private ScrollRect commandScrollList;

		[Header("Details")]
		[SerializeField]
		private GameObject detailsParent;

		[SerializeField]
		private TextMeshProUGUI basePath;

		[SerializeField]
		private TextMeshProUGUI commandName;

		[SerializeField]
		private TextMeshProUGUI commandDescription;

		[SerializeField]
		private Transform argumentsParent;

		[SerializeField]
		private Button executeButton;

		[Header("Argument Field Types")]
		[SerializeField]
		private DebugCommandArgumentRow_TextField argumentTextFieldPrefab;

		[SerializeField]
		private DebugCommandArgumentRow_Dropdown argumentDropdownPrefab;

		[Header("Debug")]
		[SerializeField]
		private GameObject debugContainer;

		[SerializeField]
		private Image debugIcon;

		[SerializeField]
		private TextMeshProUGUI debugText;

		[Space(10f)]
		[SerializeField]
		private Color debugInfoColor;

		[SerializeField]
		private Color debugSuccessColor;

		[SerializeField]
		private Color debugWarningColor;

		[SerializeField]
		private Color debugErrorColor;

		[Space(10f)]
		[SerializeField]
		private Sprite debugInfoIcon;

		[SerializeField]
		private Sprite debugSuccessIcon;

		[SerializeField]
		private Sprite debugWarningIcon;

		[SerializeField]
		private Sprite debugErrorIcon;

		private List<DebugCommandButton> allCommandButtons;

		private Dictionary<string, List<DebugCommandButton>> directoryContents;

		private Dictionary<string, DebugCommand> commands;

		private int selectedCommandIndex;

		private List<AbstractDebugCommandArgumentRow> commandInputs;

		private DebugCommandArgumentRowFieldContainer selectedFieldContainer;

		public override string Name => "Commands";

		public override bool ConsumingNavActions => detailsParent.activeSelf;

		private void Awake()
		{
			DebugTool.OnLogMessageReceived += SetCommandLog;
		}

		private void OnDestroy()
		{
			DebugTool.OnLogMessageReceived -= SetCommandLog;
		}

		private void Update()
		{
			foreach (IDebugToolInputProvider inputProvider in InputProviders)
			{
				if (detailsParent.activeSelf)
				{
					if (EventSystem.current.currentSelectedGameObject != executeButton.gameObject && inputProvider.GetConfirm())
					{
						selectedFieldContainer.Confirm();
						break;
					}
					if (inputProvider.GetCancel())
					{
						if (selectedFieldContainer != null && selectedFieldContainer.IsFieldSelected)
						{
							selectedFieldContainer.Cancel();
							break;
						}
						ResetAndHideDetails();
					}
				}
				else
				{
					UpdateSelectedCommandIfScrolling(inputProvider);
					if (inputProvider.GetConfirm())
					{
						OnButtonClick(allCommandButtons[selectedCommandIndex]);
					}
				}
			}
		}

		private void UpdateSelectedCommandIfScrolling(IDebugToolInputProvider inputProvider)
		{
			int num = selectedCommandIndex;
			if (inputProvider.GetNavDown())
			{
				selectedCommandIndex = Mathf.Clamp(selectedCommandIndex + 1, 0, allCommandButtons.Count - 1);
			}
			else if (inputProvider.GetNavUp())
			{
				selectedCommandIndex = Mathf.Clamp(selectedCommandIndex - 1, 0, allCommandButtons.Count - 1);
			}
			DebugCommandButton debugCommandButton = allCommandButtons[selectedCommandIndex];
			if (num == selectedCommandIndex)
			{
				return;
			}
			if (num < selectedCommandIndex && !debugCommandButton.gameObject.activeInHierarchy)
			{
				for (int i = selectedCommandIndex + 1; i < allCommandButtons.Count; i++)
				{
					if (allCommandButtons[i].gameObject.activeInHierarchy)
					{
						selectedCommandIndex = i;
						break;
					}
				}
			}
			else if (num > selectedCommandIndex && !debugCommandButton.gameObject.activeInHierarchy)
			{
				for (int num2 = selectedCommandIndex - 1; num2 >= 0; num2--)
				{
					if (allCommandButtons[num2].gameObject.activeInHierarchy)
					{
						selectedCommandIndex = num2;
						break;
					}
				}
			}
			HighlightButton(selectedCommandIndex);
			commandScrollList.content.anchoredPosition += new Vector2(0f, (float)(selectedCommandIndex - num) * ((RectTransform)commandButtonPrefab.transform).rect.height);
			ResetAndHideDetails();
		}

		public override void Show()
		{
			List<DebugCommand> list = new List<DebugCommand>(DebugTool.Commands);
			for (int num = commandButtonParent.childCount - 1; num >= 0; num--)
			{
				UnityEngine.Object.Destroy(commandButtonParent.GetChild(num).gameObject);
			}
			ResetAndHideDetails();
			list.Sort((DebugCommand c1, DebugCommand c2) => string.Compare(c1.CommandPath, c2.CommandPath, StringComparison.Ordinal));
			allCommandButtons = new List<DebugCommandButton>();
			commands = new Dictionary<string, DebugCommand>();
			directoryContents = new Dictionary<string, List<DebugCommandButton>>();
			int num2 = 0;
			for (int num3 = 0; num3 < list.Count; num3++)
			{
				DebugCommand debugCommand = list[num3];
				string[] array = debugCommand.CommandPath.Split('/');
				string[] array2 = new string[array.Length];
				for (int num4 = 0; num4 < array.Length - 1; num4++)
				{
					string text = (array2[num4] = string.Join("/", array, 0, num4 + 1));
					if (!directoryContents.ContainsKey(text))
					{
						directoryContents[text] = new List<DebugCommandButton>();
						string text2 = ((num4 > 0) ? array2[num4 - 1] : string.Empty);
						DebugCommandButton debugCommandButton = CreateDirectory(array[num4], text2, text, num2++);
						allCommandButtons.Add(debugCommandButton);
						debugCommandButton.SetIndentLevel(num4);
						debugCommandButton.ToggleDirectory(directory: true);
						debugCommandButton.Expanded = false;
						if (text2 != string.Empty)
						{
							directoryContents[text2].Add(debugCommandButton);
							debugCommandButton.gameObject.SetActive(value: false);
						}
					}
				}
				DebugCommandButton button = UnityEngine.Object.Instantiate(commandButtonPrefab, commandButtonParent);
				ParameterInfo[] parameters = debugCommand.ActionInfo.GetParameters();
				allCommandButtons.Add(button);
				string directory = debugCommand.Directory;
				int btnIndex = num2;
				button.Initialize(debugCommand.CommandName, debugCommand.CommandPath, parameters, () =>
				{
					selectedCommandIndex = btnIndex;
					HighlightButton(btnIndex);
					OnButtonClick(button);
				}, btnIndex);
				num2++;
				button.SetIndentLevel(directory.Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries).Length);
				button.ToggleDirectory(directory: false);
				commands[debugCommand.CommandPath] = debugCommand;
				if (directory.Length > 0)
				{
					directoryContents[directory].Add(button);
					button.gameObject.SetActive(value: false);
				}
			}
			selectedCommandIndex = 0;
			HighlightButton(selectedCommandIndex);
		}

		public override void OnBackButtonPressed()
		{
			ResetAndHideDetails();
		}

		private DebugCommandButton CreateDirectory(string directoryName, string parentDirectoryPath, string directoryPath, int index)
		{
			DebugCommandButton directoryButton = UnityEngine.Object.Instantiate(commandButtonPrefab, commandButtonParent);
			directoryButton.Initialize(directoryName, directoryPath, null, () =>
			{
				selectedCommandIndex = index;
				HighlightButton(selectedCommandIndex);
				OnButtonClick(directoryButton);
			}, index);
			return directoryButton;
		}

		private void OnButtonClick(DebugCommandButton btn)
		{
			if (btn.IsDirectory)
			{
				OnDirectoryClicked(btn);
			}
			else
			{
				OnCommandClicked(btn);
			}
		}

		private void OnCommandClicked(DebugCommandButton btn)
		{
			DebugCommand command = commands[btn.Path];
			ParameterInfo[] parameters = command.ActionInfo.GetParameters();
			if (parameters.Length == 0)
			{
				if (command.CloseBeforeExecute)
				{
					DebugUIEvents.OnBeforeCommandExecuted(command);
				}
				ResetAndHideDetails();
				command.ActionInfo.Invoke(command.Target, Array.Empty<object>());
				return;
			}
			ResetAndHideDetails();
			detailsParent.gameObject.SetActive(value: true);
			basePath.SetText(command.Directory.Replace("/", " / ") + " / <b>" + command.CommandName + "</b>");
			commandName.SetText(command.CommandName);
			commandDescription.SetText(command.Description);
			commandInputs = new List<AbstractDebugCommandArgumentRow>();
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				_ = parameterInfo.Name;
				AbstractDebugCommandArgumentRow abstractDebugCommandArgumentRow = InstantiateInputFieldRow(command, i, parameterInfo);
				abstractDebugCommandArgumentRow.SetInfo(parameterInfo.Name, DebugTool.GetTypeName(parameterInfo.ParameterType));
				abstractDebugCommandArgumentRow.OnFieldContainerSelected += OnFieldRowContainerSelected;
				commandInputs.Add(abstractDebugCommandArgumentRow);
			}
			for (int j = 0; j < commandInputs.Count; j++)
			{
				Selectable above = null;
				Selectable selectable = null;
				if (j > 0)
				{
					above = commandInputs[j - 1].BottomField;
				}
				if (j < commandInputs.Count - 1)
				{
					selectable = commandInputs[j + 1].TopField;
				}
				else
				{
					selectable = executeButton;
					Navigation navigation = executeButton.navigation;
					navigation.mode = Navigation.Mode.Explicit;
					navigation.selectOnUp = commandInputs[j].BottomField;
					executeButton.navigation = navigation;
				}
				commandInputs[j].SetNavigation(above, selectable);
			}
			executeButton.onClick.RemoveAllListeners();
			executeButton.onClick.AddListener(() =>
			{
				ExecuteCommandWithParameters(command);
			});
			EventSystem.current.SetSelectedGameObject(argumentsParent.GetComponentsInChildren<Selectable>().First().gameObject);
			DebugUIEvents.OnDetailsToggledVisible(obj: true);
		}

		private void OnFieldRowContainerSelected(DebugCommandArgumentRowFieldContainer container)
		{
			selectedFieldContainer = container;
		}

		private AbstractDebugCommandArgumentRow InstantiateInputFieldRow(DebugCommand command, int argIndex, ParameterInfo paramInfo)
		{
			if (DebugTool.HasDropdownValueCollector(paramInfo.ParameterType))
			{
				DebugCommandArgumentRow_Dropdown debugCommandArgumentRow_Dropdown = UnityEngine.Object.Instantiate(argumentDropdownPrefab, argumentsParent);
				debugCommandArgumentRow_Dropdown.SetDropdownOptions(DebugTool.GetDropdownValues(paramInfo.ParameterType));
				return debugCommandArgumentRow_Dropdown;
			}
			DebugCommandArgumentRow_TextField debugCommandArgumentRow_TextField = UnityEngine.Object.Instantiate(argumentTextFieldPrefab, argumentsParent);
			debugCommandArgumentRow_TextField.SetPlaceholder(command.GetArgumentPlaceholder(argIndex));
			return debugCommandArgumentRow_TextField;
		}

		private void OnDirectoryClicked(DebugCommandButton btn)
		{
			List<DebugCommandButton> collection = directoryContents[btn.Path];
			bool flag = !btn.Expanded;
			Queue<DebugCommandButton> queue = new Queue<DebugCommandButton>(collection);
			while (queue.Count > 0)
			{
				DebugCommandButton debugCommandButton = queue.Dequeue();
				debugCommandButton.gameObject.SetActive(flag);
				if (flag || !debugCommandButton.IsDirectory || !debugCommandButton.Expanded)
				{
					continue;
				}
				foreach (DebugCommandButton item in directoryContents[debugCommandButton.Path])
				{
					queue.Enqueue(item);
				}
				debugCommandButton.Expanded = false;
			}
			btn.Expanded = flag;
		}

		private void ExecuteCommandWithParameters(DebugCommand command)
		{
			ParameterInfo[] parameters = command.ActionInfo.GetParameters();
			object[] array = new object[parameters.Length];
			List<Exception> list = new List<Exception>();
			for (int i = 0; i < parameters.Length; i++)
			{
				string empty = string.Empty;
				empty = commandInputs[i].GetValue();
				try
				{
					array[i] = DebugTool.ParseInputToType(empty, parameters[i].Name, parameters[i].ParameterType);
				}
				catch (Exception item)
				{
					list.Add(item);
				}
			}
			if (list.Count > 0)
			{
				string message = "Unexpected error(s) occurred:\n" + string.Join("\n", list.Select((Exception e) => e.Message));
				foreach (Exception item2 in list)
				{
					Debug.LogException(item2);
				}
				SetCommandLog(message, DebugCommandMessageType.Error);
				return;
			}
			if (command.CloseBeforeExecute)
			{
				DebugUIEvents.OnBeforeCommandExecuted(command);
			}
			try
			{
				command.ActionInfo.Invoke(command.Target, array);
			}
			catch (TargetInvocationException ex)
			{
				Debug.LogException(ex);
				if (command.CloseBeforeExecute)
				{
					throw ex.InnerException ?? ex;
				}
				string message2 = "Unexpected error occurred: " + ((ex.InnerException == null) ? ex.Message : ex.InnerException.Message);
				SetCommandLog(message2, DebugCommandMessageType.Error);
			}
			catch (Exception ex2)
			{
				if (command.CloseBeforeExecute)
				{
					throw;
				}
				Debug.LogException(ex2);
				string message3 = "Unexpected error occurred: " + ex2.Message;
				SetCommandLog(message3, DebugCommandMessageType.Error);
			}
		}

		private void ResetAndHideDetails()
		{
			debugText.text = string.Empty;
			selectedFieldContainer = null;
			if (detailsParent.activeSelf)
			{
				detailsParent.SetActive(value: false);
				executeButton.onClick.RemoveAllListeners();
				commandInputs = new List<AbstractDebugCommandArgumentRow>();
				for (int num = argumentsParent.childCount - 1; num >= 0; num--)
				{
					UnityEngine.Object.Destroy(argumentsParent.GetChild(num).gameObject);
				}
				DebugUIEvents.OnDetailsToggledVisible(obj: false);
			}
		}

		private void HighlightButton(int highlightedIndex)
		{
			for (int i = 0; i < allCommandButtons.Count; i++)
			{
				DebugCommandButton debugCommandButton = allCommandButtons[i];
				bool highlighted = i == highlightedIndex;
				debugCommandButton.ToggleHighlight(highlighted);
			}
		}

		private void ClearCommandLog()
		{
			debugContainer.SetActive(value: false);
			debugText.text = "";
		}

		private void SetCommandLog(string message, DebugCommandMessageType type)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				ClearCommandLog();
				return;
			}
			debugContainer.SetActive(value: true);
			debugText.text = "<b>" + type.ToString().ToUpper() + ":</b> " + message;
			switch (type)
			{
			default:
				Debug.LogError(string.Format("[{0}] {1}.{2} has not been configured.", "DebugCommandOverlayPage", "DebugCommandMessageType", type));
				ClearCommandLog();
				break;
			case DebugCommandMessageType.Info:
				debugText.color = debugInfoColor;
				debugIcon.color = debugInfoColor;
				debugIcon.sprite = debugInfoIcon;
				break;
			case DebugCommandMessageType.Success:
				debugText.color = debugSuccessColor;
				debugIcon.color = debugSuccessColor;
				debugIcon.sprite = debugSuccessIcon;
				break;
			case DebugCommandMessageType.Warning:
				debugText.color = debugWarningColor;
				debugIcon.color = debugWarningColor;
				debugIcon.sprite = debugWarningIcon;
				break;
			case DebugCommandMessageType.Error:
				debugText.color = debugErrorColor;
				debugIcon.color = debugErrorColor;
				debugIcon.sprite = debugErrorIcon;
				break;
			}
		}
	}
}
