using System.Collections;
using UnityEngine;

public class Asteroid : PoolableBehavior
{
	public Sprite[] AsteroidImages;

	public Sprite[] BrokenImages;

	private int imgIdx;

	public FloatRange MoveSpeed = new FloatRange(2f, 5f);

	public FloatRange RotateSpeed = new FloatRange(-10f, 10f);

	public SpriteRenderer Explosion;

	public Vector3 TargetPosition { get; internal set; }

	public void FixedUpdate()
	{
		transform.localRotation = Quaternion.Euler(0f, 0f, transform.localRotation.eulerAngles.z + RotateSpeed.Last * Time.fixedDeltaTime);
		Vector3 vector = TargetPosition - transform.localPosition;
		if (vector.sqrMagnitude > 0.05f)
		{
			vector.Normalize();
			transform.localPosition += vector * MoveSpeed.Last * Time.fixedDeltaTime;
		}
		else
		{
			OwnerPool.Reclaim(this);
		}
	}

	public override void Reset()
	{
		enabled = true;
		Explosion.enabled = false;
		SpriteRenderer component = GetComponent<SpriteRenderer>();
		imgIdx = AsteroidImages.RandomIdx();
		component.sprite = AsteroidImages[imgIdx];
		component.enabled = true;
		ButtonBehavior component2 = GetComponent<ButtonBehavior>();
		component2.enabled = true;
		component2.OnClick.RemoveAllListeners();
		transform.Rotate(0f, 0f, RotateSpeed.Next());
		MoveSpeed.Next();
		base.Reset();
	}

	public IEnumerator CoBreakApart()
	{
		enabled = false;
		GetComponent<ButtonBehavior>().enabled = false;
		VibrationManager.Vibrate(0.5f, 0.5f, 0.35f, VibrationManager.VibrationFalloff.Linear);
		Explosion.enabled = true;
		yield return new WaitForLerp(0.1f, (float t) =>
		{
			Explosion.transform.localScale = new Vector3(t, t, t);
		});
		yield return new WaitForSeconds(0.05f);
		yield return new WaitForLerp(0.05f, (float t) =>
		{
			Explosion.transform.localScale = new Vector3(1f - t, 1f - t, 1f - t);
		});
		SpriteRenderer rend = GetComponent<SpriteRenderer>();
		yield return null;
		rend.sprite = BrokenImages[imgIdx];
		yield return new WaitForSeconds(0.2f);
		OwnerPool.Reclaim(this);
	}
}
