using Object = UnityEngine.Object;
using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

public class SystemConsole : MonoBehaviour, IUsable
{
	public ImageNames useIcon = ImageNames.UseButton;

	public Vector2 SafePositionLocal;

	public float usableDistance = 1f;

	public bool FreeplayOnly;

	public bool onlyFromBelow;

	public SpriteRenderer Image;

	public Minigame MinigamePrefab;

	public ImageNames UseIcon => useIcon;

	public float UsableDistance => usableDistance;

	public float PercentCool => 0f;

	public void Start()
	{
		if (FreeplayOnly && !DestroyableSingleton<TutorialManager>.InstanceExists)
		{
			Object.Destroy(gameObject);
		}
	}

	public unsafe void SetOutline(bool on, bool mainTarget)
	{ /* spliced-from-2019 */
		if ((bool)Image)
		{
			Image.material.SetFloat("_Outline", on ? 1 : 0);
			Image.material.SetColor("_OutlineColor", Color.white);
			Image.material.SetColor("_AddColor", mainTarget ? Color.white : Color.clear);
		}
	}

	public float CanUse(NetworkedPlayerInfo pc, out bool canUse, out bool couldUse)
	{
		float num = float.MaxValue;
		PlayerControl playerControl = pc.Object;
		Vector2 truePosition = playerControl.GetTruePosition();
		couldUse = playerControl.CanMove && enabled && (!pc.IsDead || !(MinigamePrefab is EmergencyMinigame));
		canUse = couldUse && (!onlyFromBelow || truePosition.y < transform.position.y);
		if (canUse)
		{
			num = Vector2.Distance(truePosition, transform.position);
			canUse &= num <= UsableDistance;
		}
		return num;
	}

	// TODO(verify): minigame placement; (0,0,-50) from 2019 baseline (the current build also special-cases TaskAdderGame with SafePositionLocal, details lost).
	public void Use()
	{
		CanUse(PlayerControl.LocalPlayer.Data, out var canUse, out var _);
		if (canUse)
		{
			PlayerControl.LocalPlayer.NetTransform.Halt();
			Minigame minigame = Object.Instantiate(MinigamePrefab);
			if (minigame is TaskAdderGame taskAdderGame)
			{
				taskAdderGame.SafePositionWorld = SafePositionLocal + (Vector2)transform.position;
			}
			minigame.transform.SetParent(Camera.main.transform, worldPositionStays: false);
			minigame.transform.localPosition = new Vector3(0f, 0f, -50f);
			minigame.Begin(null);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Color color = Gizmos.color;
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, usableDistance);
		Gizmos.color = color;
	}
}
