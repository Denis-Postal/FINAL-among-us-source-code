using Object = UnityEngine.Object;
using UnityEngine;

public class GarbageBehaviour : MonoBehaviour
{
	public void FixedUpdate()
	{
		if (transform.localPosition.y < -3.49f)
		{
			Object.Destroy(gameObject);
		}
	}
}
