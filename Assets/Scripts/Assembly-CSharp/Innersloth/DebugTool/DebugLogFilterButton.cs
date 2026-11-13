using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public class DebugLogFilterButton : MonoBehaviour
	{
		[SerializeField]
		private Image logIcon;

		[SerializeField]
		private Color enabledColor;

		[SerializeField]
		private TextMeshProUGUI logCountText;

		[SerializeField]
		private Button button;

		public bool Enabled { get; private set; }

		public void Initialize(Action onClick)
		{
			ToggleEnabled(enabled: true);
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() =>
			{
				onClick();
			});
		}

		public void SetCount(int count)
		{ logCountText.text = count.ToString(); }

		public unsafe void ToggleEnabled(bool enabled)
		{
			Enabled = enabled;
			logIcon.color = (enabled ? enabledColor : Color.grey.SetAlpha(0.5f));
		}
	}
}
