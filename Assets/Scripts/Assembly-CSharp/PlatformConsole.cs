using System;
using Cpp2ILInjected;
using UnityEngine;

public class PlatformConsole : MonoBehaviour, IUsable
{
	public float usableDistance = 0.5f;

	public SpriteRenderer Image;

	public MovingPlatformBehaviour Platform;

	public float UsableDistance => usableDistance;

	public float PercentCool => 0f;

	public ImageNames UseIcon => ImageNames.UseButton;

	public float CanUse(NetworkedPlayerInfo pc, out bool canUse, out bool couldUse)
	{
		float num = float.MaxValue;
		PlayerControl playerControl = pc.Object;
		couldUse = !pc.IsDead && playerControl.CanMove && !Platform.InUse && Vector2.Distance(Platform.transform.position, transform.position) < 2f && pc.Role.CanUse(this);
		canUse = couldUse;
		if (canUse)
		{
			Vector2 truePosition = playerControl.GetTruePosition();
			Vector3 position = transform.position;
			num = Vector2.Distance(truePosition, position);
			canUse &= num <= UsableDistance && !PhysicsHelpers.AnythingBetween(truePosition, position, Constants.ShipOnlyMask, useTriggers: false);
		}
		return num;
	}

	// TODO(verify): outline colour (white assumed; the decompile lost the Color constant).
	public void SetOutline(bool on, bool mainTarget)
	{
		if ((bool)Image)
		{
			Image.material.SetFloat("_Outline", on ? 1 : 0);
			Image.material.SetColor("_OutlineColor", Color.white);
			Image.material.SetColor("_AddColor", mainTarget ? Color.white : Color.clear);
		}
	}

	public void Use()
	{
		CanUse(PlayerControl.LocalPlayer.Data, out var canUse, out var _);
		if (canUse)
		{
			Platform.Use();
		}
	}
}
