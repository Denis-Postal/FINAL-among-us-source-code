using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;

public class MouseCursorForConsole : MonoBehaviour
{
	public float speed = 5f;

	public float acceleration = 1f;

	public Vector2 screenBounds;

	public Vector3 position;

	public Camera cam;

	public static Vector2 currentPosition;

	public static bool buttonDown;

	public static bool joystickMoved;

	public static MouseCursorForConsole instance;

	public static int horizontalAxis = 9;

	public static int verticalAxis = 10;

	private int framesVisible;

	private const int minFramesToAppear = 3;

	private SpriteRenderer sr;

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

	public void SetScreenPosition(Vector2 screenPos)
	{
		position = screenPos;
		Vector3 vector = (transform.position = position + cam.transform.position);
		currentPosition = vector;
	}

	private void Update()
	{
		sr.enabled = false;
	}
}
