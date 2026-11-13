using System;
using Cpp2ILInjected;
using UnityEngine;

[Serializable]
public struct Vector2Range
{
	public Vector2 min;

	public Vector2 max;

	public float Width => max.x - min.x;

	public float Height => max.y - min.y;

	public Vector2Range(Vector2 min, Vector2 max)
	{
		this.min = min;
		this.max = max;
	}

	public unsafe void LerpUnclamped(ref Vector3 output, float t, float z)
	{ /* spliced-from-2019 */
		output.Set(Mathf.LerpUnclamped(min.x, max.x, t), Mathf.LerpUnclamped(min.y, max.y, t), z);
	}

	public unsafe void Lerp(ref Vector3 output, float t, float z)
	{ /* spliced-from-2019 */
		output.Set(Mathf.Lerp(min.x, max.x, t), Mathf.Lerp(min.y, max.y, t), z);
	}

	public Vector2 Next()
	{ /* spliced-from-2019 */
		return new Vector2(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y));
	}

	public static Vector2 NextEdge()
	{
		float f = MathF.PI * 2f * UnityEngine.Random.value;
		float x = Mathf.Cos(f);
		float y = Mathf.Sin(f);
		return new Vector2(x, y);
	}
}
