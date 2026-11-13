using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class VertLineBehaviour : MonoBehaviour
{
	public int NumPoints = 128;

	public FloatRange Width;

	public FloatRange Height;

	public Mesh mesh;

	public MeshRenderer rend;

	public Vector3[] vecs;

	public float Duration = 4f;

	private float timer;

	public int Offset = 25;

	public int beats = 4;

	public int beatPadding = 5;

	public Color color
	{
		set
		{
			rend.material.SetColor("_Color", value);
		}
	}

	// TODO(verify): rebuilt from decomp9 (control flow of Awake/Update/SetAlive/SetDead); colours that were lost in the decompile are marked below.
	public void Awake()
	{
		rend = GetComponent<MeshRenderer>();
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		mesh.MarkDynamic();
		vecs = new Vector3[NumPoints];
		Vector2[] array = new Vector2[NumPoints];
		int[] array2 = new int[NumPoints];
		float num = vecs.Length - 1;
		for (int i = 0; i < vecs.Length; i++)
		{
			vecs[i].y = Height.Lerp((float)i / num);
			array2[i] = i;
			array[i] = new Vector2(0.5f, (float)i / num);
		}
		mesh.vertices = vecs;
		mesh.uv = array;
		mesh.SetIndices(array2, MeshTopology.LineStrip, 0);
	}

	public void Update()
	{
		timer += Time.deltaTime;
		if (timer >= Duration)
		{
			timer = 0f;
		}
		float value = timer / Duration;
		mesh.vertices = vecs;
		rend.material.SetFloat("_Fade", value);
	}

	public void SetAlive()
	{
		color = Color.white;
		_ = vecs.Length;
		int num = vecs.Length / (beats + beatPadding);
		for (int i = 0; i < beats; i++)
		{
			int num2 = (int)((float)i * ((float)vecs.Length / (float)beats)) + Offset;
			for (int j = 0; j < num; j++)
			{
				int num3 = (j + num2) % vecs.Length;
				Vector3 vector = vecs[num3];
				float num4 = (float)j / (float)num;
				if ((double)num4 < 0.3)
				{
					float t = num4 / 0.3f;
					vector.x = Width.Lerp(Mathf.Lerp(0.5f, 0f, t));
				}
				else if (num4 < 0.6f)
				{
					float t2 = (num4 - 0.3f) / 0.3f;
					vector.x = Width.Lerp(Mathf.Lerp(0f, 1f, t2));
				}
				else
				{
					float t3 = (num4 - 0.6f) / 0.3f;
					vector.x = Width.Lerp(Mathf.Lerp(1f, 0.5f, t3));
				}
				vecs[num3] = vector;
			}
		}
		mesh.vertices = vecs;
	}

	public void SetDead()
	{
		color = Color.red;
		float num = vecs.Length - 1;
		for (int i = 0; i < vecs.Length; i++)
		{
			Vector3 vector = vecs[i];
			vector.x = Width.Lerp(0.5f);
			vector.y = Height.Lerp((float)i / num);
			vecs[i] = vector;
		}
		mesh.vertices = vecs;
	}
}
