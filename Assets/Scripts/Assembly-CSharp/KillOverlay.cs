using System;
using System.Collections;
using System.Collections.Generic;
using AmongUs.GameOptions;
using UnityEngine;

public class KillOverlay : MonoBehaviour
{
	public SpriteRenderer background;

	public GameObject flameParent;

	public OverlayKillAnimation[] KillAnims;

	public OverlayKillAnimation[] HorseWrangleAnims;

	public OverlayKillAnimation[] ClassicKillAnims;

	public OverlayKillAnimation[] CustomKillAnimations;

	private Queue<Func<IEnumerator>> queue = new Queue<Func<IEnumerator>>();

	private Coroutine showAll;

	private Coroutine showOne;

	public bool IsOpen
	{
		get
		{
			if (showAll == null)
			{
				return queue.Count > 0;
			}
			return true;
		}
	}

	public IEnumerator WaitForFinish()
	{
		while (showAll != null || queue.Count > 0)
		{
			yield return null;
		}
	}

	public void ShowKillAnimation(NetworkedPlayerInfo killer, NetworkedPlayerInfo victim)
	{
		IEnumerable<OverlayKillAnimation> self = KillAnims;
		OverlayKillAnimation killAnimation;
		if ((bool)killer.Object)
		{
			OverlayKillAnimation[] killAnimations = killer.Object.MyPhysics.Animations.GetKillAnimations();
			if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek && AprilFoolsMode.ShouldHorseAround())
			{
				self = HorseWrangleAnims;
			}
			else if (GameOptionsManager.Instance.CurrentGameOptions.GameMode != GameModes.HideNSeek && AprilFoolsMode.ShouldClassicMode())
			{
				self = ClassicKillAnims;
				if (killer.Role.CustomKillAnimations.Length != 0)
				{
					self = killer.Role.CustomKillAnimations;
				}
			}
			else if (killAnimations.Length != 0)
			{
				self = killAnimations;
			}
			else
			{
				SkinViewData skin = ShipStatus.Instance.CosmeticsCache.GetSkin(killer.Object.CurrentOutfit.SkinId);
				if ((bool)skin && skin.CustomKillAnimID > 0)
				{
					if (!string.IsNullOrEmpty(skin.HatID_KillAnim) && skin.HatID_KillAnim != killer.Object.CurrentOutfit.HatId)
					{
						if (killer.Role.CustomKillAnimations.Length != 0)
						{
							self = killer.Role.CustomKillAnimations;
						}
						killAnimation = self.Random();
						ShowKillAnimation(killAnimation, killer, victim);
					}
					else if (!string.IsNullOrEmpty(skin.VisorID_KillAnim) && skin.VisorID_KillAnim != killer.Object.CurrentOutfit.VisorId)
					{
						if (killer.Role.CustomKillAnimations.Length != 0)
						{
							self = killer.Role.CustomKillAnimations;
						}
						killAnimation = self.Random();
						ShowKillAnimation(killAnimation, killer, victim);
					}
					else
					{
						self = new OverlayKillAnimation[1] { CustomKillAnimations[skin.CustomKillAnimID - 1] };
						killAnimation = self.Random();
						ShowKillAnimation(killAnimation, killer, victim);
					}
					return;
				}
				if (killer.Role.CustomKillAnimations.Length != 0)
				{
					self = killer.Role.CustomKillAnimations;
				}
			}
		}
		killAnimation = self.Random();
		ShowKillAnimation(killAnimation, killer, victim);
	}

	public void ShowKillAnimation(OverlayKillAnimation killAnimation, NetworkedPlayerInfo killer, NetworkedPlayerInfo victim)
	{
		ShowKillAnimation(killAnimation, new KillOverlayInitData(killer, victim));
	}

	public void ShowKillAnimation(OverlayKillAnimation killAnimation, KillOverlayInitData initData)
	{
		queue.Enqueue(() =>
		{
			OverlayKillAnimation overlayKillAnimation = UnityEngine.Object.Instantiate(killAnimation, transform);
			overlayKillAnimation.Initialize(initData);
			overlayKillAnimation.gameObject.SetActive(value: false);
			return CoShowOne(overlayKillAnimation);
		});
		if (showAll == null)
		{
			showAll = StartCoroutine(ShowAll());
		}
	}

	public void ShowMeeting(MeetingCalledAnimation prefab, NetworkedPlayerInfo.PlayerOutfit playerOutfit)
	{
		queue.Enqueue(() =>
		{
			MeetingCalledAnimation meetingCalledAnimation = UnityEngine.Object.Instantiate(prefab, transform);
			meetingCalledAnimation.Initialize(playerOutfit);
			meetingCalledAnimation.gameObject.SetActive(value: false);
			return CoShowOne(meetingCalledAnimation);
		});
		if (showAll == null)
		{
			showAll = StartCoroutine(ShowAll());
		}
	}

	private IEnumerator ShowAll()
	{
		while (queue.Count > 0 || showOne != null)
		{
			if (showOne == null)
			{
				showOne = StartCoroutine(queue.Dequeue()());
			}
			yield return null;
		}
		showAll = null;
	}

	private IEnumerator CoShowOne(OverlayAnimation anim)
	{
		yield return anim.CoShow(this);
		UnityEngine.Object.Destroy(anim.gameObject);
		showOne = null;
	}
}
