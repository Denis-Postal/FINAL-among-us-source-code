using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.CoreScripts;
using Cpp2ILInjected;
using UnityEngine;

public class Console : MonoBehaviour, IUsable
{
	public float usableDistance = 1f;

	public int ConsoleId;

	public bool onlyFromBelow;

	public bool onlySameRoom;

	public bool checkWalls;

	public bool GhostsIgnored;

	public bool AllowImpostor;

	public SystemTypes Room;

	public TaskTypes[] TaskTypes;

	public TaskSet[] ValidTasks;

	public SpriteRenderer Image;

	public ImageNames UseIcon => ImageNames.UseButton;

	public float UsableDistance => usableDistance;

	public float PercentCool => 0f;

	public unsafe void SetOutline(bool on, bool mainTarget)
	{ /* spliced-from-2019 */
		if ((bool)Image)
		{
			Image.material.SetFloat("_Outline", on ? 1 : 0);
			Image.material.SetColor("_OutlineColor", Color.yellow);
			Image.material.SetColor("_AddColor", mainTarget ? Color.yellow : Color.clear);
		}
	}

	public float CanUse(NetworkedPlayerInfo pc, out bool canUse, out bool couldUse)
	{
		float num = float.MaxValue;
		PlayerControl playerControl = pc.Object;
		Vector2 truePosition = playerControl.GetTruePosition();
		Vector3 position = transform.position;
		couldUse = (!pc.IsDead || (GameManager.Instance.LogicOptions.GetGhostsDoTasks() && !GhostsIgnored)) && playerControl.CanMove && pc.Role.CanUse(this) && (!onlySameRoom || InRoom(truePosition)) && (!onlyFromBelow || truePosition.y < position.y) && (bool)FindTask(playerControl);
		canUse = couldUse;
		if (canUse)
		{
			num = Vector2.Distance(truePosition, transform.position);
			canUse &= num <= UsableDistance;
			if (checkWalls)
			{
				canUse &= !PhysicsHelpers.AnythingBetween(truePosition, position, Constants.ShadowMask, useTriggers: false);
			}
		}
		return num;
	}

	private bool InRoom(Vector2 truePos)
	{
		PlainShipRoom plainShipRoom = ShipStatus.Instance.FastRooms[Room];
		if (!plainShipRoom || !plainShipRoom.roomArea)
		{
			return false;
		}
		try
		{
			return plainShipRoom.roomArea.OverlapPoint(truePos);
		}
		catch
		{
			return false;
		}
	}

	private PlayerTask FindTask(PlayerControl pc)
	{ /* spliced-from-2019 */
		for (int i = 0; i < pc.myTasks.Count; i++)
		{
			PlayerTask playerTask = pc.myTasks[i];
			if (!playerTask.IsComplete && playerTask.ValidConsole(this))
			{
				return playerTask;
			}
		}
		return null;
	}

	public virtual void Use()
	{
		CanUse(PlayerControl.LocalPlayer.Data, out var canUse, out var _);
		if (canUse)
		{
			PlayerControl localPlayer = PlayerControl.LocalPlayer;
			PlayerTask playerTask = FindTask(localPlayer);
			if ((bool)playerTask.MinigamePrefab)
			{
				Logger.GlobalInstance.Info("Using console " + name + " for minigame " + playerTask.MinigamePrefab.name);
				Minigame minigame = Object.Instantiate(playerTask.GetMinigamePrefab());
				minigame.transform.SetParent(Camera.main.transform, worldPositionStays: false);
				minigame.transform.localPosition = new Vector3(0f, 0f, -50f);
				minigame.Console = this;
				minigame.Begin(playerTask);
				DestroyableSingleton<UnityTelemetry>.Instance.WriteUse(localPlayer.PlayerId, playerTask.TaskType, transform.position);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Color color = Gizmos.color;
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, usableDistance);
		Gizmos.color = color;
	}
}
