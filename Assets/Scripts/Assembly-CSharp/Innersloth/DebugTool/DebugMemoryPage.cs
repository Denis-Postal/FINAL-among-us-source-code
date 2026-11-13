using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public class DebugMemoryPage : DebugToolOverlayPage
	{
		[SerializeField]
		private TextMeshProUGUI gcMemoryText;

		[SerializeField]
		private TextMeshProUGUI systemMemoryText;

		[SerializeField]
		private TextMeshProUGUI textureMemoryText;

		[SerializeField]
		private Button toggleMemoryRecordingButton;

		[SerializeField]
		private TextMeshProUGUI toggleMemoryRecordingText;

		[SerializeField]
		private Toggle toggleMemoryOverlayButton;

		private DebugToolMemoryStatsView memoryStatsView;

		private bool isVisible;

		private bool isRecording;

		private bool isOverlayEnabled;

		public override string Name => "Memory";

		public override bool ConsumingNavActions => false;

		private void Awake()
		{
			toggleMemoryRecordingButton.onClick.AddListener(StartRecording);
			toggleMemoryRecordingText.text = "Start Recording";
			toggleMemoryOverlayButton.isOn = isOverlayEnabled;
			toggleMemoryOverlayButton.onValueChanged.AddListener(ToggleMemoryOverlay);
			memoryStatsView = Object.FindObjectOfType<DebugToolMemoryStatsView>(true); // offline: include inactive objects
		}

		private void Start()
		{
			EventSystem.current.SetSelectedGameObject(toggleMemoryRecordingButton.gameObject);
		}

		private void ToggleMemoryOverlay(bool enable)
		{
			isOverlayEnabled = enable;
			Debug.Log("Memory Stats view was " + (isOverlayEnabled ? "enabled" : "disabled"));
			if (memoryStatsView != null) { memoryStatsView.ToggleVisibility(isOverlayEnabled); }
		}

		private void StartRecording()
		{
			toggleMemoryRecordingButton.onClick.RemoveListener(StartRecording);
			toggleMemoryRecordingButton.onClick.AddListener(StopRecording);
			toggleMemoryRecordingText.text = "Stop Recording";
			if (memoryStatsView != null) { memoryStatsView.StartRecording(); } // offline: stats view may not exist
			isRecording = true;
		}

		private void StopRecording()
		{
			toggleMemoryRecordingButton.onClick.RemoveListener(StopRecording);
			toggleMemoryRecordingButton.onClick.AddListener(StartRecording);
			toggleMemoryRecordingText.text = "Start Recording";
			if (memoryStatsView != null) { memoryStatsView.StopRecording(); }
			isRecording = false;
		}

		private unsafe void Update()
		{
			foreach (IDebugToolInputProvider inputProvider in InputProviders)
			{
				if (inputProvider.GetCancel())
				{
					HideDetails();
				}
			}
			if (isVisible && isRecording && memoryStatsView != null)
			{
				gcMemoryText.text = $"GC Memory: {memoryStatsView.GCMemory} MB";
				systemMemoryText.text = $"System Memory: {memoryStatsView.SystemMemory} MB";
				textureMemoryText.text = $"Texture Memory: {memoryStatsView.TextureMemory} MB";
			}
		}

		public override void Show()
		{
			isVisible = true;
			EventSystem.current.SetSelectedGameObject(toggleMemoryRecordingButton.gameObject);
		}

		public override void OnBackButtonPressed()
		{
			HideDetails();
		}

		private void HideDetails()
		{
			gcMemoryText.text = string.Empty;
			systemMemoryText.text = string.Empty;
			textureMemoryText.text = string.Empty;
			isVisible = false;
		}
	}
}
