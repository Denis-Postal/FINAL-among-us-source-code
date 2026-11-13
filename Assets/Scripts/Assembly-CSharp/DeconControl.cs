using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class DeconControl : MonoBehaviour, IUsable
{
	public DeconSystem System;

	public float usableDistance = 1f;

	public SpriteRenderer Image;

	public AudioClip UseSound;

	public Button.ButtonClickedEvent OnUse;

	private const float CooldownDuration = 6f;

	private float cooldown;

	public ImageNames UseIcon => ImageNames.UseButton;

	public float UsableDistance => usableDistance;

	public float PercentCool => cooldown / 6f;

	public unsafe void SetOutline(bool on, bool mainTarget)
	{ /* spliced-from-2019 */
		if ((bool)Image)
		{
			Image.material.SetFloat("_Outline", on ? 1 : 0);
			Image.material.SetColor("_OutlineColor", Color.white);
			Image.material.SetColor("_AddColor", mainTarget ? Color.white : Color.clear);
		}
	}

	public void Update()
	{
		cooldown = Mathf.Max(cooldown - Time.deltaTime, 0f);
	}

	public unsafe float CanUse(NetworkedPlayerInfo pc, out bool canUse, out bool couldUse)
	{
		if (System.CurState != DeconSystem.States.Idle)
		{
			canUse = false;
			couldUse = false;
			return 0f;
		}
		float num = float.MaxValue;
		PlayerControl playerControl = pc.Object;
		Vector2 truePosition = playerControl.GetTruePosition();
		Vector3 position = transform.position;
		position.y -= 0.1f;
		couldUse = playerControl.CanMove && !pc.IsDead && !PhysicsHelpers.AnythingBetween(truePosition, position, Constants.ShipAndObjectsMask, useTriggers: false);
		canUse = couldUse && cooldown == 0f;
		if (canUse)
		{
			num = Vector2.Distance(truePosition, position);
			canUse &= num <= UsableDistance;
		}
		return num;
	}

	public void Use()
	{
		CanUse(PlayerControl.LocalPlayer.Data, out var canUse, out var _);
		if (canUse)
		{
			cooldown = 6f;
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(UseSound, loop: false);
			}
			OnUse.Invoke();
		}
	}
}
