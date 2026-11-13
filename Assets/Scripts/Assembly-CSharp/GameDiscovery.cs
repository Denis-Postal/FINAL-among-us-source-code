using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using Cpp2ILInjected;
using Hazel.Udp;
using InnerNet;
using UnityEngine;

public class GameDiscovery : MonoBehaviour
{
	public JoinGameButton ButtonPrefab;

	public Transform ItemLocation;

	public float YStart = 0.56f;

	public float YOffset = -0.75f;

	public Scroller TargetArea;

	private Dictionary<string, JoinGameButton> received= new Dictionary<string, JoinGameButton>();

	public void Start()
	{
		InnerDiscover component = GetComponent<InnerDiscover>();
		component.OnPacketGet += Receive;
		component.StartAsClient();
	}

	public unsafe void Update()
	{
		float time = Time.time;
		string[] array = received.Keys.ToArray();
		Vector3 localPosition = new Vector3(0f, YStart, -1f);
		foreach (string key in array)
		{
			JoinGameButton joinGameButton = received[key];
			if (time - joinGameButton.timeRecieved > 3f)
			{
				received.Remove(key);
				Object.Destroy(joinGameButton.gameObject);
			}
			else
			{
				joinGameButton.transform.localPosition = localPosition;
				localPosition.y += YOffset;
			}
		}
		TargetArea.SetBoundsMax(Mathf.Max(0f, 0f - localPosition.y - 2f * YStart), 0f);
	}

	private void Receive(BroadcastPacket packet)
	{
		string[] array = packet.Data.Split('~');
		string address = packet.GetAddress();
		if (received.TryGetValue(address, out var value))
		{
			value.timeRecieved = Time.time;
			value.SetGameName(array);
		}
		else if (array[1].Equals("Open"))
		{
			CreateButtonForAddess(address, array);
		}
	}

	private unsafe void CreateButtonForAddess(string fromAddress, string[] gameNameParts)
	{
		JoinGameButton value = null;
		bool flag = false;
		if (!received.TryGetValue(fromAddress, out value))
		{
			value = Object.Instantiate(ButtonPrefab, ItemLocation);
			flag = true;
			Debug.Log("GameDiscovery.CreateButtonForAddess: Instantiate(" + fromAddress + ")");
		}
		if (flag)
		{
			value.transform.localPosition = new Vector3(0f, YStart + (float)(ItemLocation.childCount - 1) * YOffset, -1f);
			value.netAddress = fromAddress;
			value.GetComponentInChildren<MeshRenderer>().material.SetInt("_Mask", 4);
		}
		value.timeRecieved = Time.time;
		value.SetGameName(gameNameParts);
		if (flag)
		{
			ControllerManager.Instance.AddSelectableUiElement(value.GetComponent<PassiveButton>());
		}
		received[fromAddress] = value;
	}

	public GameDiscovery()
	{ }
}
