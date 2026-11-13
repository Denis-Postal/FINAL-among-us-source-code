using System;
using System.Linq;
using Rewired;
using UnityEngine;

public class FungleSurveillanceMinigame : Minigame
{
	private const float MAGIC_Z_INDEX = 50f;

	[SerializeField]
	private FungleSecurityCamera securityCamera;

	[SerializeField]
	private MeshRenderer viewport;

	[SerializeField]
	private Vector3 initialOffset;

	[SerializeField]
	private float cameraOrthographicSize;

	[Header("Speeds")]
	[SerializeField]
	private float keyboardMoveSpeed;

	[SerializeField]
	private float buttonMoveSpeed;

	[SerializeField]
	private float joystickMoveSpeed;

	[SerializeField]
	private float mobileJoystickMoveSpeed;

	[Header("Navigation")]
	[SerializeField]
	private PassiveButton arrowUp;

	[SerializeField]
	private PassiveButton arrowDown;

	[SerializeField]
	private PassiveButton arrowLeft;

	[SerializeField]
	private PassiveButton arrowRight;

	[SerializeField]
	private VirtualJoystick virtualJoystick;

	private Vector2 cameraSpeed;

	private void FixedUpdate()
	{
		if (!amOpening && amClosing == CloseState.None)
		{
			if (DestroyableSingleton<HudManager>.InstanceExists && DestroyableSingleton<HudManager>.Instance.joystick != null && DestroyableSingleton<HudManager>.Instance.joystick.DeltaL.sqrMagnitude > 0.01f)
			{
				SetSpeed(DestroyableSingleton<HudManager>.Instance.joystick.DeltaL * keyboardMoveSpeed);
			}
			else if (Controller.currentTouchType == Controller.TouchType.Joystick)
			{
				Vector2 axis2DRaw = ReInput.players.GetPlayer(0).GetAxis2DRaw(13, 14);
				SetSpeed(axis2DRaw * joystickMoveSpeed);
			}
			else if (virtualJoystick.DeltaR.sqrMagnitude > 0f)
			{
				SetSpeed(virtualJoystick.DeltaR * mobileJoystickMoveSpeed);
			}
			if (cameraSpeed.sqrMagnitude > 0f)
			{
				securityCamera.Move(cameraSpeed);
				cameraSpeed = Vector2.zero;
			}
		}
	}

	private void Update()
	{
		if (!amOpening)
		{
			securityCamera.CollisionsEnabled = true;
		}
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		RenderTexture temporary = RenderTexture.GetTemporary(512, 512, 24, RenderTextureFormat.ARGB32);
		securityCamera.RenderTexture = temporary;
		securityCamera.OrthographicSize = cameraOrthographicSize;
		FungleShipStatus fungleShipStatus = ShipStatus.Instance as FungleShipStatus;
		if ((bool)fungleShipStatus && fungleShipStatus.LastBinocularPos != Vector2.zero)
		{
			securityCamera.transform.localPosition = new Vector3(fungleShipStatus.LastBinocularPos.x, fungleShipStatus.LastBinocularPos.y, 50f);
		}
		else
		{
			securityCamera.transform.position = ShipStatus.Instance.AllRooms.First((PlainShipRoom room) => room.RoomId == SystemTypes.MeetingRoom).transform.position + initialOffset;
		}
		securityCamera.CollisionsEnabled = false;
		viewport.material.SetTexture("_MainTex", temporary);
		arrowDown.OnClick.AddListener(() =>
		{
			SetSpeed(new Vector2(0f, 0f - buttonMoveSpeed));
		});
		arrowUp.OnClick.AddListener(() =>
		{
			SetSpeed(new Vector2(0f, buttonMoveSpeed));
		});
		arrowLeft.OnClick.AddListener(() =>
		{
			SetSpeed(new Vector2(0f - buttonMoveSpeed, 0f));
		});
		arrowRight.OnClick.AddListener(() =>
		{
			SetSpeed(new Vector2(buttonMoveSpeed, 0f));
		});
		FungleSecurityCamera fungleSecurityCamera = securityCamera;
		fungleSecurityCamera.OnCollision = (Action<Collision2D>)Delegate.Combine(fungleSecurityCamera.OnCollision, new Action<Collision2D>(OnCameraCollision));
		FungleSecurityCamera fungleSecurityCamera2 = securityCamera;
		fungleSecurityCamera2.OnExitCollision = (Action<Collision2D>)Delegate.Combine(fungleSecurityCamera2.OnExitCollision, new Action<Collision2D>(OnCameraExitCollision));
		virtualJoystick.ToggleVisuals(on: false);
		SetupInput(disableCursor: true);
	}

	public override void Close()
	{
		base.Close();
		FungleShipStatus fungleShipStatus = ShipStatus.Instance as FungleShipStatus;
		if ((bool)fungleShipStatus)
		{
			fungleShipStatus.LastBinocularPos = new Vector2(securityCamera.transform.localPosition.x, securityCamera.transform.localPosition.y);
		}
	}

	private void SetSpeed(Vector2 speed)
	{
		cameraSpeed = speed;
	}

	private void OnCameraCollision(Collision2D col)
	{
		for (int i = 0; i < col.contactCount; i++)
		{
			ContactPoint2D contactPoint2D = col.contacts[i];
			arrowRight.gameObject.SetActive(contactPoint2D.normal.x > -0.9f);
			arrowUp.gameObject.SetActive(contactPoint2D.normal.y > -0.9f);
			arrowLeft.gameObject.SetActive(contactPoint2D.normal.x < 0.9f);
			arrowDown.gameObject.SetActive(contactPoint2D.normal.y < 0.9f);
		}
	}

	private void OnCameraExitCollision(Collision2D col)
	{
		arrowRight.gameObject.SetActive(value: true);
		arrowUp.gameObject.SetActive(value: true);
		arrowLeft.gameObject.SetActive(value: true);
		arrowDown.gameObject.SetActive(value: true);
	}
}
