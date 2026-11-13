using System;
using UnityEngine;

public class ConditionalMove : MonoBehaviour
{
	[Serializable]
	public struct MoveForPlatforms
	{
		public RuntimePlatform runtimePlatform;

		public Vector3 offset;
	}

	public MoveForPlatforms[] moveForPlatforms = new MoveForPlatforms[0];

	private unsafe void Awake()
	{
		MoveForPlatforms[] array = this.moveForPlatforms;
		for (int i = 0; i < array.Length; i++)
		{
			MoveForPlatforms moveForPlatforms = array[i];
			if (Application.platform == moveForPlatforms.runtimePlatform)
			{
				transform.position += moveForPlatforms.offset;
			}
		}
	}
}
