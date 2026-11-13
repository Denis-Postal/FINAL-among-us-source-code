using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;

public class DraggablePart : MonoBehaviour
{
	[SerializeField]
	private Collider2D collider;

	[SerializeField]
	private SpriteRenderer targetSprite;

	private Vector3 initialPosition;

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
	}

	public Collider2D GetCollider()
	{
		return collider;
	}

	public unsafe void SnapIntoPlaceOrReset()
{
		if (collider.OverlapPoint(targetSprite.transform.position))
		{
			m_OnNewPositionSavedEvent?.Invoke();
			gameObject.SetActive(value: false);
		}
		else
		{
			m_OnPartPositionResetEvent?.Invoke();
			transform.localPosition = initialPosition;
		}
	}

	public unsafe void UpdatePartPosition(Vector2 dragPosition)
{
		Vector3 position = dragPosition;
		position.z = transform.position.z;
		transform.position = position;
	}
}
