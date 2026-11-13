using UnityEngine;

public class ModManager : DestroyableSingleton<ModManager>
{
	public SpriteRenderer ModStamp;

	public Camera localCamera;

	public void ShowModStamp()
	{
		ModStamp.enabled = true;
	}

	private unsafe void LateUpdate()
	{
		if (!ModStamp.enabled)
		{
			return;
		}
		if (!localCamera)
		{
			if (DestroyableSingleton<HudManager>.InstanceExists)
			{
				localCamera = DestroyableSingleton<HudManager>.Instance.GetComponentInChildren<Camera>();
			}
			else
			{
				localCamera = Camera.main;
			}
		}
		ModStamp.transform.position = AspectPosition.ComputeWorldPosition(localCamera, AspectPosition.EdgeAlignments.RightTop, new Vector3(0.6f, 0.6f, localCamera.nearClipPlane + 0.1f));
	}
}
