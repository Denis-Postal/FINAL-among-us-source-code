using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class ConsoleHUDObjectsShifter : MonoBehaviour
{
	private Camera cam;

	private Vector2 screenBounds;

	private static int instanceCount;

	private void OnEnable()
	{
		instanceCount++;
	}

	private void OnDisable()
	{
		instanceCount--;
		if (instanceCount == 0 && (bool)DestroyableSingleton<HudManager>.Instance)
		{
			DestroyableSingleton<HudManager>.Instance.consoleUIHorizontalShift = 0f;
		}
	}

	private void Update()
	
	{
		if (!DestroyableSingleton<HudManager>.Instance)
		{
			return;
		}
		if (!cam)
		{
			cam = Camera.main;
		}
		if ((bool)cam)
		{
			float num = (float)Screen.width / (float)Screen.height;
			screenBounds = new Vector2(num * cam.orthographicSize, cam.orthographicSize);
			Vector3 vector = transform.position - cam.transform.position;
			if (vector.x > 0f - screenBounds.x)
			{
				DestroyableSingleton<HudManager>.Instance.consoleUIHorizontalShift = vector.x - (0f - screenBounds.x);
			}
		}
	}
}
