using System;
using Cpp2ILInjected;
using UnityEngine;

public class VirtualJoystick : MonoBehaviour, IVirtualJoystick
{
	private sealed class _003C_003Ec__DisplayClass16_0
	{
		public bool on;

		internal void _003CToggleVisuals_003Eb__0(SpriteRenderer s)
		{
			s.enabled = on;
		}
	}

	public float InnerRadius = 0.64f;

	public float OuterRadius = 1.28f;

	public CircleCollider2D Outer;

	public SpriteRenderer Inner;

	public SpriteRenderer[] AllSprites;

	public bool IsDragged;

	public bool IsRightJoystick;

	private Vector2 DeltaL_bf;

	private Vector2 DeltaR_bf;

	private VirtualJoystickController myController= new VirtualJoystickController();

	public Vector2 DeltaL { get; private set; }

	public Vector2 DeltaR { get; private set; }

	public void ToggleVisuals(bool on)
	{
		AllSprites.ForEach((SpriteRenderer s) =>
		{
			s.enabled = on;
		});
	}

	protected unsafe virtual void FixedUpdate()
	{
		myController.Update();
		switch (myController.CheckDrag(Outer, IsRightJoystick ? DestroyableSingleton<HudManager>.Instance.RightStickDeadZone : DestroyableSingleton<HudManager>.Instance.LeftStickDeadZone))
		{
		case DragState.TouchStart:
		case DragState.Dragging:
		{
			float maxLength = OuterRadius - InnerRadius;
			Vector2 vector = myController.DragPosition - (Vector2)transform.position;
			_ = vector.magnitude;
			Vector2 vector2 = new Vector2(Mathf.Sqrt(Mathf.Abs(vector.x)) * Mathf.Sign(vector.x), Mathf.Sqrt(Mathf.Abs(vector.y)) * Mathf.Sign(vector.y));
			if (IsRightJoystick)
			{
				DeltaR = Vector2.ClampMagnitude(vector2 / OuterRadius, 1f);
			}
			else
			{
				DeltaL = Vector2.ClampMagnitude(vector2 / OuterRadius, 1f);
			}
			Inner.transform.localPosition = Vector3.ClampMagnitude(vector, maxLength) + Vector3.back;
			IsDragged = true;
			break;
		}
		case DragState.Released:
			if (IsRightJoystick)
			{
				DeltaR = Vector2.zero;
			}
			else
			{
				DeltaL = Vector2.zero;
			}
			Inner.transform.localPosition = Vector3.back;
			IsDragged = false;
			break;
		case DragState.Holding:
			break;
		}
	}

	public unsafe virtual void UpdateJoystick(HowToPlaySceneMovementPageFingerBehaviour finger, Vector2 velocity, bool syncFinger)
	{ /* spliced-from-2019 */
		Vector3 localPosition = Inner.transform.localPosition;
		Vector3 vector = velocity.normalized * InnerRadius;
		vector.z = localPosition.z;
		if (syncFinger)
		{
			localPosition = Vector3.Lerp(localPosition, vector, Time.fixedDeltaTime * 5f);
			Inner.transform.localPosition = localPosition;
			localPosition = Inner.transform.position;
			localPosition.z = -26f;
			finger.transform.position = localPosition;
		}
		else if (Inner.gameObject != finger.gameObject)
		{
			Inner.transform.localPosition = vector;
		}
	}

	public VirtualJoystick()
	{ }
}
