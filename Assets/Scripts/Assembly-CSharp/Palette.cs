using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

public static class Palette
{
	public static readonly Color AcceptedGreen= new Color32(0, byte.MaxValue, 42, byte.MaxValue);

	public static readonly Color DisabledGrey= new Color(0.3f, 0.3f, 0.3f, 1f);

	public static readonly Color DisabledClear= new Color(1f, 1f, 1f, 0.3f);

	public static readonly Color EnabledColor= new Color(1f, 1f, 1f, 1f);

	public static readonly Color Black= new Color(0f, 0f, 0f, 1f);

	public static readonly Color ClearWhite= new Color(1f, 1f, 1f, 0f);

	public static readonly Color HalfWhite= new Color(1f, 1f, 1f, 0.5f);

	public static readonly Color White_75Alpha= new Color(1f, 1f, 1f, 0.75f);

	public static readonly Color White= new Color(1f, 1f, 1f, 1f);

	public static readonly Color LightBlue= new Color(0.5f, 0.5f, 1f);

	public static readonly Color Blue= new Color(0.2f, 0.2f, 1f);

	public static readonly Color Orange= new Color(1f, 0.6f, 0.005f);

	public static readonly Color Purple= new Color(0.6f, 0.1f, 0.6f);

	public static readonly Color Brown= new Color(0.72f, 0.43f, 0.11f);

	public static readonly Color CrewmateBlue= new Color32(140, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	public static readonly Color ImpostorRed= new Color32(byte.MaxValue, 25, 25, byte.MaxValue);

	public static readonly Color CrewmateRoleBlue= new Color32(17, 135, 178, byte.MaxValue);

	public static readonly Color CrewmateRoleHeaderBlue= new Color32(120, 204, 236, byte.MaxValue);

	public static readonly Color CrewmateRoleHeaderTextBlue= new Color32(14, 91, 119, byte.MaxValue);

	public static readonly Color CrewmateRoleHeaderDarkBlue= new Color32(10, 87, 115, byte.MaxValue);

	public static readonly Color CrewmateRoleHeaderVeryDarkBlue= new Color32(10, 87, 115, 127);

	public static readonly Color CrewmateSettingChangeText= new Color32(102, 171, 244, byte.MaxValue);

	public static readonly Color ImpostorRoleRed= new Color32(211, 35, 35, byte.MaxValue);

	public static readonly Color ImpostorRoleHeaderRed= new Color32(204, 83, 83, byte.MaxValue);

	public static readonly Color ImpostorRoleHeaderTextRed= new Color32(88, 35, 35, byte.MaxValue);

	public static readonly Color ImpostorRoleHeaderDarkRed= new Color32(137, 22, 22, byte.MaxValue);

	public static readonly Color ImpostorRoleHeaderVeryDarkRed= new Color32(137, 22, 22, 127);

	public static readonly Color CosmicubeCellUnlockedColor= new Color(6f / 255f, 200f / 255f, 100f / 255f);

	public static readonly Color CosmicubeCellLockedColor= new Color(179f / 255f, 171f / 255f, 183f / 255f);

	public static readonly Color CosmicubeQuality_NamePlate= new Color32(73, 174, 217, byte.MaxValue);

	public static readonly Color CosmicubeQuality_Hat= new Color32(225, 222, 0, byte.MaxValue);

	public static readonly Color CosmicubeQuality_Visor= new Color32(12, 22, 221, byte.MaxValue);

	public static readonly Color CosmicubeQuality_Skin= new Color32(239, 53, 230, byte.MaxValue);

	public static readonly Color CosmicubeQuality_Pet= new Color32(222, 28, 28, byte.MaxValue);

	public static readonly Color CosmicubeQuality_Kill= new Color32(0, byte.MaxValue, 179, byte.MaxValue);

	public static readonly Color InventorySelectedTextColor= new Color(0f, 201f / 255f, 106f / 255f, 1f);

	public static readonly Color InventoryTextColor= new Color(161f / 255f, 0.8f, 222f / 255f, 1f);

	public static readonly Color LogSuccessColor= new Color32(149, 240, 53, byte.MaxValue);

	public static readonly Color LogWarningColor= new Color32(240, 174, 53, byte.MaxValue);

	public static readonly Color LogErrorColor= new Color32(240, 75, 53, byte.MaxValue);

	public static readonly StringNames[] ColorNames= new StringNames[18]
	{
		StringNames.ColorRed,
		StringNames.ColorBlue,
		StringNames.ColorGreen,
		StringNames.ColorPink,
		StringNames.ColorOrange,
		StringNames.ColorYellow,
		StringNames.ColorBlack,
		StringNames.ColorWhite,
		StringNames.ColorPurple,
		StringNames.ColorBrown,
		StringNames.ColorCyan,
		StringNames.ColorLime,
		StringNames.ColorMaroon,
		StringNames.ColorRose,
		StringNames.ColorBanana,
		StringNames.ColorGray,
		StringNames.ColorTan,
		StringNames.ColorCoral
	};

	public const int PreviewGreenColorId = 2;

	public static readonly Color32[] PlayerColors= new Color32[18]
	{
		new Color32(198, 17, 17, byte.MaxValue),
		new Color32(19, 46, 210, byte.MaxValue),
		new Color32(17, 128, 45, byte.MaxValue),
		new Color32(238, 84, 187, byte.MaxValue),
		new Color32(240, 125, 13, byte.MaxValue),
		new Color32(246, 246, 87, byte.MaxValue),
		new Color32(63, 71, 78, byte.MaxValue),
		new Color32(215, 225, 241, byte.MaxValue),
		new Color32(107, 47, 188, byte.MaxValue),
		new Color32(113, 73, 30, byte.MaxValue),
		new Color32(56, byte.MaxValue, 221, byte.MaxValue),
		new Color32(80, 240, 57, byte.MaxValue),
		FromHex(6233390),
		FromHex(15515859),
		FromHex(15787944),
		FromHex(7701907),
		FromHex(9537655),
		FromHex(14115940)
	};

	public static readonly Color32[] ShadowColors= new Color32[18]
	{
		new Color32(122, 8, 56, byte.MaxValue),
		new Color32(9, 21, 142, byte.MaxValue),
		new Color32(10, 77, 46, byte.MaxValue),
		new Color32(172, 43, 174, byte.MaxValue),
		new Color32(180, 62, 21, byte.MaxValue),
		new Color32(195, 136, 34, byte.MaxValue),
		new Color32(30, 31, 38, byte.MaxValue),
		new Color32(132, 149, 192, byte.MaxValue),
		new Color32(59, 23, 124, byte.MaxValue),
		new Color32(94, 38, 21, byte.MaxValue),
		new Color32(36, 169, 191, byte.MaxValue),
		new Color32(21, 168, 66, byte.MaxValue),
		FromHex(4263706),
		FromHex(14586547),
		FromHex(13810825),
		FromHex(4609636),
		FromHex(5325118),
		FromHex(11813730)
	};

	public static readonly Color32[] TextColors= new Color32[18]
	{
		FromHex(12980497),
		FromHex(3166207),
		FromHex(1016620),
		FromHex(15619260),
		FromHex(16415747),
		FromHex(16576084),
		FromHex(0),
		FromHex(16777215),
		FromHex(8927470),
		FromHex(10052905),
		FromHex(3794140),
		FromHex(5304117),
		FromHex(12068436),
		FromHex(13729702),
		FromHex(16646078),
		FromHex(9280945),
		FromHex(9799796),
		FromHex(13723489)
	};

	public static readonly Color32[] TextOutlineColors= new Color32[18]
	{
		FromHex(983040),
		FromHex(983040),
		FromHex(3335),
		FromHex(2555941),
		FromHex(983040),
		FromHex(3152384),
		FromHex(16777215),
		FromHex(0),
		FromHex(720911),
		FromHex(985088),
		FromHex(6682),
		FromHex(464640),
		FromHex(1703936),
		FromHex(5445419),
		FromHex(1248512),
		FromHex(0),
		FromHex(920064),
		FromHex(3604492)
	};

	public static readonly Color32 VisorColor= new Color32(149, 202, 220, byte.MaxValue);

	private static Color32 FromHex(int v)
	{
		return new Color32((byte)(v >> 16), (byte)(v >> 8), (byte)v, byte.MaxValue);
	}

	public static string GetColorName(int colorId)
	{
		if (colorId < 0 || colorId >= ColorNames.Length)
		{
			Debug.LogError($"Color ID {colorId}, but Palette Length of {ColorNames.Length}");
			return "???";
		}
		return DestroyableSingleton<TranslationController>.Instance.GetString(ColorNames[colorId]);
	}

	static Palette()
	{ }
}
