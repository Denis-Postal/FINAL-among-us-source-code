using Object = UnityEngine.Object;
using UnityEngine;

public class BuildDefineConditionalObject : MonoBehaviour
{
	public bool isDefined;

	private void Awake()
	{
		gameObject.SetActive(isDefined);
	}

	private void OnEnable()
	{
		if (!isDefined)
		{
			gameObject.SetActive(isDefined);
		}
	}
}
