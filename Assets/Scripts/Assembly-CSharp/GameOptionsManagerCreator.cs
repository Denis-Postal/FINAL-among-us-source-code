using Object = UnityEngine.Object;
using System;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using InnerNet;
using UnityEngine;

public class GameOptionsManagerCreator : MonoBehaviour
{
	private void Awake()
	{
		// The constructor owns the singleton and creates its factory, as in decomp9.
		new GameOptionsManager(new UnityLogger());
	}

	private void Start()
	{
		Object.Destroy(gameObject);
	}
}
