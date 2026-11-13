using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.GameOptions;
using TMPro;
using UnityEngine;

public class TaskAdderGame : Minigame
{
	private enum FolderType
	{
		Tasks,
		Roles
	}

	public TextMeshPro PathText;

	public TaskFolder RootFolderPrefab;

	public TaskAddButton TaskPrefab;

	public Transform TaskParent;

	public List<TaskFolder> Hierarchy = new List<TaskFolder>();

	public List<Transform> ActiveItems = new List<Transform>();

	public TaskAddButton RoleButton;

	public float folderWidth;

	public float fileWidth;

	public float lineWidth;

	private TaskFolder Root;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement FolderBackButton;

	public List<UiElement> ControllerSelectable;

	private string previouslySelectedFolderName = string.Empty;

	public Vector2 SafePositionWorld { get; set; }

	private void OnEnable()
	{
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, null, ControllerSelectable, gridNav: true);
	}

	private void OnDisable()
	{
		ControllerManager.Instance.ResetAll();
	}

	public override void Begin(PlayerTask t)
	{
		base.Begin(t);
		Root = Object.Instantiate(RootFolderPrefab, transform);
		Root.gameObject.SetActive(value: false);
		Dictionary<string, TaskFolder> folders = new Dictionary<string, TaskFolder>();
		PopulateRoot(FolderType.Tasks, Root, folders, ShipStatus.Instance.CommonTasks);
		PopulateRoot(FolderType.Tasks, Root, folders, ShipStatus.Instance.LongTasks);
		PopulateRoot(FolderType.Tasks, Root, folders, ShipStatus.Instance.ShortTasks);
		Root.SubFolders = Root.SubFolders.OrderBy((TaskFolder f) => f.FolderName).ToList();
		PopulateRoot(FolderType.Roles, Root, folders);
		ShowFolder(Root);
	}

	private void PopulateRoot(FolderType folderType, TaskFolder rootFolder, Dictionary<string, TaskFolder> folders, NormalPlayerTask[] taskList = null)
	{
		switch (folderType)
		{
		case FolderType.Tasks:
			if (taskList == null)
			{
				break;
			}
			foreach (NormalPlayerTask normalPlayerTask in taskList)
			{
				SystemTypes systemTypes = normalPlayerTask.StartAt;
				if (normalPlayerTask is DivertPowerTask)
				{
					systemTypes = ((DivertPowerTask)normalPlayerTask).TargetSystem;
				}
				if (systemTypes == SystemTypes.LowerEngine)
				{
					systemTypes = SystemTypes.UpperEngine;
				}
				if (!folders.TryGetValue(systemTypes.ToString(), out var value))
				{
					TaskFolder taskFolder = (folders[systemTypes.ToString()] = Object.Instantiate(RootFolderPrefab, transform));
					value = taskFolder;
					value.SetFolderColor(TaskFolder.FolderColor.Tan);
					value.gameObject.SetActive(value: false);
					if (systemTypes == SystemTypes.UpperEngine)
					{
						value.FolderName = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Engines);
					}
					else
					{
						value.FolderName = DestroyableSingleton<TranslationController>.Instance.GetString(systemTypes);
					}
					rootFolder.SubFolders.Add(value);
				}
				value.TaskChildren.Add(normalPlayerTask);
			}
			break;
		case FolderType.Roles:
		{
			TaskFolder taskFolder = (folders["Crewmates"] = Object.Instantiate(RootFolderPrefab, transform));
			TaskFolder taskFolder3 = taskFolder;
			taskFolder3.gameObject.SetActive(value: false);
			taskFolder3.SetFolderColor(TaskFolder.FolderColor.Blue);
			taskFolder = (folders["Impostors"] = Object.Instantiate(RootFolderPrefab, transform));
			TaskFolder taskFolder5 = taskFolder;
			taskFolder5.gameObject.SetActive(value: false);
			taskFolder5.SetFolderColor(TaskFolder.FolderColor.Red);
			taskFolder3.FolderName = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Crewmate);
			taskFolder5.FolderName = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Impostor);
			rootFolder.SubFolders.Insert(0, taskFolder3);
			rootFolder.SubFolders.Insert(0, taskFolder5);
			for (int i = 0; i < DestroyableSingleton<RoleManager>.Instance.AllRoles.Count; i++)
			{
				RoleBehaviour roleBehaviour = DestroyableSingleton<RoleManager>.Instance.AllRoles[i];
				if (roleBehaviour.Role != RoleTypes.ImpostorGhost && roleBehaviour.Role != RoleTypes.CrewmateGhost)
				{
					if (roleBehaviour.TeamType == RoleTeamTypes.Crewmate)
					{
						taskFolder3.RoleChildren.Add(roleBehaviour);
					}
					else
					{
						taskFolder5.RoleChildren.Add(roleBehaviour);
					}
				}
			}
			taskFolder3.RoleChildren = taskFolder3.RoleChildren.OrderBy((RoleBehaviour r) => r.Role.ToString()).ToList();
			taskFolder5.RoleChildren = taskFolder5.RoleChildren.OrderBy((RoleBehaviour r) => r.Role.ToString()).ToList();
			break;
		}
		}
	}

	public void GoToRoot()
	{
		Hierarchy.Clear();
		ShowFolder(Root);
	}

	public void GoUpOne()
	{
		if (Hierarchy.Count > 1)
		{
			TaskFolder taskFolder = Hierarchy[Hierarchy.Count - 2];
			Hierarchy.RemoveAt(Hierarchy.Count - 1);
			Hierarchy.RemoveAt(Hierarchy.Count - 1);
			ShowFolder(taskFolder);
		}
	}

	public void SetPreviousControllerSelection(string folderName)
	{
		if (ActiveInputManager.currentControlType != ActiveInputManager.InputType.Joystick)
		{
			return;
		}
		TaskFolder[] componentsInChildren = TaskParent.gameObject.GetComponentsInChildren<TaskFolder>();
		foreach (TaskFolder taskFolder in componentsInChildren)
		{
			if (taskFolder.FolderName == previouslySelectedFolderName)
			{
				ControllerManager.Instance.SetCurrentSelected(taskFolder.Button);
				break;
			}
		}
		previouslySelectedFolderName = folderName;
	}

	public void ShowFolder(TaskFolder taskFolder)
	{
		StringBuilder stringBuilder = new StringBuilder(64);
		Hierarchy.Add(taskFolder);
		for (int i = 0; i < Hierarchy.Count; i++)
		{
			stringBuilder.Append(Hierarchy[i].FolderName);
			stringBuilder.Append("\\");
		}
		PathText.text = stringBuilder.ToString();
		for (int j = 0; j < ActiveItems.Count; j++)
		{
			Object.Destroy(ActiveItems[j].gameObject);
		}
		ActiveItems.Clear();
		float xCursor = 0f;
		float yCursor = 0f;
		float maxHeight = 0f;
		for (int k = 0; k < taskFolder.SubFolders.Count; k++)
		{
			TaskFolder taskFolder2 = Object.Instantiate(taskFolder.SubFolders[k], TaskParent);
			taskFolder2.gameObject.SetActive(value: true);
			taskFolder2.Parent = this;
			taskFolder2.transform.localPosition = new Vector3(xCursor, yCursor, 0f);
			taskFolder2.transform.localScale = Vector3.one;
			maxHeight = Mathf.Max(maxHeight, taskFolder2.Text.bounds.size.y + 1.1f);
			xCursor += folderWidth;
			if (xCursor > lineWidth)
			{
				xCursor = 0f;
				yCursor -= maxHeight;
				maxHeight = 0f;
			}
			ActiveItems.Add(taskFolder2.transform);
			if (taskFolder2 != null && taskFolder2.Button != null)
			{
				ControllerManager.Instance.AddSelectableUiElement(taskFolder2.Button);
			}
		}
		List<NormalPlayerTask> list = taskFolder.TaskChildren.OrderBy((NormalPlayerTask t) => t.TaskType).ToList();
		for (int num = 0; num < list.Count; num++)
		{
			TaskAddButton taskAddButton = Object.Instantiate(TaskPrefab);
			taskAddButton.MyTask = list[num];
			if (taskAddButton.MyTask.TaskType == TaskTypes.DivertPower)
			{
				SystemTypes targetSystem = ((DivertPowerTask)taskAddButton.MyTask).TargetSystem;
				taskAddButton.Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DivertPowerTo, DestroyableSingleton<TranslationController>.Instance.GetString(targetSystem));
			}
			else if (taskAddButton.MyTask.TaskType == TaskTypes.FixWeatherNode)
			{
				int nodeId = ((WeatherNodeTask)taskAddButton.MyTask).NodeId;
				taskAddButton.Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FixWeatherNode) + " " + DestroyableSingleton<TranslationController>.Instance.GetString(WeatherSwitchGame.ControlNames[nodeId]);
			}
			else
			{
				taskAddButton.Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(taskAddButton.MyTask.TaskType);
			}
			AddFileAsChild(taskFolder, taskAddButton, ref xCursor, ref yCursor, ref maxHeight);
			if (taskAddButton != null && taskAddButton.Button != null)
			{
				ControllerManager.Instance.AddSelectableUiElement(taskAddButton.Button);
			}
		}
		List<RoleBehaviour> roleChildren = taskFolder.RoleChildren;
		for (int num2 = 0; num2 < roleChildren.Count; num2++)
		{
			TaskAddButton taskAddButton2 = Object.Instantiate(RoleButton);
			taskAddButton2.SafePositionWorld = SafePositionWorld;
			taskAddButton2.Text.text = "Be_" + roleChildren[num2].NiceName + ".exe";
			taskAddButton2.Role = roleChildren[num2];
			AddFileAsChild(taskFolder, taskAddButton2, ref xCursor, ref yCursor, ref maxHeight);
			if (taskAddButton2 != null && taskAddButton2.Button != null)
			{
				ControllerManager.Instance.AddSelectableUiElement(taskAddButton2.Button);
			}
		}
		if (Hierarchy.Count == 1)
		{
			ControllerManager.Instance.SetBackButton(BackButton);
		}
		else
		{
			ControllerManager.Instance.SetBackButton(FolderBackButton);
		}
		SetPreviousControllerSelection(taskFolder.FolderName);
	}

	private void AddFileAsChild(TaskFolder taskFolder, TaskAddButton item, ref float xCursor, ref float yCursor, ref float maxHeight)
	{
		item.transform.SetParent(TaskParent);
		item.transform.localPosition = new Vector3(xCursor, yCursor, 0f);
		item.transform.localScale = Vector3.one;
		maxHeight = Mathf.Max(maxHeight, item.Text.bounds.size.y + 1.1f);
		xCursor += fileWidth;
		if (xCursor > lineWidth)
		{
			xCursor = 0f;
			yCursor -= maxHeight;
			maxHeight = 0f;
		}
		ActiveItems.Add(item.transform);
	}
}
