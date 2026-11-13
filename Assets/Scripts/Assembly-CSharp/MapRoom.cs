using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

public class MapRoom : MonoBehaviour
{
	public SystemTypes room;

	public SpriteRenderer door;

	public SpriteRenderer special;

	public InfectedOverlay Parent { get; set; }

	public void Start()
	{
		if ((bool)door)
		{
			door.SetCooldownNormalizedUvs();
		}
		if ((bool)special)
		{
			special.SetCooldownNormalizedUvs();
		}
	}

	public void DoorsUpdate()
	{
		if ((bool)door && (bool)ShipStatus.Instance)
		{
			float timer = ((RunTimer)ShipStatus.Instance.Systems[SystemTypes.Doors]).GetTimer(room);
			float value = (Parent.CanUseDoors ? timer : 1f);
			door.material.SetFloat("_Percent", value);
		}
	}

	internal void SetSpecialActive(float perc)
	{ /* spliced-from-2019 */
		if ((bool)special)
		{
			special.material.SetFloat("_Percent", perc);
		}
	}

	public void SabotageReactor()
	{
		if (Parent.CanUseSabotage)
		{
			ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Sabotage, 3);
			DestroyableSingleton<AchievementManager>.Instance.SabotageCalledLocally();
			DestroyableSingleton<DebugAnalytics>.Instance.Analytics.SabotageStart(SystemTypes.Reactor);
		}
	}

	public void SabotageHeli()
	{
		if (Parent.CanUseSabotage)
		{
			ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Sabotage, 58);
			DestroyableSingleton<AchievementManager>.Instance.SabotageCalledLocally();
		}
	}

	public void SabotageComms()
	{
		if (Parent.CanUseSabotage)
		{
			ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Sabotage, 14);
			DestroyableSingleton<AchievementManager>.Instance.SabotageCalledLocally();
			DestroyableSingleton<DebugAnalytics>.Instance.Analytics.SabotageStart(SystemTypes.Comms);
		}
	}

	public void SabotageOxygen()
	{
		if (Parent.CanUseSabotage)
		{
			ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Sabotage, 8);
			DestroyableSingleton<AchievementManager>.Instance.SabotageCalledLocally();
			DestroyableSingleton<DebugAnalytics>.Instance.Analytics.SabotageStart(SystemTypes.LifeSupp);
		}
	}

	public void SabotageLights()
	{
		if (Parent.CanUseSabotage)
		{
			ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Sabotage, 7);
			DestroyableSingleton<AchievementManager>.Instance.SabotageCalledLocally();
			DestroyableSingleton<DebugAnalytics>.Instance.Analytics.SabotageStart(SystemTypes.Electrical);
		}
	}

	public void SabotageSeismic()
	{
		if (Parent.CanUseSabotage)
		{
			ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Sabotage, 21);
			DestroyableSingleton<AchievementManager>.Instance.SabotageCalledLocally();
			DestroyableSingleton<DebugAnalytics>.Instance.Analytics.SabotageStart(SystemTypes.Laboratory);
		}
	}

	public void SabotageMushroomMixup()
	{
		if (Parent.CanUseSabotage)
		{
			ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Sabotage, 57);
			DestroyableSingleton<AchievementManager>.Instance.SabotageCalledLocally();
		}
	}

	public void SabotageDoors()
	{
		if (Parent.CanUseDoors && !(((RunTimer)ShipStatus.Instance.Systems[SystemTypes.Doors]).GetTimer(room) > 0f))
		{
			ShipStatus.Instance.RpcCloseDoorsOfType(room);
			DestroyableSingleton<DebugAnalytics>.Instance.Analytics.SabotageStart(SystemTypes.Doors);
		}
	}
}
