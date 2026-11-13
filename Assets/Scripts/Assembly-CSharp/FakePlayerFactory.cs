using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

// Host-only creation of a networked fake player with random cosmetics (debug "Create character").
// The player goes through AmongUsClient.CoCreateFakeClientPlayer (fake ClientData + CreatePlayer/GameData/spawn), then
// the normal Rpc* setters, so every client sees it. TODO(verify): this is a new debug feature, not a recovered native method.
public static class FakePlayerFactory
{
	private static readonly string[] Words = { "sneaky", "vent", "cyan", "crewmate", "impostor", "banana", "rocket", "pixel", "shadow", "comet", "lime", "orbit", "tasker", "ghost", "mango", "plasma", "nebula", "pepper" };

	public static string NextPlayerName()
	{
		bool[] used = new bool[256];
		foreach (NetworkedPlayerInfo info in GameData.Instance.AllPlayers)
		{
			Match match = Regex.Match(info.PlayerName ?? string.Empty, @"^Player ([0-9]+)$");
			if (match.Success && int.TryParse(match.Groups[1].Value, out int n) && n > 0 && n < used.Length)
			{
				used[n] = true;
			}
		}
		int x = 1;
		while (x < used.Length && used[x])
		{
			x++;
		}
		return "Player " + x;
	}

	public static string RandomFriendCode()
	{
		return Words[UnityEngine.Random.Range(0, Words.Length)] + "#" + UnityEngine.Random.Range(0, 10000).ToString("D4");
	}

	public static string RandomPuid()
	{
		return Guid.NewGuid().ToString("N");
	}

	private static string RandomId<T>(T[] unlocked, T[] all, string fallback) where T : CosmeticData
	{
		T[] pool = (unlocked != null && unlocked.Length > 0) ? unlocked : all;
		if (pool == null || pool.Length == 0)
		{
			return fallback;
		}
		return pool[UnityEngine.Random.Range(0, pool.Length)].ProductId;
	}

	public static int RandomFreeColor()
	{
		int count = Palette.PlayerColors.Length;
		bool[] used = new bool[count];
		foreach (NetworkedPlayerInfo info in GameData.Instance.AllPlayers)
		{
			int c = info.DefaultOutfit != null ? info.DefaultOutfit.ColorId : -1;
			if (c >= 0 && c < count)
			{
				used[c] = true;
			}
		}
		int[] free = Enumerable.Range(0, count).Where(i => !used[i]).ToArray();
		return free.Length > 0 ? free[UnityEngine.Random.Range(0, free.Length)] : UnityEngine.Random.Range(0, count);
	}

	public static IEnumerator CoCreate(Action<PlayerControl> done = null)
	{
		AmongUsClient client = AmongUsClient.Instance;
		if (client == null || !client.AmHost)
		{
			throw new InvalidOperationException("Create character is host-only.");
		}
		string name = NextPlayerName();
		string friendCode = RandomFriendCode();
		string puid = RandomPuid();
		uint level = (uint)UnityEngine.Random.Range(1, 100);
		PlayerControl created = null;
		yield return client.CoCreateFakeClientPlayer(name, level, puid, friendCode, pc => created = pc);
		if (!created)
		{
			Debug.LogWarning("[FakePlayer] creation failed");
			yield break;
		}
		for (int i = 0; i < 100 && !created.Data; i++)
		{
			yield return null;
		}
		if (!created.Data)
		{
			Debug.LogWarning("[FakePlayer] no player data");
			yield break;
		}
		HatManager hats = DestroyableSingleton<HatManager>.Instance;
		int color = RandomFreeColor();
		string hatId = RandomId(hats.GetUnlockedHats(), hats.allHats, "hat_NoHat");
		HatData hat = hats.GetHatById(hatId);
		string visorId = (hat != null && hat.BlocksVisors) ? "visor_EmptyVisor" : RandomId(hats.GetUnlockedVisors(), hats.allVisors, "visor_EmptyVisor");
		string skinId = RandomId(hats.GetUnlockedSkins(), hats.allSkins, "skin_None");
		string petId = RandomId(hats.GetUnlockedPets(), hats.allPets, "pet_EmptyPet");
		string plateId = RandomId(hats.GetUnlockedNamePlates(), hats.allNamePlates, "nameplate_NoPlate");
		created.RpcSetName(name);
		created.RpcSetColor((byte)color);
		created.RpcSetHat(hatId);
		created.RpcSetSkin(skinId);
		created.RpcSetPet(petId);
		created.RpcSetVisor(visorId);
		created.RpcSetNamePlate(plateId);
		created.RpcSetLevel(level);
		created.FriendCode = friendCode;
		created.Puid = puid;
		created.Data.FriendCode = friendCode;
		created.Data.Puid = puid;
		PlayerControl local = PlayerControl.LocalPlayer;
		if (local)
		{
			created.NetTransform.RpcSnapTo((Vector2)local.transform.position + new Vector2(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(-1f, 1f)));
		}
		if (DestroyableSingleton<TutorialManager>.InstanceExists)
		{
			// Freeplay: let the existing DummyBehaviour drive it (wander/vote) like the stock tutorial dummies.
			created.isDummy = true;
			DummyBehaviour dummy = created.GetComponent<DummyBehaviour>();
			if (dummy)
			{
				dummy.enabled = true;
			}
			created.NetTransform.enabled = false;
			created.Data.RpcSetTasks(Array.Empty<byte>());
			created.RpcSetRole(AmongUs.GameOptions.RoleTypes.Crewmate);
		}
		Debug.Log("[FakePlayer] created " + name + " id=" + created.PlayerId + " clientId=" + created.OwnerId + " color=" + color + " hat=" + hatId + " skin=" + skinId + " pet=" + petId + " visor=" + visorId + " plate=" + plateId + " fc=" + friendCode + " puid=" + puid + " level=" + level);
		done?.Invoke(created);
	}
}
