using UnityEngine;

public class MapSelectButton : MonoBehaviour
{
	public PassiveButton Button;

	[SerializeField]
	private SpriteRenderer[] MapIcon;

	[SerializeField]
	private SpriteRenderer[] allSprites;

	[SerializeField]
	private BoxCollider2D boxCollider;

	public int MapID = -1;

	public void SetImage(Sprite mapSprite, int maskLayer)
	{
		SpriteRenderer[] mapIcon = MapIcon;
		for (int i = 0; i < mapIcon.Length; i++)
		{
			mapIcon[i].sprite = mapSprite;
		}
		mapIcon = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < mapIcon.Length; i++)
		{
			mapIcon[i].material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		}
	}

	public MapSelectButton()
	{
		//IL_000f: Expected I4, but got I8
	}
}
