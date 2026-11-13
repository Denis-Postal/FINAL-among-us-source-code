using UnityEngine;

public class Ladder : MonoBehaviour, IUsableCoolDown, IUsable
{
	public byte Id;

	public SpriteRenderer SpotArea;

	public bool IsTop;

	public Ladder Destination;

	public AudioClip UseSound;

	public SpriteRenderer Image;

	public float UsableDistance => 0.5f;

	public float PercentCool => CoolDown / MaxCoolDown;

	public ImageNames UseIcon => ImageNames.UseButton;

	public float CoolDown { get; set; }

	public float MaxCoolDown => 5f;

	public bool IsCoolingDown()
	{
		return CoolDown > 0f;
	}

	private void Update()
	{
		CoolDown = Mathf.Max(CoolDown - Time.deltaTime, 0f);
	}

	public float CanUse(NetworkedPlayerInfo pc, out bool canUse, out bool couldUse)
	{
		float num = float.MaxValue;
		PlayerControl playerControl = pc.Object;
		couldUse = !pc.IsDead && playerControl.CanMove && pc.Role.CanUse(this);
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

	public void SetOutline(bool on, bool mainTarget)
	{
		Image.material.SetFloat("_Outline", on ? 1 : 0);
		Image.material.SetColor("_OutlineColor", Color.white);
		Image.material.SetColor("_AddColor", mainTarget ? Color.white : Color.clear);
	}

	public void Use()
	{
		if (!IsCoolingDown())
		{
			NetworkedPlayerInfo data = PlayerControl.LocalPlayer.Data;
			CanUse(data, out var canUse, out var _);
			if (canUse)
			{
				PlayerControl.LocalPlayer.MyPhysics.RpcClimbLadder(this);
				CoolDown = MaxCoolDown;
			}
		}
	}

	public void SetDestinationCooldown()
	{
		Destination.CoolDown = MaxCoolDown;
	}
}
