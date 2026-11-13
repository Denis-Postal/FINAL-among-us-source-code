using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static DynamicSound.GetDynamicsFunction _003C_003E9__41_0;

		public static DynamicSound.GetDynamicsFunction _003C_003E9__43_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal void _003CPlayNamedSound_003Eb__41_0(AudioSource a, float b)
		{
		}

		internal void _003CGetNamedAudioSource_003Eb__43_0(AudioSource a, float b)
		{
		}
	}

	private static SoundManager instance;

	[SerializeField]
	private AudioMixer mixer;

	[SerializeField]
	public AudioMixerGroup musicMixer;

	[SerializeField]
	private AudioMixerGroup ambienceMixer;

	[SerializeField]
	private AudioMixerGroup ambienceHighPassMixer;

	[SerializeField]
	private AudioMixerGroup sfxMixer;

	public Dictionary<AudioClip, AudioSource> allSources= new Dictionary<AudioClip, AudioSource>();

	private List<ISoundPlayer> soundPlayers= new List<ISoundPlayer>();

	private static float musicVolume = 1f;

	private static float sfxVolume = 1f;

	private static float ambienceVolume = 1f;

	public static SoundManager Instance
	{
		get
		{
			if (!instance)
			{
				instance = Object.FindObjectOfType<SoundManager>() ?? new GameObject("SoundManager").AddComponent<SoundManager>();
			}
			return instance;
		}
	}

	public static float MusicVolume => musicVolume;

	public static float AmbienceVolume => ambienceVolume;

	public static float SfxVolume => sfxVolume;

	public AudioMixerGroup MusicChannel => musicMixer;

	public AudioMixerGroup AmbienceChannel => ambienceMixer;

	public AudioMixerGroup SfxChannel => sfxMixer;

	public AudioMixerGroup AmbienceHighPassChannel => ambienceHighPassMixer;

	private void Start()
	{
		if ((bool)instance && instance != this)
		{
			Object.Destroy(gameObject);
			return;
		}
		instance = this;
		Object.DontDestroyOnLoad(gameObject);
		UpdateChannelVolumes();
		DataManager.Settings.Audio.OnChangedEvent += OnAudioSettingsChanged;
	}

	private void OnDestroy()
	{
		DataManager.Settings.Audio.OnChangedEvent -= OnAudioSettingsChanged;
	}

	private void Update()
	{ /* spliced-from-2019 */
		for (int i = 0; i < soundPlayers.Count; i++)
		{
			soundPlayers[i].Update(Time.deltaTime);
		}
	}

	private void OnAudioSettingsChanged()
	{
		UpdateChannelVolumes();
	}

	private unsafe void UpdateChannelVolumes()
	{
		ChangeMusicVolume(DataManager.Settings.Audio.MusicVolume);
		ChangeAmbienceVolume(DataManager.Settings.Audio.AmbienceVolume);
		ChangeSfxVolume(DataManager.Settings.Audio.SfxVolume);
	}

	private unsafe void ChangeMusicVolume(float volume)
	{ // TODO(verify): rewritten from the 2019 SoundManager
		SetChannelVolume(volume, ref musicVolume, "MusicVolume");
	}

	private unsafe void ChangeAmbienceVolume(float volume)
	{ // TODO(verify): rewritten from the 2019 SoundManager
		SetChannelVolume(volume, ref ambienceVolume, "AmbienceVolume");
	}

	private unsafe void ChangeSfxVolume(float volume)
	{ // TODO(verify): rewritten from the 2019 SoundManager
		SetChannelVolume(volume, ref sfxVolume, "SfxVolume");
	}

	private unsafe void SetChannelVolume(float volume, ref float value, string channelName)
	{
		value = ((volume <= 0f) ? (-80f) : GetCalculatedVolume(volume));
		mixer.SetFloat(channelName, value);
	}

	private static float GetCalculatedVolume(float volume)
	{
		return Mathf.Log10(volume) * 20f;
	}

	public unsafe bool HasNamedSound(string name)
	{
		foreach (ISoundPlayer soundPlayer in soundPlayers)
		{
			if (soundPlayer.Name.Equals(name))
			{
				return true;
			}
		}
		return false;
	}

	public void StopNamedSound(string name)
	{
		for (int num = soundPlayers.Count - 1; num >= 0; num--)
		{
			ISoundPlayer soundPlayer = soundPlayers[num];
			if (soundPlayer.Name.Equals(name))
			{
				Object.Destroy(soundPlayer.Player);
				soundPlayers.RemoveAt(num);
				break;
			}
		}
	}

	public unsafe void StopSound(AudioClip clip)
	{
		if (allSources.TryGetValue(clip, out var value))
		{
			allSources.Remove(clip);
			value.Stop();
			Object.Destroy(value);
		}
		for (int i = 0; i < soundPlayers.Count; i++)
		{
			ISoundPlayer soundPlayer = soundPlayers[i];
			if (soundPlayer.Player.clip == clip)
			{
				Object.Destroy(soundPlayer.Player);
				soundPlayers.RemoveAt(i);
				break;
			}
		}
	}

	public unsafe void StopAllSound()
	{
		for (int i = 0; i < soundPlayers.Count; i++)
		{
			Object.Destroy(soundPlayers[i].Player);
		}
		soundPlayers.Clear();
		foreach (KeyValuePair<AudioClip, AudioSource> allSource in allSources)
		{
			AudioSource value = allSource.Value;
			value.volume = 0f;
			value.Stop();
			Object.Destroy(allSource.Value);
		}
		allSources.Clear();
	}

	public unsafe AudioSource PlayNamedSound(string name, AudioClip sound, bool loop, AudioMixerGroup channel)
	{
		return PlayDynamicSound(name, sound, loop, (AudioSource a, float b) =>
		{
		}, channel);
	}

	public unsafe AudioSource GetNamedSfxSource(string name)
	{
		return GetNamedAudioSource(name, SfxChannel);
	}

	public unsafe AudioSource GetNamedAudioSource(string name, AudioMixerGroup channel)
	{
		return PlayDynamicSound(name, null, loop: false, (AudioSource a, float b) =>
		{
		}, channel);
	}

	public AudioSource PlayDynamicSound(string name, AudioClip clip, bool loop, DynamicSound.GetDynamicsFunction volumeFunc, AudioMixerGroup channel)
	{
		DynamicSound dynamicSound = null;
		for (int i = 0; i < soundPlayers.Count; i++)
		{
			ISoundPlayer soundPlayer = soundPlayers[i];
			if (soundPlayer.Name == name && soundPlayer is DynamicSound)
			{
				dynamicSound = (DynamicSound)soundPlayer;
				break;
			}
		}
		if (dynamicSound == null)
		{
			dynamicSound = new DynamicSound();
			dynamicSound.Name = name;
			dynamicSound.Player = gameObject.AddComponent<AudioSource>();
			dynamicSound.Player.outputAudioMixerGroup = channel;
			dynamicSound.Player.playOnAwake = false;
			soundPlayers.Add(dynamicSound);
		}
		dynamicSound.Player.loop = loop;
		dynamicSound.SetTarget(clip, volumeFunc);
		return dynamicSound.Player;
	}

	public void CrossFadeSound(string name, AudioClip clip, float maxVolume, float duration = 1.5f)
	{
		CrossFader crossFader = null;
		for (int i = 0; i < soundPlayers.Count; i++)
		{
			ISoundPlayer soundPlayer = soundPlayers[i];
			if (soundPlayer.Name == name && soundPlayer is CrossFader)
			{
				crossFader = (CrossFader)soundPlayer;
				break;
			}
		}
		if (crossFader == null)
		{
			crossFader = new CrossFader();
			crossFader.Name = name;
			crossFader.Duration = duration;
			crossFader.MaxVolume = maxVolume;
			crossFader.Player = gameObject.AddComponent<AudioSource>();
			crossFader.Player.outputAudioMixerGroup = MusicChannel;
			crossFader.Player.playOnAwake = false;
			crossFader.Player.loop = true;
			soundPlayers.Add(crossFader);
		}
		crossFader.SetTarget(clip);
	}

	public AudioSource PlaySoundImmediate(AudioClip clip, bool loop, float volume = 1f, float pitch = 1f, AudioMixerGroup audioMixer = null)
	{
		if (clip == null)
		{
			Debug.LogWarning("Missing audio clip");
			return null;
		}
		if (audioMixer == null)
		{
			audioMixer = (loop ? MusicChannel : SfxChannel);
		}
		if (allSources.TryGetValue(clip, out var value))
		{
			value.pitch = pitch;
			value.loop = loop;
			value.outputAudioMixerGroup = audioMixer;
			value.Play();
		}
		else
		{
			value = gameObject.AddComponent<AudioSource>();
			value.outputAudioMixerGroup = audioMixer;
			value.playOnAwake = false;
			value.volume = volume;
			value.pitch = pitch;
			value.loop = loop;
			value.clip = clip;
			value.Play();
			allSources.Add(clip, value);
		}
		return value;
	}

	public bool SoundIsPlaying(AudioClip clip)
	{ /* spliced-from-2019 */
		if (allSources.TryGetValue(clip, out var value))
		{
			return !value.isPlaying;
		}
		return false;
	}

	public AudioSource PlaySound(AudioClip clip, bool loop, float volume = 1f, AudioMixerGroup audioMixer = null)
	{
		if (clip == null)
		{
			Debug.LogWarning("Missing audio clip");
			return null;
		}
		if (audioMixer == null)
		{
			audioMixer = (loop ? MusicChannel : SfxChannel);
		}
		if (allSources.TryGetValue(clip, out var value))
		{
			if (!value.isPlaying)
			{
				value.volume = volume;
				value.outputAudioMixerGroup = audioMixer;
				value.loop = loop;
				value.Play();
			}
		}
		else
		{
			value = gameObject.AddComponent<AudioSource>();
			value.outputAudioMixerGroup = audioMixer;
			value.playOnAwake = false;
			value.volume = volume;
			value.loop = loop;
			value.clip = clip;
			value.Play();
			allSources.Add(clip, value);
		}
		return value;
	}


	public static float GetSoundVolume(Vector2 soundPosition, Vector2 listenerPosition, float falloffStartDistance = 2f, float falloffEndDistance = 6f, float minVolume = 0f)
	{
		float num = Vector2.Distance(listenerPosition, soundPosition);
		if (num > falloffEndDistance)
		{
			return minVolume;
		}
		if (num >= falloffStartDistance)
		{
			float t = (num - falloffStartDistance) / (falloffEndDistance - falloffStartDistance);
			return 1f - Mathf.Clamp01(Mathf.SmoothStep(0f, 1f, t));
		}
		return 1f;
	}

	public SoundManager()
	{ }
}
