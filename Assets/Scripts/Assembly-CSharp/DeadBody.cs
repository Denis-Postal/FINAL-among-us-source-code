using System;
using Cpp2ILInjected;
using UnityEngine;

public class DeadBody : MonoBehaviour
{
	public bool Reported;

	public byte ParentId;

	public Collider2D myCollider;

	public SpriteRenderer bloodSplatter;

	public SpriteRenderer[] bodyRenderers;

	public unsafe Vector2 TruePosition => transform.TransformPoint(myCollider.offset);

	public unsafe void OnClick()
	{
		if (!Reported && GameManager.Instance.CanReportBodies())
		{
			Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
			Vector2 truePosition2 = TruePosition;
			if (Vector2.Distance(truePosition2, truePosition) <= PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, useTriggers: false))
			{
				Reported = true;
				NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(ParentId);
				PlayerControl.LocalPlayer.CmdReportDeadBody(playerById);
			}
		}
	}
}
