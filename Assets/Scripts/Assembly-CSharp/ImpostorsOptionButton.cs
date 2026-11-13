using Object = UnityEngine.Object;
using TMPro;
using UnityEngine;

public class ImpostorsOptionButton : MonoBehaviour
{
	public SpriteRenderer SpriteRenderer { get; private set; }

	public TextMeshPro TextMesh { get; private set; }

	public BoxCollider2D BoxCollider { get; private set; }

	public PassiveButton PassiveButton { get; private set; }

	public ButtonRolloverHandler ButtonRollover { get; private set; }

	private void Awake()
	{
		SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		TextMesh = gameObject.GetComponentInChildren<TextMeshPro>();
		BoxCollider = gameObject.GetComponent<BoxCollider2D>();
		PassiveButton = gameObject.GetComponent<PassiveButton>();
		ButtonRollover = gameObject.GetComponent<ButtonRolloverHandler>();
	}

	public unsafe void SetOptionEnabled(bool b)
	{
		TextMesh.color = (b ? Color.white : Palette.DisabledGrey);
		SpriteRenderer.color = (b ? Color.white : Palette.DisabledGrey);
		BoxCollider.enabled = b;
		PassiveButton.enabled = b;
		ButtonRollover.enabled = b;
	}
}
