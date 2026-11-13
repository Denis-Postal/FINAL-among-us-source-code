using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EdgeTransition : MonoBehaviour
{
	public float Duration = 0.2f;

	public Vector3 OpenPosition;

	public Vector3 ClosedPosition;

	public AspectPosition.EdgeAlignments Alignment;

	public Button.ButtonClickedEvent OnClose = new Button.ButtonClickedEvent();

	public void Awake()
	{
		transform.localPosition = AspectPosition.ComputePosition(Alignment, ClosedPosition);
	}

	public void Open()
	{
		gameObject.SetActive(value: true);
		StopAllCoroutines();
		StartCoroutine(CoOpen());
	}

	private IEnumerator CoOpen()
	{
		Vector3 sourcePos = transform.localPosition;
		Vector3 targetPos = AspectPosition.ComputePosition(Alignment, OpenPosition);
		for (float timer = 0f; timer < Duration; timer += Time.deltaTime)
		{
			Vector3 localPosition = Vector3.Lerp(sourcePos, targetPos, timer / Duration);
			transform.localPosition = localPosition;
			yield return null;
		}
		transform.localPosition = targetPos;
	}

	public void Close()
	{
		if (isActiveAndEnabled)
		{
			StopAllCoroutines();
			StartCoroutine(CoClose());
		}
	}

	private IEnumerator CoClose()
	{
		Vector3 sourcePos = transform.localPosition;
		Vector3 targetPos = AspectPosition.ComputePosition(Alignment, ClosedPosition);
		for (float timer = 0f; timer < Duration; timer += Time.deltaTime)
		{
			Vector3 localPosition = Vector3.Lerp(sourcePos, targetPos, timer / Duration);
			transform.localPosition = localPosition;
			yield return null;
		}
		transform.localPosition = targetPos;
		OnClose.Invoke();
	}
}
