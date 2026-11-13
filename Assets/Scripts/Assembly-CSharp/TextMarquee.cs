using System.Collections;
using TMPro;
using UnityEngine;

public class TextMarquee : MonoBehaviour
{
	public TextMeshPro Target;

	private MeshRenderer[] allRenderers;

	private int lastChildCount;

	private string targetText;

	public float ScrollSpeed = 1f;

	public float PauseTime = 1f;

	public float AreaWidth = 3f;

	public bool IgnoreTextChanges;

	public void Start()
	{
		StartCoroutine(Run());
	}

	private void UpdateRendererList()
	{
		lastChildCount = Target.transform.childCount;
		allRenderers = Target.gameObject.GetComponentsInChildren<MeshRenderer>();
		allRenderers.ForEach((MeshRenderer render) =>
		{
			render.material.SetInt("_Mask", 4);
		});
	}

	private void Update()
	{
		if (Target.transform.childCount != lastChildCount)
		{
			UpdateRendererList();
		}
	}

	private IEnumerator Run()
	{
		yield return null;
		UpdateRendererList();
		int i = 0;
		while (i < 1000)
		{
			Vector4 temp = default;
			targetText = Target.text;
			allRenderers.ForEach((MeshRenderer render) =>
			{
				render.material.SetFloat("_VertexOffsetX", temp.x);
				render.material.SetFloat("_VertexOffsetY", temp.y);
			});
			for (float timer = 0f; timer < PauseTime; timer += Time.deltaTime)
			{
				if (!IgnoreTextChanges && targetText != Target.text)
				{
					break;
				}
				yield return null;
			}
			for (float timer = 0f; timer < 100f; timer += Time.deltaTime)
			{
				if (!IgnoreTextChanges && targetText != Target.text)
				{
					break;
				}
				temp.x -= ScrollSpeed * Time.deltaTime;
				allRenderers.ForEach((MeshRenderer render) =>
				{
					render.material.SetFloat("_VertexOffsetX", temp.x);
					render.material.SetFloat("_VertexOffsetY", temp.y);
				});
				if (Target.bounds.size.x + temp.x < AreaWidth)
				{
					break;
				}
				yield return null;
			}
			for (float timer = 0f; timer < PauseTime; timer += Time.deltaTime)
			{
				if (!IgnoreTextChanges && targetText != Target.text)
				{
					break;
				}
				yield return null;
			}
			yield return null;
			int num = i + 1;
			i = num;
		}
	}
}
