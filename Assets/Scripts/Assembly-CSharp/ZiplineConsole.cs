using UnityEngine;

public class ZiplineConsole : MonoBehaviour, IUsableCoolDown, IUsable
{
	[SerializeField]
	private float usableDistance = 0.5f;

	[SerializeField]
	private SpriteRenderer image;

	[SerializeField]
	private ZiplineBehaviour zipline;

	[SerializeField]
	private bool atTop;

	[SerializeField]
	private ZiplineConsole destination;

	public float UsableDistance => usableDistance;

	public float PercentCool => CoolDown / 7f;

	public ImageNames UseIcon => ImageNames.UseButton;

	public float CoolDown { get; set; }

	public float MaxCoolDown => 7f;

	public bool IsCoolingDown()
	{
		return CoolDown > 0f;
	}

	private void Update()
	{
		CoolDown = Mathf.Max(CoolDown - Time.deltaTime, 0f);
	}

	public void SetOutline(bool on, bool mainTarget)
	{
		image.material.SetFloat("_Outline", on ? 1 : 0);
		image.material.SetColor("_OutlineColor", Color.white);
		image.material.SetColor("_AddColor", mainTarget ? Color.white : Color.clear);
	}

	public float CanUse(NetworkedPlayerInfo pc, out bool canUse, out bool couldUse)
	{
		float num = float.MaxValue;
		PlayerControl playerControl = pc.Object;
		couldUse = !pc.IsDead && playerControl.CanMove && !playerControl.isKilling && pc.Role.CanUse(this);
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

	public void Use()
	{
		if (!IsCoolingDown())
		{
			CanUse(PlayerControl.LocalPlayer.Data, out var canUse, out var _);
			if (canUse)
			{
				zipline.Use(atTop, this);
				CoolDown = MaxCoolDown;
			}
		}
	}

	public void SetDestinationCooldown()
	{
		destination.CoolDown = MaxCoolDown;
	}
}
