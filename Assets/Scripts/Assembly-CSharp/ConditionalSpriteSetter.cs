using Cpp2ILInjected;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ConditionalSpriteSetter : MonoBehaviour
{
	public bool ignoreIfNoSpriteForPlatform = true;

	public ConditionalSprite sprite;

	private void Start()
	{
		SpriteRenderer component = GetComponent<SpriteRenderer>();
		if ((bool)component && ((bool)sprite.Select() || !ignoreIfNoSpriteForPlatform))
		{
			component.sprite = sprite;
		}
	}
}
