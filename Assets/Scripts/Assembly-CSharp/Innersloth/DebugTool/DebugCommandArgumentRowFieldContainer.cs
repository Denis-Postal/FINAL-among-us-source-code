using System;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public class DebugCommandArgumentRowFieldContainer : Selectable
	{
		[SerializeField]
		private Selectable field;

		public bool isFieldSelected;

		private Action<DebugCommandArgumentRowFieldContainer> m_OnSelected;

		public Selectable Selectable => field;

		public bool IsSelected => EventSystem.current.currentSelectedGameObject == gameObject;

		public bool IsFieldSelected => isFieldSelected;

		public event Action<DebugCommandArgumentRowFieldContainer> OnSelected
		{
			add
			{
				m_OnSelected = (Action<DebugCommandArgumentRowFieldContainer>)System.Delegate.Combine(m_OnSelected, value);
			}
			remove
			{
				m_OnSelected = (Action<DebugCommandArgumentRowFieldContainer>)System.Delegate.Remove(m_OnSelected, value);
			}
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			m_OnSelected?.Invoke(this);
		}

		public void Deselect()
		{
			isFieldSelected = false;
		}

		public unsafe void SetNavigationAbove(Selectable selectable)
		{
			Navigation navigation = base.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnUp = selectable;
			base.navigation = navigation;
		}

		public unsafe void SetNavigationBelow(Selectable selectable)
		{
			Navigation navigation = base.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnDown = selectable;
			base.navigation = navigation;
		}

		public void Confirm()
		{
			if (!isFieldSelected)
			{
				isFieldSelected = true;
				Selectable.Select();
			}
		}

		public void Cancel()
		{
			if (isFieldSelected)
			{
				isFieldSelected = false;
				Select();
			}
		}
	}
}
