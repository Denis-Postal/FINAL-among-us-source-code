using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class TaskAddButton : MonoBehaviour
{
	public TextMeshPro Text;

	public SpriteRenderer Overlay;

	public SpriteRenderer FileImage;

	public ButtonRolloverHandler RolloverHandler;

	public Sprite CheckImage;

	public Sprite ExImage;

	public PlayerTask MyTask;

	private Vector2 SafePositionWorld_bf;

	[HideInInspector]
	public PassiveButton Button;

	public RoleBehaviour role;

	public Vector2 SafePositionWorld { get; set; }

	public unsafe RoleBehaviour Role
	{
		get
		{
			return role;
		}
		set
		{
			role = value;
			if (role.IsImpostor)
			{
				FileImage.color = Palette.ImpostorRed;
			}
			else
			{
				FileImage.color = Palette.CrewmateBlue;
			}
			RolloverHandler.OutColor = FileImage.color;
		}
	}

	private void Awake()
	{
		Button = GetComponent<PassiveButton>();
	}

	// TODO(verify): rebuilt from decomp9
	public void Start()
	{
		if ((bool)Role)
		{
			NetworkedPlayerInfo data = PlayerControl.LocalPlayer.Data;
			Overlay.enabled = data.Role.Role == Role.Role;
			Overlay.sprite = CheckImage;
			return;
		}
		PlayerTask playerTask = FindTaskByType();
		if ((bool)playerTask)
		{
			Overlay.enabled = true;
			Overlay.sprite = (playerTask.IsComplete ? CheckImage : ExImage);
		}
		else
		{
			Overlay.enabled = false;
		}
	}

	public void Update()
	{
		if ((bool)Role)
		{
			Overlay.enabled = PlayerControl.LocalPlayer.Data.Role.Role == Role.Role;
		}
	}

	// TODO(verify): rebuilt from decomp9 (the post-role-change teleport/ghost handling was lost)
	public void AddTask()
	{
		if ((bool)Role)
		{
			if (PlayerControl.LocalPlayer.Data.Role.Role != Role.Role)
			{
				if (PlayerTask.DestroyTasksOfType<ImportantTextTask>(PlayerControl.LocalPlayer) && !PlayerTask.PlayerHasTaskOfType<NormalPlayerTask>(PlayerControl.LocalPlayer))
				{
					ShipStatus.Instance.Begin();
				}
				PlayerControl.LocalPlayer.RpcSetRole(Role.Role);
				PlayerControl.LocalPlayer.transform.position = SafePositionWorld;
				if (PlayerControl.LocalPlayer.Data.Role is ImpostorGhostRole impostorGhostRole)
				{
					impostorGhostRole.WasManuallyPicked = true;
				}
			}
		}
		else
		{
			PlayerTask playerTask = FindTaskByType();
			if (!playerTask)
			{
				AddTaskToPlayer(MyTask);
				Overlay.sprite = ExImage;
				Overlay.enabled = true;
			}
			else
			{
				PlayerControl.LocalPlayer.RemoveTask(playerTask);
				Overlay.enabled = false;
			}
		}
	}

	public static void AddTaskToPlayer(PlayerTask taskPrefab)
	{
		PlayerTask playerTask = Object.Instantiate(taskPrefab, PlayerControl.LocalPlayer.transform);
		PlayerControl.LocalPlayer.myTasks.Add(playerTask);
		playerTask.Id = GameData.Instance.TutOnlyAddTask(PlayerControl.LocalPlayer.PlayerId);
		playerTask.Index = taskPrefab.Index;
		playerTask.Owner = PlayerControl.LocalPlayer;
		playerTask.Initialize();
	}

	private PlayerTask FindTaskByType()
	{
		for (int num = PlayerControl.LocalPlayer.myTasks.Count - 1; num > -1; num--)
		{
			PlayerTask playerTask = PlayerControl.LocalPlayer.myTasks[num];
			if (playerTask.TaskType == MyTask.TaskType)
			{
				if (playerTask.TaskType == TaskTypes.DivertPower)
				{
					if (((DivertPowerTask)playerTask).TargetSystem == ((DivertPowerTask)MyTask).TargetSystem)
					{
						return playerTask;
					}
				}
				else if (playerTask.TaskType == TaskTypes.EmptyGarbage || playerTask.TaskType == TaskTypes.RecordTemperature || playerTask.TaskType == TaskTypes.UploadData)
				{
					if (playerTask.StartAt == MyTask.StartAt)
					{
						return playerTask;
					}
				}
				else
				{
					if (playerTask.TaskType != TaskTypes.FixWeatherNode)
					{
						return playerTask;
					}
					if (((WeatherNodeTask)playerTask).NodeId == ((WeatherNodeTask)MyTask).NodeId)
					{
						return playerTask;
					}
				}
			}
		}
		return null;
	}
}
