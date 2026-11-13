using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public class DebugLogOverlayPageListSelector : Selectable
	{
		private Action m_OnSelected;

		private Action m_OnDeselected;

		public bool IsSelected => EventSystem.current.currentSelectedGameObject == gameObject;

		public event Action OnSelected
		{
			add
			{
				m_OnSelected = (Action)System.Delegate.Combine(m_OnSelected, value);
			}
			remove
			{
				m_OnSelected = (Action)System.Delegate.Remove(m_OnSelected, value);
			}
		}

		public event Action OnDeselected
		{
			add
			{
				m_OnDeselected = (Action)System.Delegate.Combine(m_OnDeselected, value);
			}
			remove
			{
				m_OnDeselected = (Action)System.Delegate.Remove(m_OnDeselected, value);
			}
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			m_OnSelected?.Invoke();
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			m_OnDeselected?.Invoke();
		}

		public bool IsChildSelected()
		{
			Selectable[] componentsInChildren = gameObject.GetComponentsInChildren<Selectable>();
			foreach (Selectable selectable in componentsInChildren)
			{
				if (EventSystem.current.currentSelectedGameObject == selectable.gameObject)
				{
					return true;
				}
			}
			return false;
		}
	}
}
