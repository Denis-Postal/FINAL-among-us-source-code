using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class VibrationManager : DestroyableSingleton<VibrationManager>
{
	public enum VibrationFalloff
	{
		None,
		Linear,
		InverseLinear
	}

	private class LocalVibration
	{
		public Vector2 intensity;

		public float t;

		public float duration;

		public VibrationFalloff falloff;

		public AudioClip clip;

		public bool loopClip;

		public string name;

		public bool Alive => t <= duration;

		public void Init()
		{
			if (clip != null)
			{
				duration = Mathf.Max(duration, clip.length);
			}
		}

		public Vector2 UpdateIntensity(float deltaTime)
		{
			float num = 1f;
			switch (falloff)
			{
			case VibrationFalloff.Linear:
			{
				float num2 = Mathf.Clamp01(t / duration);
				num = 1f - num2;
				break;
			}
			case VibrationFalloff.InverseLinear:
				num = Mathf.Clamp01(t / duration);
				break;
			}
			if (clip != null)
			{
				float num3 = t / clip.length;
				if (num3 > 1f)
				{
					num = 0f;
					t = duration;
				}
				else
				{
					int offsetSamples = (int)(Mathf.Clamp01(num3) * (float)(clip.samples - 1));
					if (clip.GetData(samples, offsetSamples))
					{
						num *= Mathf.Abs(samples[0]);
					}
				}
			}
			t += deltaTime;
			if (t >= duration && loopClip)
			{
				t -= duration;
				if (t < 0f)
				{
					t = 0f;
				}
			}
			return intensity * num;
		}
	}

	private class WorldVibration
	{
		public float intensity;

		public Vector2 location;

		public float radius;

		public float t;

		public float duration;

		public VibrationFalloff falloff;

		public AudioClip clip;

		public bool loopClip;

		public string name;

		public bool Alive => t <= duration;

		public void Init()
		{
			if (clip != null)
			{
				duration = Mathf.Max(duration, clip.length);
			}
		}

		public Vector2 UpdateIntensity(Vector2 cameraPos, float deltaTime)
		{
			float num = 1f;
			switch (falloff)
			{
			case VibrationFalloff.Linear:
			{
				float num2 = Mathf.Clamp01(t / duration);
				num = 1f - num2;
				break;
			}
			case VibrationFalloff.InverseLinear:
				num = Mathf.Clamp01(t / duration);
				break;
			}
			Vector2 vector = location - cameraPos;
			float magnitude = vector.magnitude;
			vector /= magnitude;
			Vector2 vector2 = new Vector2(1f - Mathf.Clamp01(vector.x), 1f - Mathf.Clamp01(0f - vector.x));
			num *= 1f - Mathf.Clamp01(magnitude / radius);
			if (clip != null)
			{
				float num3 = t / clip.length;
				if (num3 > 1f)
				{
					num = 0f;
					t = duration;
				}
				else if (clip.channels == 2)
				{
					int offsetSamples = (int)(Mathf.Clamp01(num3) * (float)(clip.samples / 2 - 1)) * 2;
					if (clip.GetData(samples, offsetSamples))
					{
						vector2.x *= Mathf.Abs(samples[0]);
						vector2.y *= Mathf.Abs(samples[1]);
					}
				}
				else
				{
					int offsetSamples2 = (int)(Mathf.Clamp01(num3) * (float)(clip.samples - 1));
					if (clip.GetData(samples, offsetSamples2))
					{
						num *= Mathf.Abs(samples[0]);
					}
				}
			}
			t += deltaTime;
			if (t >= duration && loopClip)
			{
				t -= duration;
				if (t < 0f)
				{
					t = 0f;
				}
			}
			return intensity * num * vector2;
		}
	}

	public const float SMALL_INTENSITY = 0.3f;

	public const float MEDIUM_INTENSITY = 0.7f;

	public const float FRAME_DURATION = 0.02f;

	private List<LocalVibration> currentLocalVibration= new List<LocalVibration>();

	private List<WorldVibration> currentWorldVibration= new List<WorldVibration>();

	private Vector2 singleFrameVibration= Vector2.zero;

	private bool hasFrameVibration;

	private bool zeroNextFrame;

	public int numVibrationsActive;

	public Vector2 currentVibration;

	private Camera cam;

	private WorldVibration tempSingleFrameWorldVibration= new WorldVibration();

	private WorldVibration tempAmbientSoundVibration= new WorldVibration();

	private bool shouldVibrate= true;

	private static float[] samples= new float[2];

	private void Start()
	{
		cam = Camera.main;
	}

	private void OnEnable()
	{
		SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
	}

	private void OnDisable()
	{
		SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
	}

	private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
	{
		ClearAllVibration();
	}

	public static void ClearAllVibration()
	{
		DestroyableSingleton<VibrationManager>.Instance.currentLocalVibration.Clear();
		DestroyableSingleton<VibrationManager>.Instance.currentWorldVibration.Clear();
		DestroyableSingleton<VibrationManager>.Instance.currentVibration = Vector2.zero;
	}

	private unsafe void Update()
	{
		if (!cam)
		{
			cam = Camera.main;
		}
		if (!shouldVibrate)
		{
			return;
		}
		if (RaycastAmbientSoundPlayer.players.Count > 0)
		{
			Vector2 cameraPos = cam.transform.position;
			foreach (RaycastAmbientSoundPlayer player3 in RaycastAmbientSoundPlayer.players)
			{
				if ((bool)player3.AmbientSound)
				{
					player3.t += Time.deltaTime;
					if (player3.t > player3.AmbientSound.length)
					{
						player3.t -= player3.AmbientSound.length;
					}
					if (player3.ambientVolume > 0.01f && player3.ShouldVibrate)
					{
						tempAmbientSoundVibration.clip = player3.AmbientSound;
						tempAmbientSoundVibration.duration = player3.AmbientSound.length;
						tempAmbientSoundVibration.intensity = player3.ambientVolume * 0.5f;
						tempAmbientSoundVibration.location = player3.transform.position;
						tempAmbientSoundVibration.radius = ((player3.AmbientMaxDist > 0f) ? player3.AmbientMaxDist : 10000f);
						tempAmbientSoundVibration.t = player3.t;
						singleFrameVibration += tempAmbientSoundVibration.UpdateIntensity(cameraPos, 0f);
						hasFrameVibration = true;
					}
				}
			}
		}
		if (currentLocalVibration.Count > 0 || currentWorldVibration.Count > 0 || hasFrameVibration)
		{
			Player player = ReInput.players.GetPlayer(0);
			Vector2 vector = singleFrameVibration;
			singleFrameVibration = Vector2.zero;
			for (int num = currentLocalVibration.Count - 1; num >= 0; num--)
			{
				if (currentLocalVibration[num].Alive)
				{
					vector += currentLocalVibration[num].UpdateIntensity(Time.deltaTime);
				}
				else
				{
					currentLocalVibration.RemoveAt(num);
				}
			}
			Vector2 cameraPos2 = cam.transform.position;
			for (int num2 = currentWorldVibration.Count - 1; num2 >= 0; num2--)
			{
				if (currentWorldVibration[num2].Alive)
				{
					vector += currentWorldVibration[num2].UpdateIntensity(cameraPos2, Time.deltaTime);
				}
				else
				{
					currentWorldVibration.RemoveAt(num2);
				}
			}
			currentVibration = vector;
			vector.x = Mathf.Clamp01(vector.x);
			vector.y = Mathf.Clamp01(vector.y);
			if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
			{
				player.SetVibration(0, vector.x);
				player.SetVibration(1, vector.y);
			}
			zeroNextFrame = true;
		}
		else if (zeroNextFrame)
		{
			Player player2 = ReInput.players.GetPlayer(0);
			player2.SetVibration(0, 0f);
			player2.SetVibration(1, 0f);
		}
		numVibrationsActive = currentLocalVibration.Count + currentWorldVibration.Count;
	}

	public static void CancelVibration(AudioClip clipToCancel)
	{
		for (int num = DestroyableSingleton<VibrationManager>.Instance.currentLocalVibration.Count - 1; num >= 0; num--)
		{
			if (DestroyableSingleton<VibrationManager>.Instance.currentLocalVibration[num].clip == clipToCancel)
			{
				DestroyableSingleton<VibrationManager>.Instance.currentLocalVibration.RemoveAt(num);
				return;
			}
		}
		for (int num2 = DestroyableSingleton<VibrationManager>.Instance.currentWorldVibration.Count - 1; num2 >= 0; num2--)
		{
			if (DestroyableSingleton<VibrationManager>.Instance.currentWorldVibration[num2].clip == clipToCancel)
			{
				DestroyableSingleton<VibrationManager>.Instance.currentWorldVibration.RemoveAt(num2);
				break;
			}
		}
	}

	public static void CancelVibration(string name)
	{
		for (int num = DestroyableSingleton<VibrationManager>.Instance.currentLocalVibration.Count - 1; num >= 0; num--)
		{
			if (DestroyableSingleton<VibrationManager>.Instance.currentLocalVibration[num].name == name)
			{
				DestroyableSingleton<VibrationManager>.Instance.currentLocalVibration.RemoveAt(num);
				return;
			}
		}
		for (int num2 = DestroyableSingleton<VibrationManager>.Instance.currentWorldVibration.Count - 1; num2 >= 0; num2--)
		{
			if (DestroyableSingleton<VibrationManager>.Instance.currentWorldVibration[num2].name == name)
			{
				DestroyableSingleton<VibrationManager>.Instance.currentWorldVibration.RemoveAt(num2);
				break;
			}
		}
	}

	public static void Vibrate(float left, float right)
	{
		DestroyableSingleton<VibrationManager>.Instance.singleFrameVibration += new Vector2(left, right);
		DestroyableSingleton<VibrationManager>.Instance.hasFrameVibration = true;
	}

	public static void Vibrate(float left, float right, float duration, VibrationFalloff falloffType = VibrationFalloff.None, AudioClip sourceClip = null, bool loopClip = false, string name = "")
	{
		LocalVibration localVibration = new LocalVibration();
		localVibration.intensity = new Vector2(left, right);
		localVibration.duration = duration;
		localVibration.falloff = falloffType;
		localVibration.t = 0f;
		localVibration.clip = sourceClip;
		localVibration.loopClip = loopClip;
		localVibration.name = name;
		localVibration.Init();
		DestroyableSingleton<VibrationManager>.Instance.currentLocalVibration.Add(localVibration);
	}

	public static void Vibrate(float intensity, Vector2 worldPosition, float radius)
	{
		DestroyableSingleton<VibrationManager>.Instance.tempSingleFrameWorldVibration.intensity = intensity;
		DestroyableSingleton<VibrationManager>.Instance.tempSingleFrameWorldVibration.location = worldPosition;
		DestroyableSingleton<VibrationManager>.Instance.tempSingleFrameWorldVibration.radius = radius;
		// Offline remake: cam can still be null/destroyed here (Update only refreshes it while a pad is active); a NRE aborted MovingPlatformBehaviour.UsePlatform and left the platform stuck in use.
		if (!DestroyableSingleton<VibrationManager>.Instance.cam)
		{
			DestroyableSingleton<VibrationManager>.Instance.cam = Camera.main;
			if (!DestroyableSingleton<VibrationManager>.Instance.cam)
			{
				return;
			}
		}
		Vector2 cameraPos = DestroyableSingleton<VibrationManager>.Instance.cam.transform.position;
		DestroyableSingleton<VibrationManager>.Instance.singleFrameVibration += DestroyableSingleton<VibrationManager>.Instance.tempSingleFrameWorldVibration.UpdateIntensity(cameraPos, 0f);
		DestroyableSingleton<VibrationManager>.Instance.hasFrameVibration = true;
	}

	public static void Vibrate(float intensity, Vector2 worldPosition, float radius, float duration, VibrationFalloff falloffType = VibrationFalloff.None, AudioClip sourceClip = null, bool loopClip = false)
	{
		WorldVibration worldVibration = new WorldVibration();
		worldVibration.intensity = intensity;
		worldVibration.location = worldPosition;
		worldVibration.duration = duration;
		worldVibration.falloff = falloffType;
		worldVibration.radius = radius;
		worldVibration.t = 0f;
		worldVibration.clip = sourceClip;
		worldVibration.loopClip = loopClip;
		worldVibration.Init();
		DestroyableSingleton<VibrationManager>.Instance.currentWorldVibration.Add(worldVibration);
	}

	public VibrationManager()
	{ }

	static VibrationManager()
	{ }
}
