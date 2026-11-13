using Cpp2ILInjected;
using UnityEngine;

public class MeshAnimator : MonoBehaviour
{
	private MeshFilter filter;

	public Mesh[] Frames;

	public float frameRate;

	private float timer;

	private int frameId;

	private Vector3 originalPosition;

	private Vector3 originalScale;

	public MeshSequence sequence;

	private void Awake()
	{
		originalPosition = transform.localPosition;
		originalScale = transform.localScale;
	}

	private void Start()
	{
		filter = GetComponent<MeshFilter>();
	}

	private unsafe void Update()
	{
		timer += Time.deltaTime;
		if (!(timer > 1f / frameRate))
		{
			return;
		}
		timer = 0f;
		frameId++;
		if (sequence == null)
		{
			if (frameId >= Frames.Length)
			{
				frameId = 0;
			}
			filter.mesh = Frames[frameId];
			return;
		}
		if (frameId >= sequence.Frames.Length)
		{
			frameId = 0;
		}
		float num = 0.01f;
		Vector3 vector = sequence.Positions[frameId];
		float x = vector.x * num;
		float y = vector.y * num;
		Vector3 vector2 = new Vector3(x, y, 0f);
		filter.mesh = sequence.Frames[frameId];
		transform.localPosition = vector2 + originalPosition;
		transform.localScale = sequence.Scales[frameId].Mul(originalScale);
	}
}
