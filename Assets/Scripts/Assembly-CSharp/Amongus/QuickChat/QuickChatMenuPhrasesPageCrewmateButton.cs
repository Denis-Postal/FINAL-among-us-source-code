using System.Collections;
using System.Linq;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatMenuPhrasesPageCrewmateButton : AbstractQuickChatMenuPhrasesPageButton
	{
		[Header("Crewmate Button")]
		[SerializeField]
		private PoolablePlayer player;

		[SerializeField]
		private SpriteRenderer playerMask;

		[SerializeField]
		private Material playerMaterial;

		[SerializeField]
		private SpriteRenderer[] playerMaterialRenderers;

		[SerializeField]
		private SpriteRenderer[] playerUnmaskedRenderers;

		[SerializeField]
		private SpriteRenderer[] playerColorBackgrounds;

		public override string ButtonType => "Crewmate";

		protected override void Awake()
		{
			Button.OnMouseOver.AddListener(OnButtonMouseOver);
			Button.OnMouseOut.AddListener(OnButtonMouseOut);
		}

		protected override float GetInitialWidth()
		{
			return Collider.size.x;
		}

		protected override void OnSetup()
		{
			Collider.size = new Vector2(Width, Height);
			Collider.offset = new Vector2(Width / 2f, (0f - Height) / 2f);
			TextRect.sizeDelta = Collider.size;
			Vector3 localPosition = TextRect.localPosition;
			localPosition.x = Collider.offset.x;
			localPosition.y = Collider.offset.y;
			TextRect.localPosition = localPosition;
			playerMask.material = new Material(playerMask.material);
			playerMask.material.SetInt(PlayerMaterial.MaskLayer, MaskLayer);
			NetworkedPlayerInfo data = PlayerControl.AllPlayerControls.Where((PlayerControl p) => p.PlayerId == Phrase.PlayerId).First().Data;
			player.UpdateFromEitherPlayerDataOrCache(data, PlayerOutfitType.Default, PlayerMaterial.MaskType.ComplexUI, includePet: false);
			player.SetHatAndVisorMaskType(PlayerMaterial.MaskType.None);
			StartCoroutine(CoSetPlayerRendererIndexes());
			playerColorBackgrounds.ForEach((SpriteRenderer x) =>
			{
				x.color = Phrase.ToPlayerBackgroundColor();
			});
		}

		private IEnumerator CoSetPlayerRendererIndexes()
		{
			playerMaterialRenderers.ForEach((SpriteRenderer x) =>
			{
				x.material.SetInt(PlayerMaterial.MaskLayer, MaskLayer);
			});
			playerUnmaskedRenderers.ForEach((SpriteRenderer x) =>
			{
				x.material.SetInt(PlayerMaterial.MaskLayer, 180);
			});
			yield return null;
			playerMaterialRenderers.ForEach((SpriteRenderer x) =>
			{
				x.material.SetInt(PlayerMaterial.MaskLayer, MaskLayer);
			});
			playerUnmaskedRenderers.ForEach((SpriteRenderer x) =>
			{
				x.material.SetInt(PlayerMaterial.MaskLayer, 180);
			});
		}

		private void OnButtonMouseOver()
		{
			TextRect.localScale = Vector3.one * 0.9f;
		}

		private void OnButtonMouseOut()
		{
			TextRect.localScale = Vector3.one;
		}
	}
}
