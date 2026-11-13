using Object = UnityEngine.Object;
using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

public class MapConsole : MonoBehaviour, IUsable
{
	public ImageNames useIcon = ImageNames.AdminMapButton;

	public float usableDistance = 1f;

	public SpriteRenderer Image;

	public ImageNames UseIcon => useIcon;

	public float UsableDistance => usableDistance;

	public float PercentCool => 0f;

	public unsafe void SetOutline(bool on, bool mainTarget)
	{ /* spliced-from-2019 */
		if ((bool)Image)
		{
			Image.material.SetFloat("_Outline", on ? 1 : 0);
			Image.material.SetColor("_OutlineColor", Color.white);
			Image.material.SetColor("_AddColor", mainTarget ? Color.white : Color.clear);
		}
	}

	public unsafe float CanUse(NetworkedPlayerInfo pc, out bool canUse, out bool couldUse)
	{
		float num = float.MaxValue;
		PlayerControl playerControl = pc.Object;
		couldUse = pc.Object.CanMove;
		canUse = couldUse;
		if (canUse)
		{
			num = Vector2.Distance(playerControl.GetTruePosition(), transform.position);
			canUse &= num <= UsableDistance;
		}
		return num;
	}

	public void Use()
	{
		CanUse(PlayerControl.LocalPlayer.Data, out var canUse, out var _);
		if (canUse)
		{
			PlayerControl.LocalPlayer.NetTransform.Halt();
			DestroyableSingleton<HudManager>.Instance.ToggleMapVisible(new MapOptions
			{
				Mode = MapOptions.Modes.CountOverlay
			});
			if (PlayerControl.LocalPlayer.AmOwner)
			{
				PlayerControl.LocalPlayer.MyPhysics.inputHandler.enabled = true;
				ConsoleJoystick.SetMode_Task();
			}
		}
	}
}
