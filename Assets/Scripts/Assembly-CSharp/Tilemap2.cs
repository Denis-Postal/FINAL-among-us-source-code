using System;
using Cpp2ILInjected;
using UnityEngine;

public class Tilemap2 : MonoBehaviour
{
	public Sprite[] sprites;

	private Tile2[] tileData;

	public int Width = 1;

	public int Height = 1;

	internal void SetTile(Vector3Int vec, int tileId)
	{
		int num = vec.x + vec.y * Width;
		tileData[num].SpriteId = tileId;
	}

	internal void SetTransformMatrix(Vector3Int vec, Matrix4x4 rot90)
	{
	}

	internal MonoBehaviour GetTile(Vector3Int touchCellPos)
	{
		throw new NotImplementedException();
	}

	internal unsafe Vector3Int WorldToCell(Vector2 worldPos)
	{
		Vector2 vector = worldPos - (Vector2)transform.transform.position;
		return new Vector3Int(Mathf.RoundToInt(vector.x / (float)Width), Mathf.RoundToInt(vector.y / (float)Height), 0);
	}
}
