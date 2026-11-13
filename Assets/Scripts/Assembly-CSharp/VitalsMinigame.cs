using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class VitalsMinigame : Minigame
{
	public TextMeshPro BatteryText;

	public VitalsPanel PanelPrefab;

	public TextMeshPro SabText;

	public float XStart = -0.8f;

	public float YStart = 2.15f;

	public float XOffset = 1.95f;

	public float YOffset = -0.65f;

	private VitalsPanel[] vitals;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		DeadBody[] source = Object.FindObjectsOfType<DeadBody>();
		List<NetworkedPlayerInfo> list = new List<NetworkedPlayerInfo>();
		list.AddRange(GameData.Instance.AllPlayers);
		Dictionary<byte, bool> vitalDisconnected = new Dictionary<byte, bool>();
		foreach (NetworkedPlayerInfo player in list)
		{
			bool flag = !source.Any((DeadBody b) => b.ParentId == player.PlayerId);
			vitalDisconnected[player.PlayerId] = flag && (player.Disconnected || player.IsDead);
		}
		list.Sort((NetworkedPlayerInfo a, NetworkedPlayerInfo b) =>
		{
			if (vitalDisconnected[a.PlayerId] && !vitalDisconnected[b.PlayerId])
			{
				return 1;
			}
			return (!vitalDisconnected[a.PlayerId] && vitalDisconnected[b.PlayerId]) ? (-1) : 0;
		});
		vitals = new VitalsPanel[list.Count];
		for (int num = 0; num < list.Count; num++)
		{
			NetworkedPlayerInfo networkedPlayerInfo = list[num];
			int num2 = num % 3;
			int num3 = num / 3;
			VitalsPanel vitalsPanel = Object.Instantiate(PanelPrefab, transform);
			vitalsPanel.transform.localPosition = new Vector3(XStart + (float)num2 * XOffset, YStart + (float)num3 * YOffset, -1f);
			vitalsPanel.SetPlayer(num, networkedPlayerInfo);
			if (vitalDisconnected[networkedPlayerInfo.PlayerId])
			{
				vitalsPanel.SetDisconnected();
			}
			else if (networkedPlayerInfo.IsDead)
			{
				vitalsPanel.SetDead();
			}
			else
			{
				vitalsPanel.SetAlive();
			}
			vitals[num] = vitalsPanel;
		}
		SetupInput(disableCursor: true);
	}

	private void Update()
	{
		if (SabText.isActiveAndEnabled && !PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
		{
			SabText.gameObject.SetActive(value: false);
			for (int i = 0; i < vitals.Length; i++)
			{
				vitals[i].gameObject.SetActive(value: true);
			}
		}
		else if (!SabText.isActiveAndEnabled && PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
		{
			SabText.gameObject.SetActive(value: true);
			for (int j = 0; j < vitals.Length; j++)
			{
				vitals[j].gameObject.SetActive(value: false);
			}
		}
		for (int k = 0; k < vitals.Length; k++)
		{
			VitalsPanel vitalsPanel = vitals[k];
			if (!vitalsPanel.PlayerInfo.IsDead && vitalsPanel.PlayerInfo.Disconnected && !vitalsPanel.IsDiscon)
			{
				vitalsPanel.SetDisconnected();
			}
			else if (vitalsPanel.PlayerInfo.IsDead && !vitalsPanel.IsDead && !vitalsPanel.IsDiscon)
			{
				vitalsPanel.SetDead();
			}
		}
	}
}
