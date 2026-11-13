using UnityEngine;
using System;
using Cpp2ILInjected;
using TMPro;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public class DebugCommandArgumentRow_TextField : AbstractDebugCommandArgumentRow
	{
		[SerializeField]
		private DebugCommandArgumentRowFieldContainer fieldContainer;

		[SerializeField]
		private TMP_InputField inputField;

		[SerializeField]
		public TextMeshProUGUI placeholderText;

		public override DebugCommandArgumentRowFieldContainer TopField => fieldContainer;

		public override DebugCommandArgumentRowFieldContainer BottomField => fieldContainer;

		public override string GetValue()
		{
			return inputField.text.Trim();
		}

		public override void SetNavigation(Selectable above, Selectable below)
		{
			fieldContainer.SetNavigationAbove(above);
			fieldContainer.SetNavigationBelow(below);
		}

		public void SetPlaceholder(string placeholder)
		{
			placeholderText.text = placeholder;
		}
	}
}
