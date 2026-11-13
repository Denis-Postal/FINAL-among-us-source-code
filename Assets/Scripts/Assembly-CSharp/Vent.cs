using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using PowerTools;
using UnityEngine;
using UnityEngine.Audio;

public class Vent : MonoBehaviour, IUsable
{
	private sealed class _003CExitVent_003Ed__40 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		public PlayerControl pc;

		public Vent _003C_003E4__this;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CExitVent_003Ed__40(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			if (_003C_003E1__state == 0)
			{
				_003C_003E1__state = -1;
				if (pc.AmOwner)
				{
					currentVent = null;
				}
				if ((bool)_003C_003E4__this.ExitVentAnim && !_003C_003E4__this.myAnim.IsPlaying(_003C_003E4__this.ExitVentAnim))
				{
					if ((bool)_003C_003E4__this.additionalExitAnimation)
					{
						_003C_003E4__this.additionalExitAnimation.SetActive(value: false);
					}
					_003C_003E4__this.myAnim.Play(_003C_003E4__this.ExitVentAnim);
					if (_003C_003E4__this.myAnim.FrameTime < _003C_003E4__this.numFramesUntilPlayerReappearsOnExit)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
				}
			}
			else
			{
				if (_003C_003E1__state != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				if (_003C_003E4__this.myAnim.FrameTime < _003C_003E4__this.numFramesUntilPlayerReappearsOnExit)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
			}
			if ((bool)_003C_003E4__this.additionalExitAnimation)
			{
				_003C_003E4__this.additionalExitAnimation.SetActive(value: true);
			}
			if (pc.AmOwner && Constants.ShouldPlaySfx())
			{
				AudioClip clip = ShipStatus.Instance.VentEnterSound;
				if (!ShipStatus.Instance.VentExitSound)
				{
					SoundManager.Instance.StopSound(ShipStatus.Instance.VentEnterSound);
				}
				else
				{
					clip = ShipStatus.Instance.VentExitSound;
				}
				SoundManager.Instance.PlaySound(clip, loop: false).pitch = FloatRange.Next(0.8f, 1.2f);
				VibrationManager.Vibrate(0.4f, _003C_003E4__this.transform.position, 3.5f, 0.2f);
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		void IEnumerator.Reset()
		{
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			NotSupportedException ex = new NotSupportedException();
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			throw ex;
		}
	}

	private const float VIBRATION_INTENSITY = 0.4f;

	private const float VIBRATION_FALLOFF_DISTANCE = 3.5f;

	private const float VIBRATION_DURATION = 0.2f;

	public static Vent currentVent;

	public int Id;

	public Vent Left;

	public Vent Right;

	public Vent Center;

	public ButtonBehavior[] Buttons;

	public GameObject[] CleaningIndicators;

	public AnimationClip EnterVentAnim;

	public AnimationClip ExitVentAnim;

	public Vector3 Offset= new Vector3(0f, 0.3636057f, 0f);

	public float spreadAmount;

	public float spreadShift;

	[SerializeField]
	private SpriteRenderer myRend;

	[SerializeField]
	private SpriteAnim myAnim;

	[SerializeField]
	public int numFramesUntilPlayerDisappearsOnEnter;

	[SerializeField]
	private int numFramesUntilPlayerReappearsOnExit;

	[SerializeField]
	private GameObject additionalExitAnimation;

	public ImageNames UseIcon => ImageNames.VentButton;

	public float UsableDistance => 0.75f;

	public float PercentCool => 0f;

	public int NumFramesUntilPlayerDisappears => numFramesUntilPlayerDisappearsOnEnter;

	private Vent[] NearbyVents => new Vent[3] { Right, Left, Center };

	private void Start()
	{
		SetButtons(enabled: false);
		if (additionalExitAnimation != null)
		{
			additionalExitAnimation.SetActive(value: false);
		}
		if (myRend == null)
		{
			myRend = GetComponent<SpriteRenderer>();
		}
		if (myAnim == null)
		{
			myAnim = GetComponent<SpriteAnim>();
		}
	}

	public void SetButtons(bool enabled)
	{
		Vent[] nearbyVents = NearbyVents;
		Vector2 vector = ((!Right || !Left) ? Vector2.zero : ((Vector2)((Right.transform.position + Left.transform.position) / 2f - transform.position)));
		for (int i = 0; i < Buttons.Length; i++)
		{
			ButtonBehavior buttonBehavior = Buttons[i];
			if (enabled)
			{
				Vent vent = nearbyVents[i];
				if ((bool)vent)
				{
					bool ventBeingCleaned = ShipStatus.Instance.Systems[SystemTypes.Ventilation] is VentilationSystem ventilationSystem && ventilationSystem.IsVentCurrentlyBeingCleaned(vent.Id);
					buttonBehavior.gameObject.SetActive(value: true);
					ToggleNeighborVentBeingCleaned(ventBeingCleaned, buttonBehavior, CleaningIndicators[i]);
					Vector3 vector2 = vent.transform.position - transform.position;
					Vector3 vector3 = vector2.normalized * (0.7f + spreadShift);
					vector3.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
					vector3.y -= 0.08f;
					vector3.z = -10f;
					buttonBehavior.transform.localPosition = vector3;
					buttonBehavior.transform.LookAt2d(vent.transform);
					vector3 = vector3.RotateZ((vector.AngleSigned(vector2) > 0f) ? spreadAmount : (0f - spreadAmount));
					buttonBehavior.transform.localPosition = vector3;
					buttonBehavior.transform.Rotate(0f, 0f, (vector.AngleSigned(vector2) > 0f) ? spreadAmount : (0f - spreadAmount));
				}
				else
				{
					buttonBehavior.gameObject.SetActive(value: false);
				}
			}
			else
			{
				buttonBehavior.gameObject.SetActive(value: false);
			}
		}
	}

	public unsafe float CanUse(NetworkedPlayerInfo pc, out bool canUse, out bool couldUse)
	{
		float num = float.MaxValue;
		PlayerControl playerControl = pc.Object;
		couldUse = pc.Role.CanVent && GameManager.Instance.LogicUsables.CanUse(this, playerControl) && pc.Role.CanUse(this) && (!playerControl.MustCleanVent(Id) || (playerControl.inVent && currentVent == this)) && !pc.IsDead && (playerControl.CanMove || playerControl.inVent);
		if (ShipStatus.Instance.Systems.TryGetValue(SystemTypes.Ventilation, out var value) && value is VentilationSystem ventilationSystem && ventilationSystem.IsVentCurrentlyBeingCleaned(Id))
		{
			couldUse = false;
		}
		canUse = couldUse;
		if (canUse)
		{
			Vector3 center = playerControl.Collider.bounds.center;
			Vector3 position = transform.position;
			num = Vector2.Distance(center, position);
			canUse &= num <= UsableDistance && !PhysicsHelpers.AnythingBetween(playerControl.Collider, center, position, Constants.ShipOnlyMask, useTriggers: false);
		}
		return num;
	}

	public unsafe void SetOutline(bool on, bool mainTarget)
	{
		Color color = (PlayerControl.LocalPlayer.Data.Role.IsImpostor ? Palette.ImpostorRed : Palette.CrewmateBlue);
		myRend.material.SetFloat("_Outline", on ? 1 : 0);
		myRend.material.SetColor("_OutlineColor", color);
		myRend.material.SetColor("_AddColor", mainTarget ? color : Color.clear);
	}

	public void ClickRight()
	{
		if (!TryMoveToVent(Right, out var error))
		{
			Logger.GlobalInstance.Warning("Local Player failed to move to Right vent because of " + error);
		}
	}

	public void ClickLeft()
	{
		if (!TryMoveToVent(Left, out var error))
		{
			Logger.GlobalInstance.Warning("Local Player failed to move to Left vent because of " + error);
		}
	}

	public void ClickCenter()
	{
		if (!TryMoveToVent(Center, out var error))
		{
			Logger.GlobalInstance.Warning("Local Player failed to move to Center vent because of " + error);
		}
	}

	public void Use()
	{
		CanUse(PlayerControl.LocalPlayer.Data, out var canUse, out var _);
		if (canUse)
		{
			DestroyableSingleton<AchievementManager>.Instance.OnConsoleUse(this);
			PlayerControl localPlayer = PlayerControl.LocalPlayer;
			if (localPlayer.inVent && !localPlayer.walkingToVent)
			{
				localPlayer.MyPhysics.RpcExitVent(Id);
				SetButtons(enabled: false);
			}
			else if (!localPlayer.walkingToVent)
			{
				localPlayer.MyPhysics.RpcEnterVent(Id);
				SetButtons(enabled: true);
			}
		}
	}

	public void UpdateArrows(VentilationSystem ventSystem)
	{
		if (this != currentVent || ventSystem == null)
		{
			return;
		}
		Vent[] nearbyVents = NearbyVents;
		for (int i = 0; i < nearbyVents.Length; i++)
		{
			Vent vent = nearbyVents[i];
			if ((bool)vent)
			{
				bool ventBeingCleaned = ventSystem.IsVentCurrentlyBeingCleaned(vent.Id);
				ButtonBehavior b = Buttons[i];
				GameObject c = CleaningIndicators[i];
				ToggleNeighborVentBeingCleaned(ventBeingCleaned, b, c);
			}
		}
	}

	internal void EnterVent(PlayerControl pc)
	{
		if (pc.AmOwner)
		{
			currentVent = this;
			ConsoleJoystick.SetMode_Vent();
		}
		if ((bool)EnterVentAnim)
		{
			myAnim.Play(EnterVentAnim);
			if (pc.AmOwner && Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.StopSound(ShipStatus.Instance.VentEnterSound);
				SoundManager.Instance.PlaySound(ShipStatus.Instance.VentEnterSound, loop: false).pitch = FloatRange.Next(0.8f, 1.2f);
				VibrationManager.Vibrate(0.4f, transform.position, 3.5f, 0.2f);
			}
		}
	}

	internal IEnumerator ExitVent(PlayerControl pc)
	{
		if (pc.AmOwner)
		{
			currentVent = null;
		}
		if (!ExitVentAnim || myAnim.IsPlaying(ExitVentAnim))
		{
			yield break;
		}
		if (additionalExitAnimation != null)
		{
			additionalExitAnimation.SetActive(value: false);
		}
		myAnim.Play(ExitVentAnim);
		while (myAnim.FrameTime < numFramesUntilPlayerReappearsOnExit)
		{
			yield return null;
		}
		if (additionalExitAnimation != null)
		{
			additionalExitAnimation.SetActive(value: true);
		}
		if (pc.AmOwner && Constants.ShouldPlaySfx())
		{
			AudioClip clip = ShipStatus.Instance.VentEnterSound;
			if ((bool)ShipStatus.Instance.VentExitSound)
			{
				clip = ShipStatus.Instance.VentExitSound;
			}
			else
			{
				SoundManager.Instance.StopSound(ShipStatus.Instance.VentEnterSound);
			}
			SoundManager.Instance.PlaySound(clip, loop: false).pitch = FloatRange.Next(0.8f, 1.2f);
			VibrationManager.Vibrate(0.4f, transform.position, 3.5f, 0.2f);
		}
	}

	private unsafe bool TryMoveToVent(Vent otherVent, out string error)
	{
		if (otherVent == null)
		{
			error = "Vent does not exist";
			return false;
		}
		PlayerControl localPlayer = PlayerControl.LocalPlayer;
		if (!localPlayer.inVent)
		{
			error = "Player is not currently inside a vent";
			return false;
		}
		if (localPlayer.walkingToVent || localPlayer.Visible)
		{
			error = "Player was still in the middle of animating into current vent; not allowed to move vents that fast";
			return false;
		}
		Vector3 position = otherVent.transform.position;
		position -= (Vector3)localPlayer.Collider.offset;
		localPlayer.NetTransform.RpcSnapTo(position);
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(ShipStatus.Instance.VentMoveSounds.Random(), loop: false).pitch = FloatRange.Next(0.8f, 1.2f);
		}
		SetButtons(enabled: false);
		otherVent.SetButtons(enabled: true);
		currentVent = otherVent;
		VentilationSystem.Update(VentilationSystem.Operation.Move, currentVent.Id);
		error = string.Empty;
		return true;
	}

	private void ToggleNeighborVentBeingCleaned(bool ventBeingCleaned, ButtonBehavior b, GameObject c)
	{
		b.enabled = !ventBeingCleaned;
		c.SetActive(ventBeingCleaned);
	}

	private void OnDrawGizmosSelected()
	{
		if (Left != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, Left.transform.position);
		}
		if (Right != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, Right.transform.position);
		}
		if (Center != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, Center.transform.position);
		}
	}

	public Vent()
	{ }
}
