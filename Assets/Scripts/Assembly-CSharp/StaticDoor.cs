using UnityEngine;

public class StaticDoor : MonoBehaviour
{
	public Sprite OpenDoorImage;

	public Sprite CloseDoorImage;

	public bool IsOpen { get; private set; }

	public void SetOpen(bool isOpen)
	{
		IsOpen = isOpen;
		Collider2D component = GetComponent<Collider2D>();
		SpriteRenderer component2 = GetComponent<SpriteRenderer>();
		EdgeCollider2D componentInChildren = GetComponentInChildren<EdgeCollider2D>();
		if (isOpen)
		{
			component.enabled = false;
			component2.sprite = OpenDoorImage;
			if ((bool)componentInChildren)
			{
				componentInChildren.enabled = false;
			}
		}
		else
		{
			component.enabled = true;
			component2.sprite = CloseDoorImage;
			if ((bool)componentInChildren)
			{
				componentInChildren.enabled = true;
			}
		}
	}
}
