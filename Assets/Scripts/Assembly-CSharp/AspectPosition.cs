using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

public class AspectPosition : MonoBehaviour
{
	public enum EdgeAlignments
	{
		RightBottom = 6,
		LeftBottom = 5,
		RightTop = 10,
		Left = 1,
		Right = 2,
		Top = 8,
		Bottom = 4,
		LeftTop = 9,
		Center = 16
	}

	public Camera parentCam;

	private const int LeftFlag = 1;

	private const int RightFlag = 2;

	private const int BottomFlag = 4;

	private const int TopFlag = 8;

	private const int CenterFlag = 16;

	public bool updateAlways;

	public Vector2 anchorPoint= new Vector2(0.5f, 0.5f) ;

	public Vector3 DistanceFromEdge;

	public EdgeAlignments Alignment;

	public void Update()
	{
		if (updateAlways)
		{
			AdjustPosition();
		}
	}

	private void OnEnable()
	{
		parentCam = (parentCam ? parentCam : Camera.main);
		AdjustPosition();
		ResolutionManager.ResolutionChanged += OnResolutionChanged;
	}

	internal unsafe void SetNormalizedX(float nx, float widthPadding)
	{
		float num = CameraSafeArea.GetSafeOrthographicSize(parentCam) * parentCam.aspect + widthPadding;
		float x = DistanceFromEdge.x;
		Vector3 localPosition = transform.localPosition;
		localPosition.x = Mathf.Lerp(x, num * 2f - x, nx);
		int alignment = (int)Alignment;
		if ((alignment & 1) != 0)
		{
			localPosition.x -= num;
		}
		else if ((alignment & 2) != 0)
		{
			localPosition.x = num - localPosition.x;
		}
		transform.localPosition = localPosition;
	}

	private void OnDisable()
	{
		ResolutionManager.ResolutionChanged -= OnResolutionChanged;
	}

	public void AdjustPosition()
	{
		Rect safeArea = Screen.safeArea;
		float aspect = Mathf.Min((parentCam ? parentCam : Camera.main).aspect, safeArea.width / safeArea.height);
		AdjustPosition(aspect);
	}

	private void OnResolutionChanged(float aspectRatio, int width, int height, bool fullscreen)
	{
		AdjustPosition(aspectRatio);
	}

	public unsafe void AdjustPosition(float aspect)
	{
		Camera cam = (parentCam ? parentCam : Camera.main);
		float safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(cam);
		if (Alignment == EdgeAlignments.Center)
		{
			transform.localPosition = ComputePositionAnchor(cam, DistanceFromEdge, anchorPoint, aspect);
		}
		else
		{
			transform.localPosition = ComputePosition(Alignment, DistanceFromEdge, safeOrthographicSize, aspect);
		}
	}

	public unsafe static Vector3 ComputeWorldPosition(Camera cam, EdgeAlignments alignment, Vector3 relativePos)
	{
		Rect safeArea = Screen.safeArea;
		float aspect = Mathf.Min(cam.aspect, safeArea.width / safeArea.height);
		float safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(cam);
		return cam.transform.TransformPoint(ComputePosition(alignment, relativePos, safeOrthographicSize, aspect));
	}

	public unsafe static Vector3 ComputePosition(EdgeAlignments alignment, Vector3 relativePos)
	{
		Rect safeArea = Screen.safeArea;
		Camera main = Camera.main;
		float aspect = Mathf.Min(main.aspect, safeArea.width / safeArea.height);
		float safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(main);
		return ComputePosition(alignment, relativePos, safeOrthographicSize, aspect);
	}

	public unsafe static Vector3 ComputePosition(EdgeAlignments alignment, Vector3 relativePos, float cHeight, float aspect)
	{ // Recovered from decomp9 and native 0x180625860; matches the 2019 edge formulas.
		float num = cHeight * aspect;
		Vector3 result = relativePos;
		if ((alignment & EdgeAlignments.Left) != 0)
		{
			result.x -= num;
		}
		else if ((alignment & EdgeAlignments.Right) != 0)
		{
			result.x = num - result.x;
		}
		if ((alignment & EdgeAlignments.Bottom) != 0)
		{
			result.y -= cHeight;
		}
		else if ((alignment & EdgeAlignments.Top) != 0)
		{
			result.y = cHeight - result.y;
		}
		return result;
	}

	public unsafe static Vector3 ComputePositionSafeArea(EdgeAlignments alignment, Vector3 relativePos, float cHeight, float aspect, Camera cam)
	{
		float num = cHeight * aspect;
		Vector3 result = relativePos;
		if ((alignment & EdgeAlignments.Left) != 0)
		{
			result.x -= num;
			Vector3 vector = cam.ScreenToWorldPoint(new Vector3(Screen.safeArea.xMin, 0f, 0f)) - cam.ScreenToWorldPoint(Vector3.zero);
			result.x += vector.x / 2f;
		}
		else if ((alignment & EdgeAlignments.Right) != 0)
		{
			result.x = num - result.x;
			Vector3 vector = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)) - cam.ScreenToWorldPoint(new Vector3(Screen.safeArea.xMax, 0f, 0f));
			result.x -= vector.x / 2f;
		}
		if ((alignment & EdgeAlignments.Bottom) != 0)
		{
			result.y -= cHeight;
			Vector3 vector = cam.ScreenToWorldPoint(new Vector3(0f, Screen.safeArea.yMin, 0f)) - cam.ScreenToWorldPoint(Vector3.zero);
			result.y += vector.y / 2f;
		}
		else if ((alignment & EdgeAlignments.Top) != 0)
		{
			result.y = cHeight - result.y;
			Vector3 vector = cam.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)) - cam.ScreenToWorldPoint(new Vector3(0f, Screen.safeArea.yMax, 0f));
			result.y += vector.y / 2f;
		}
		return result;
	}

	public unsafe Vector3 ComputePositionAnchor(Camera cam, Vector3 relativePos, Vector2 anchor, float aspect)
	{
		float num = cam.orthographicSize * 2f;
		float num2 = num * aspect;
		Vector3 vector = new Vector3(num2 * anchor.x, num * anchor.y, 0f);
		Vector3 vector2 = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
		Vector3 result = vector + vector2 + relativePos;
		result -= cam.transform.position;
		result.z = relativePos.z;
		Vector3 vector3 = cam.ScreenToWorldPoint(new Vector3(Screen.safeArea.xMin, 0f, 0f)) - cam.ScreenToWorldPoint(Vector3.zero);
		result.x += vector3.x / 2f;
		return result;
	}

	public AspectPosition()
	{ }
}
