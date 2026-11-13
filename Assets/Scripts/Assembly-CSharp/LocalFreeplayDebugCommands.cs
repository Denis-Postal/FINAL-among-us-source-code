using System;
using System.Linq;
using AmongUs.GameOptions;
using Innersloth.DebugTool;
using UnityEngine;

// User-requested local commands. The gameplay operations reuse the existing Freeplay/RPC paths.
// TODO(verify): command UI integration and each command's runtime outcome need captures.
public static class LocalFreeplayDebugCommands
{
    private static PlayerControl Local()
    {
        if (!DestroyableSingleton<TutorialManager>.InstanceExists || !ShipStatus.Instance || !PlayerControl.LocalPlayer)
            throw new InvalidOperationException("These commands require an active Freeplay session.");
        return PlayerControl.LocalPlayer;
    }

    // Host-only: creates a networked fake player ("Player N") with random cosmetics, friend code and PUID for every client.
    [DebugCommand("Freeplay/Create character", closeBeforeExecute: true)]
    public static void CreateCharacter()
    {
        if (!AmongUsClient.Instance || !AmongUsClient.Instance.AmHost || !GameData.Instance)
            throw new InvalidOperationException("Create character is host-only and needs a running game.");
        AmongUsClient.Instance.StartCoroutine(FakePlayerFactory.CoCreate());
    }

    // Programmatic local-only dummy (kept for capture helpers); the debug-menu command above is the networked one.
    public static void CreateCharacter(string name, int color, string hat)
    {
        var local = Local();
        if (color < 0 || color >= Palette.PlayerColors.Length) throw new ArgumentOutOfRangeException(nameof(color));
        var dummy = UnityEngine.Object.Instantiate(DestroyableSingleton<TutorialManager>.Instance.PlayerPrefab);
        dummy.PlayerId = (byte)GameData.Instance.GetAvailableId();
        var info = GameData.Instance.AddDummy(dummy);
        AmongUsClient.Instance.Spawn(info, -2);
        AmongUsClient.Instance.Spawn(dummy, -2);
        dummy.isDummy = true;
        dummy.transform.position = local.transform.position + Vector3.right;
        dummy.GetComponent<DummyBehaviour>().enabled = true;
        dummy.NetTransform.enabled = false;
        dummy.SetName(name);
        dummy.SetColor(color);
        dummy.SetHat(hat ?? "", color);
        dummy.SetSkin("", color);
        dummy.SetPet("");
        dummy.SetVisor("", color);
        dummy.SetNamePlate("");
        dummy.SetLevel(0);
        info.RpcSetTasks(Array.Empty<byte>());
        dummy.RpcSetRole(RoleTypes.Crewmate);
    }

    [DebugCommand("Freeplay/Spawn dummy", closeBeforeExecute: true)]
    public static void SpawnDummy() { CreateCharacter(); }

    [DebugCommand("Freeplay/Role", closeBeforeExecute: true)]
    public static void Role(RoleTypes role) { Local().RpcSetRole(role); }

    [DebugCommand("Freeplay/Kill me", closeBeforeExecute: true)]
    public static void KillMe() { var player = Local(); player.RpcMurderPlayer(player, true); }

    [DebugCommand("Freeplay/Spawn body", closeBeforeExecute: true)]
    public static void SpawnBody()
    {
        Local();
        var dummy = PlayerControl.AllPlayerControls.FirstOrDefault(p => p.isDummy && p.Data != null && !p.Data.IsDead);
        if (!dummy) throw new InvalidOperationException("Spawn a living dummy first.");
        dummy.NetTransform.RpcSnapTo(Local().GetTruePosition() + Vector2.right);
        Local().RpcMurderPlayer(dummy, true);
    }

    [DebugCommand("Freeplay/Call meeting", closeBeforeExecute: true)]
    public static void CallMeeting() { Local().CmdReportDeadBody(null); }

    [DebugCommand("Freeplay/Complete all tasks", closeBeforeExecute: true)]
    public static void CompleteTasks()
    {
        foreach (var task in Local().myTasks.OfType<NormalPlayerTask>().ToArray())
            for (int i = task.taskStep; i < task.MaxStep && !task.IsComplete; i++) task.NextStep();
    }

    [DebugCommand("Freeplay/Sabotage", closeBeforeExecute: true)]
    public static void Sabotage(SystemTypes system)
    {
        Local();
        if (!ShipStatus.Instance.Systems.ContainsKey(system)) throw new ArgumentException("This map has no such system.");
        switch (system)
        {
            case SystemTypes.Reactor: case SystemTypes.Electrical: case SystemTypes.Comms: case SystemTypes.LifeSupp:
            case SystemTypes.Laboratory: case SystemTypes.HeliSabotage: case SystemTypes.MushroomMixupSabotage:
                ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Sabotage, (byte)system); break;
            default: throw new ArgumentException("Select a supported sabotage system.");
        }
    }

    [DebugCommand("Freeplay/Teleport to room", closeBeforeExecute: true)]
    public static void Teleport(SystemTypes room)
    {
        var player = Local();
        var target = ShipStatus.Instance.AllRooms.FirstOrDefault(r => r.RoomId == room);
        if (!target) throw new ArgumentException("This map has no such room.");
        player.NetTransform.RpcSnapTo(target.roomArea.bounds.center);
    }

    [DebugCommand("Freeplay/Noclip", closeBeforeExecute: true)]
    public static void Noclip(bool enabled) { Local().Collider.enabled = !enabled; }

    [DebugCommand("Freeplay/Speed", closeBeforeExecute: true)]
    public static void Speed(float multiplier)
    {
        if (float.IsNaN(multiplier) || float.IsInfinity(multiplier) || multiplier <= 0 || multiplier > 10)
            throw new ArgumentOutOfRangeException(nameof(multiplier));
        // Base walk speed is 2.5 (PlayerPhysics prefab value; the old 4.5 made "1" run 1.8x), ghosts use GhostSpeed (3) instead of Speed.
        Local().MyPhysics.Speed = 2.5f * multiplier;
        Local().MyPhysics.GhostSpeed = 3f * multiplier;
    }
}
