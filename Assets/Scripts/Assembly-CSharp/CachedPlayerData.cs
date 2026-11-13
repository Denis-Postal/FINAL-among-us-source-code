using System;
using AmongUs.GameOptions;
using Cpp2ILInjected;

public class CachedPlayerData
{
	public readonly string PlayerName;

	public readonly NetworkedPlayerInfo.PlayerOutfit Outfit;

	public readonly bool IsYou;

	public readonly bool IsImpostor;

	public readonly bool IsDead;

	public readonly RoleTypes RoleWhenAlive;

	public int ColorId => Outfit.ColorId;

	public string SkinId => Outfit.SkinId;

	public string PetId => Outfit.PetId;

	public string HatId => Outfit.HatId;

	public string VisorId => Outfit.VisorId;

	public string NamePlateId => Outfit.NamePlateId;

	public CachedPlayerData(NetworkedPlayerInfo player)
	{
		PlayerName = player.PlayerName;
		Outfit = player.DefaultOutfit;
		IsYou = player.Object == PlayerControl.LocalPlayer;
		IsDead = player.IsDead || player.Disconnected;
		IsImpostor = player.Role.IsImpostor;
		RoleWhenAlive = player.RoleWhenAlive.GetValueOrDefault();
	}
}
