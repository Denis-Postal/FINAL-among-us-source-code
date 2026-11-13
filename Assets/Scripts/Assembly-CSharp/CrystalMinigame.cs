using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class CrystalMinigame : Minigame
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Func<CrystalBehaviour, bool> _003C_003E9__14_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003CCheckSolution_003Eb__14_0(CrystalBehaviour c)
		{
			return !c.CanMove;
		}
	}

	public CrystalBehaviour[] CrystalPieces;

	private CrystalBehaviour[] Shuffed;

	public Transform[] CrystalSlots;

	public FloatRange XRange;

	public float TrayY = -2.28f;

	public AudioClip[] PickUpSounds;

	public AudioClip AttachSound;

	private Controller myController= new Controller();

	private bool prevHadStick;

	private bool prevHadButton;

	private int prevSelectedCrystal= -1;

	private int selectedCrystal;

	public void Start()
	{
		Shuffed = CrystalPieces.ToArray();
		Shuffed.Shuffle();
		for (int i = 0; i < Shuffed.Length; i++)
		{
			Shuffed[i].transform.localPosition = new Vector3(XRange.Lerp(((float)i + 0.5f) / (float)Shuffed.Length), TrayY, ((float)i - (float)Shuffed.Length / 2f) / 100f);
		}
		SetupInput(disableCursor: true);
	}

	public void Update()
	{
		myController.Update();
		if (selectedCrystal != prevSelectedCrystal)
		{
			if (prevSelectedCrystal != -1)
			{
				CrystalPieces[prevSelectedCrystal].ReceiveMouseOut();
			}
			prevSelectedCrystal = selectedCrystal;
			if (selectedCrystal != -1)
			{
				CrystalPieces[selectedCrystal].ReceiveMouseOver();
			}
		}
		if (selectedCrystal == -1)
		{
			return;
		}
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			if (amOpening || amClosing != CloseState.None)
			{
				return;
			}
			Player player = ReInput.players.GetPlayer(0);
			Vector2 axis2DRaw = player.GetAxis2DRaw(13, 14);
			bool button = player.GetButton(11);
			CrystalBehaviour crystalBehaviour = CrystalPieces[selectedCrystal];
			if (button)
			{
				if (!prevHadButton)
				{
					inputHandler.disableVirtualCursor = false;
					VirtualCursor.instance.SetWorldPosition(CrystalPieces[selectedCrystal].transform.position);
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(PickUpSounds.Random(), loop: false);
					}
					crystalBehaviour.StopAllCoroutines();
				}
				else
				{
					Vector3 position = CrystalPieces[selectedCrystal].transform.position;
					position.x = VirtualCursor.currentPosition.x;
					position.y = VirtualCursor.currentPosition.y;
					crystalBehaviour.transform.position = position;
				}
				prevHadButton = true;
				return;
			}
			if (prevHadButton)
			{
				inputHandler.disableVirtualCursor = true;
				CheckSolution(selectedCrystal);
				if (crystalBehaviour.CanMove)
				{
					int num = Shuffed.IndexOf(crystalBehaviour);
					crystalBehaviour.StartCoroutine(Effects.Slide2D(crystalBehaviour.transform, crystalBehaviour.transform.localPosition, new Vector2(XRange.Lerp(((float)num + 0.5f) / (float)CrystalPieces.Length), TrayY), 0.15f));
				}
			}
			prevHadButton = false;
			if (axis2DRaw.sqrMagnitude > 0.7f)
			{
				if (!prevHadStick && selectedCrystal >= 0)
				{
					Vector2 normalized = axis2DRaw.normalized;
					int num2 = -1;
					float num3 = 0f;
					float num4 = 0.4f;
					for (int i = 0; i < CrystalPieces.Length; i++)
					{
						Vector2 lhs = CrystalPieces[i].transform.position - CrystalPieces[selectedCrystal].transform.position;
						float magnitude = lhs.magnitude;
						lhs /= magnitude;
						float num5 = Vector2.Dot(lhs, normalized) / Mathf.Lerp(magnitude, 1f, 0.95f);
						if (i != selectedCrystal && num5 > num3 && num5 > 0f && num5 > num4)
						{
							num3 = num5;
							num2 = i;
						}
					}
					if (num2 != -1)
					{
						selectedCrystal = num2;
					}
				}
				prevHadStick = true;
			}
			else
			{
				prevHadStick = false;
			}
			return;
		}
		prevHadButton = false;
		prevHadStick = false;
		if (prevSelectedCrystal != -1)
		{
			CrystalPieces[prevSelectedCrystal].Renderer.color = Color.white;
			CrystalPieces[prevSelectedCrystal].OnMouseOut.Invoke();
			prevSelectedCrystal = -1;
		}
		for (int j = 0; j < CrystalPieces.Length; j++)
		{
			CrystalBehaviour crystalBehaviour2 = CrystalPieces[j];
			switch (myController.CheckDrag(crystalBehaviour2.Collider))
			{
			case DragState.TouchStart:
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(PickUpSounds.Random(), loop: false);
				}
				crystalBehaviour2.StopAllCoroutines();
				break;
			case DragState.Dragging:
			{
				Vector3 position2 = myController.DragPosition;
				position2.z = transform.position.z;
				crystalBehaviour2.transform.position = position2;
				break;
			}
			case DragState.Released:
				CheckSolution(j);
				if (crystalBehaviour2.CanMove)
				{
					int num6 = Shuffed.IndexOf(crystalBehaviour2);
					crystalBehaviour2.StartCoroutine(Effects.Slide2D(crystalBehaviour2.transform, crystalBehaviour2.transform.localPosition, new Vector2(XRange.Lerp(((float)num6 + 0.5f) / (float)CrystalPieces.Length), TrayY), 0.15f));
				}
				break;
			}
		}
	}

	private unsafe void CheckSolution(int startAt)
	{
		CrystalBehaviour crystalBehaviour = CrystalPieces[startAt];
		if (!crystalBehaviour.CanMove)
		{
			return;
		}
		Transform transform = CrystalSlots[startAt];
		if (crystalBehaviour.Collider.OverlapPoint(transform.position))
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(AttachSound, loop: false);
			}
			crystalBehaviour.CanMove = false;
			crystalBehaviour.TargetPosition = transform;
			for (int i = startAt; i < CrystalPieces.Length; i++)
			{
				CrystalPieces[i].Flash((float)(i - startAt) * 0.1f);
			}
			for (int num = startAt - 1; num >= 0; num--)
			{
				CrystalPieces[num].Flash((float)(startAt - num) * 0.1f);
			}
			int num2 = -1;
			float num3 = 0f;
			for (int j = 0; j < CrystalPieces.Length; j++)
			{
				if (CrystalPieces[j].CanMove && (num2 == -1 || CrystalPieces[j].transform.localPosition.x < num3))
				{
					num2 = j;
					num3 = CrystalPieces[j].transform.localPosition.x;
				}
			}
			if (selectedCrystal != -1)
			{
				CrystalPieces[selectedCrystal].Renderer.color = Color.white;
			}
			if (num2 != -1)
			{
				selectedCrystal = num2;
			}
		}
		if (CrystalPieces.All((CrystalBehaviour c) => !c.CanMove))
		{
			MyNormTask.NextStep();
			StartCoroutine(CoStartClose());
			selectedCrystal = -1;
		}
	}

	public CrystalMinigame()
	{ }
}
