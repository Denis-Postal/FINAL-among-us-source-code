using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;

public class DoorConsole : MonoBehaviour, IUsable
{
	public Minigame MinigamePrefab;

	private OpenableDoor MyDoor;

	private SpriteRenderer Image;

	public ImageNames UseIcon => ImageNames.UseButton;

	public float UsableDistance => 1f;

	public float PercentCool => 0f;

	public void Awake()
	{
		MyDoor = GetComponent<OpenableDoor>();
		Image = GetComponent<SpriteRenderer>();
	}

	public float CanUse(NetworkedPlayerInfo pc, out bool canUse, out bool couldUse)
	{
		float num = Vector2.Distance(pc.Object.GetTruePosition(), transform.position);
		couldUse = !pc.IsDead && !MyDoor.IsOpen && pc.Role.CanUse(this);
		canUse = couldUse && num <= UsableDistance;
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
			PlayerControl.LocalPlayer.NetTransform.Halt();
			Minigame minigame = Object.Instantiate(MinigamePrefab, Camera.main.transform);
			minigame.transform.localPosition = new Vector3(0f, 0f, -50f);
			((IDoorMinigame)minigame).SetDoor(MyDoor);
			minigame.Begin(null);
		}
	}
}
