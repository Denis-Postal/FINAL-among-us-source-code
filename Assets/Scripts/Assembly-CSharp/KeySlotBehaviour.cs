using UnityEngine;

public class KeySlotBehaviour : MonoBehaviour
{
	public Sprite Highlit;

	public Sprite Inserted;

	public Sprite Finished;

	public SpriteRenderer Image;

	public BoxCollider2D Hitbox;

	internal unsafe void SetFinished()
	{
		Image.sprite = Finished;
		transform.localEulerAngles = new Vector3(0f, 0f, 90f);
	}

	internal void SetInserted()
	{
		Image.sprite = Inserted;
	}

	internal void SetHighlight()
	{
		Image.sprite = Highlit;
	}
}
