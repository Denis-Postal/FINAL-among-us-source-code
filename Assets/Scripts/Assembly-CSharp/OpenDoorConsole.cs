using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;

public class OpenDoorConsole : MonoBehaviour, IUsable
{
	private ImageNames useIcon = ImageNames.UseButton;

	public float usableDisance = 1.5f;

	private OpenableDoor myDoor;

	private SpriteRenderer image;

	public ImageNames UseIcon => useIcon;

	public float UsableDistance => usableDisance;

	public float PercentCool => 0f;

	public void Awake()
	{
		myDoor = GetComponent<OpenableDoor>();
		image = GetComponent<SpriteRenderer>();
	}

	public float CanUse(NetworkedPlayerInfo pc, out bool canUse, out bool couldUse)
	{
		float num = Vector2.Distance(pc.Object.GetTruePosition(), transform.position);
		couldUse = !pc.IsDead && !myDoor.IsOpen;
		canUse = couldUse && num <= UsableDistance;
		return num;
	}

	// TODO(verify): outline colour (white assumed; the decompile lost the Color constant).
	public void SetOutline(bool on, bool mainTarget)
	{
		if ((bool)image)
		{
			image.material.SetFloat("_Outline", on ? 1 : 0);
			image.material.SetColor("_OutlineColor", Color.white);
			image.material.SetColor("_AddColor", mainTarget ? Color.white : Color.clear);
		}
	}

	public void Use()
	{
		CanUse(PlayerControl.LocalPlayer.Data, out var canUse, out var _);
		if (canUse)
		{
			ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Doors, (byte)(myDoor.Id | 0x40));
			myDoor.SetDoorway(open: true);
		}
	}
}
