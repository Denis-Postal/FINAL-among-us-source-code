using System.Collections;
using AmongUs.GameOptions;
using UnityEngine;

public class KillAnimation : MonoBehaviour
{
	public AnimationClip BlurAnim;

	public Vector3 BodyOffset;

	public IEnumerator CoPerformKill(PlayerControl source, PlayerControl target)
	{
		FollowerCamera cam = Camera.main.GetComponent<FollowerCamera>();
		bool isParticipant = PlayerControl.LocalPlayer == source || PlayerControl.LocalPlayer == target;
		PlayerPhysics sourcePhys = source.MyPhysics;
		SetMovement(source, canMove: false);
		SetMovement(target, canMove: false);
		if (isParticipant)
		{
			PlayerControl.LocalPlayer.isKilling = true;
			source.isKilling = true;
		}
		DeadBody deadBody = Object.Instantiate(GameManager.Instance.GetDeadBody(source.Data.Role));
		deadBody.enabled = false;
		deadBody.ParentId = target.PlayerId;
		deadBody.bodyRenderers.ForEach((SpriteRenderer b) =>
		{
			target.SetPlayerMaterialColors(b);
		});
		if (deadBody.bloodSplatter != null)
		{
			target.SetPlayerMaterialColors(deadBody.bloodSplatter);
		}
		Vector3 position = target.transform.position + BodyOffset;
		position.z = position.y / 1000f;
		deadBody.transform.position = position;
		source.Data.Role.KillAnimSpecialSetup(deadBody, source, target);
		target.Data.Role.KillAnimSpecialSetup(deadBody, source, target);
		if (PlayerControl.LocalPlayer.Data.Role.Role == RoleTypes.Detective && !PlayerControl.LocalPlayer.Data.IsDead && !PlayerControl.LocalPlayer.Data.Disconnected)
		{
			(PlayerControl.LocalPlayer.Data.Role as DetectiveRole).KillAnimSpecialSetup(deadBody, source, target);
		}
		if (isParticipant)
		{
			cam.Locked = true;
			ConsoleJoystick.SetMode_Task();
			if (PlayerControl.LocalPlayer.AmOwner)
			{
				PlayerControl.LocalPlayer.MyPhysics.inputHandler.enabled = true;
			}
		}
		target.Die(DeathReason.Kill, assignGhostRole: true);
		yield return source.MyPhysics.Animations.CoPlayCustomAnimation(BlurAnim);
		source.NetTransform.SnapTo(target.transform.position);
		sourcePhys.Animations.PlayIdleAnimation();
		SetMovement(source, canMove: true);
		SetMovement(target, canMove: true);
		deadBody.enabled = true;
		if (isParticipant)
		{
			cam.Locked = false;
			PlayerControl.LocalPlayer.isKilling = false;
			source.isKilling = false;
		}
	}

	public static void SetMovement(PlayerControl source, bool canMove)
	{
		source.moveable = canMove;
		source.MyPhysics.ResetMoveState(stopCoroutines: false);
		source.NetTransform.enabled = canMove;
		source.MyPhysics.enabled = canMove;
		source.NetTransform.Halt();
	}
}
