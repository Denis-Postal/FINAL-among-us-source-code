using System;
using UnityEngine;

public class FreeplayPopoverButton : MonoBehaviour
{
	[SerializeField]
	public MapNames map;

	[SerializeField]
	private BoxCollider2D collider;

	[SerializeField]
	public PassiveButton button; // made public: FreeplayPopover.cs (work copy) reads it directly

	public MapNames Map => map;

	public UiElement Button => button;

	public Vector2 Size => collider.size * transform.localScale;

	public event Action<FreeplayPopoverButton> OnPressEvent;

	private void Awake()
	{
		button.AddOnClickListeners(OnPressed);
		if (Map == MapNames.Skeld && AprilFoolsMode.ShouldFlipSkeld())
		{
			button.GetComponent<SpriteRenderer>().flipX = true;
		}
	}

	private void OnPressed()
	{
		OnPressEvent?.Invoke(this);
	}
}
