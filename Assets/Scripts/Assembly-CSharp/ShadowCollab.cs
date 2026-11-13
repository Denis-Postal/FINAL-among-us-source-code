using UnityEngine;

public class ShadowCollab : MonoBehaviour
{
	public Camera ShadowCamera;

	public MeshRenderer ShadowQuad;

	private Camera mainCamera;

	public void OnEnable()
	{
		SyncToMainCamera();
	}

	private void LateUpdate()
	{
		SyncToMainCamera();
	}

	private void OnPreCull()
	{
		SyncToMainCamera();
	}

	private void SyncToMainCamera()
	{
		mainCamera = Camera.main;
		if (mainCamera != null && ShadowCamera != null && ShadowQuad != null)
		{
			SyncShadowCamera();
		}
	}

	private void SyncShadowCamera()
	{
		ShadowCamera.transform.SetPositionAndRotation(mainCamera.transform.position, mainCamera.transform.rotation);
		ShadowCamera.orthographic = mainCamera.orthographic;
		ShadowCamera.aspect = mainCamera.aspect;
		ShadowCamera.nearClipPlane = mainCamera.nearClipPlane;
		ShadowCamera.farClipPlane = mainCamera.farClipPlane;
		if (mainCamera.orthographic)
		{
			ShadowCamera.orthographicSize = mainCamera.orthographicSize;
		}
		else
		{
			ShadowCamera.fieldOfView = mainCamera.fieldOfView;
		}
		ShadowQuad.transform.localScale = new Vector3(mainCamera.orthographicSize * mainCamera.aspect, mainCamera.orthographicSize) * 2f;
	}
}
