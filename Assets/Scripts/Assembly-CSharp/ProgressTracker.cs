using System.Linq;
using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
	public MeshRenderer TileParent;

	private float curValue;

	public void Start()
	{
		if (GameManager.Instance.IsHideAndSeek())
		{
			gameObject.SetActive(value: false);
		}
		TileParent.material.SetFloat("_Buckets", 1f);
		TileParent.material.SetFloat("_FullBuckets", 0f);
	}

	public void FixedUpdate()
	{
		if (PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
		{
			TileParent.enabled = false;
			return;
		}
		if (!TileParent.enabled)
		{
			TileParent.enabled = true;
		}
		GameData instance = GameData.Instance;
		if (!instance || instance.TotalTasks <= 0)
		{
			return;
		}
		int num = (DestroyableSingleton<TutorialManager>.InstanceExists ? 1 : (instance.AllPlayers.Count - GameManager.Instance.LogicOptions.NumImpostors));
		num -= instance.AllPlayers.Count((NetworkedPlayerInfo p) => p.Disconnected);
		if (num < 1)
		{
			num = 1;
		}
		switch (GameManager.Instance.LogicOptions.GetTaskBarMode())
		{
		case TaskBarMode.Normal:
		{
			float b = (float)instance.CompletedTasks / (float)instance.TotalTasks * (float)num;
			curValue = Mathf.Lerp(curValue, b, Time.fixedDeltaTime * 2f);
			break;
		}
		case TaskBarMode.MeetingOnly:
			if (!MeetingHud.Instance)
			{
				break;
			}
			goto case TaskBarMode.Normal;
		case TaskBarMode.Invisible:
			gameObject.SetActive(value: false);
			break;
		}
		TileParent.material.SetFloat("_Buckets", num);
		TileParent.material.SetFloat("_FullBuckets", curValue);
	}
}
