using System.Collections;
using AmongUs.Data;
using AmongUs.GameOptions;
using InnerNet;
using UnityEngine;

public class TutorialManager : DestroyableSingleton<TutorialManager>
{
	public PlayerControl PlayerPrefab;

	[SerializeField]
	private GameObject skeldDetectiveLocationsPrefab;

	[SerializeField]
	private GameObject miraDetectiveLocationsPrefab;

	[SerializeField]
	private GameObject polusDetectiveLocationsPrefab;

	[SerializeField]
	private GameObject airshipDetectiveLocationsPrefab;

	[SerializeField]
	private GameObject fungleDetectiveLocationsPrefab;

	public override void Awake()
	{
		base.Awake();
		// Local customization: user-requested Freeplay debug commands + overlay.
		Innersloth.DebugTool.DebugTool.RegisterCommandHandler(typeof(LocalFreeplayDebugCommands));
		DebugToolInstantiator.EnsureOverlay();
		DataManager.Player.Stats.SetStatTrackingEnabled(enabled: false);
		StartCoroutine(RunTutorial());
		GameDebugCommands.AddCommands();
		TutorialDebugCommands.AddCommands(gameObject);
	}

	public override void OnDestroy()
	{
		Innersloth.DebugTool.DebugTool.UnregisterCommandHandler(typeof(LocalFreeplayDebugCommands));
		DataManager.Player.Stats.SetStatTrackingEnabled(enabled: true);
		GameDebugCommands.RemoveCommands();
		TutorialDebugCommands.RemoveCommands(gameObject);
		base.OnDestroy();
	}

	private IEnumerator RunTutorial()
	{
		while (!ShipStatus.Instance)
		{
			yield return null;
		}
		ShipStatus.Instance.Timer = 15f;
		while (!PlayerControl.LocalPlayer)
		{
			yield return null;
		}
		while (!GameManager.Instance)
		{
			yield return null;
		}
		if (DestroyableSingleton<DiscordManager>.InstanceExists)
		{
			DestroyableSingleton<DiscordManager>.Instance.SetHowToPlay();
		}
		NormalGameOptionsV12 normalGameOptionsV = new NormalGameOptionsV12(new UnityLogger());
		normalGameOptionsV.SetInt(Int32OptionNames.NumImpostors, 0);
		normalGameOptionsV.DiscussionTime = 0;
		normalGameOptionsV.NumEmergencyMeetings = 9;
		normalGameOptionsV.GhostsDoTasks = true;
		GameOptionsManager.Instance.CurrentGameOptions = normalGameOptionsV;
		PlayerControl.LocalPlayer.RpcSetRole(RoleTypes.Crewmate);
		PlayerControl.LocalPlayer.AdjustLighting();
		PlayerControl.LocalPlayer.cosmetics.SetAsLocalPlayer();
		for (int i = 0; i < ShipStatus.Instance.DummyLocations.Length; i++)
		{
			PlayerControl playerControl = Object.Instantiate(PlayerPrefab);
			playerControl.PlayerId = (byte)GameData.Instance.GetAvailableId();
			NetworkedPlayerInfo networkedPlayerInfo = GameData.Instance.AddDummy(playerControl);
			AmongUsClient.Instance.Spawn(networkedPlayerInfo);
			AmongUsClient.Instance.Spawn(playerControl);
			playerControl.isDummy = true;
			playerControl.transform.position = ShipStatus.Instance.DummyLocations[i].position;
			playerControl.GetComponent<DummyBehaviour>().enabled = true;
			playerControl.NetTransform.enabled = false;
			playerControl.SetName(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Dummy) + " " + (i + 1));
			byte b = (byte)((i < DataManager.Player.Customization.Color) ? i : (i + 1));
			playerControl.SetColor(b);
			playerControl.SetHat("", b);
			playerControl.SetSkin("", b);
			playerControl.SetPet("");
			playerControl.SetVisor("", b);
			playerControl.SetNamePlate("");
			playerControl.SetLevel(0u);
			networkedPlayerInfo.RpcSetTasks(new byte[0]);
		}
		switch ((MapNames)AmongUsClient.Instance.TutorialMapId)
		{
		case MapNames.Skeld:
		case MapNames.Dleks:
			Object.Instantiate(skeldDetectiveLocationsPrefab);
			break;
		case MapNames.Polus:
			Object.Instantiate(polusDetectiveLocationsPrefab);
			break;
		case MapNames.MiraHQ:
			Object.Instantiate(miraDetectiveLocationsPrefab);
			break;
		case MapNames.Airship:
			Object.Instantiate(airshipDetectiveLocationsPrefab);
			break;
		case MapNames.Fungle:
			Object.Instantiate(fungleDetectiveLocationsPrefab);
			break;
		}
		yield return ShipStatus.Instance.CosmeticsCache.PopulateFromPlayers();
		ShipStatus.Instance.Begin();
		GameManager.Instance.StartGame();
		ShipStatus.Instance.StartSFX();
		Logger.GlobalInstance.Info($"Started Freeplay Game in {(MapNames)AmongUsClient.Instance.TutorialMapId}");
	}
}
