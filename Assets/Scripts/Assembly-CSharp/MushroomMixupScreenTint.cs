using System.Collections;
using UnityEngine;

public class MushroomMixupScreenTint : MonoBehaviour
{
	private static readonly int Opacity = Shader.PropertyToID("_Opacity");

	[SerializeField]
	private MeshRenderer meshRenderer;

	[SerializeField]
	private float maxOpacity;

	private void Awake()
	{
		meshRenderer.material = new Material(meshRenderer.sharedMaterial);
	}

	public void Activate()
	{
		if (transform.parent != DestroyableSingleton<HudManager>.Instance.transform)
		{
			transform.SetParent(DestroyableSingleton<HudManager>.Instance.transform, worldPositionStays: false);
			transform.localPosition = new Vector3(0f, 0f, 10f);
		}
		meshRenderer.enabled = true;
		StartCoroutine(CoAnimateOpacity(0f, maxOpacity));
	}

	public void Deactivate()
	{
		StartCoroutine(CoAnimateOpacity(maxOpacity, 0f));
	}

	private IEnumerator CoAnimateOpacity(float startValue, float finalValue)
	{
		float currentValue = startValue;
		if (startValue < finalValue)
		{
			while (currentValue < finalValue)
			{
				meshRenderer.material.SetFloat(Opacity, currentValue);
				currentValue += Time.deltaTime;
				yield return null;
			}
			yield break;
		}
		while (currentValue > finalValue)
		{
			meshRenderer.material.SetFloat(Opacity, currentValue);
			currentValue -= Time.deltaTime;
			yield return null;
		}
		meshRenderer.enabled = false;
	}
}
