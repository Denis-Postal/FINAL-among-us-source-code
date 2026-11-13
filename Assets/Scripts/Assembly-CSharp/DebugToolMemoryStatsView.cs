using Cpp2ILInjected;
using TMPro;
using Unity.Profiling;
using UnityEngine;

public class DebugToolMemoryStatsView : MonoBehaviour
{
	[SerializeField]
	private GameObject background;

	[SerializeField]
	private TextMeshProUGUI gcMemoryText;

	[SerializeField]
	private TextMeshProUGUI systemMemoryText;

	[SerializeField]
	private TextMeshProUGUI textureMemoryText;

	private string statsText;

	private ProfilerRecorder systemMemoryRecorder;

	private ProfilerRecorder gcMemoryRecorder;

	private ProfilerRecorder textureMemoryRecorder;

	private bool isVisible;

	private bool isRecording;

	public unsafe long GCMemory
	{
		get
		{
			if (!gcMemoryRecorder.IsRunning)
			{
				return 0L;
			}
			return gcMemoryRecorder.LastValue / 1048576;
		}
	}

	public unsafe long SystemMemory
	{
		get
		{
			if (!systemMemoryRecorder.IsRunning)
			{
				return 0L;
			}
			return systemMemoryRecorder.LastValue / 1048576;
		}
	}

	public unsafe long TextureMemory
	{
		get
		{
			if (!textureMemoryRecorder.IsRunning)
			{
				return 0L;
			}
			return textureMemoryRecorder.LastValue / 1048576;
		}
	}

	public void StartRecording()
	{
		systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
		gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
		textureMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Texture Memory");
		isRecording = true;
	}

	public unsafe void StopRecording()
	{
		systemMemoryRecorder.Dispose();
		gcMemoryRecorder.Dispose();
		textureMemoryRecorder.Dispose();
		isRecording = false;
	}

	private unsafe void Update()
	{
		if (isVisible && isRecording)
		{
			gcMemoryText.text = $"GC Memory: {GCMemory} MB";
			systemMemoryText.text = $"System Memory: {SystemMemory} MB";
			textureMemoryText.text = $"Texture Memory: {TextureMemory} MB";
		}
	}

	public void ToggleVisibility(bool visible)
	{
		isVisible = visible;
		background.SetActive(visible);
		if (!visible)
		{
			HideDetails();
		}
	}

	private void HideDetails()
	{
		gcMemoryText.text = string.Empty;
		systemMemoryText.text = string.Empty;
		textureMemoryText.text = string.Empty;
	}
}
