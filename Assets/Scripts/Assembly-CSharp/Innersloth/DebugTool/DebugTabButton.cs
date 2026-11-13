using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public class DebugTabButton : MonoBehaviour
	{
		private static readonly Color COLOR_NORMAL = new Color32(21, 21, 21, byte.MaxValue);

		private static readonly Color COLOR_SELECTED = new Color32(95, 100, 161, byte.MaxValue);

		[SerializeField]
		private string pageName;

		[SerializeField]
		private TextMeshProUGUI buttonLabel;

		[SerializeField]
		private Button button;

		public string PageName => pageName;

		private void Awake()
		{
			buttonLabel.text = pageName;
			button.onClick.AddListener(() =>
			{
				DebugUIEvents.OnTabClicked(pageName);
			});
		}

		public void ToggleSelect(bool selected)
		{
			button.image.color = (selected ? COLOR_SELECTED : COLOR_NORMAL);
		}
	}
}
