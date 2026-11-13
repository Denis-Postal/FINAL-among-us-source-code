using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cpp2ILInjected;
using Innersloth.Assets;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FungleShipStatus : ShipStatus
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Func<ISystemType, bool> _003C_003E9__13_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003COnEnable_003Eb__13_0(ISystemType i)
		{
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A7D10");
			object obj = default;
			bool flag = obj == null;
			return !flag;
		}
	}

	private readonly Dictionary<int, Mushroom> sporeMushrooms;

	[SerializeField]
	private MushroomMixupSabotageSystem specialSabotage;

	[SerializeField]
	private GameObject[] startAMBSounds;

	[SerializeField]
	private AssetReference staticWavesAsset;

	[SerializeField]
	private AssetReference animatedWavesAsset;

	private Vector2 LastBinocularPos_bf;

	public ZiplineBehaviour Zipline { get; private set; }

	public Vector2 LastBinocularPos
	{
		get
		{
			return LastBinocularPos_bf;
		}
		set
		{
			LastBinocularPos_bf = value;
		}
	}

	protected override void OnEnable()
	{
		if (Systems != null)
		{
			return;
		}
		DestroyableSingleton<HudManager>.Instance.ShadowQuad.material.SetInt("_Mask", 7);
		Zipline = GetComponentInChildren<ZiplineBehaviour>();
		Dictionary<SystemTypes, ISystemType> dictionary = new Dictionary<SystemTypes, ISystemType>(SystemTypeComparer.Instance);
		dictionary.Add(SystemTypes.Ventilation, new VentilationSystem());
		dictionary.Add(SystemTypes.Comms, new HqHudSystemType());
		dictionary.Add(SystemTypes.Reactor, new ReactorSystemType(60f, SystemTypes.Reactor));
		dictionary.Add(SystemTypes.Doors, new DoorsSystemType());
		dictionary.Add(SystemTypes.MushroomMixupSabotage, specialSabotage);
		Systems = dictionary;
		Systems.Add(SystemTypes.Sabotage, new SabotageSystemType(Systems.Values.Where((ISystemType i) => i is IActivatable).Cast<IActivatable>().ToArray()));
		Camera main = Camera.main;
		main.backgroundColor = CameraColor;
		FollowerCamera component = main.GetComponent<FollowerCamera>();
		if ((bool)component)
		{
			component.shakeAmount = 0f;
		}
		sporeMushrooms.Clear();
		Mushroom[] componentsInChildren = GetComponentsInChildren<Mushroom>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			sporeMushrooms[componentsInChildren[j].Id] = componentsInChildren[j];
		}
		if (sporeMushrooms.Count != componentsInChildren.Length)
		{
			logger.Error("Spore Mushroom IDs are not all unique. Please use Fix Spore Screen Mushroom IDs to assign unique IDs, otherwise they will not work properly in networked games.");
		}
		LastBinocularPos = Vector2.zero;
		// TODO(verify): the decompile only shows the animated waves asset being loaded here (no static-waves branch visible).
		LoadWavesAsync(animatedWavesAsset);
	}

	private void LoadWavesAsync(AssetReference waveAsset)
	{
		Action<GameObject> onFinished = (GameObject prefab) =>
		{
			Transform parent = transform;
			GameObject gameObject = UnityEngine.Object.Instantiate(prefab, parent);
		};
		IEnumerator routine = AddressableAssetExtensions.CoLoadAssetAsync(this, waveAsset, onFinished);
		Coroutine coroutine = StartCoroutine(routine);
	}

	public override void StartSFX()
	{
		for (int i = 0; i < startAMBSounds.Length; i++)
		{
			startAMBSounds[i].SetActive(value: true);
		}
	}

	public override void OnMeetingCalled()
	{
		Zipline.OnMeetingCalled();
		base.OnMeetingCalled();
	}

	public void TriggerSporesFromMushroom(int mushroomId)
	{
		if (!sporeMushrooms.TryGetValue(mushroomId, out var value))
		{
			logger.Error($"Failed to trigger spore mushroom {mushroomId} - no mushroom exists");
			return;
		}
		value.TriggerSpores();
	}

	public Mushroom GetMushroomFromId(int mushroomId)
	{
		if (!sporeMushrooms.TryGetValue(mushroomId, out var value))
		{
			logger.Error($"Failed to check spore mushroom {mushroomId} - no mushroom exists");
			return null;
		}
		return value;
	}

	public bool IsMushroomMixupActive()
	{
		return specialSabotage.currentSecondsUntilHeal > 0f;
	}

	public FungleShipStatus()
	{
		sporeMushrooms = new Dictionary<int, Mushroom>();
	}

	private void _003CLoadWavesAsync_003Eb__14_0(GameObject prefab)
	{
		Transform parent = transform;
		GameObject gameObject = UnityEngine.Object.Instantiate(prefab, parent);
	}
}
