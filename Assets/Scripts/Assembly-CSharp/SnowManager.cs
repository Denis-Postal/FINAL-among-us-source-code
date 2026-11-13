using System.Collections;
using UnityEngine;

public class SnowManager : MonoBehaviour
{
	public ParticleSystem particles;

	private ParticleSystemRenderer rend;

	private float timer;

	private void Start()
	{
		rend = GetComponent<ParticleSystemRenderer>();
		StartCoroutine(Run());
	}

	private IEnumerator Run()
	{
		ContactFilter2D filter = new ContactFilter2D
		{
			layerMask = Constants.ShipOnlyMask,
			useLayerMask = true,
			useTriggers = true
		};
		Collider2D[] buffer = new Collider2D[10];
		WaitForSeconds wait = new WaitForSeconds(0.1f);
		while (true)
		{
			yield return wait;
			if (!PlayerControl.LocalPlayer)
			{
				continue;
			}
			bool flag = false;
			int num = PlayerControl.LocalPlayer.Collider.Overlap(filter, buffer);
			for (int i = 0; i < num; i++)
			{
				if (buffer[i].tag == "NoSnow")
				{
					flag = true;
				}
			}
			if (!particles.isPlaying)
			{
				if (!flag)
				{
					particles.Play();
				}
			}
			else if (flag)
			{
				timer = Mathf.Max(0f, timer - 0.2f);
			}
			else
			{
				timer = Mathf.Min(1f, timer + 0.2f);
			}
			SetPartAlpha();
		}
	}

	private void SetPartAlpha()
	{
		Color value = new Color(1f, 1f, 1f, timer);
		rend.material.SetColor("_Color", value);
	}
}
