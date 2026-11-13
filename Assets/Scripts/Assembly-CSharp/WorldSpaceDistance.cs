using Cpp2ILInjected;
using UnityEngine;

public class WorldSpaceDistance : MonoBehaviour
{
	public GameObject TargetObj;

	public float Distance;

	public float DistanceSq;

	private void Update()
	{
		if (!(TargetObj == null))
		{
			Distance = Vector2.Distance(transform.position, TargetObj.transform.position);
			DistanceSq = Distance * Distance;
		}
	}
}
