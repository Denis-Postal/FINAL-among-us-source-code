using Object = UnityEngine.Object;
using System.Collections;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class AirshipGarbageGame : Minigame
{
	public GarbageCanBehaviour[] GarbagePrefabs;

	public Sprite RelaxeHandle;

	public Sprite PulledHandle;

	private GarbageCanBehaviour can;

	public AudioClip grabSound;

	public Controller controller= new Controller();

	public Transform handCursorObject;

	public GameObject waitingHands;

	public GameObject grabbedHands;

	public SpriteRenderer[] handSprites;

	private bool prevHadLeftInput;

	private const float stickVelocityMagnitude = 6f;

	public override void Begin(PlayerTask task)
{
		base.Begin(task);
		can = Object.Instantiate(GarbagePrefabs[ConsoleId], transform);
		SetupInput(disableCursor: true);
		SpriteRenderer[] array = handSprites;
		foreach (SpriteRenderer playerMaterialColors in array)
		{
			PlayerControl.LocalPlayer.SetPlayerMaterialColors(playerMaterialColors);
		}
	}

	private unsafe void Update()
{
		if (amOpening || amClosing != CloseState.None)
		{
			return;
		}
		controller.Update();
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			Player player = ReInput.players.GetPlayer(0);
			handCursorObject.position = can.Handle.transform.position;
			if (player.GetButton(24) && player.GetButton(21))
			{
				if (!grabbedHands.activeSelf)
				{
					grabbedHands.SetActive(value: true);
					waitingHands.SetActive(value: false);
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(grabSound, loop: false);
					}
				}
				can.Handle.sprite = PulledHandle;
				Vector2 axis2DRaw = player.GetAxis2DRaw(13, 14);
				if (axis2DRaw.magnitude > 0.9f)
				{
					if (!prevHadLeftInput)
					{
						can.Body.linearVelocity = axis2DRaw.normalized * 6f;
					}
					prevHadLeftInput = true;
				}
				else
				{
					prevHadLeftInput = false;
				}
			}
			else
			{
				if (!waitingHands.activeSelf)
				{
					grabbedHands.SetActive(value: false);
					waitingHands.SetActive(value: true);
				}
				can.Handle.sprite = RelaxeHandle;
			}
		}
		else
		{
			switch (controller.CheckDrag(can.Hitbox))
			{
			case DragState.TouchStart:
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(grabSound, loop: false);
				}
				break;
			case DragState.Dragging:
			{
				can.Handle.sprite = PulledHandle;
				Vector2 vector = controller.DragPosition - (Vector2)can.Handle.transform.position;
				can.Body.linearVelocity = 10f * vector;
				break;
			}
			default:
				can.Handle.sprite = RelaxeHandle;
				break;
			}
		}
		if (!can.Body.IsTouching(can.Success))
		{
			MyNormTask.NextStep();
			StartCoroutine(CoStartClose(0.7f));
		}
	}

	public AirshipGarbageGame()
	{ }
}
