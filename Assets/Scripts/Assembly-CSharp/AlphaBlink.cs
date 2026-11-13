using System;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class AlphaBlink : MonoBehaviour
{
	public float Period = 1f;

	public float Ratio = 0.5f;

	private SpriteRenderer rend;

	private MeshRenderer mesh;

	private TextMeshPro tmPro;

	public FloatRange AlphaRange= new FloatRange(0.2f, 0.5f);

	public Color baseColor= Color.white;

	public void SetColor(Color c)
	{ /* spliced-from-2019 */
		Start();
		baseColor = c;
		Update();
	}

	private void Start()
	{
		mesh = GetComponent<MeshRenderer>();
		rend = GetComponent<SpriteRenderer>();
		tmPro = GetComponent<TextMeshPro>();
		if ((bool)tmPro)
		{
			baseColor = tmPro.color;
		}
	}

	public unsafe void Update()
	{
		float num = Time.time % Period / Period;
		num = ((num < Ratio) ? 1 : 0);
		if ((bool)rend)
		{
			rend.color = new Color(baseColor.r, baseColor.g, baseColor.b, AlphaRange.Lerp(num));
		}
		if ((bool)tmPro)
		{
			tmPro.color = new Color(baseColor.r, baseColor.g, baseColor.b, AlphaRange.Lerp(num));
		}
		else if ((bool)mesh)
		{
			mesh.material.SetColor("_Color", new Color(baseColor.r, baseColor.g, baseColor.b, AlphaRange.Lerp(num)));
		}
	}

	public AlphaBlink()
	{ }
}
