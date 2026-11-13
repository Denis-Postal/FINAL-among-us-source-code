using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Tilemaps;

public class Weather1Game : Minigame
{
	private static int[] BarrierValidX = new int[10] { -10, -8, -6, -4, -2, 0, 2, 4, 6, 8 };

	private static int[] BarrierValidY = new int[3] { -2, 0, 2 };

	private const int MinX = -10;

	private const int MaxX = 8;

	private const int MinY = -3;

	private const int MaxY = 3;

	public Tilemap BarrierMap;

	public Tile fillTile;

	public Tile controlTile;

	public Tile barrierTile;

	public SpriteRenderer pulseCircle1;

	public SpriteRenderer pulseCircle2;

	public AudioClip NodeMove;

	private Controller control= new Controller();

	private bool inControl;

	private Vector3Int controlTilePos= new Vector3Int(-9, 3, 0);

	private static Vector3Int[] Directions= new Vector3Int[4]
	{
		Vector3Int.up,
		Vector3Int.down,
		Vector3Int.left,
		Vector3Int.right
	};

	private float moveCooldown;

	public unsafe void Start()
{
		Vector3Int vector3Int = new Vector3Int(-9, 3, 0);
		HashSet<Vector3Int> hashSet = new HashSet<Vector3Int>();
		hashSet.Add(vector3Int);
		SolveMaze(vector3Int, hashSet);
		Matrix4x4 matrix4x = Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, 90f));
		new List<Vector3Int>();
		Vector3Int vector3Int2 = new Vector3Int
		{
			x = -10
		};
		while (vector3Int2.x <= 8)
		{
			bool flag = vector3Int2.x % 2 == 0;
			vector3Int2.y = -3;
			int y;
			while (vector3Int2.y <= 3)
			{
				bool flag2 = vector3Int2.y % 2 == 0;
				if (PointIsValid(vector3Int2) && !hashSet.Contains(vector3Int2) && flag == flag2 && BoolRange.Next(0.75f))
				{
					BarrierMap.SetTile(vector3Int2, barrierTile);
					if (flag)
					{
						BarrierMap.SetTransformMatrix(vector3Int2, matrix4x);
					}
				}
				y = vector3Int2.y + 1;
				vector3Int2.y = y;
			}
			y = vector3Int2.x + 1;
			vector3Int2.x = y;
		}
		SetupInput(disableCursor: true);
	}

	private unsafe bool SolveMaze(Vector3Int curPos, HashSet<Vector3Int> solution)
{
		if (solution.Count > 50)
		{
			return false;
		}
		bool[] array = new bool[4];
		while (Contains(array, item: false))
		{
			int num = array.RandomIdx();
			while (array[num])
			{
				num = (num + 1) % array.Length;
			}
			array[num] = true;
			Vector3Int vector3Int = Directions[num] + curPos;
			if (PointIsValid(vector3Int) && solution.Add(vector3Int))
			{
				if (vector3Int.x == 7 && vector3Int.y == -3)
				{
					return true;
				}
				if (SolveMaze(vector3Int, solution))
				{
					return true;
				}
				solution.Remove(vector3Int);
			}
		}
		return false;
	}

	public unsafe void Update()
{
		if (amClosing != CloseState.None)
		{
			return;
		}
		control.Update();
		if (BarrierMap.GetTile(new Vector3Int(7, -3, 0)) == controlTile)
		{
			pulseCircle1.enabled = false;
			pulseCircle2.enabled = false;
			MyNormTask.NextStep();
			StartCoroutine(CoStartClose());
			return;
		}
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			Player player = ReInput.players.GetPlayer(0);
			if (player.GetButton(11))
			{
				if (!inControl)
				{
					inControl = true;
				}
				if (moveCooldown > 0f)
				{
					moveCooldown -= Time.deltaTime;
				}
				Vector2 axis2DRaw = player.GetAxis2DRaw(13, 14);
				Vector2 vector = new Vector2(Mathf.Abs(axis2DRaw.x), Mathf.Abs(axis2DRaw.y));
				float num = Mathf.Max(vector.x, vector.y);
				float num2 = Mathf.Min(vector.x, vector.y);
				Vector3Int vector3Int = Vector3Int.zero;
				bool flag = false;
				if (num > 0.7f && num2 < 0.2f)
				{
					if (vector.x > vector.y)
					{
						vector3Int = new Vector3Int((int)Mathf.Sign(axis2DRaw.x), 0, 0);
						flag = true;
					}
					else
					{
						vector3Int = new Vector3Int(0, (int)Mathf.Sign(axis2DRaw.y), 0);
						flag = true;
					}
				}
				else
				{
					moveCooldown = 0f;
				}
				if (flag && moveCooldown <= 0f)
				{
					moveCooldown = 0.15f;
					Vector3Int vector3Int2 = controlTilePos + vector3Int;
					if (!BarrierMap.GetTile(vector3Int2) && PointIsValid(vector3Int2) && !AnythingBetween(controlTilePos, vector3Int2))
					{
						FillLine(controlTilePos, vector3Int2);
						controlTilePos = vector3Int2;
						BarrierMap.SetTile(controlTilePos, controlTile);
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySoundImmediate(NodeMove, loop: false);
						}
					}
				}
			}
			else
			{
				moveCooldown = 0f;
				if (inControl)
				{
					inControl = false;
					Vector3Int position = new Vector3Int
					{
						x = -10
					};
					while (position.x <= 8)
					{
						position.y = -3;
						int y;
						while (position.y <= 3)
						{
							if (BarrierMap.GetTile(position) == fillTile)
							{
								BarrierMap.SetTile(position, null);
							}
							y = position.y + 1;
							position.y = y;
						}
						y = position.x + 1;
						position.x = y;
					}
					BarrierMap.SetTile(controlTilePos, null);
					controlTilePos.x = -9;
					controlTilePos.y = 3;
					BarrierMap.SetTile(controlTilePos, controlTile);
				}
			}
		}
		else if (control.AnyTouch)
		{
			for (int i = 0; i < control.Touches.Length; i++)
			{
				Controller.TouchState touch = control.GetTouch(i);
				Vector3Int vector3Int3 = BarrierMap.WorldToCell(touch.Position);
				TileBase tile = BarrierMap.GetTile(vector3Int3);
				if (touch.TouchStart)
				{
					if (tile == controlTile)
					{
						inControl = true;
						controlTilePos = vector3Int3;
					}
				}
				else if (inControl && !tile && PointIsValid(vector3Int3) && !AnythingBetween(controlTilePos, vector3Int3))
				{
					FillLine(controlTilePos, vector3Int3);
					controlTilePos = vector3Int3;
					BarrierMap.SetTile(controlTilePos, controlTile);
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySoundImmediate(NodeMove, loop: false);
					}
				}
			}
		}
		else if (control.AnyTouchUp)
		{
			for (int j = 0; j < control.Touches.Length; j++)
			{
				control.GetTouch(j);
				if (!inControl)
				{
					continue;
				}
				Vector3Int position2 = new Vector3Int
				{
					x = -10
				};
				while (position2.x <= 8)
				{
					position2.y = -3;
					int y;
					while (position2.y <= 3)
					{
						if (BarrierMap.GetTile(position2) == fillTile)
						{
							BarrierMap.SetTile(position2, null);
						}
						y = position2.y + 1;
						position2.y = y;
					}
					y = position2.x + 1;
					position2.x = y;
				}
				BarrierMap.SetTile(controlTilePos, null);
				controlTilePos.x = -9;
				controlTilePos.y = 3;
				BarrierMap.SetTile(controlTilePos, controlTile);
			}
		}
		else
		{
			inControl = false;
		}
		pulseCircle1.transform.position = BarrierMap.CellToWorld(controlTilePos) + new Vector3(0.16f, 0.16f, 0f);
	}

	private void ResetPath()
	{
		for (int i = -10; i <= 8; i++)
		{
			for (int j = -3; j <= 3; j++)
			{
				Vector3Int position = new Vector3Int(i, j, 0);
				if (BarrierMap.GetTile(position) == fillTile)
				{
					BarrierMap.SetTile(position, null);
				}
			}
		}
		BarrierMap.SetTile(controlTilePos, null);
		controlTilePos = new Vector3Int(-9, 0, 0);
		BarrierMap.SetTile(controlTilePos, controlTile);
	}

	private unsafe void FillLine(Vector3Int controlTilePos, Vector3Int touchCellPos)
{
		Vector3Int position = controlTilePos;
		BarrierMap.SetTile(position, fillTile);
		if (controlTilePos.x == touchCellPos.x)
		{
			int num = (int)Mathf.Sign(touchCellPos.y - controlTilePos.y);
			while ((position.y += num) != touchCellPos.y)
			{
				BarrierMap.SetTile(position, fillTile);
			}
		}
		else if (controlTilePos.y == touchCellPos.y)
		{
			int num2 = (int)Mathf.Sign(touchCellPos.x - controlTilePos.x);
			while ((position.x += num2) != touchCellPos.x)
			{
				BarrierMap.SetTile(position, fillTile);
			}
		}
	}

	private unsafe bool AnythingBetween(Vector3Int controlTilePos, Vector3Int touchCellPos)
{
		Vector3Int vector3Int = controlTilePos;
		if (controlTilePos.x == touchCellPos.x)
		{
			int num = (int)Mathf.Sign(touchCellPos.y - controlTilePos.y);
			while ((vector3Int.y += num) != touchCellPos.y)
			{
				if ((bool)BarrierMap.GetTile(vector3Int) || !PointIsValid(vector3Int))
				{
					return true;
				}
			}
			return false;
		}
		if (controlTilePos.y == touchCellPos.y)
		{
			int num2 = (int)Mathf.Sign(touchCellPos.x - controlTilePos.x);
			while ((vector3Int.x += num2) != touchCellPos.x)
			{
				if ((bool)BarrierMap.GetTile(vector3Int) || !PointIsValid(vector3Int))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	private bool PointIsValid(Vector3Int touchCellPos)
{
		bool flag = touchCellPos.x % 2 == 0;
		bool result = touchCellPos.y % 2 == 0;
		if (touchCellPos.x <= 8 && touchCellPos.x >= -10 && touchCellPos.y <= 3 && touchCellPos.y >= -3)
		{
			if (flag)
			{
				return result;
			}
			return true;
		}
		return false;
	}

	private unsafe bool Contains<T>(T[] self, T item) where T : IComparable
{
		for (int i = 0; i < self.Length; i++)
		{
			if (self[i].Equals(item))
			{
				return true;
			}
		}
		return false;
	}

	public Weather1Game()
	{ }

	static Weather1Game()
{ }
}
