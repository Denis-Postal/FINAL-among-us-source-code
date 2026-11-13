using System.Collections;
using AmongUs.GameOptions;
using Hazel;
using UnityEngine;

public class LogicPingsHnS : GameLogicComponent
{
	private LogicOptionsHnS options;

	private LogicGameFlowHnS flow;

	private ObjectPoolBehavior pingPool;

	private Coroutine seekerPingCoroutine;

	public LogicPingsHnS(GameManager manager, ObjectPoolBehavior pool)
		: base(manager)
	{
		options = manager.LogicOptions as LogicOptionsHnS;
		flow = manager.LogicFlow as LogicGameFlowHnS;
		pingPool = pool;
	}

	public override void OnGameStart()
	{
	}

	public override void OnGameEnd()
	{
		DestroyPingCoroutine();
	}

	private void DestroyPingCoroutine()
	{
		if (seekerPingCoroutine != null)
		{
			Manager.StopCoroutine(seekerPingCoroutine);
			seekerPingCoroutine = null;
		}
		SetPingsInactive();
	}

	private void SetPingsInactive()
	{
		foreach (ArrowBehaviour activeChild in pingPool.activeChildren)
		{
			if (!(activeChild == null))
			{
				activeChild.target = Vector3.zero;
				activeChild.gameObject.SetActive(value: false);
			}
		}
	}

	public override void FixedUpdate()
	{
		if (Manager.GameHasStarted && flow.IsFinalCountdown && seekerPingCoroutine == null && options.GetSeekerPings())
		{
			seekerPingCoroutine = Manager.StartCoroutine(SeekerPing());
		}
	}

	private IEnumerator SeekerPing()
	{
		while (Manager.GameHasStarted)
		{
			for (int i = 0; i < PlayerControl.AllPlayerControls.Count; i++)
			{
				if (PlayerControl.AllPlayerControls[i].Data.Role.TeamType == RoleTeamTypes.Crewmate && !PlayerControl.AllPlayerControls[i].Data.IsDead && (PlayerControl.LocalPlayer.Data.RoleType == RoleTypes.Impostor || PlayerControl.AllPlayerControls[i] == PlayerControl.LocalPlayer))
				{
					PingBehaviour pingBehaviour = pingPool.Get<PingBehaviour>();
					pingBehaviour.target = PlayerControl.AllPlayerControls[i].GetTruePosition();
					pingBehaviour.AmSeeker = PlayerControl.LocalPlayer.Data.RoleType == RoleTypes.Impostor;
					pingBehaviour.UpdatePosition();
					pingBehaviour.gameObject.SetActive(value: true);
					pingBehaviour.SetImageEnabled(isOn: true);
				}
			}
			yield return new WaitForSeconds(options.GetShowPingTime());
			foreach (ArrowBehaviour activeChild in pingPool.activeChildren)
			{
				activeChild.target = Vector3.zero;
				activeChild.SetImageEnabled(isOn: false);
				activeChild.gameObject.SetActive(value: false);
			}
			float maxPingTime = options.GetMaxPingTime();
			yield return new WaitForSeconds(maxPingTime);
		}
	}

	public override void OnDestroy()
	{
		DestroyPingCoroutine();
	}

	public override bool Serialize(MessageWriter writer)
	{
		return false;
	}

	public override void Deserialize(MessageReader reader)
	{
	}
}
