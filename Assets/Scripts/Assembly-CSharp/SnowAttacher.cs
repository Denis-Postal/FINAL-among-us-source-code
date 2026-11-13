using Object = UnityEngine.Object;
using UnityEngine;

public class SnowAttacher : MonoBehaviour
{
	public GameObject SnowPrefab;

	public unsafe void Start()
	{
		Object.Instantiate(SnowPrefab, DestroyableSingleton<HudManager>.Instance.transform).transform.localPosition = new Vector3(0f, 3f, 0f);
	}
}
