using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class QuadGen : MonoBehaviour
{
	public float Width = 1f;

	public float Height = 1f;

	public int WidthSubdivisions;

	public int HeightSubdivisions;

	public float PerlinFreqX = 10f;

	public void Generate()
	{
		Mesh mesh = (GetComponent<MeshFilter>().mesh = new Mesh());
		Mesh mesh3 = mesh;
		int num = WidthSubdivisions + 2;
		int num2 = HeightSubdivisions + 2;
		Vector3[] array = new Vector3[num * num2];
		Vector2[] array2 = new Vector2[num * num2];
		Vector3 vector = default;
		Vector2 vector2 = default;
		for (int i = 0; i < num2; i++)
		{
			vector.y = Height * (0.5f - (float)i / ((float)HeightSubdivisions + 1f));
			for (int j = 0; j < num; j++)
			{
				vector.x = Width * (0.5f - (float)j / ((float)WidthSubdivisions + 1f));
				int num3 = j + i * num;
				array[num3] = vector;
				vector2.y = Mathf.Lerp(-1f, 1f, Mathf.PerlinNoise((float)j * PerlinFreqX, i));
				array2[num3] = vector2;
			}
		}
		List<int> list = new List<int>();
		for (int k = 0; k < array.Length; k++)
		{
			if ((k + 1) % num != 0)
			{
				if (k + num >= array.Length)
				{
					break;
				}
				list.Add(k);
				list.Add(k + num);
				list.Add(k + num + 1);
				list.Add(k);
				list.Add(k + num + 1);
				list.Add(k + 1);
			}
		}
		mesh3.vertices = array;
		mesh3.uv = array2;
		mesh3.SetIndices(list.ToArray(), MeshTopology.Triangles, 0);
	}
}
