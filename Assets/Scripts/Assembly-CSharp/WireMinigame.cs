using Object = UnityEngine.Object;
using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class WireMinigame : Minigame
{
	private static readonly Color[] colors= new Color[4]
	{
		Color.red,
		new Color(0.15f, 0.15f, 1f, 1f),
		Color.yellow,
		Color.magenta
	};

	public Sprite[] Symbols;

	public Wire[] LeftNodes;

	public WireNode[] RightNodes;

	public SpriteRenderer[] LeftLights;

	public SpriteRenderer[] RightLights;

	private Controller myController = new Controller();

	private sbyte[] ExpectedWires = new sbyte[4];

	private sbyte[] ActualWires = new sbyte[4];

	public AudioClip[] WireSounds;

	private int prevSelectedWireIndex = -1;

	private int selectedWireIndex;

	private bool prevButtonDown;

	private float inputCooldown;

	public Vector2 controllerWirePos= Vector2.zero;

	private const float controllerWireSpeed = 7f;

	public GameObject[] selectingWireGlyphs;

	public GameObject[] movingWireGlyphs;

	public Transform selectedWireUI;

	private bool TaskIsForThisPanel()
	{
		if (MyNormTask.taskStep < MyNormTask.Data.Length && !MyNormTask.IsComplete)
		{
			return MyNormTask.Data[MyNormTask.taskStep] == ConsoleId;
		}
		return false;
	}

	public unsafe override void Begin(PlayerTask task)
	{
		base.Begin(task);
		IntRange.FillRandomRange(ExpectedWires);
		for (int i = 0; i < LeftNodes.Length; i++)
		{
			ActualWires[i] = -1;
			int num = ExpectedWires[i];
			Wire wire = LeftNodes[i];
			wire.SetColor(colors[num], Symbols[num]);
			wire.WireId = (sbyte)i;
			RightNodes[i].SetColor(colors[i], Symbols[i]);
			RightNodes[i].WireId = (sbyte)i;
			int num2 = ActualWires[i];
			if (num2 > -1)
			{
				wire.ConnectRight(RightNodes[num2]);
			}
			else
			{
				wire.ResetLine(Vector3.zero, reset: true);
			}
		}
		UpdateLights();
		SetupInput(disableCursor: true);
	}

	public unsafe void Update()
	{
		if (!TaskIsForThisPanel())
		{
			return;
		}
		myController.Update();
		selectedWireUI.gameObject.SetActive(Controller.currentTouchType == Controller.TouchType.Joystick);
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			if (!amOpening)
			{
				_ = (Vector2)transform.position;
				Player player = ReInput.players.GetPlayer(0);
				Vector2 vector = new Vector2(player.GetAxis(13), player.GetAxis(14));
				bool button = player.GetButton(11);
				if (button != prevButtonDown)
				{
					GameObject[] array = movingWireGlyphs;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].SetActive(button);
					}
					array = selectingWireGlyphs;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].SetActive(!button);
					}
					if (button)
					{
						controllerWirePos = LeftNodes[selectedWireIndex].transform.position + Vector3.right * 0.2f;
						VibrationManager.Vibrate(0.3f, 0f, 0.05f);
					}
				}
				if (button)
				{
					prevSelectedWireIndex = selectedWireIndex;
					Wire wire = LeftNodes[selectedWireIndex];
					controllerWirePos += vector * Time.deltaTime * 7f;
					controllerWirePos.x = Mathf.Min(controllerWirePos.x, RightNodes[0].transform.position.x);
					WireNode wireNode = CheckRightSide(controllerWirePos);
					wireNode = CheckRightSide(controllerWirePos);
					if ((bool)wireNode)
					{
						Vector2 vector2 = wireNode.transform.position;
						ActualWires[wire.WireId] = wireNode.WireId;
						wire.ResetLine(vector2);
					}
					else
					{
						ActualWires[wire.WireId] = -1;
						wire.ResetLine(controllerWirePos);
					}
				}
				else
				{
					prevSelectedWireIndex = -1;
					if (prevButtonDown)
					{
						Wire wire2 = LeftNodes[selectedWireIndex];
						if (ActualWires[wire2.WireId] == -1)
						{
							wire2.ResetLine(wire2.BaseWorldPos, reset: true);
						}
						else
						{
							if (Constants.ShouldPlaySfx())
							{
								SoundManager.Instance.PlaySound(WireSounds.Random(), loop: false);
							}
							VibrationManager.Vibrate(0f, 0.3f, 0.05f);
						}
						CheckTask();
					}
					else if (Math.Abs(vector.y) > 0.5f)
					{
						if (inputCooldown > 0f)
						{
							inputCooldown -= Time.deltaTime;
						}
						else
						{
							int num = selectedWireIndex;
							int num2 = (int)Mathf.Sign(0f - vector.y);
							selectedWireIndex = Mathf.Clamp(selectedWireIndex + num2, 0, 3);
							if (selectedWireIndex != num)
							{
								inputCooldown = 0.25f;
								Vector3 localPosition = selectedWireUI.localPosition;
								localPosition.y = LeftNodes[selectedWireIndex].transform.localPosition.y;
								selectedWireUI.localPosition = localPosition;
							}
						}
					}
					else
					{
						inputCooldown = 0f;
					}
				}
				prevButtonDown = button;
			}
		}
		else
		{
			prevButtonDown = false;
			if (prevSelectedWireIndex != -1)
			{
				Wire wire3 = LeftNodes[prevSelectedWireIndex];
				if (ActualWires[wire3.WireId] == -1)
				{
					wire3.ResetLine(wire3.BaseWorldPos, reset: true);
				}
				else if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(WireSounds.Random(), loop: false);
				}
				CheckTask();
				prevSelectedWireIndex = -1;
			}
			_ = (Vector2)transform.position;
			for (int j = 0; j < LeftNodes.Length; j++)
			{
				Wire wire4 = LeftNodes[j];
				switch (myController.CheckDrag(wire4.hitbox))
				{
				case DragState.Dragging:
				{
					Vector2 vector3 = myController.DragPosition;
					WireNode wireNode2 = CheckRightSide(vector3);
					if ((bool)wireNode2)
					{
						vector3 = wireNode2.transform.position;
						ActualWires[wire4.WireId] = wireNode2.WireId;
					}
					else
					{
						vector3 -= wire4.BaseWorldPos.normalized * 0.05f;
						ActualWires[wire4.WireId] = -1;
					}
					wire4.ResetLine(vector3);
					break;
				}
				case DragState.Released:
					if (ActualWires[wire4.WireId] == -1)
					{
						wire4.ResetLine(wire4.BaseWorldPos, reset: true);
					}
					else if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(WireSounds.Random(), loop: false);
					}
					CheckTask();
					break;
				}
			}
		}
		UpdateLights();
	}

	private unsafe void UpdateLights()
	{ /* spliced-from-2019 */
		for (int i = 0; i < ActualWires.Length; i++)
		{
			Color yellow = Color.yellow;
			yellow *= 1f - Mathf.PerlinNoise(i, Time.time * 35f) * 0.3f;
			yellow.a = 1f;
			if (ActualWires[i] != ExpectedWires[i])
			{
				RightLights[ExpectedWires[i]].color = new Color(0.2f, 0.2f, 0.2f);
			}
			else
			{
				RightLights[ExpectedWires[i]].color = yellow;
			}
			LeftLights[i].color = yellow;
		}
	}

	private WireNode CheckRightSide(Vector2 pos)
	{ /* spliced-from-2019 */
		for (int i = 0; i < RightNodes.Length; i++)
		{
			WireNode wireNode = RightNodes[i];
			if (wireNode.hitbox.OverlapPoint(pos))
			{
				return wireNode;
			}
		}
		return null;
	}

	private void CheckTask()
	{ /* spliced-from-2019 */
		bool flag = true;
		for (int i = 0; i < ActualWires.Length; i++)
		{
			if (ActualWires[i] != ExpectedWires[i])
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			MyNormTask.NextStep();
			Close();
		}
	}

	public WireMinigame()
	{ }

	static WireMinigame()
	{ }
}
