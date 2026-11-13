using Object = UnityEngine.Object;
using UnityEngine;

public class LoadingSpinner : MonoBehaviour
{
	public float speed = 20f;

	public Transform toRotate;

	public SpriteRenderer loadingSpinner;

	public SpriteRenderer background;

	public void Toggle(bool on)
	{
		gameObject.SetActive(on);
	}

	public void SetMaskLayer(int layer)
	{
		loadingSpinner.material.SetInt(PlayerMaterial.MaskLayer, layer);
		background.material.SetInt(PlayerMaterial.MaskLayer, layer);
	}

	private void Start()
	{
		PlayerMaterial.SetColors(2, loadingSpinner);
	}

	private unsafe void Update()
	{
		toRotate.RotateAround(toRotate.position, Vector3.forward, speed * Time.deltaTime);
	}
}
