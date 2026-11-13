using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using UnityEngine;

public class MapBehaviour : MonoBehaviour
{
	public static MapBehaviour Instance;

	public AlphaPulse ColorControl;

	public SpriteRenderer HerePoint;

	public SpriteRenderer TrackedHerePoint;

	public MapCountOverlay countOverlay;

	public InfectedOverlay infectedOverlay;

	public MapTaskOverlay taskOverlay;

	[SerializeField]
	private GameObject fadedBackground;

	[SerializeField]
	private GameObject detectiveMapButtonPrefab;

	private bool detectiveMapOpen;

	private SpecialInputHandler specialInputHandler;

	private bool countOverlayAllowsMovement;

	private Vector3? preMeetingPosition;

	private Vector3? trackedPreMeetingPosition;

	private int trackedColorID;

	private float trackedPointDelayTime;

	private List<UiElement> detectiveLocationControllerButtons = new List<UiElement>();

	public bool IsOpen => isActiveAndEnabled;

	public bool IsOpenStopped
	{
		get
		{
			if (IsOpen && countOverlay.isActiveAndEnabled)
			{
				return !countOverlayAllowsMovement;
			}
			return false;
		}
	}

	public void SetPreMeetingPosition(Vector3 preMeetingPosition, bool trackedPlayer = false)
	{
		if (trackedPlayer)
		{
			trackedPreMeetingPosition = preMeetingPosition;
		}
		else
		{
			this.preMeetingPosition = preMeetingPosition;
		}
	}

	public void Show(MapOptions opts)
	{
		if (IsOpen)
		{
			Close();
		}
		else if (PlayerControl.LocalPlayer.CanMove || (bool)MeetingHud.Instance || opts.Mode == MapOptions.Modes.Detective)
		{
			Logger.GlobalInstance.Debug($"GAMEPLAY::MapBehaviour::Showing map in mode {opts.Mode}");
			switch (opts.Mode)
			{
			case MapOptions.Modes.Normal:
				ShowNormalMap();
				break;
			case MapOptions.Modes.CountOverlay:
				ShowCountOverlay(opts.AllowMovementWhileMapOpen, opts.ShowLivePlayerPosition, opts.IncludeDeadBodies);
				break;
			case MapOptions.Modes.Sabotage:
				ShowSabotageMap();
				break;
			case MapOptions.Modes.Detective:
				ShowDetectiveMap();
				break;
			default:
				throw new ArgumentOutOfRangeException($"Unexpected map mode {opts.Mode}");
			}
		}
	}

	public void Close()
	{
		Logger.GlobalInstance.Debug("GAMEPLAY::MapBehaviour::Closing map");
		gameObject.SetActive(value: false);
		countOverlay.gameObject.SetActive(value: false);
		infectedOverlay.gameObject.SetActive(value: false);
		taskOverlay.Hide();
		HerePoint.enabled = true;
		DestroyableSingleton<HudManager>.Instance.DeselectMapButton();
		if (!MeetingHud.Instance)
		{
			DestroyableSingleton<HudManager>.Instance.SetHudActive(isActive: true);
		}
		if (detectiveMapOpen)
		{
			foreach (UiElement detectiveLocationControllerButton in detectiveLocationControllerButtons)
			{
				UnityEngine.Object.Destroy(detectiveLocationControllerButton.gameObject);
			}
			detectiveLocationControllerButtons.Clear();
			UnityEngine.Object.FindObjectOfType<DetectiveNotesMinigame>()?.MapClosed();
			detectiveMapOpen = false;
		}
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public void DisableTrackerOverlays()
	{
		Logger.GlobalInstance.Debug("GAMEPLAY::MapBehaviour::Closing Tracker Overlays");
		if (TrackedHerePoint != null)
		{
			TrackedHerePoint.gameObject.SetActive(value: false);
		}
	}

	private void Validate()
	{
		countOverlay.CountAreas.ForEach((CounterArea area) =>
		{
			if (area == null)
			{
				Debug.LogError("One of the count areas in the count overlay is null");
			}
			else if (ShipStatus.Instance.AllRooms.All((PlainShipRoom r) => r.RoomId != area.RoomType))
			{
				Debug.LogError($"There was a count area for a room that isn't in the current ship: {area.RoomType}");
			}
		});
		infectedOverlay.rooms.ForEach((MapRoom area) =>
		{
			if (area == null)
			{
				Debug.LogError("One of the sabotage areas in the sabotage overlay is null");
			}
		});
	}

	private void ShowSabotageMap()
	{
		if (IsOpen)
		{
			Close();
		}
		else if (PlayerControl.LocalPlayer.CanMove)
		{
			if (specialInputHandler != null)
			{
				specialInputHandler.disableVirtualCursor = true;
			}
			PlayerControl.LocalPlayer.SetPlayerMaterialColors(HerePoint);
			GenericShow();
			infectedOverlay.gameObject.SetActive(value: true);
			ColorControl.SetColor(Palette.ImpostorRed);
			taskOverlay.Show();
			DestroyableSingleton<HudManager>.Instance.SetHudActive(isActive: false);
			ConsoleJoystick.SetMode_Sabotage();
		}
	}

	private void ShowNormalMap()
	{
		if (IsOpen)
		{
			Close();
		}
		else
		{
			if (!PlayerControl.LocalPlayer.CanMove && !MeetingHud.Instance)
			{
				return;
			}
			PlayerControl.LocalPlayer.SetPlayerMaterialColors(HerePoint);
			if (PlayerControl.LocalPlayer.isTrackingPlayer)
			{
				if (PlayerControl.LocalPlayer.trackedPlayer.Data.Disconnected)
				{
					TrackedHerePoint.gameObject.SetActive(value: false);
				}
				else
				{
					SetTrackedHerePointColor(PlayerControl.LocalPlayer.trackedPlayerColorID);
					TrackedHerePoint.gameObject.SetActive(value: true);
					UpdateTrackedPosition();
				}
			}
			GenericShow();
			taskOverlay.Show();
			ColorControl.SetColor(new Color(0.05f, 0.2f, 1f, 1f));
			DestroyableSingleton<HudManager>.Instance.SetHudActive(isActive: false);
		}
	}

	private void ShowDetectiveMap()
	{
		if (IsOpen)
		{
			Close();
			return;
		}
		detectiveMapOpen = true;
		if (specialInputHandler != null)
		{
			specialInputHandler.disableVirtualCursor = true;
		}
		PlayerControl.LocalPlayer.SetPlayerMaterialColors(HerePoint);
		GenericShow();
		detectiveLocationControllerButtons.Clear();
		countOverlay.CountAreas.ForEach((CounterArea area) =>
		{
			if (!area.name.Contains("Hallway"))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(detectiveMapButtonPrefab, area.transform.position, Quaternion.identity, transform.parent);
				gameObject.GetComponent<DetectiveLocationButton>().SetInfo(UnityEngine.Object.FindObjectOfType<DetectiveNotesMinigame>(), DestroyableSingleton<TranslationController>.Instance.GetString(area.RoomType));
				detectiveLocationControllerButtons.Add(gameObject.GetComponentInChildren<PassiveButton>());
			}
		});
		ControllerManager.Instance.OpenOverlayMenu(name, null, detectiveLocationControllerButtons[0], detectiveLocationControllerButtons);
		ColorControl.SetColor(new Color(0.05f, 0.2f, 1f, 1f));
		DestroyableSingleton<HudManager>.Instance.SetHudActive(isActive: false);
	}

	private void ShowCountOverlay(bool allowedToMove, bool showLivePlayerPosition, bool includeDeadBodies)
	{
		GenericShow();
		countOverlay.gameObject.SetActive(value: true);
		countOverlay.SetOptions(showLivePlayerPosition, includeDeadBodies);
		countOverlayAllowsMovement = allowedToMove;
		taskOverlay.Hide();
		HerePoint.enabled = !showLivePlayerPosition;
		TrackedHerePoint.gameObject.SetActive(value: false);
		if (HerePoint.enabled)
		{
			PlayerControl.LocalPlayer.SetPlayerMaterialColors(HerePoint);
		}
		DestroyableSingleton<HudManager>.Instance.SetHudActive(isActive: false);
	}

	private void FixedUpdate()
	{
		if (!ShipStatus.Instance)
		{
			return;
		}
		Vector3 localPosition = PlayerControl.LocalPlayer.transform.position;
		if ((bool)MeetingHud.Instance && preMeetingPosition.HasValue)
		{
			localPosition = preMeetingPosition.Value;
		}
		else if (preMeetingPosition.HasValue)
		{
			preMeetingPosition = null;
		}
		localPosition /= ShipStatus.Instance.MapScale;
		localPosition.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
		localPosition.z = -1f;
		HerePoint.transform.localPosition = localPosition;
		if (PlayerControl.LocalPlayer.isTrackingPlayer)
		{
			trackedPointDelayTime += Time.deltaTime;
			if (trackedPointDelayTime >= GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDelay))
			{
				UpdateTrackedPosition();
				trackedPointDelayTime = 0f;
			}
		}
	}

	private void Awake()
	{
		Instance = this;
		specialInputHandler = GetComponent<SpecialInputHandler>();
	}

	private void GenericShow()
	{
		transform.localScale = Vector3.one;
		transform.localPosition = new Vector3(0f, 0f, -25f);
		Vector3 localScale = taskOverlay.transform.localScale;
		if (Mathf.Sign(localScale.x) != Mathf.Sign(ShipStatus.Instance.transform.localScale.x))
		{
			localScale.x *= -1f;
		}
		taskOverlay.transform.localScale = localScale;
		fadedBackground.SetActive(MeetingHud.Instance);
		HerePoint.material.SetInt(PlayerMaterial.MaskLayer, 255);
		TrackedHerePoint.material.SetInt(PlayerMaterial.MaskLayer, 255);
		gameObject.SetActive(value: true);
	}

	public void SetTrackedHerePointColor(int colorID)
	{
		trackedColorID = colorID;
		PlayerMaterial.SetColors(trackedColorID, TrackedHerePoint);
	}

	private void UpdateTrackedPosition()
	{
		if (!PlayerControl.LocalPlayer.trackedPlayer.Data.IsDead && !PlayerControl.LocalPlayer.trackedPlayer.Data.Disconnected)
		{
			Vector3 localPosition = PlayerControl.LocalPlayer.trackedPlayer.transform.position;
			if ((bool)MeetingHud.Instance && trackedPreMeetingPosition.HasValue)
			{
				localPosition = trackedPreMeetingPosition.Value;
			}
			else if (trackedPreMeetingPosition.HasValue)
			{
				trackedPreMeetingPosition = null;
			}
			localPosition /= ShipStatus.Instance.MapScale;
			localPosition.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
			localPosition.z = -1f;
			TrackedHerePoint.transform.localPosition = localPosition;
		}
	}
}
