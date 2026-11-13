using System.Collections;
using UnityEngine;

public class SpinAnimator : MonoBehaviour
{
	private enum States
	{
		Visible,
		Invisible,
		Spinning,
		Pulsing
	}

	public float Speed = 60f;

	public GameObject inputGlyph;

	private States curState;

	private void Update()
	{
		if (curState == States.Spinning)
		{
			transform.Rotate(0f, 0f, Speed * Time.deltaTime);
		}
	}

	public void Appear()
	{
		if (curState == States.Invisible)
		{
			curState = States.Visible;
			gameObject.SetActive(value: true);
			inputGlyph.SetActive(value: true);
			StopAllCoroutines();
			StartCoroutine(Effects.ScaleIn(transform, 0f, 1f, 0.125f));
		}
	}

	public void Disappear()
	{
		if (curState != States.Invisible)
		{
			curState = States.Invisible;
			StopAllCoroutines();
			StartCoroutine(CoDisappear());
		}
	}

	private IEnumerator CoDisappear()
	{
		yield return Effects.ScaleIn(transform, 1f, 0f, 0.125f);
		gameObject.SetActive(value: false);
		inputGlyph.SetActive(value: false);
	}

	public void StartPulse()
	{
		if (curState != States.Pulsing)
		{
			curState = States.Pulsing;
			SpriteRenderer component = GetComponent<SpriteRenderer>();
			StartCoroutine(Effects.CycleColors(component, Color.white, Color.green, 1f, float.MaxValue));
		}
	}

	internal void Play()
	{
		curState = States.Spinning;
		inputGlyph.SetActive(value: false);
	}
}
