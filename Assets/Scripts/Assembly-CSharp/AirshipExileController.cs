using System.Collections;
using UnityEngine;

public class AirshipExileController : ExileController
{
	public SpriteRenderer ForegroundCloud;

	public SpriteRenderer BackgroundCloud;

	public SpriteRenderer Cloud1;

	public SpriteRenderer Cloud2;

	public SpriteRenderer Cloud3;

	public SpriteRenderer HandSlot;

	public AudioClip Stinger;

	private float CloudSlowMo = 1f;

	private float PlayerSlowMo = 1f;

	protected override IEnumerator Animate()
	{
		if (initData != null && initData.outfit != null)
		{
			PlayerMaterial.SetColors(initData.outfit.ColorId, HandSlot);
		}
		float duration = 0.7f + Duration + 0.5f + 2f;
		StartCoroutine(Effects.All(SlowMoSlide2D(ForegroundCloud.transform, new Vector2(-1.4f, -2.41f), new Vector2(-0.4f, -2.41f), duration), SlowMoSlide2D(BackgroundCloud.transform, new Vector2(-0.97f, -1.043f), new Vector2(0.25f, -1.043f), duration), SlowMoSlide2D(Cloud1.transform, new Vector2(3f, 0.25f), new Vector2(6.5f, 0.25f), duration), SlowMoSlide2D(Cloud2.transform, new Vector2(-6f, 3f), new Vector2(5f, 3f), duration), SlowMoSlide2D(Cloud3.transform, new Vector2(-4f, -2.2f), new Vector2(4f, -2.2f), duration)));
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			yield return DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.black, Color.clear);
		}
		yield return Effects.Wait(0.5f);
		yield return Effects.All(PlayerFall(), HandleText(1.75f, Duration * 0.5f));
		yield return new WaitForSeconds(0.5f);
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			yield return DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.clear, Color.black);
		}
		else
		{
			yield return Effects.Wait(0.2f);
		}
		yield return WrapUpAndSpawn();
	}

	private IEnumerator SlowMoSlide2D(Transform target, Vector2 source, Vector2 dest, float duration)
	{
		Vector3 temp = new Vector3
		{
			z = target.localPosition.z
		};
		for (float time = 0f; time < duration; time += Time.deltaTime * CloudSlowMo)
		{
			float t = time / duration;
			temp.x = Mathf.SmoothStep(source.x, dest.x, t);
			temp.y = Mathf.SmoothStep(source.y, dest.y, t);
			target.localPosition = temp;
			yield return null;
		}
		temp.x = dest.x;
		temp.y = dest.y;
		target.localPosition = temp;
	}

	protected IEnumerator WrapUpAndSpawn()
	{
		if (initData != null && initData.networkedPlayer != null)
		{
			PlayerControl playerControl = initData.networkedPlayer.Object;
			if ((bool)playerControl)
			{
				playerControl.Exiled();
			}
			initData.networkedPlayer.IsDead = true;
		}
		if (DestroyableSingleton<TutorialManager>.InstanceExists || (GameManager.Instance != null && !GameManager.Instance.LogicFlow.IsGameOverDueToDeath()))
		{
			yield return ShipStatus.Instance.PrespawnStep();
			ReEnableGameplay();
		}
		Object.Destroy(gameObject);
	}

	protected override IEnumerator HandleText(float firstWaitTime, float animDuration)
	{
		yield return Effects.Wait(firstWaitTime);
		if (initData != null && initData.outfit != null)
		{
			CloudSlowMo = 0.1f;
			PlayerSlowMo = 0.1f;
			SoundManager.Instance.PlaySound(Stinger, loop: false);
		}
		float newDur = Duration * 0.5f;
		for (float t = 0f; t <= newDur; t += Time.deltaTime)
		{
			int num = (int)(t / newDur * (float)completeString.Length);
			if (num > Text.text.Length)
			{
				Text.text = completeString.Substring(0, num);
				Text.gameObject.SetActive(value: true);
				if (completeString[num - 1] != ' ')
				{
					SoundManager.Instance.PlaySoundImmediate(TextSound, loop: false, 0.8f);
				}
			}
			yield return null;
		}
		Text.text = completeString;
		yield return Effects.Wait(1f);
		CloudSlowMo = 1f;
		PlayerSlowMo = 6f;
		yield return Effects.Wait(0.5f);
		if (initData.confirmImpostor)
		{
			ImpostorText.gameObject.SetActive(value: true);
		}
		yield return Effects.Bloop(0f, ImpostorText.transform);
	}

	private IEnumerator PlayerFall()
	{
		Camera main = Camera.main;
		float num = main.orthographicSize + 1.5f;
		float num2 = main.orthographicSize * main.aspect;
		Vector2 sourcePos = Vector2.up * num + Vector2.right * num2 / 4f;
		Vector2 targetPos = Vector2.down * num + Vector2.left * num2 / 4f;
		Vector2 vector = (targetPos - sourcePos) / 2f;
		Vector2 anchor = sourcePos + vector + vector.Rotate(-90f).normalized * 0.5f;
		float d = Duration;
		for (float t = 0f; t <= d; t += Time.deltaTime * PlayerSlowMo)
		{
			float t2 = t / d;
			Vector2 vector2 = Effects.Bezier(t2, sourcePos, targetPos, anchor);
			Player.transform.localPosition = vector2;
			float z = Mathf.Lerp(0f, 80f, t2);
			Player.transform.localEulerAngles = new Vector3(0f, 0f, z);
			yield return null;
		}
	}
}
