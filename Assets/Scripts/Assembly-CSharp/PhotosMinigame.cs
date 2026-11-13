using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rewired;
using UnityEngine;

public class PhotosMinigame : Minigame
{
	public GamePhotoBehaviour[] photos;

	public Sprite[] PhotoContents;

	public Collider2D PoolHitbox;

	public Transform selectorObject;

	public SpriteRenderer selectorHand;

	public Rect PolaroidBounds;

	private Controller controller = new Controller();

	private bool prevHadButton;

	private int currentlyGrabbedObject = -1;

	private bool AllowDraggingPhotos => MyNormTask.TimerStarted != NormalPlayerTask.TimerState.Started;

	private List<Sprite> GetUniqueListOfPhotos(int count)
	{
		List<Sprite> list = PhotoContents.ToList();
		List<Sprite> list2 = new List<Sprite>();
		for (int i = 0; i < count; i++)
		{
			Sprite item = list.Random();
			list.Remove(item);
			list2.Add(item);
		}
		return list2;
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		byte[] data = MyNormTask.Data;
		if (data == null || data.Length != 0)
		{
			ReadInitialData();
		}
		else
		{
			List<Sprite> uniqueListOfPhotos = GetUniqueListOfPhotos(photos.Length);
			for (int i = 0; i < photos.Length; i++)
			{
				photos[i].Image.sprite = uniqueListOfPhotos[i];
			}
			WriteInitialData();
		}
		PlayerControl.LocalPlayer.SetPlayerMaterialColors(selectorHand);
		SetupInput(disableCursor: false);
	}

	private void WriteInitialData()
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
		{
			for (int i = 0; i < photos.Length; i++)
			{
				GamePhotoBehaviour p = photos[i];
				Vector3 localPosition = p.transform.localPosition;
				binaryWriter.Write(PhotoContents.IndexOf((Sprite s) => s == p.Image.sprite));
				binaryWriter.Write(localPosition.x);
				binaryWriter.Write(localPosition.y);
			}
		}
		MyNormTask.Data = memoryStream.ToArray();
	}

	private void ReadInitialData()
	{
		using MemoryStream input = new MemoryStream(MyNormTask.Data);
		using BinaryReader binaryReader = new BinaryReader(input);
		for (int i = 0; i < photos.Length; i++)
		{
			GamePhotoBehaviour obj = photos[i];
			obj.Image.sprite = PhotoContents[binaryReader.ReadInt32()];
			obj.transform.localPosition = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), 1f);
		}
	}

	public IEnumerator Start()
	{
		float z = 0f;
		yield return null;
		GamePhotoBehaviour[] array = photos;
		foreach (GamePhotoBehaviour gamePhotoBehaviour in array)
		{
			gamePhotoBehaviour.zOffset = z;
			gamePhotoBehaviour.transform.SetLocalZ(1f + z);
			z -= 0.01f;
			bool flag = gamePhotoBehaviour.Hitbox.IsTouching(PoolHitbox);
			gamePhotoBehaviour.TargetColor = (flag ? GamePhotoBehaviour.InWaterPink : Color.white);
			gamePhotoBehaviour.Frame.color = gamePhotoBehaviour.TargetColor;
			gamePhotoBehaviour.Image.color = Color.Lerp(gamePhotoBehaviour.TargetColor, Palette.ClearWhite, MyNormTask.TaskTimer / 60f);
		}
	}

	private void Update()
	{
		if (amClosing == CloseState.None)
		{
			controller.Update();
			GamePhotoBehaviour[] array = photos;
			foreach (GamePhotoBehaviour gamePhotoBehaviour in array)
			{
				gamePhotoBehaviour.Frame.color = gamePhotoBehaviour.TargetColor;
				gamePhotoBehaviour.Image.color = Color.Lerp(gamePhotoBehaviour.TargetColor, Palette.ClearWhite, MyNormTask.TaskTimer / 60f);
				clampToBounds(gamePhotoBehaviour);
			}
			if (Controller.currentTouchType == Controller.TouchType.Joystick)
			{
				HandleJoystick();
			}
			else
			{
				HandleMouse();
			}
			if (MyNormTask.TimerStarted == NormalPlayerTask.TimerState.NotStarted && !controller.AnyTouch && photos.All((GamePhotoBehaviour p) => p.Hitbox.IsTouching(PoolHitbox)))
			{
				MyNormTask.TimerStarted = NormalPlayerTask.TimerState.Started;
				StartCoroutine(CoStartClose());
			}
			else if (MyNormTask.TimerStarted == NormalPlayerTask.TimerState.Finished && !controller.AnyTouch && !amOpening && photos.All((GamePhotoBehaviour p) => !p.Hitbox.IsTouching(PoolHitbox)))
			{
				MyNormTask.NextStep();
				Close();
			}
		}
	}

	private void HandleJoystick()
	{
		bool button = ReInput.players.GetPlayer(0).GetButton(11);
		Vector3 position = selectorObject.transform.position;
		position.x = VirtualCursor.currentPosition.x;
		position.y = VirtualCursor.currentPosition.y;
		selectorObject.transform.position = position;
		if (button && AllowDraggingPhotos)
		{
			if (!prevHadButton)
			{
				float num = 0f;
				currentlyGrabbedObject = -1;
				for (int i = 0; i < photos.Length; i++)
				{
					float sqrMagnitude = ((Vector2)(photos[i].transform.localPosition - selectorObject.transform.localPosition)).sqrMagnitude;
					if (sqrMagnitude <= 1f && (currentlyGrabbedObject == -1 || sqrMagnitude < num))
					{
						num = sqrMagnitude;
						currentlyGrabbedObject = i;
					}
				}
				if (currentlyGrabbedObject != -1)
				{
					GamePhotoBehaviour gamePhotoBehaviour = photos[currentlyGrabbedObject];
					gamePhotoBehaviour.StopAllCoroutines();
					gamePhotoBehaviour.StartCoroutine(gamePhotoBehaviour.Pickup());
					FixZ(gamePhotoBehaviour);
				}
			}
			else if (currentlyGrabbedObject != -1)
			{
				GamePhotoBehaviour obj = photos[currentlyGrabbedObject];
				Vector3 position2 = obj.transform.position;
				position2.x = position.x;
				position2.y = position.y;
				obj.transform.position = position2;
			}
			prevHadButton = true;
		}
		else if (AllowDraggingPhotos)
		{
			if (prevHadButton && currentlyGrabbedObject != -1)
			{
				GamePhotoBehaviour obj2 = photos[currentlyGrabbedObject];
				bool inWater = obj2.Hitbox.IsTouching(PoolHitbox);
				obj2.StopAllCoroutines();
				obj2.StartCoroutine(obj2.Drop(inWater));
				WriteInitialData();
			}
			prevHadButton = false;
			currentlyGrabbedObject = -1;
		}
		if (currentlyGrabbedObject != -1 && selectorObject.gameObject.activeSelf)
		{
			selectorObject.gameObject.SetActive(value: false);
		}
		else if (currentlyGrabbedObject == -1 && !selectorObject.gameObject.activeSelf)
		{
			selectorObject.gameObject.SetActive(value: true);
		}
	}

	private void HandleMouse()
	{
		if (selectorObject.gameObject.activeSelf)
		{
			selectorObject.gameObject.SetActive(value: false);
		}
		if (currentlyGrabbedObject != -1)
		{
			GamePhotoBehaviour obj = photos[currentlyGrabbedObject];
			bool inWater = obj.Hitbox.IsTouching(PoolHitbox);
			obj.StopAllCoroutines();
			obj.StartCoroutine(obj.Drop(inWater));
			WriteInitialData();
			currentlyGrabbedObject = -1;
			prevHadButton = false;
		}
		if (!AllowDraggingPhotos)
		{
			return;
		}
		for (int i = 0; i < photos.Length; i++)
		{
			GamePhotoBehaviour gamePhotoBehaviour = photos[i];
			switch (controller.CheckDrag(gamePhotoBehaviour.Hitbox))
			{
			case DragState.TouchStart:
				gamePhotoBehaviour.StopAllCoroutines();
				gamePhotoBehaviour.StartCoroutine(gamePhotoBehaviour.Pickup());
				FixZ(gamePhotoBehaviour);
				break;
			case DragState.Dragging:
			{
				Vector3 position = controller.DragPosition;
				position.z = gamePhotoBehaviour.transform.position.z;
				gamePhotoBehaviour.transform.position = position;
				clampToBounds(gamePhotoBehaviour);
				break;
			}
			case DragState.Released:
			{
				bool inWater2 = gamePhotoBehaviour.Hitbox.IsTouching(PoolHitbox);
				gamePhotoBehaviour.StopAllCoroutines();
				gamePhotoBehaviour.StartCoroutine(gamePhotoBehaviour.Drop(inWater2));
				WriteInitialData();
				break;
			}
			}
		}
	}

	private void clampToBounds(GamePhotoBehaviour photo)
	{
		Vector3 localPosition = photo.transform.localPosition;
		localPosition.x = Math.Max(localPosition.x, PolaroidBounds.xMin);
		localPosition.x = Math.Min(localPosition.x, PolaroidBounds.xMax);
		localPosition.y = Math.Max(localPosition.y, PolaroidBounds.yMin);
		localPosition.y = Math.Min(localPosition.y, PolaroidBounds.yMax);
		photo.transform.localPosition = localPosition;
	}

	private void FixZ(GamePhotoBehaviour current)
	{
		if (current.zOffset == (float)photos.Length / -100f)
		{
			return;
		}
		current.zOffset = (float)photos.Length / -100f;
		GamePhotoBehaviour[] array = photos;
		foreach (GamePhotoBehaviour gamePhotoBehaviour in array)
		{
			if (!(gamePhotoBehaviour == current))
			{
				gamePhotoBehaviour.zOffset += 0.01f;
				gamePhotoBehaviour.transform.SetLocalZ(1f + gamePhotoBehaviour.zOffset);
			}
		}
	}
}
