using System;
using System.Collections.Generic;
using UnityEngine;

public class SlicedAspectScaler : MonoBehaviour
{
	private sealed class _003C_003Ec__DisplayClass4_0
	{
		public float aspectDiff;

		internal void _003CStart_003Eb__0(AspectScaledAsset ob)
		{
			ob.ScaleObject(aspectDiff);
		}
	}

	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public float aspectDiff;

		internal void _003CUpdate_003Eb__0(AspectScaledAsset ob)
		{
			ob.ScaleObject(aspectDiff);
		}
	}

	private const float NORMAL_ASPECT = 1.777f;

	[SerializeField]
	private List<AspectScaledAsset> objectsToScale;

	[SerializeField]
	private bool updateAlways;

	private Camera parentCam;

	private void Start()
	{
		SetParentCam();
		float aspectDiff = parentCam.aspect / 1.777f;
		objectsToScale.ForEach((AspectScaledAsset ob) =>
		{
			ob.ScaleObject(aspectDiff);
		});
	}

	private void SetParentCam()
	{
		parentCam = (parentCam ? parentCam : Camera.main);
	}

	private void Update()
	{
		if (updateAlways)
		{
			if (parentCam == null)
			{
				SetParentCam();
			}
			float aspectDiff = parentCam.aspect / 1.777f;
			objectsToScale.ForEach((AspectScaledAsset ob) =>
			{
				ob.ScaleObject(aspectDiff);
			});
		}
	}
}
