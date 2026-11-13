using System;
using UnityEngine;

public class CreateStoreButton : MonoBehaviour
{
	public GameObject ConnectIcon;

	public void Click()
	{
		DestroyableSingleton<TransitionFade>.Instance.DoTransitionFade(null, DestroyableSingleton<StoreMenu>.Instance.gameObject, DestroyableSingleton<StoreMenu>.Instance.Open);
	}
}
