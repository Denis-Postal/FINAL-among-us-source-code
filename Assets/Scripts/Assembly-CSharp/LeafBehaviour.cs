using Cpp2ILInjected;
using UnityEngine;

public class LeafBehaviour : MonoBehaviour
{
	public Sprite[] Images;

	public FloatRange SpinSpeed= new FloatRange(-45f, 45f);

	public Vector2Range StartVel;

	public float AccelRate= 30f;

	[HideInInspector]
	public LeafMinigame Parent;

	public bool Held;

	private static RandomFill<Sprite> ImageFiller= new RandomFill<Sprite>();

	[HideInInspector]
	public Rigidbody2D body;

	public unsafe void Start()
	{
		ImageFiller.Set(Images);
		Sprite sprite = ImageFiller.Get();
		if (!sprite)
		{
			ImageFiller = new RandomFill<Sprite>();
			ImageFiller.Set(Images);
			sprite = ImageFiller.Get();
		}
		GetComponent<SpriteRenderer>().sprite = sprite;
		body = GetComponent<Rigidbody2D>();
		body.angularVelocity = SpinSpeed.Next();
		body.linearVelocity = StartVel.Next();
	}

	public void FixedUpdate()
	{
		if (!Held && (double)transform.localPosition.x < -2.5)
		{
			Parent.LeafDone(this);
		}
	}

	public LeafBehaviour()
	{ }

	static LeafBehaviour()
	{ }
}
