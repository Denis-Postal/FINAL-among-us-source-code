using Object = UnityEngine.Object;
using System.Collections;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class TowelMinigame : Minigame
{
	private FloatRange towelRange= new FloatRange(-2.5f, -0.15f);

	public SpriteRenderer BasketGauge;

	public Collider2D BasketHitbox;

	public Collider2D[] Towels;

	private Controller controller= new Controller();

	private int towelsDone;

	public AudioClip TowelGrab;

	public AudioClip TowelLand;

	public Transform interactionCursor;

	public SpriteRenderer openHand;

	public SpriteRenderer holdingHand;

	public Vector2Range ValidArea;

	public float stickMoveSpeed= 4f;

	public float stickHeldSpeed= 3f;

	public float towelDragSpeed= 4f;

	private Collider2D[] overlapResults;

	private bool isHolding;

	private GameObject heldTowel;

	public override void Begin(PlayerTask task)
{
		base.Begin(task);
		overlapResults = new Collider2D[Towels.Length * 3];
		SetupInput(disableCursor: true);
		PlayerControl.LocalPlayer.SetPlayerMaterialColors(openHand);
		PlayerControl.LocalPlayer.SetPlayerMaterialColors(holdingHand);
		openHand.enabled = true;
		holdingHand.enabled = false;
	}

	private unsafe void Update()
{
		if (amClosing != CloseState.None)
		{
			return;
		}
		controller.Update();
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			if (!interactionCursor.gameObject.activeSelf)
			{
				interactionCursor.gameObject.SetActive(value: true);
			}
			if (isHolding)
			{
				openHand.enabled = false;
				holdingHand.enabled = true;
			}
			else
			{
				openHand.enabled = true;
				holdingHand.enabled = false;
			}
			Player player = ReInput.players.GetPlayer(0);
			Vector2 vector = new Vector2(player.GetAxis(13), player.GetAxis(14));
			Vector3 localPosition = interactionCursor.localPosition;
			if (!isHolding)
			{
				localPosition.x += vector.x * Time.deltaTime * stickMoveSpeed;
				localPosition.y += vector.y * Time.deltaTime * stickMoveSpeed;
			}
			else
			{
				localPosition.x += vector.x * Time.deltaTime * stickHeldSpeed;
				localPosition.y += vector.y * Time.deltaTime * stickHeldSpeed;
			}
			int num = Physics2D.OverlapCircleNonAlloc(interactionCursor.position, interactionCursor.transform.localScale.x, overlapResults, LayerMask.GetMask("UICollide"));
			localPosition.x = Mathf.Clamp(localPosition.x, ValidArea.min.x, ValidArea.max.x);
			localPosition.y = Mathf.Clamp(localPosition.y, ValidArea.min.y, ValidArea.max.y);
			interactionCursor.localPosition = localPosition;
			if (player.GetButton(11))
			{
				if (!isHolding)
				{
					for (int i = 0; i < num; i++)
					{
						Collider2D collider2D = overlapResults[i];
						if ((bool)collider2D.GetComponent<Rigidbody2D>())
						{
							if (Constants.ShouldPlaySfx())
							{
								SoundManager.Instance.PlaySound(TowelGrab, loop: false);
							}
							heldTowel = collider2D.gameObject;
							isHolding = true;
							break;
						}
					}
				}
				else
				{
					if (heldTowel.activeSelf)
					{
						heldTowel.GetComponent<Rigidbody2D>().linearVelocity = (localPosition - heldTowel.transform.localPosition) * towelDragSpeed;
					}
					if (heldTowel.GetComponent<Collider2D>().IsTouching(BasketHitbox))
					{
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySoundImmediate(TowelLand, loop: false);
						}
						towelsDone++;
						heldTowel.gameObject.SetActive(value: false);
					}
				}
			}
			if (player.GetButtonUp(11))
			{
				isHolding = false;
				if ((bool)heldTowel)
				{
					heldTowel.GetComponent<Rigidbody2D>().linearVelocity = (localPosition - heldTowel.transform.localPosition) * towelDragSpeed;
					heldTowel = null;
				}
			}
		}
		else if (interactionCursor.gameObject.activeSelf)
		{
			interactionCursor.gameObject.SetActive(value: false);
		}
		Collider2D[] towels = Towels;
		foreach (Collider2D collider2D2 in towels)
		{
			if (!collider2D2.isActiveAndEnabled)
			{
				continue;
			}
			if (!collider2D2.IsTouching(BasketHitbox))
			{
				switch (controller.CheckDrag(collider2D2))
				{
				case DragState.TouchStart:
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(TowelGrab, loop: false);
					}
					break;
				case DragState.Dragging:
				{
					Rigidbody2D component = collider2D2.GetComponent<Rigidbody2D>();
					component.linearVelocity = (controller.DragPosition - component.position) * 4f;
					break;
				}
				}
			}
			else
			{
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySoundImmediate(TowelLand, loop: false);
				}
				towelsDone++;
				collider2D2.gameObject.SetActive(value: false);
			}
		}
		BasketGauge.transform.localPosition = new Vector3(0f, towelRange.Lerp((float)towelsDone / (float)Towels.Length), 0.5f);
		if (towelsDone >= Towels.Length)
		{
			MyNormTask.NextStep();
			StartCoroutine(CoStartClose());
		}
	}

	public TowelMinigame()
	{ }
}
