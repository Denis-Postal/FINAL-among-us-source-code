using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;

public class OptionsConsole : MonoBehaviour, IUsable
{
	public Vector3 CustomPosition;

	public bool HostOnly;

	public GameObject MenuPrefab;

	public SpriteRenderer Outline;

	public ImageNames CustomUseIcon;

	public ImageNames UseIcon => CustomUseIcon;

	public float UsableDistance => 1f;

	public float PercentCool => 0f;

	public float CanUse(NetworkedPlayerInfo pc, out bool canUse, out bool couldUse)
	{
		float num = float.MaxValue;
		PlayerControl playerControl = pc.Object;
		couldUse = playerControl.CanMove;
		if (HostOnly)
		{
			couldUse &= AmongUsClient.Instance.AmHost;
		}
		canUse = couldUse;
		if (canUse)
		{
			num = Vector2.Distance(playerControl.GetTruePosition(), transform.position);
			canUse &= num <= UsableDistance;
		}
		return num;
	}

	public void SetOutline(bool on, bool mainTarget)
	{
		if ((bool)Outline)
		{
			Outline.material.SetFloat("_Outline", on ? 1 : 0);
			Outline.material.SetColor("_OutlineColor", Color.white);
			Outline.material.SetColor("_AddColor", mainTarget ? Color.white : Color.clear);
		}
	}

	// TODO(verify): menu local position (CustomPosition assumed; the decompile lost the Vector3 source).
	public void Use()
	{
		CanUse(PlayerControl.LocalPlayer.Data, out var canUse, out var _);
		if (canUse)
		{
			PlayerControl.LocalPlayer.NetTransform.Halt();
			GameObject gameObject = Object.Instantiate(MenuPrefab);
			gameObject.transform.SetParent(Camera.main.transform, worldPositionStays: false);
			gameObject.transform.localPosition = CustomPosition;
			DestroyableSingleton<TransitionFade>.Instance.DoTransitionFade(null, gameObject.gameObject, null);
		}
	}
}
