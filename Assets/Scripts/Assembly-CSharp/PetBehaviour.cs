using System;
using System.Collections;
using PowerTools;
using UnityEngine;

public class PetBehaviour : MonoBehaviour
{
	private const float MIN_DISTANCE = 0.2f;

	private const float DAMPING = 0.7f;

	private const float EASING = 0.2f;

	private const float SPEED = 5f;

	private const float SNAP_DISTANCE = 2f;

	[SerializeField]
	private PetData data;

	[SerializeField]
	[Space(10f)]
	private SpriteAnim animator;

	[SerializeField]
	private Rigidbody2D rigidbody;

	[SerializeField]
	private Collider2D collider;

	[Header("Offsets")]
	[SerializeField]
	private float yOffset = -0.1f;

	[SerializeField]
	private Transform pettingHandPosition;

	[SerializeField]
	[Header("Animations")]
	private AnimationClip idleClip;

	[SerializeField]
	private AnimationClip sadClip;

	[SerializeField]
	private AnimationClip scaredClip;

	[SerializeField]
	private AnimationClip walkClip;

	[SerializeField]
	private AnimationClip petClip;

	[Header("Sprite Renderers")]
	[SerializeField]
	private SpriteRenderer[] renderers;

	[SerializeField]
	private SpriteRenderer[] shadows;

	private PlayerControl targetPlayer;

	private bool visible = true;

	private bool flipX;

	private bool beingPet;

	private bool manualMoving;

	private bool viewOnly;

	public PetData Data => data;

	public PlayerControl TargetPlayer => targetPlayer;

	public Vector3 PettingHandPosition
	{
		get
		{
			if (!(pettingHandPosition != null))
			{
				return transform.position;
			}
			return pettingHandPosition.position;
		}
	}

	public Vector3 PettingPlayerPosition => transform.position + new Vector3(-0.65f, -0.25100002f);

	public int RendererCount => renderers.Length;

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			visible = value;
			ForEachRenderer(includeShadows: true, (SpriteRenderer x) =>
			{
				x.enabled = visible;
			});
		}
	}

	public bool FlipX
	{
		get
		{
			return flipX;
		}
		set
		{
			flipX = value;
			ForEachRenderer(includeShadows: true, (SpriteRenderer x) =>
			{
				x.flipX = flipX;
			});
		}
	}

	public void SetupNewPet(PetData data, Rigidbody2D body, CircleCollider2D collider, SpriteAnim animator, AnimationClip idleClip, AnimationClip scaredClip, AnimationClip walkClip, AnimationClip sadClip, AnimationClip petClip, SpriteRenderer[] renderers, SpriteRenderer[] shadows)
	{
		this.data = data;
		rigidbody = body;
		this.collider = collider;
		this.animator = animator;
		this.idleClip = idleClip;
		this.scaredClip = scaredClip;
		this.walkClip = walkClip;
		this.sadClip = sadClip;
		this.petClip = petClip;
		this.renderers = renderers;
		this.shadows = shadows;
		gameObject.layer = 8;
	}

	public void SetTargetPlayer(PlayerControl targetPlayer)
	{
		this.targetPlayer = targetPlayer;
	}

	public void SetDefaultMaterial()
	{
		ForEachRenderer(includeShadows: false, (SpriteRenderer x) =>
		{
			if (x.sharedMaterial.shader.name == "Sprites/Default")
			{
				x.material = DestroyableSingleton<HatManager>.Instance.DefaultShader;
			}
		});
		ForEachShadow((SpriteRenderer x) =>
		{
			x.material = DestroyableSingleton<HatManager>.Instance.DefaultShader;
		});
	}

	public void SetCrewmateColor(int colorId)
	{
		ForEachRenderer(includeShadows: false, (SpriteRenderer x) =>
		{
			PlayerMaterial.SetColors(colorId, x);
		});
	}

	public void SetRawColor(Color color)
	{
		ForEachRenderer(includeShadows: false, (SpriteRenderer x) =>
		{
			x.color = color;
		});
	}

	public void SetAsLocalPlayer()
	{
		ForEachRenderer(includeShadows: true, (SpriteRenderer x) =>
		{
			PlayerMaterial.SetMaskLayerBasedOnLocalPlayer(x, isLocalPlayer: true);
		});
	}

	public void SetAlpha(float alpha)
	{
		ForEachRenderer(includeShadows: true, (SpriteRenderer x) =>
		{
			x.color = x.color.SetAlpha(alpha);
		});
	}

	public bool HasCrewmateColorShader()
	{
		SpriteRenderer[] array = renderers;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].sharedMaterial.shader.name.Contains("PlayerShader"))
			{
				return true;
			}
		}
		return false;
	}

	public void SetIdleOrMourning()
	{
		if (TargetPlayer != null && !TargetPlayer.Data.IsDead)
		{
			SetIdle();
		}
		else
		{
			SetMourning();
		}
	}

	public void SetMourning()
	{
		targetPlayer = null;
		rigidbody.linearVelocity = Vector2.zero;
		animator.Play(sadClip);
	}

	public void SetIdle()
	{
		animator.Play(idleClip);
	}

	public void SetScared()
	{
		animator.Play(scaredClip);
	}

	public void PrepareForUI()
	{
		collider.enabled = false;
		ForEachRenderer(includeShadows: false, (SpriteRenderer x) =>
		{
			x.gameObject.layer = LayerMask.NameToLayer("UI");
		});
	}

	public void PrepareForPreview(bool previewUsesCrewmateColor)
	{
		if (previewUsesCrewmateColor)
		{
			Material defaultMaterial = new Material(Shader.Find("Sprites/Default"));
			ForEachRenderer(includeShadows: false, (SpriteRenderer x) =>
			{
				x.sharedMaterial = defaultMaterial;
			});
		}
		else
		{
			SetCrewmateColor(0);
		}
		ForEachShadow((SpriteRenderer x) =>
		{
			x.enabled = false;
		});
	}

	private void Start()
	{
		if (Application.targetFrameRate > 30)
		{
			rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
		}
	}

	private void Update()
	{
		if (viewOnly || manualMoving)
		{
			return;
		}
		if (!TargetPlayer || beingPet)
		{
			rigidbody.linearVelocity = Vector2.zero;
			return;
		}
		if (TargetPlayer != null && TargetPlayer.Data != null && TargetPlayer.Data.IsDead)
		{
			transform.position = TargetPlayer.GetTruePosition();
			SetMourning();
			return;
		}
		Vector2 truePosition = TargetPlayer.GetTruePosition();
		Vector2 truePosition2 = GetTruePosition();
		Vector2 vector = rigidbody.linearVelocity;
		Vector2 vector2 = truePosition - truePosition2;
		float num = 0f;
		if (TargetPlayer.CanMove)
		{
			num = 0.2f;
		}
		if (vector2.sqrMagnitude > num)
		{
			if (vector2.sqrMagnitude > 2f)
			{
				transform.position = truePosition;
				return;
			}
			vector2 *= 5f * TargetPlayer.MyPhysics.SpeedMod;
			vector = vector * 0.8f + vector2 * 0.2f;
		}
		else
		{
			vector *= 0.7f;
		}
		AnimationClip currentAnimation = animator.GetCurrentAnimation();
		if (vector.sqrMagnitude > 0.01f)
		{
			if (currentAnimation != walkClip)
			{
				StartWalkAnim();
			}
			if (vector.x < -0.01f)
			{
				FlipX = true;
			}
			else if (vector.x > 0.01f)
			{
				FlipX = false;
			}
		}
		else if (currentAnimation == walkClip)
		{
			animator.Play(idleClip);
		}
		rigidbody.linearVelocity = vector;
	}

	private void LateUpdate()
	{
		if (!viewOnly)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition.z = (localPosition.y + yOffset) / 1000f + 0.0002f;
			transform.localPosition = localPosition;
		}
	}

	public void SetGettingPet(bool petting, Vector2 petPos)
	{
		FlipX = true;
		bool flag = beingPet != petting;
		beingPet = petting;
		transform.position = petPos;
		Visible = true;
		manualMoving = false;
		if (!beingPet & flag)
		{
			SetIdleOrMourning();
		}
	}

	public void StartPetAnim()
	{
		animator.Play(petClip);
	}

	public void StartWalkAnim()
	{
		animator.Play(walkClip);
	}

	public void ToggleViewOnly(bool value)
	{
		viewOnly = value;
		Logger.GlobalInstance.Info("[PetBehaviour] Setting Pet '" + gameObject.name + "' viewOnly to " + value.ToString().ToUpper());
	}

	private Vector2 GetTruePosition()
	{
		return (Vector2)transform.position + collider.offset * 0.7f;
	}

	private void ForEachRenderer(bool includeShadows, Action<SpriteRenderer> action)
	{
		renderers.ForEach((SpriteRenderer x) =>
		{
			action(x);
		});
		if (includeShadows)
		{
			ForEachShadow(action);
		}
	}

	private void ForEachShadow(Action<SpriteRenderer> action)
	{
		shadows.ForEach((SpriteRenderer x) =>
		{
			action(x);
		});
	}

	public IEnumerator MoveBehindSource(float speed)
	{
		manualMoving = true;
		Vector2 truePosition = TargetPlayer.GetTruePosition();
		truePosition.y += 0.1f;
		yield return WalkPetTo(truePosition, speed);
	}

	private IEnumerator WalkPetTo(Vector2 worldPos, float speed, float tolerance = 0.01f)
	{
		manualMoving = true;
		Rigidbody2D body = rigidbody;
		if (animator.GetCurrentAnimation() != walkClip)
		{
			StartWalkAnim();
		}
		Vector2 del = worldPos - (Vector2)transform.position;
		while (del.sqrMagnitude > tolerance)
		{
			Mathf.Clamp(del.magnitude * 2f, 0.05f, 1f);
			body.linearVelocity = del.normalized * speed;
			yield return null;
			if (body.linearVelocity.magnitude < 0.005f && (double)del.sqrMagnitude < 0.1)
			{
				break;
			}
			del = worldPos - (Vector2)transform.position;
		}
		body.linearVelocity = Vector2.zero;
		manualMoving = false;
	}
}
