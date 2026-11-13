using System.Collections;
using TMPro;
using UnityEngine;

public class RoomTracker : MonoBehaviour
{
	public static RoomTracker Instance;

	public TextMeshPro text;

	public float SourceY = -2.5f;

	public float TargetY = -3.25f;

	private Collider2D playerCollider;

	private ContactFilter2D filter;

	private Collider2D[] buffer = new Collider2D[10];

	private Collider2D[] detectiveBuffer = new Collider2D[30];

	public PlainShipRoom LastRoom;

	private Coroutine slideInRoutine;

	public void Awake()
	{
		filter = default;
		filter.layerMask = Constants.PlayersOnlyMask;
		filter.useLayerMask = true;
		filter.useTriggers = false;
		if (!Instance)
		{
			Instance = this;
		}
	}

	public void OnDisable()
	{
		LastRoom = null;
		Vector3 localPosition = text.transform.localPosition;
		localPosition.y = TargetY;
		text.transform.localPosition = localPosition;
	}

	public void FixedUpdate()
	{
		PlainShipRoom[] array = null;
		if ((bool)LobbyBehaviour.Instance)
		{
			PlainShipRoom[] allRooms = LobbyBehaviour.Instance.AllRooms;
			array = allRooms;
		}
		if ((bool)ShipStatus.Instance)
		{
			array = ShipStatus.Instance.AllRooms;
		}
		if (array == null)
		{
			return;
		}
		PlainShipRoom plainShipRoom = null;
		if ((bool)LastRoom)
		{
			int hitCount = LastRoom.roomArea.Overlap(filter, buffer);
			if (CheckHitsForPlayer(buffer, hitCount))
			{
				plainShipRoom = LastRoom;
			}
		}
		if (!plainShipRoom)
		{
			foreach (PlainShipRoom plainShipRoom2 in array)
			{
				if ((bool)plainShipRoom2.roomArea)
				{
					int hitCount2 = plainShipRoom2.roomArea.Overlap(filter, buffer);
					if (CheckHitsForPlayer(buffer, hitCount2))
					{
						plainShipRoom = plainShipRoom2;
					}
				}
			}
		}
		if ((bool)plainShipRoom)
		{
			if (LastRoom != plainShipRoom)
			{
				LastRoom = plainShipRoom;
				if (slideInRoutine != null)
				{
					StopCoroutine(slideInRoutine);
				}
				if (plainShipRoom.RoomId != SystemTypes.Hallway)
				{
					slideInRoutine = StartCoroutine(CoSlideIn(plainShipRoom.RoomId));
				}
				else
				{
					slideInRoutine = StartCoroutine(SlideOut());
				}
			}
			else if (slideInRoutine == null && plainShipRoom.RoomId != SystemTypes.Hallway)
			{
				slideInRoutine = StartCoroutine(CoSlideIn(plainShipRoom.RoomId));
			}
		}
		else if ((bool)LastRoom)
		{
			if (slideInRoutine != null)
			{
				StopCoroutine(slideInRoutine);
			}
			slideInRoutine = StartCoroutine(SlideOut());
		}
	}

	private IEnumerator CoSlideIn(SystemTypes newRoom)
	{
		yield return SlideOut();
		Vector3 tempPos = text.transform.localPosition;
		Color tempColor = Color.white;
		text.text = DestroyableSingleton<TranslationController>.Instance.GetString(newRoom);
		float timer = 0f;
		while (timer < 0.25f)
		{
			timer = Mathf.Min(0.25f, timer + Time.deltaTime);
			float t = timer / 0.25f;
			tempPos.y = Mathf.SmoothStep(TargetY, SourceY, t);
			tempColor.a = Mathf.Lerp(0f, 1f, t);
			text.transform.localPosition = tempPos;
			text.color = tempColor;
			yield return null;
		}
	}

	private IEnumerator SlideOut()
	{
		Vector3 tempPos = text.transform.localPosition;
		Color tempColor = Color.white;
		float timer = FloatRange.ReverseLerp(tempPos.y, SourceY, TargetY) * 0.1f;
		while (timer < 0.1f)
		{
			timer = Mathf.Min(0.1f, timer + Time.deltaTime);
			float t = timer / 0.1f;
			tempPos.y = Mathf.SmoothStep(SourceY, TargetY, t);
			tempColor.a = Mathf.Lerp(1f, 0f, t);
			text.transform.localPosition = tempPos;
			text.color = tempColor;
			yield return null;
		}
	}

	private static bool CheckHitsForPlayer(Collider2D[] buffer, int hitCount, PlayerControl playerToCheck = null)
	{
		if (!PlayerControl.LocalPlayer && !playerToCheck)
		{
			return false;
		}
		if (!playerToCheck)
		{
			playerToCheck = PlayerControl.LocalPlayer;
		}
		for (int i = 0; i < hitCount; i++)
		{
			if (buffer[i].gameObject == playerToCheck.gameObject)
			{
				return true;
			}
		}
		return false;
	}

	public string GetRoomForPlayer(NetworkedPlayerInfo player)
	{
		if ((bool)LobbyBehaviour.Instance)
		{
			return string.Empty;
		}
		PlainShipRoom plainShipRoom = null;
		for (int i = 0; i < ShipStatus.Instance.AllRooms.Length; i++)
		{
			PlainShipRoom plainShipRoom2 = ShipStatus.Instance.AllRooms[i];
			if ((bool)plainShipRoom2.roomArea)
			{
				int hitCount = plainShipRoom2.roomArea.Overlap(filter, detectiveBuffer);
				if (CheckHitsForPlayer(detectiveBuffer, hitCount, player.Object))
				{
					plainShipRoom = plainShipRoom2;
				}
			}
		}
		if (plainShipRoom == null || plainShipRoom.RoomId == SystemTypes.Hallway)
		{
			if (DetectiveLocationsController.Instance?.GetPlayerLocation(player) != null)
			{
				return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DetectiveNearLocation, DestroyableSingleton<TranslationController>.Instance.GetString(DetectiveLocationsController.Instance.GetPlayerLocation(player).associatedRoom));
			}
			return DestroyableSingleton<TranslationController>.Instance.GetString(DetectiveLocationsController.Instance.DefaultLocation);
		}
		return DestroyableSingleton<TranslationController>.Instance.GetString(plainShipRoom.RoomId);
	}
}
