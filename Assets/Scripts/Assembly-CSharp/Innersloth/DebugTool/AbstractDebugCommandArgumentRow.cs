using System;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public abstract class AbstractDebugCommandArgumentRow : MonoBehaviour
	{
		[SerializeField]
		public TextMeshProUGUI nameText;

		[SerializeField]
		public TextMeshProUGUI typeText;

		public DebugCommandArgumentRowFieldContainer[] fieldContainers;

		private Action<DebugCommandArgumentRowFieldContainer> m_OnFieldContainerSelected;

		public abstract DebugCommandArgumentRowFieldContainer TopField { get; }

		public abstract DebugCommandArgumentRowFieldContainer BottomField { get; }

		public event Action<DebugCommandArgumentRowFieldContainer> OnFieldContainerSelected
		{
			add
			{
				m_OnFieldContainerSelected = (Action<DebugCommandArgumentRowFieldContainer>)System.Delegate.Combine(m_OnFieldContainerSelected, value);
			}
			remove
			{
				m_OnFieldContainerSelected = (Action<DebugCommandArgumentRowFieldContainer>)System.Delegate.Remove(m_OnFieldContainerSelected, value);
			}
		}

		public abstract string GetValue();

		public abstract void SetNavigation(Selectable above, Selectable below);

		public unsafe virtual void Awake()
		{
			fieldContainers = GetComponentsInChildren<DebugCommandArgumentRowFieldContainer>();
			fieldContainers.ForEach((DebugCommandArgumentRowFieldContainer x) =>
			{
				x.OnSelected += OnChildFieldContainerSelected;
			});
		}

		public void SetInfo(string name, string type)
		{
			nameText.text = name;
			typeText.text = "(" + type + ")";
		}

		public void OnChildFieldContainerSelected(DebugCommandArgumentRowFieldContainer selection)
		{
			m_OnFieldContainerSelected?.Invoke(selection);
			DebugCommandArgumentRowFieldContainer[] array = fieldContainers;
			foreach (DebugCommandArgumentRowFieldContainer debugCommandArgumentRowFieldContainer in array)
			{
				if (debugCommandArgumentRowFieldContainer != selection)
				{
					debugCommandArgumentRowFieldContainer.Deselect();
				}
			}
		}

		public void _003CAwake_003Eb__12_0(DebugCommandArgumentRowFieldContainer x)
		{
			OnChildFieldContainerSelected(x);
		}
	}
}
