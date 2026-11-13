using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeetingIntroAnimation : MonoBehaviour
{
	public Transform VoteButtonParent;

	public Transform OverlayParent;

	public Transform DeadParent;

	public GameObject ProtectedRecently;

	public TextMeshPro DeadBodiesText;

	public SpriteRenderer BloodSplat;

	public Vector3 VoteButtonParentPos = new Vector3(0.25f, 0f, 0f);

	public Vector3 ReporterPos;

	public SpriteRenderer background;

	public AnimationCurve SlamCurve;

	public PlayerVoteArea VoteAreaPrefab;

	public AudioClip PlayerDeadSound;

	public AudioClip ProtectedRecentlySound;

	private List<PlayerVoteArea> deadCards = new List<PlayerVoteArea>();

	public SpriteRenderer[] OutsideMasks;

	public void Start()
	{
		VoteButtonParent.localPosition = Vector3.right * 20f;
		DeadParent.localPosition = Vector3.right * 20f;
		SpriteRenderer[] outsideMasks = OutsideMasks;
		for (int i = 0; i < outsideMasks.Length; i++)
		{
			outsideMasks[i].material.SetInt(PlayerMaterial.MaskLayer, 254);
		}
	}

	public void Init(NetworkedPlayerInfo reporter, NetworkedPlayerInfo[] deadBodies)
	{
		PlayerVoteArea playerVoteArea = Object.Instantiate(VoteAreaPrefab, OverlayParent);
		playerVoteArea.transform.localPosition = ReporterPos;
		playerVoteArea.SetMaskLayer(0);
		playerVoteArea.SetCosmetics(reporter);
		playerVoteArea.SetDead(isDead: false);
		playerVoteArea.SetReporter();
		float num = background.size.x / 2f;
		float num2 = VoteAreaPrefab.MaskArea.bounds.size.y + 0.2f;
		float x = VoteAreaPrefab.MaskArea.bounds.extents.x;
		int num3 = Mathf.CeilToInt((float)deadBodies.Length / 3f);
		for (int i = 0; i < deadBodies.Length; i += 3)
		{
			float y = (float)(num3 / 2 - i / 3) * num2;
			int num4 = Mathf.Min(deadBodies.Length - i, 3);
			for (int j = 0; j < num4; j++)
			{
				int num5 = i + j;
				float x2 = FloatRange.SpreadEvenly(0f - num - x, num + x, j, num4);
				PlayerVoteArea playerVoteArea2 = Object.Instantiate(VoteAreaPrefab, DeadParent);
				playerVoteArea2.transform.localPosition = new Vector3(x2, y, 0f);
				playerVoteArea2.SetMaskLayer(num5);
				playerVoteArea2.SetCosmetics(deadBodies[num5]);
				playerVoteArea2.SetDead(isDead: true);
				deadCards.Add(playerVoteArea2);
			}
		}
		if (deadBodies.Length == 0)
		{
			DeadBodiesText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NoDeadBodiesFound);
			DeadBodiesText.transform.localPosition = Vector3.zero;
			BloodSplat.enabled = false;
		}
		bool someoneWasProtected = false;
		PlayerControl.AllPlayerControls.ForEach((PlayerControl pc) =>
		{
			if (pc.protectedByGuardianThisRound)
			{
				pc.protectedByGuardianThisRound = false;
				if (pc.Data != null && !pc.Data.IsDead)
				{
					someoneWasProtected = true;
				}
			}
		});
		if (someoneWasProtected)
		{
			SoundManager.Instance.PlaySound(ProtectedRecentlySound, loop: false);
		}
		ProtectedRecently.SetActive(someoneWasProtected);
	}

	public IEnumerator CoRun()
	{
		gameObject.SetActive(value: true);
		foreach (PlayerVoteArea deadCard in deadCards)
		{
			StartCoroutine(deadCard.CoAnimateOverlay());
		}
		DeadParent.localPosition = Vector3.zero;
		yield return Effects.Lerp(0.2f, (float t) =>
		{
			DeadParent.transform.localScale = Vector3.one * Mathf.LerpUnclamped(8f, 1f, SlamCurve.Evaluate(t));
		});
		if (deadCards.Count > 0)
		{
			SoundManager.Instance.PlaySound(PlayerDeadSound, loop: false);
		}
		yield return Effects.Wait(3f);
		yield return Effects.All(Effects.Slide2D(VoteButtonParent, Vector2.right * 20f, VoteButtonParentPos), Effects.Slide2D(OverlayParent, Vector2.zero, Vector2.left * 20f));
		gameObject.SetActive(value: false);
	}
}
