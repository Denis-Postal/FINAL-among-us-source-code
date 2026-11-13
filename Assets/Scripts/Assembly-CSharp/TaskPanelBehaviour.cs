using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class TaskPanelBehaviour : MonoBehaviour
{
	[SerializeField]
	private Vector3 openPosition;

	[SerializeField]
	private Vector3 closedPosition;

	[SerializeField]
	private SpriteRenderer background;

	[SerializeField]
	private SpriteRenderer tab;

	[SerializeField]
	private TextMeshPro taskText;

	[SerializeField]
	private float animationTimeSeconds;

	private bool open = true;

	private float timer;

	public void ToggleOpen()
	{
		open = !open;
	}

	private unsafe void Update()
{
		background.transform.localScale = ((taskText.textBounds.size.x > 0f) ? new Vector3(taskText.textBounds.size.x + 0.2f, taskText.textBounds.size.y + 0.2f, 1f) : Vector3.zero);
		Vector3 extents = background.sprite.bounds.extents;
		extents.y = 0f - extents.y;
		extents = extents.Mul(background.transform.localScale);
		background.transform.localPosition = extents;
		Vector3 extents2 = tab.sprite.bounds.extents;
		extents2 = extents2.Mul(tab.transform.localScale);
		extents2.y = 0f - extents2.y;
		extents2.x += extents.x * 2f;
		tab.transform.localPosition = extents2;
		if (!(GameManager.Instance == null))
		{
			if (GameManager.Instance.IsHideAndSeek())
			{
				closedPosition.y = (openPosition.y = 1.6f);
			}
			else
			{
				closedPosition.y = (openPosition.y = 0.6f);
			}
			closedPosition.x = (0f - background.sprite.bounds.size.x) * background.transform.localScale.x;
			if (open)
			{
				timer = Mathf.Min(1f, timer + Time.deltaTime / animationTimeSeconds);
			}
			else
			{
				timer = Mathf.Max(0f, timer - Time.deltaTime / animationTimeSeconds);
			}
			Vector3 relativePos = new Vector3(Mathf.SmoothStep(closedPosition.x, openPosition.x, timer), Mathf.SmoothStep(closedPosition.y, openPosition.y, timer), openPosition.z);
			transform.localPosition = AspectPosition.ComputePosition(AspectPosition.EdgeAlignments.LeftTop, relativePos);
		}
	}

	public void SetTaskText(string str)
{
		taskText.text = str;
	}
}
