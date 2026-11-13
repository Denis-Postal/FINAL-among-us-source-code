using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class MushroomDoorSabotageMinigame : Minigame, IDoorMinigame
{
	private const int NUM_IMMEDIATELY_VISIBLE_MUSHROOMS = 3;

	private const int NUM_DELAYED_MUSHROOMS = 3;

	private const int TOTAL_MUSHROOM_COUNT = 6;

	[SerializeField]
	private FloatRange mushroomInvisibleSeconds;

	[SerializeField]
	private FloatRange mushroomVisibleSeconds;

	[SerializeField]
	private TextMeshPro counterText;

	[SerializeField]
	private Transform[] spawnPoints;

	[SerializeField]
	private MushroomDoorSabotageMinigameMushroom[] mushroomVariants;

	private OpenableDoor myDoor;

	private int mushroomWhackCount;

	private List<Transform> spawnPointBag;

	private List<MushroomDoorSabotageMinigameMushroom> mushrooms;

	private unsafe void Update()
	{
		foreach (MushroomDoorSabotageMinigameMushroom mushroom in mushrooms)
		{
			switch (mushroom.CurrentState)
			{
			case MushroomDoorSabotageMinigameMushroom.State.Visible:
				mushroom.DeductTimeInState(Time.deltaTime);
				if (mushroom.IsTimeInStateComplete)
				{
					spawnPointBag.Shuffle();
					spawnPointBag.Add(mushroom.transform.parent);
					mushroom.SetState(MushroomDoorSabotageMinigameMushroom.State.Invisible, mushroomInvisibleSeconds.Next());
				}
				break;
			case MushroomDoorSabotageMinigameMushroom.State.Invisible:
				mushroom.DeductTimeInState(Time.deltaTime);
				if (mushroom.IsTimeInStateComplete)
				{
					Transform transform = spawnPointBag.First();
					spawnPointBag.Remove(transform);
					mushroom.transform.SetParent(transform, worldPositionStays: false);
					mushroom.SetState(MushroomDoorSabotageMinigameMushroom.State.Visible, mushroomVisibleSeconds.Next());
				}
				break;
			}
		}
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		if (spawnPoints.Length < 6)
		{
			Logger.GlobalInstance.Error("There are not enough spawn points to finish this minigame. Completing immediately.");
			FixDoorAndCloseMinigame();
			return;
		}
		spawnPointBag = new List<Transform>(spawnPoints);
		spawnPointBag.Shuffle();
		mushrooms = new List<MushroomDoorSabotageMinigameMushroom>();
		for (int i = 0; i < 3; i++)
		{
			Transform transform = spawnPointBag.First();
			MushroomDoorSabotageMinigameMushroom mushroomDoorSabotageMinigameMushroom = Object.Instantiate(mushroomVariants.Random(), transform, worldPositionStays: false);
			mushroomDoorSabotageMinigameMushroom.SetState(MushroomDoorSabotageMinigameMushroom.State.Visible, mushroomVisibleSeconds.Next());
			mushroomDoorSabotageMinigameMushroom.OnClick += UpdateMushroomWhackCount;
			mushrooms.Add(mushroomDoorSabotageMinigameMushroom);
			spawnPointBag.Remove(transform);
		}
		for (int j = 0; j < 3; j++)
		{
			Transform transform2 = spawnPointBag.First();
			MushroomDoorSabotageMinigameMushroom mushroomDoorSabotageMinigameMushroom2 = Object.Instantiate(mushroomVariants.Random(), transform2, worldPositionStays: false);
			mushroomDoorSabotageMinigameMushroom2.SetState(MushroomDoorSabotageMinigameMushroom.State.Invisible, mushroomInvisibleSeconds.Next());
			mushroomDoorSabotageMinigameMushroom2.OnClick += UpdateMushroomWhackCount;
			mushrooms.Add(mushroomDoorSabotageMinigameMushroom2);
			spawnPointBag.Remove(transform2);
		}
		SetCounterText(0);
	}

	public void SetDoor(OpenableDoor door)
	{
		myDoor = door;
	}

	private void SetCounterText(int whackedCount)
	{
		counterText.SetText($"{whackedCount}/{6}");
	}

	private void UpdateMushroomWhackCount(MushroomDoorSabotageMinigameMushroom mushroom)
	{
		spawnPointBag.Add(mushroom.transform.parent);
		mushroomWhackCount++;
		SetCounterText(mushroomWhackCount);
		if (mushroomWhackCount >= 6)
		{
			FixDoorAndCloseMinigame();
		}
	}

	private void FixDoorAndCloseMinigame()
	{
		ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Doors, (byte)(myDoor.Id | 0x40));
		myDoor.SetDoorway(open: true);
		Close();
	}
}
