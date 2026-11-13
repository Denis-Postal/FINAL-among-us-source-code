using System.Collections;
using System.Linq;
using PowerTools;
using UnityEngine;

public class DripBehaviour : MonoBehaviour
{
	public Vector2Range SpawnArea;

	public FloatRange FirstWait = new FloatRange(0f, 3f);

	public FloatRange Frequency = new FloatRange(0.75f, 3f);

	public SpriteAnim myAnim;

	public Collider2D[] IgnoreAreas;

	public bool FixDepth = true;

	public void Start()
	{
		StartCoroutine(Run());
	}

	private IEnumerator Run()
	{
		yield return Effects.Wait(FirstWait.Next());
		while (true)
		{
			Vector3 pos = SpawnArea.Next();
			transform.localPosition = pos;
			if (FixDepth)
			{
				pos = transform.position;
				pos.z = pos.y / 1000f;
				transform.position = pos;
			}
			if (!IgnoreAreas.Any((Collider2D i) => i.OverlapPoint(pos)))
			{
				myAnim.Play();
				while (myAnim.IsPlaying())
				{
					yield return null;
				}
				yield return Effects.Wait(Frequency.Next());
			}
		}
	}
}
