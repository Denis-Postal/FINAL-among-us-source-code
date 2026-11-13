using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public class DebugTapToOpenButton : MonoBehaviour
	{
		[SerializeField]
		private float maxSecondsBetweenTaps;

		[SerializeField]
		private DebugToolOverlay debugOverlay;

		[SerializeField]
		private Button tapButton;

		private float lastTapTime = -1f;

		private int numTapsSoFar;

		private void Awake()
		{
			tapButton.onClick.AddListener(Tap);
			debugOverlay.AddToggleActiveCallback((bool visible) =>
			{
				tapButton.gameObject.SetActive(!visible);
			});
		}

		public void SetRaycastTarget(bool raycast)
		{
			tapButton.GetComponent<Image>().raycastTarget = raycast;
		}

		private void Tap()
		{
			float time = Time.time;
			if (numTapsSoFar > 0 && time - lastTapTime <= maxSecondsBetweenTaps)
			{
				numTapsSoFar++;
			}
			else
			{
				numTapsSoFar = 1;
			}
			lastTapTime = time;
			if (numTapsSoFar > 2)
			{
				numTapsSoFar = 0;
				debugOverlay.ToggleUIVisible(visible: true);
			}
		}

		private void _003CAwake_003Eb__5_0(bool visible)
		{
			GameObject gameObject = tapButton.gameObject;
			bool active = (byte)((visible ? 1u : 0u) ^ 1u) != 0;
			gameObject.SetActive(active);
		}
	}
}
