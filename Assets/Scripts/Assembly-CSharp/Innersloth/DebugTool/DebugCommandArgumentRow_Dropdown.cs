using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public class DebugCommandArgumentRow_Dropdown : AbstractDebugCommandArgumentRow
	{
		[Header("Dropdown Input")]
		[SerializeField]
		private DebugCommandArgumentRowFieldContainer dropdownFieldContainer;

		[SerializeField]
		private TMP_Dropdown dropdown;

		[SerializeField]
		[Header("Filter Input")]
		private DebugCommandArgumentRowFieldContainer filterFieldContainer;

		[SerializeField]
		private TMP_InputField filterField;

		private List<string> initialDropdownOptions = new List<string>();

		public override DebugCommandArgumentRowFieldContainer TopField => dropdownFieldContainer;

		public override DebugCommandArgumentRowFieldContainer BottomField => filterFieldContainer;

		public override string GetValue()
		{
			return dropdown.captionText.text;
		}

		public override void Awake()
		{
			base.Awake();
			filterField.onValueChanged.AddListener(OnFilterChanged);
		}

		public override void SetNavigation(Selectable above, Selectable below)
		{
			dropdownFieldContainer.SetNavigationAbove(above);
			filterFieldContainer.SetNavigationBelow(below);
		}

		public void SetDropdownOptions(IEnumerable<string> options)
		{
			initialDropdownOptions = new List<string>(options);
			dropdown.AddOptions(initialDropdownOptions);
		}

		public void OnFilterChanged(string filter)
		{
			ResetFilter();
			if (string.IsNullOrWhiteSpace(filter))
			{
				return;
			}
			foreach (TMP_Dropdown.OptionData item in dropdown.options.Where((TMP_Dropdown.OptionData option) => !option.text.ToLower().Contains(filter.ToLower().Trim())).ToList())
			{
				dropdown.options.Remove(item);
			}
			dropdown.RefreshShownValue();
		}

		private void ResetFilter()
		{
			dropdown.ClearOptions();
			dropdown.AddOptions(initialDropdownOptions);
		}
	}
}
