using System;
using Cpp2ILInjected;
using UnityEngine;

public class DraggablePartFixed : MonoBehaviour
{
	[SerializeField]
	public Collider2D collider;

	[SerializeField]
	private Transform targetPoint;

	[SerializeField]
	private GameObject brokenPart;

	[SerializeField]
	private SpriteRenderer sprite;

	[SerializeField]
	private Color finalColor;

	private Vector3 initialPosition;

	private Vector3 initialScale;

	private Action m_OnNewPositionSavedEvent;

	private Action m_OnPartPositionResetEvent;

	public event Action OnNewPositionSavedEvent
	{
		add
		{
			m_OnNewPositionSavedEvent = (Action)Delegate.Combine(m_OnNewPositionSavedEvent, value);
		}
		remove
		{
			m_OnNewPositionSavedEvent = (Action)Delegate.Remove(m_OnNewPositionSavedEvent, value);
		}
	}

	public event Action OnPartPositionResetEvent
	{
		add
		{
			m_OnPartPositionResetEvent = (Action)Delegate.Combine(m_OnPartPositionResetEvent, value);
		}
		remove
		{
			m_OnPartPositionResetEvent = (Action)Delegate.Remove(m_OnPartPositionResetEvent, value);
		}
	}

	public void Initialize()
{
		initialPosition = transform.localPosition;
		initialScale = transform.localScale;
	}

	public Collider2D GetCollider()
	{
		return collider;
	}

	public unsafe void StartDrag()
{
		transform.localScale = Vector3.one;
	}

	public unsafe void SnapIntoPlaceOrReset()
{
		if (collider.OverlapPoint(targetPoint.position) && !brokenPart.activeInHierarchy)
		{
			m_OnNewPositionSavedEvent?.Invoke();
			collider.enabled = false;
			transform.localPosition = targetPoint.localPosition;
			sprite.color = finalColor;
		}
		else
		{
			m_OnPartPositionResetEvent?.Invoke();
			transform.localPosition = initialPosition;
			transform.localScale = initialScale;
		}
	}

	public unsafe void UpdatePartPosition(Vector2 dragPosition)
{
		Vector3 position = dragPosition;
		position.z = transform.position.z;
		transform.position = position;
	}
}
