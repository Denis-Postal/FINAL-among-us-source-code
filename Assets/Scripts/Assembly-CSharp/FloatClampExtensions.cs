using UnityEngine;

// TODO(verify): original ClampToInt lives in a library missing from the decompile; capacity values are whole numbers so rounding is not observable.
public static class FloatClampExtensions
{
	public static int ClampToInt(this float value, int min, int max)
	{
		return Mathf.Clamp(Mathf.RoundToInt(value), min, max);
	}
}
