using System;
using Cpp2ILInjected;
using UnityEngine;

public class AspectSize : MonoBehaviour
{
	public Sprite Background;

	public SpriteRenderer Renderer;

	private Vector2 lastScreenSize= Vector2.zero;

	public float PercentWidth= 0.95f;

	private Vector3? originalScale;

	private void OnValidate()
	{
		if (transform.localScale != Vector3.one)
		{
			Debug.LogWarning("Warning! AspectSize should be placed on an object with a local scale of (1,1). Otherwise it will not work correctly. Gameobject name: " + name);
		}
	}

	public static float CalculateSize(Vector3 parentPos, Sprite sprite, float percentWidth)
	{
		Camera main = Camera.main;
		float num = CameraSafeArea.GetSafeOrthographicSize(main) * main.aspect + parentPos.x;
		float x = sprite.bounds.size.x;
		float b = num / x * percentWidth;
		return Mathf.Min(1f, b);
	}

	private void Update()
	{
		if (lastScreenSize.x != (float)Screen.width || lastScreenSize.y != (float)Screen.height)
		{
			lastScreenSize = new Vector2(Screen.width, Screen.height);
			DoSetUp();
		}
	}

	private unsafe void DoSetUp()
	{
		if (!originalScale.HasValue)
		{
			originalScale = transform.localScale;
		}
		Camera main = Camera.main;
		AdjustScale(main.aspect);
		ResolutionManager.ResolutionChanged += OnResolutionChanged;
	}

	private void OnEnable()
	{
		DoSetUp();
	}

	private void OnDisable()
	{
		ResolutionManager.ResolutionChanged -= OnResolutionChanged;
	}

	private void OnDestroy()
	{
		ResolutionManager.ResolutionChanged -= OnResolutionChanged;
	}

	private void OnResolutionChanged(float aspectRatio, int width, int height, bool fullscreen)
	{
		AdjustScale(aspectRatio);
	}

	private unsafe void AdjustScale(float aspect)
	{
		if (this == null)
		{
			ResolutionManager.ResolutionChanged -= OnResolutionChanged;
			return;
		}
		transform.localScale = originalScale.Value;
		Camera main = Camera.main;
		float scale = GetScale(main.orthographicSize, main.aspect);
		if (scale < 1f)
		{
			transform.localScale = new Vector3(scale, scale, scale);
		}
	}

	private unsafe float GetScale(float orthographicSize, float aspect)
	{
		float num = orthographicSize * aspect;
		Vector3 vector;
		if ((bool)Background)
		{
			vector = Background.bounds.size;
		}
		else
		{
			vector = ((Renderer.drawMode == SpriteDrawMode.Simple) ? Renderer.sprite.bounds.size : ((Vector3)Renderer.size));
			vector = Renderer.transform.TransformVector(vector);
		}
		float num2 = vector.x / 2f;
		return num / num2 * PercentWidth;
	}

	public AspectSize()
	{ }
}
