using System;
using System.Collections.Generic;
using Assets.InnerNet;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class NewRequestBanner : MonoBehaviour
{
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public int totalCount;

		public NewRequestBanner _003C_003E4__this;

		internal unsafe void _003CUpdate_003Eb__0(TextMeshPro t)
		{
			// TODO(verify): no-op, original logic unrecoverable from decomp
		}

		internal unsafe void _003CUpdate_003Eb__1(TextMeshPro t)
		{
			// TODO(verify): no-op, original logic unrecoverable from decomp
		}
	}

	[SerializeField]
	private GameObject activeBanner;

	[SerializeField]
	private GameObject inactiveBanner;

	[SerializeField]
	private SpriteRenderer parentRenderer;

	[SerializeField]
	private SpriteRenderer ourSprite;

	[SerializeField]
	private TextMeshPro[] notifText;

	private unsafe void Update()
	{
		int totalCount = DestroyableSingleton<FriendsListManager>.Instance.ReceivedLobbyInvites.Count + DestroyableSingleton<FriendsListManager>.Instance.ReceivedRequests.Count;
		activeBanner.SetActive(totalCount > 0);
		inactiveBanner.SetActive(totalCount > 0);
		notifText.ForEach((TextMeshPro t) =>
		{
			t.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NewRequests, totalCount);
		});
		transform.position = new Vector3(parentRenderer.transform.position.x - ourSprite.size.x / 2f * ourSprite.transform.localScale.x - parentRenderer.size.x * 0.4f * parentRenderer.transform.localScale.x, transform.position.y, transform.position.z);
		notifText.ForEach((TextMeshPro t) =>
		{
			t.transform.position = new Vector3(ourSprite.transform.position.x, t.transform.position.y, t.transform.position.z);
		});
	}
}
