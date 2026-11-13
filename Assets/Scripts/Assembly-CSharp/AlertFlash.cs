using Cpp2ILInjected;
using UnityEngine;

public class AlertFlash : MonoBehaviour
{
	private const string FLASH_TRIGGER = "OnFlash";

	private const string OVERLAY_BOOL = "Overlay";

	[SerializeField]
	private float opacity = 1f;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private MeshRenderer screenSpaceRenderer;

	private void Awake()
	{
		// TODO(verify): from decomp9
		screenSpaceRenderer.material = new Material(screenSpaceRenderer.sharedMaterial);
	}

	private void LateUpdate()
	{
		// TODO(verify): from decomp9 (enabled while opacity > 0.01)
		bool flag = opacity > 0.01f;
		if (screenSpaceRenderer.enabled != flag)
		{
			screenSpaceRenderer.enabled = flag;
		}
		if (flag)
		{
			screenSpaceRenderer.material.SetFloat("_Opacity", opacity);
		}
	}

	public void Flash()
	{
		animator.SetTrigger("OnFlash");
	}

	public void SetOverlay(bool enabled)
	{
		animator.SetBool("Overlay", enabled);
	}
}
