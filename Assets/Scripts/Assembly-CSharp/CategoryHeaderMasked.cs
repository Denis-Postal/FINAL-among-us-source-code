using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class CategoryHeaderMasked : MonoBehaviour
{
	[SerializeField]
	protected TextMeshPro Title;

	[SerializeField]
	protected SpriteRenderer Background;

	[SerializeField]
	protected SpriteRenderer Divider;

	public virtual void SetHeader(StringNames name, int maskLayer)
	{
		Title.text = DestroyableSingleton<TranslationController>.Instance.GetString(name);
		Background.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		if (Divider != null)
		{
			Divider.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		}
		Title.fontMaterial.SetFloat("_StencilComp", 3f);
		Title.fontMaterial.SetFloat("_Stencil", maskLayer);
	}
}
