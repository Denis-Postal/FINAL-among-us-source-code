using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PowerTools;
using TMPro;
using UnityEngine;

public class SpawnInMinigame : Minigame
{
	[Serializable]
	public struct SpawnLocation
	{
		public StringNames Name;

		public Sprite Image;

		public AnimationClip Rollover;

		public AudioClip RolloverSfx;

		public Vector3 Location;
	}

	public SpawnLocation[] Locations;

	public PassiveButton[] LocationButtons;

	public TextMeshPro Text;

	public AudioClip DefaultRolloverSound;

	[Header("Console Controller Navigation")]
	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	private bool gotButton;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		SpawnLocation[] array = Locations.ToArray();
		array.Shuffle();
		array = (from s in array.Take(LocationButtons.Length)
			orderby s.Location.x, s.Location.y descending
			select s).ToArray();
		for (int num = 0; num < LocationButtons.Length; num++)
		{
			PassiveButton obj = LocationButtons[num];
			SpawnLocation pt = array[num];
			obj.OnClick.AddListener(() =>
			{
				SpawnAt(pt);
			});
			obj.GetComponent<SpriteAnim>().Stop();
			obj.GetComponent<SpriteRenderer>().sprite = pt.Image;
			obj.GetComponentInChildren<TextMeshPro>().text = DestroyableSingleton<TranslationController>.Instance.GetString(pt.Name);
			ButtonAnimRolloverHandler component = obj.GetComponent<ButtonAnimRolloverHandler>();
			component.StaticOutImage = pt.Image;
			component.RolloverAnim = pt.Rollover;
			component.HoverSound = (pt.RolloverSfx ? pt.RolloverSfx : DefaultRolloverSound);
		}
		if (GameManager.Instance != null && GameManager.Instance.IsNormal())
		{
			foreach (NetworkedPlayerInfo allPlayer in GameData.Instance.AllPlayers)
			{
				if (!(allPlayer == null) && !(allPlayer.Object == null) && !allPlayer.Disconnected && !allPlayer.Object.isDummy)
				{
					allPlayer.Object.NetTransform.transform.position = new Vector2(-25f, 40f);
					allPlayer.Object.NetTransform.Halt();
				}
			}
		}
		StartCoroutine(RunTimer());
		ControllerManager.Instance.OpenOverlayMenu(name, null, DefaultButtonSelected, ControllerSelectable);
		PlayerControl.HideCursorTemporarily();
		ConsoleJoystick.SetMode_Menu();
	}

	public override void Close()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
		if (!gotButton)
		{
			LocationButtons.Random().ReceiveClickUp();
		}
		base.Close();
	}

	private IEnumerator RunTimer()
	{
		for (float time = 10f; time >= 0f; time -= Time.deltaTime)
		{
			Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.TimeRemaining, Mathf.CeilToInt(time));
			yield return null;
		}
		LocationButtons.Random().ReceiveClickUp();
	}

	private void SpawnAt(SpawnLocation spawnPoint)
	{
		if (amClosing == CloseState.None)
		{
			Logger.GlobalInstance.Info($"Player selected spawn point {spawnPoint.Name}");
			gotButton = true;
			PlayerControl.LocalPlayer.SetKinematic(b: true);
			PlayerControl.LocalPlayer.NetTransform.SetPaused(isPaused: true);
			PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(spawnPoint.Location);
			DestroyableSingleton<HudManager>.Instance.PlayerCam.SnapToTarget();
			StopAllCoroutines();
			StartCoroutine(CoSpawnAt(PlayerControl.LocalPlayer, spawnPoint));
		}
	}

	private IEnumerator CoSpawnAt(PlayerControl playerControl, SpawnLocation spawnLocation)
	{
		yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate();
		playerControl.SetKinematic(b: false);
		playerControl.NetTransform.SetPaused(isPaused: false);
		Close();
	}

	public IEnumerator WaitForFinish()
	{
		yield return null;
		while (amClosing == CloseState.None)
		{
			yield return null;
		}
	}
}
