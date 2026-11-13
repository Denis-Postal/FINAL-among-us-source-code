using UnityEngine;

public class MedScannerBehaviour : MonoBehaviour
{
	public Vector3 Offset;

	public Vector3 Position => transform.position + Offset;
}
