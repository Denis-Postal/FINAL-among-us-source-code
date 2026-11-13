using Rewired;
using Rewired.ControllerExtensions;
using UnityEngine;

public class VirtualCursor : MonoBehaviour
{
	public float speed = 5f;

	private float currentSpeed;

	public float acceleration = 1f;

	public Vector2 screenBounds;

	public Vector3 position;

	private const float deadzone = 0.03f;

	private const float touchpadSensitivity = 4f;

	public Camera cam;

	public static Vector2 currentPosition;

	public static bool buttonDown;

	public static bool joystickMoved;

	public static VirtualCursor instance;

	public static int horizontalAxis = 9;

	public static int verticalAxis = 10;

	private int framesVisible;

	private const int minFramesToAppear = 3;

	private SpriteRenderer sr;

	private Vector2 prevTouchPos;

	private bool setTouchPos;

	public static bool CursorActive
	{
		get
		{
			if ((bool)instance && instance.isActiveAndEnabled)
			{
				return SpecialInputHandler.disableVirtualCursorCount == 0;
			}
			return false;
		}
	}

	private void Awake()
	{
		if ((bool)instance)
		{
			Object.Destroy(gameObject);
			return;
		}
		instance = this;
		Object.DontDestroyOnLoad(gameObject);
		sr = GetComponentInChildren<SpriteRenderer>(includeInactive: true);
	}

	private void OnEnable()
	{
		cam = Camera.main;
		if ((bool)cam)
		{
			SetScreenPosition(Vector2.zero);
		}
		framesVisible = 0;
		sr.enabled = false;
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	private void Start()
	{
		cam = Camera.main;
		position = transform.position;
		if ((bool)cam)
		{
			float num = (float)Screen.width / (float)Screen.height;
			screenBounds = new Vector2(num * cam.orthographicSize, cam.orthographicSize);
		}
	}

	public void SetWorldPosition(Vector2 worldPos)
	{
		Vector3 vector = (transform.position = worldPos);
		currentPosition = vector;
		position = worldPos - new Vector2(cam.transform.position.x, cam.transform.position.y);
	}

	public void SetScreenPosition(Vector2 screenPos)
	{
		position = screenPos;
		Vector3 vector = (transform.position = position + cam.transform.position);
		currentPosition = vector;
	}

	private void Update()
	{
		Player player = ReInput.players.GetPlayer(0);
		Vector3 vector = new Vector3(player.GetAxis(horizontalAxis), player.GetAxis(verticalAxis), 0f);
		joystickMoved = vector.x != 0f || vector.y != 0f;
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick && SpecialInputHandler.count == 0)
		{
			if (framesVisible < 3)
			{
				framesVisible++;
			}
			else
			{
				sr.enabled = true;
			}
		}
		else
		{
			framesVisible = 0;
			sr.enabled = false;
		}
		if (ControllerManager.Instance.IsUiControllerActive)
		{
			gameObject.SetActive(value: false);
			return;
		}
		if (player.controllers.joystickCount > 0)
		{
			IDualShock4Extension extension = player.controllers.Joysticks[0].GetExtension<IDualShock4Extension>();
			if (extension != null && extension.touchCount > 0)
			{
				// Rewired shim (unity_shims/Rewired.cs) returns the touch position instead of using a ref parameter.
				Vector2 vector2 = extension.GetTouchPosition(0);
				float num = (float)Screen.width / (float)Screen.height;
				if (setTouchPos)
				{
					Vector2 vector3 = vector2 - prevTouchPos;
					position.x += vector3.x * (4f * num);
					position.y += vector3.y * 4f;
				}
				setTouchPos = true;
				prevTouchPos = vector2;
			}
			else
			{
				setTouchPos = false;
			}
		}
		if (vector.magnitude > 0.03f)
		{
			currentSpeed += Time.deltaTime * acceleration;
			if (currentSpeed > speed)
			{
				currentSpeed = speed;
			}
			position += vector * Time.deltaTime * currentSpeed;
		}
		else
		{
			currentSpeed = 0f;
		}
		if (!cam)
		{
			cam = Camera.main;
		}
		if ((bool)cam)
		{
			float num2 = (float)Screen.width / (float)Screen.height;
			screenBounds = new Vector2(num2 * cam.orthographicSize, cam.orthographicSize);
			position.x = Mathf.Clamp(position.x, 0f - screenBounds.x, screenBounds.x);
			position.y = Mathf.Clamp(position.y, 0f - screenBounds.y, screenBounds.y);
			Vector3 vector4 = (transform.position = position + cam.transform.position);
			currentPosition = vector4;
		}
		buttonDown = player.GetButton(11);
	}
}
