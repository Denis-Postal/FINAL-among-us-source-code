using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

public class PolusShipStatus : ShipStatus
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Func<ISystemType, bool> _003C_003E9__0_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003COnEnable_003Eb__0_0(ISystemType i)
		{
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A7D10");
			object obj = default;
			bool flag = obj == null;
			return !flag;
		}
	}

	protected override void OnEnable()
	{
		if (Systems == null)
		{
			Systems = new Dictionary<SystemTypes, ISystemType>(SystemTypeComparer.Instance)
			{
				{
					SystemTypes.Electrical,
					new SwitchSystem()
				},
				{
					SystemTypes.MedBay,
					new MedScanSystem()
				}
			};
			Camera main = Camera.main;
			main.backgroundColor = CameraColor;
			FollowerCamera component = main.GetComponent<FollowerCamera>();
			DestroyableSingleton<HudManager>.Instance.ShadowQuad.material.SetInt("_Mask", 7);
			Systems.Add(SystemTypes.Doors, new DoorsSystemType());
			Systems.Add(SystemTypes.Comms, new HudOverrideSystemType());
			Systems.Add(SystemTypes.Security, new SecurityCameraSystemType());
			Systems.Add(SystemTypes.Ventilation, new VentilationSystem());
			Systems.Add(SystemTypes.Laboratory, new ReactorSystemType(60f, SystemTypes.Laboratory));
			if ((bool)component)
			{
				component.shakeAmount = 0f;
				component.shakePeriod = 0f;
			}
			Systems.Add(SystemTypes.Sabotage, new SabotageSystemType(Systems.Values.Where((ISystemType i) => i is IActivatable).Cast<IActivatable>().ToArray()));
		}
	}

	// TODO(verify): meeting layout (two rows around MeetingSpawnCenter/MeetingSpawnCenter2, 0.6 spacing) from public Polus code; the decompile lost the vector maths.
	public override void SpawnPlayer(PlayerControl player, int numPlayers, bool initialSpawn)
	{
		if (initialSpawn)
		{
			base.SpawnPlayer(player, numPlayers, initialSpawn);
			return;
		}
		int num = Mathf.FloorToInt((float)numPlayers / 2f);
		int num2 = player.PlayerId % 15;
		Vector2 position = ((num2 >= num) ? (MeetingSpawnCenter2 + Vector2.right * (num2 - num) * 0.6f) : (MeetingSpawnCenter + Vector2.right * num2 * 0.6f));
		player.NetTransform.SnapTo(position);
	}
}
