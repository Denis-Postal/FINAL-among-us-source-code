using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlideOpen : MonoBehaviour
{
	public float duration = 0.2f;

	public Button.ButtonClickedEvent OnClose = new Button.ButtonClickedEvent();

	public Camera parentCam;

	public Vector3 openPosition = Vector3.zero;

	public Vector3 closedPosition = Vector3.zero;

	public bool isOpen;

	[SerializeField]
	private GameObject subMenu;

	private Vector3 computedClosedPosition = Vector3.zero;

	public void Awake()
	{
		Camera camera = (parentCam ? parentCam : Camera.main);
		Rect safeArea = Screen.safeArea;
		float aspect = Mathf.Min(camera.aspect, safeArea.width / safeArea.height);
		AdjustPosition(aspect);
		if (subMenu != null)
		{
			subMenu.SetActive(value: false);
		}
	}

	public void Toggle()
	{
		StopAllCoroutines();
		if (isOpen)
		{
			Close();
		}
		else
		{
			Open();
		}
	}

	public void Close()
	{
		if (isOpen)
		{
			StartCoroutine(AnimateClose());
		}
	}

	public void Open()
	{
		if (!isOpen)
		{
			StartCoroutine(AnimateOpen());
		}
	}

	private void AdjustPosition(float aspect)
	{
		float safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(parentCam ? parentCam : Camera.main);
		computedClosedPosition = AspectPosition.ComputePosition(AspectPosition.EdgeAlignments.Left, closedPosition, safeOrthographicSize, aspect);
		if (!isOpen)
		{
			transform.localPosition = computedClosedPosition;
		}
	}

	private IEnumerator AnimateClose()
	{
		for (float t = 0f; t < duration; t += Time.deltaTime)
		{
			float t2 = t / duration;
			Vector3 positionVector = Vector3.Lerp(openPosition, computedClosedPosition, t2);
			SetPositionVector(positionVector);
			yield return null;
		}
		if (subMenu != null)
		{
			subMenu.SetActive(value: false);
		}
		SetPositionVector(computedClosedPosition);
		isOpen = false;
	}

	private IEnumerator AnimateOpen()
	{
		if (subMenu != null)
		{
			subMenu.SetActive(value: true);
		}
		for (float t = 0f; t < duration; t += Time.deltaTime)
		{
			float t2 = t / duration;
			Vector3 positionVector = Vector3.Lerp(computedClosedPosition, openPosition, t2);
			SetPositionVector(positionVector);
			yield return null;
		}
		isOpen = true;
		SetPositionVector(openPosition);
	}

	private void SetPositionVector(Vector3 pos)
	{
		transform.localPosition = pos;
	}

	private void OnEnable()
	{
		ResolutionManager.ResolutionChanged += OnResolutionChanged;
	}

	private void OnDisable()
	{
		ResolutionManager.ResolutionChanged -= OnResolutionChanged;
	}

	private void OnResolutionChanged(float aspectRatio, int width, int height, bool fullscreen)
	{
		AdjustPosition(aspectRatio);
	}
}
