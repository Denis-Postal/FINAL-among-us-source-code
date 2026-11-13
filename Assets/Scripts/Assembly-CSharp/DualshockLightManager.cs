using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using Rewired;
using Rewired.ControllerExtensions;
using UnityEngine;

public class DualshockLightManager : DestroyableSingleton<DualshockLightManager>
{
	public class LightOverlayHandle
	{
		public Color color;

		public float intensity = 1f;

		public void Dispose()
		{
			DualshockLightManager instance = DestroyableSingleton<DualshockLightManager>.Instance;
			bool flag = instance.overlays.Remove(this);
		}
	}

	public class LightOverlayFlash
	{
		public LightOverlayHandle handle;

		public AudioClip clip;

		public float t;

		public float duration;

		private static float[] samples;

		public bool Alive
		{
			get
			{
				bool flag = duration < t;
				return !flag;
			}
		}

		public void Init()
		{
			if (clip != null)
			{
				float num = duration;
				float length = clip.length;
				if (duration < length)
				{
					num = length;
				}
				duration = num;
			}
		}

		public void Dispose()
		{
			if (handle != null)
			{
				DualshockLightManager instance = DestroyableSingleton<DualshockLightManager>.Instance;
				bool flag = instance.overlays.Remove(handle);
				handle = null;
			}
		}

		public void Update(float deltaTime)
		{
			// TODO(verify): no-op, original logic unrecoverable from decomp
		}

		static LightOverlayFlash()
		{
			float[] array = new float[2];
			samples = array;
		}
	}

	private const float lightIntensity = 0.5f;

	private Color baseColor = new Color(0f, 0.5f, 1f, 1f);

	private Color oldColor;

	private List<LightOverlayHandle> overlays;

	private List<LightOverlayFlash> lightOverlayFlashes;

	public Color BaseColor
	{
		get
		{ return baseColor; }
		set
		{ baseColor = value; }
	}

	public LightOverlayHandle AllocateLight()
	{ // from decomp_mono (IntroCutscene.ShowTeam writes overlayHandle.color, a null handle crashed the intro)
		LightOverlayHandle lightOverlayHandle = new LightOverlayHandle();
		overlays.Add(lightOverlayHandle);
		return lightOverlayHandle;
	}

	private float GetExternalBrightnessFromElectrical()
	{ // from decomp_mono
		if ((bool)ShipStatus.Instance && ShipStatus.Instance.Systems.Count > 7)
		{
			float t = (float)(int)((SwitchSystem)ShipStatus.Instance.Systems[SystemTypes.Electrical]).Value / 255f;
			return Mathf.Lerp(0.05f, 1f, t);
		}
		return 1f;
	}

	private unsafe void Update()
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	public static void Flash(Color c, float intensity, AudioClip clip)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	public DualshockLightManager()
	{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
		// TODO(verify): lost struct initializer (decomp: baseColor = (Color)0;)
		// TODO(verify): lost struct initializer (decomp: oldColor = (Color)0;)
		List<LightOverlayHandle> list = new List<LightOverlayHandle>();
		overlays = list;
		lightOverlayFlashes = new List<LightOverlayFlash>();
	}
}
