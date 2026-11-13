using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

public class MapTaskOverlay : MonoBehaviour
{
	public ObjectPoolBehavior icons;

	private Dictionary<string, PooledMapIcon> data= new Dictionary<string, PooledMapIcon>();

	public void Show()
	{
		gameObject.SetActive(value: true);
		if (PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
		{
			return;
		}
		for (int i = 0; i < PlayerControl.LocalPlayer.myTasks.Count; i++)
		{
			PlayerTask playerTask = PlayerControl.LocalPlayer.myTasks[i];
			if (playerTask.HasLocation && !playerTask.IsComplete)
			{
				SetIconLocation(playerTask);
			}
		}
	}

	public void Update()
	{
		if (PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
		{
			return;
		}
		for (int i = 0; i < PlayerControl.LocalPlayer.myTasks.Count; i++)
		{
			PlayerTask playerTask = PlayerControl.LocalPlayer.myTasks[i];
			if (playerTask.HasLocation && !playerTask.IsComplete && playerTask.LocationDirty)
			{
				SetIconLocation(playerTask);
			}
		}
	}

	// TODO(verify): reconstructed from a damaged decompile (icon colours/scale from the 2019 baseline)
	private unsafe void SetIconLocation(PlayerTask task)
	{
		List<Vector2> locations = task.Locations;
		for (int i = 0; i < locations.Count; i++)
		{
			Vector3 localPosition = locations[i] / ShipStatus.Instance.MapScale;
			localPosition.z = -1f;
			PooledMapIcon pooledMapIcon = icons.Get<PooledMapIcon>();
			pooledMapIcon.transform.localScale = new Vector3(pooledMapIcon.NormalSize, pooledMapIcon.NormalSize, pooledMapIcon.NormalSize);
			if (PlayerTask.TaskIsEmergency(task))
			{
				pooledMapIcon.rend.color = Color.red;
				pooledMapIcon.alphaPulse.enabled = true;
				pooledMapIcon.rend.material.SetFloat("_Outline", 1f);
			}
			else
			{
				pooledMapIcon.rend.color = Color.yellow;
			}
			pooledMapIcon.name = task.name;
			pooledMapIcon.lastMapTaskStep = task.TaskStep;
			pooledMapIcon.transform.localPosition = localPosition;
			if (task.TaskStep > 0)
			{
				pooledMapIcon.alphaPulse.enabled = true;
				pooledMapIcon.rend.material.SetFloat("_Outline", 1f);
			}
			string text = task.name;
			text += i;
			if (!data.ContainsKey(text))
			{
				data.Add(text, pooledMapIcon);
			}
		}
	}

	public unsafe void Hide()
	{
		foreach (KeyValuePair<string, PooledMapIcon> datum in data)
		{
			datum.Value.OwnerPool.Reclaim(datum.Value);
		}
		data.Clear();
		gameObject.SetActive(value: false);
	}

	public MapTaskOverlay()
	{ }
}
