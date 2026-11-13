using System;
using System.Reflection;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public class DebugCommandButton : MonoBehaviour
	{
		private const int INDENT_SIZE = 50;

		[SerializeField]
		private TextMeshProUGUI buttonLabel;

		[SerializeField]
		private HorizontalLayoutGroup layoutGroup;

		[SerializeField]
		private Button button;

		[SerializeField]
		public Image background;

		[SerializeField]
		public Image expandCollapseImage;

		[SerializeField]
		private Sprite expandedIcon;

		[SerializeField]
		public Sprite collapsedIcon;

		[SerializeField]
		public Color evenColor;

		[SerializeField]
		public Color oddColor;

		[SerializeField]
		public Color highlightColor;

		public int index;

		private int initialLeftPadding;

		public bool IsDirectory { get; private set; }

		public string Path { get; private set; }

		public bool Expanded
		{
			get
			{
				if (IsDirectory)
				{
					return expandCollapseImage.sprite == expandedIcon;
				}
				return true;
			}
			set
			{
				expandCollapseImage.sprite = (value ? expandedIcon : collapsedIcon);
			}
		}

		private void Awake()
		{ initialLeftPadding = layoutGroup.padding.left; }

		public void Initialize(string label, string path, ParameterInfo[] parameters, Action action, int index)
		{
			string text = "";
			if (parameters != null && parameters.Length != 0)
			{
				foreach (ParameterInfo parameterInfo in parameters)
				{
					text = text + "(" + parameterInfo.Name + ":<color=#FFA70066>" + DebugTool.GetTypeName(parameterInfo.ParameterType) + "</color>) ";
				}
			}
			buttonLabel.text = label + " <size=75%><color=#FFFFFF66>" + text + "</color></size>";
			name = $"DebugCommandButton [{path}] [{index}]";
			this.index = index;
			Path = path;
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() =>
			{
				action();
			});
			background.color = ((index % 2 == 0) ? evenColor : oddColor);
		}

		public void ToggleDirectory(bool directory)
		{
			IsDirectory = directory;
			expandCollapseImage.enabled = directory;
		}

		public void SetIndentLevel(int indentLevel)
		{
			RectOffset padding = layoutGroup.padding;
			padding.left = initialLeftPadding + 50 * indentLevel;
			layoutGroup.padding = padding;
		}

		public unsafe void ToggleHighlight(bool highlighted)
		{
			if (highlighted)
			{
				background.color = highlightColor;
			}
			else
			{
				background.color = ((index % 2 == 0) ? evenColor : oddColor);
			}
		}
	}
}
