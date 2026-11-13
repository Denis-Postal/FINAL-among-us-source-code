using Cpp2ILInjected;
using UnityEngine;

public class GaugeRandomizer : MonoBehaviour
{
	public FloatRange Range;

	public SpriteRenderer Gauge;

	public float Frequency= 1f;

	public float Offset= 1f;

	private float naturalY;

	private float naturalSizeY;

	private Color goodLineColor= new Color(100f, 193f, 255f);

	public void Start()
	{
		naturalSizeY = Gauge.size.y;
		naturalY = transform.localPosition.y;
	}

	private unsafe void Update()
	{
		float num = Range.Lerp(Mathf.PerlinNoise(Offset, Time.time * Frequency) / 2f + 0.5f);
		Vector2 size = Gauge.size;
		size.y = num;
		Gauge.size = size;
		Vector3 localPosition = transform.localPosition;
		localPosition.y = naturalY - (naturalSizeY - num) / 2f;
		transform.localPosition = localPosition;
	}

	public GaugeRandomizer()
	{ }
}
