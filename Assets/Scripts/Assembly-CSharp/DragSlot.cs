using System;
using Cpp2ILInjected;
using UnityEngine;

public class DragSlot : MonoBehaviour
{
	public Vector3 Offset;

	public Behaviour Occupant;

	public unsafe Vector3 TargetPosition => transform.position + Offset;
}
