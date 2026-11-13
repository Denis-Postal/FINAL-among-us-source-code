using Object = UnityEngine.Object;
using UnityEngine;

public class DivertPowerMetagame : Minigame
{
	public Minigame DistributePrefab;

	public Minigame ReceivePrefab;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		Minigame minigame = ((MyNormTask.taskStep != 0) ? Object.Instantiate(ReceivePrefab, transform.parent) : Object.Instantiate(DistributePrefab, transform.parent));
		minigame.Begin(task);
		Object.Destroy(gameObject);
	}
}
