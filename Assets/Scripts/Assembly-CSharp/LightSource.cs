using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using UnityEngine;

public class LightSource : MonoBehaviour
{
	public const string FLASHLIGHT_KEYWORD = "FLASHLIGHT";

	private const float FLASHLIGHT_LERP_SPEED = 5f;

	private const float FLASHLIGHT_LERP_SPEED_CONSOLE = 10f;

	private const float DEFAULT_PLAYER_RADIUS = 0.5f;

	public static Dictionary<GameObject, NoShadowBehaviour> NoShadows= new Dictionary<GameObject, NoShadowBehaviour>();

	public static Dictionary<GameObject, OneWayShadows> OneWayShadows= new Dictionary<GameObject, OneWayShadows>();

	[SerializeField]
	public float viewDistance = 3f;

	[SerializeField]
	public Material lightCutawayMaterial;

	[SerializeField]
	public Mesh lightChildMesh;

	[SerializeField]
	private LightSourceRendererType rendererType;

	[SerializeField]
	private bool useFlashlight;

	[SerializeField]
	public Material gpuShadowCasterMaterial;

	[SerializeField]
	public int gpuShadowmapResolution = 2048;

	[SerializeField]
	public RenderTextureFormat gpuPreferredRTFormat = RenderTextureFormat.RHalf;

	[SerializeField]
	public int raycastMinRayCount = 16;

	[SerializeField]
	public float raycastTolerance = 0.001f;

	private LightSourceRenderer renderer;

	public GameObject lightChild;

	public MeshFilter lightChildMeshFilter;

	private Controller controller= new Controller();

	private float flashlightSize= 0.25f;

	private Vector2 lastFlashlightDirection;

	private Vector3 lightOffset;

	private Transform touchFlashlightTarget;

	public float ViewDistance => viewDistance;

	public float PlayerRadius => 0.5f;

	public Material LightCutawayMaterial => lightCutawayMaterial;

	public GameObject LightChild => lightChild;

	public Mesh LightChildMesh => lightChildMesh;

	public MeshFilter LightChildMeshFilter => lightChildMeshFilter;

	public unsafe Vector3 LightOffset => lightOffset;

	public float FlashlightSize => CalculateFlashlightSize();

	public Material GpuShadowCasterMaterial => gpuShadowCasterMaterial;

	public int GpuShadowmapResolution => gpuShadowmapResolution;

	public RenderTextureFormat GpuPreferredRTFormat => gpuPreferredRTFormat;

	public int RaycastMinimumRayCount => raycastMinRayCount;

	public float RaycastTolerance => raycastTolerance;

	private void Awake()
	{
		GameObject gameObject = new GameObject("LightChild");
		gameObject.layer = 10;
		lightChildMeshFilter = gameObject.AddComponent<MeshFilter>();
		lightCutawayMaterial = new Material(lightCutawayMaterial);
		lightCutawayMaterial.DisableKeyword("FLASHLIGHT");
		gameObject.AddComponent<MeshRenderer>().sharedMaterial = lightCutawayMaterial;
		lightChild = gameObject;
	}

	private unsafe void Update()
	
	{
		Vector3 position = transform.position;
		position.z -= 7f;
		UpdateFlashlightAngle();
		LightCutawayMaterial.SetFloat("_PlayerRadius", PlayerRadius);
		LightCutawayMaterial.SetFloat("_LightRadius", ViewDistance);
		LightCutawayMaterial.SetVector("_LightOffset", LightOffset);
		LightCutawayMaterial.SetFloat("_FlashlightSize", FlashlightSize);
		LightCutawayMaterial.SetFloat("_FlashlightAngle", PlayerControl.LocalPlayer.FlashlightAngle);
		lightChild.transform.position = position;
		renderer.Render(position);
	}

	public unsafe void Initialize(Vector3 lightOffset)
	
	{
		transform.localPosition = lightOffset;
		this.lightOffset = lightOffset;
		UpdateShadowType();
		SetFlashlightEnabled(enabled: false);
	}

	public void SetViewDistance(float distance)
	{
		viewDistance = distance;
	}

	public void SetupLightingForGameplay(bool enableFlashlight, float flashlightSize, Transform touchFlashlightTarget)
	{
		this.flashlightSize = flashlightSize;
		this.touchFlashlightTarget = touchFlashlightTarget.transform;
		SetFlashlightEnabled(enableFlashlight);
	}

	private float CalculateFlashlightSize()
	{
		float num = ViewDistance + PlayerRadius / 2f - 0.001f;
		return Mathf.Clamp(flashlightSize * 2f * num, 0f, num);
	}

	private void SetFlashlightEnabled(bool enabled)
	{
		useFlashlight = enabled;
		if (enabled)
		{
			LightCutawayMaterial.EnableKeyword("FLASHLIGHT");
		}
		else
		{
			LightCutawayMaterial.DisableKeyword("FLASHLIGHT");
		}
	}

	private void UpdateShadowType()
	{
		if (rendererType == LightSourceRendererType.Auto)
		{
			rendererType = LightSourceRenderer.GetPlatformType();
		}
		if (renderer == null || renderer.Type != rendererType)
		{
			if (renderer != null)
			{
				renderer.Dispose();
			}
			renderer = LightSourceRenderer.Create(rendererType, this);
			renderer.Initialize();
			SetFlashlightEnabled(useFlashlight);
		}
	}

	private void UpdateFlashlightAngle()
	{
		if (PlayerControl.LocalPlayer != null)
		{
			controller.Update();
			PlayerControl.LocalPlayer.FlashlightAngle = GetFlashlightAngle();
		}
	}

	private unsafe float GetFlashlightAngle()
	
	{
		switch (DataManager.Settings.Input.InputMode)
		{
		case ControlTypes.Keyboard:
			if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Keyboard)
			{
				return GetKeyboardMouseFAngle();
			}
			return GetControllerFAngle();
		case ControlTypes.Controller:
			return GetControllerFAngle();
		case ControlTypes.VirtualJoystick:
			return GetVirtualJFAngle();
		case ControlTypes.ScreenJoystick:
			return GetScreenFAngle();
		default:
		{
			Vector2 normalized = PlayerControl.LocalPlayer.MyPhysics.Velocity.normalized;
			Vector3 vector = Vector3.Slerp(lastFlashlightDirection, normalized, Time.deltaTime * 5f);
			lastFlashlightDirection = vector;
			return Mathf.Atan2(vector.y, vector.x);
		}
		}
	}

	private unsafe float GetVirtualJFAngle()
	{
		float result = 0f;
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			result = GetControllerFAngle();
		}
		else if (DestroyableSingleton<HudManager>.Instance.joystickR != null)
		{
			Vector2 deltaR = DestroyableSingleton<HudManager>.Instance.joystickR.DeltaR;
			Vector3 vector = Vector3.Slerp(lastFlashlightDirection, deltaR, Time.deltaTime * 10f);
			lastFlashlightDirection = vector;
			result = Mathf.Atan2(vector.y, vector.x);
		}
		return result;
	}

	private float GetKeyboardMouseFAngle()
	{
		Vector3 vector = controller.HoverPosition;
		Vector3 normalized = (transform.position - LightOffset - vector).normalized;
		return Mathf.Atan2(0f - normalized.y, 0f - normalized.x);
	}

	private unsafe float GetControllerFAngle()
	{
		Vector2 vector = ((!DestroyableSingleton<HudManager>.InstanceExists || DestroyableSingleton<HudManager>.Instance.joystick == null) ? Vector2.zero : DestroyableSingleton<HudManager>.Instance.joystick.DeltaR);
		Vector3 vector2 = Vector3.Slerp(lastFlashlightDirection, vector, Time.deltaTime * 10f);
		lastFlashlightDirection = vector2;
		return Mathf.Atan2(vector2.y, vector2.x);
	}

	private unsafe float GetScreenFAngle()
	{
		float result;
		if (!(touchFlashlightTarget != null))
		{
			result = ((ActiveInputManager.currentControlType != ActiveInputManager.InputType.Joystick) ? Mathf.Atan2(lastFlashlightDirection.y, lastFlashlightDirection.x) : GetControllerFAngle());
		}
		else if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			result = GetControllerFAngle();
			if (touchFlashlightTarget.gameObject.activeSelf)
			{
				touchFlashlightTarget.gameObject.SetActive(value: false);
			}
		}
		else if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Keyboard)
		{
			if (!touchFlashlightTarget.gameObject.activeSelf)
			{
				touchFlashlightTarget.gameObject.SetActive(value: true);
			}
			result = GetAngleDot();
		}
		else
		{
			int num = 0;
			Controller.TouchState[] touches = controller.Touches;
			for (int i = 0; i < touches.Length; i++)
			{
				if (touches[i].IsDown)
				{
					num++;
				}
			}
			if (num > 1)
			{
				if (!touchFlashlightTarget.gameObject.activeSelf)
				{
					Controller.TouchState touch = controller.GetTouch(1);
					touchFlashlightTarget.position = new Vector3(touch.Position.x, touch.Position.y, touchFlashlightTarget.position.z);
					touchFlashlightTarget.gameObject.SetActive(value: true);
				}
				result = GetAngleDot();
			}
			else
			{
				result = GetAngleMove();
				if (touchFlashlightTarget.gameObject.activeSelf)
				{
					touchFlashlightTarget.gameObject.SetActive(value: false);
				}
			}
		}
		return result;
	}

	private float GetAngleDot()
	{
		lastFlashlightDirection = (touchFlashlightTarget.position - transform.position).normalized;
		return Mathf.Atan2(lastFlashlightDirection.y, lastFlashlightDirection.x);
	}

	private unsafe float GetAngleMove()
	{
		Vector2 normalized = PlayerControl.LocalPlayer.MyPhysics.Velocity.normalized;
		Vector3 vector = Vector3.Slerp(lastFlashlightDirection, normalized, Time.deltaTime * 5f);
		lastFlashlightDirection = vector;
		return Mathf.Atan2(vector.y, vector.x);
	}

	public LightSource()
	{ }

	static LightSource()
	{ }
}
