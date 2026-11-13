using System.Runtime.CompilerServices;
using UnityEngine;

public static class CameraSafeArea
{
	[MethodImpl((MethodImplOptions)256)]
	public static float GetSafeOrthographicSize(Camera cam)
	{
		return cam.orthographicSize;
	}
}
