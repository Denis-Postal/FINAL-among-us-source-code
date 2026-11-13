using System.Collections;
using PowerTools;
using UnityEngine;

public class PbExileController : ExileController
{
	private const float BOTTOM_Y = 2.81f;

	private const float PADDED_BOTTOM_Y = 4f;

	public SpriteRenderer HandSlot;

	public Sprite BadHand;

	public Sprite GoodHand;

	public AudioClip SplashSound;

	public SpriteAnim Sploosher;

	public AnimationClip Sploosh;

	private IEnumerator finalSinkCoroutine;

	protected override IEnumerator Animate()
	{
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			yield return DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.black, Color.clear);
		}
		yield return Effects.Wait(0.75f);
		yield return Effects.All(PlayerFall(), PlayerSpin(), HandleText(Duration * 0.5f, Duration * 0.5f));
		if (initData.confirmImpostor)
		{
			ImpostorText.gameObject.SetActive(value: true);
		}
		yield return Effects.Bloop(0f, ImpostorText.transform);
		yield return new WaitForSeconds(0.5f);
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			yield return DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.clear, Color.black);
		}
		else
		{
			yield return Effects.Wait(0.2f);
		}
		if (finalSinkCoroutine != null)
		{
			StopCoroutine(finalSinkCoroutine);
		}
		WrapUp();
	}

	private IEnumerator PlayerFall()
	{
		float num = Camera.main.orthographicSize + 1f;
		Vector2 top = Vector2.up * num;
		Vector2 bottom = Vector2.down * 2.81f;
		bool started = initData == null || initData.outfit == null;
		float d1 = Duration / 1.8f;
		for (float t = 0f; t <= d1; t += Time.deltaTime)
		{
			float time = t / d1;
			float t2 = LerpCurve.Evaluate(time);
			Vector2 vector = Vector2.Lerp(top, bottom, t2);
			Player.transform.localPosition = vector;
			if (!started && vector.y < -1.7f)
			{
				started = true;
				Sploosher.Play(Sploosh);
				SoundManager.Instance.PlaySound(SplashSound, loop: false);
				VibrationManager.Vibrate(0.2f, 0.2f, 0.35f, VibrationManager.VibrationFalloff.Linear);
			}
			yield return null;
		}
	}

	private IEnumerator PlayerSpin()
	{
		float num = Camera.main.orthographicSize + 1f;
		Vector2 top = Vector2.up * num;
		Vector2 bottom = Vector2.down * 2.81f;
		float d1 = Duration / 1.8f;
		for (float t = 0f; t <= d1; t += Time.deltaTime)
		{
			_ = t / d1;
			float num2 = (t + 0.75f) * 25f / Mathf.Exp(t * 0.75f + 1f);
			Player.transform.Rotate(new Vector3(0f, 0f, num2 * Time.deltaTime * 5f));
			yield return null;
		}
		if (initData.confirmImpostor)
		{
			if (initData != null && initData.outfit != null)
			{
				HandSlot.sprite = (initData.isImpostor ? GoodHand : BadHand);
				PlayerMaterial.SetColors(initData.outfit.ColorId, HandSlot);
			}
			Player.transform.eulerAngles = new Vector3(0f, 0f, -10f);
			float duration = Duration / 4f;
			top.y = -1.78f;
			yield return Effects.Overlerp(duration, (float p) =>
			{
				Player.transform.localPosition = Vector2.LerpUnclamped(bottom, top, p);
			});
			float d3 = Duration / 2f;
			for (float t = 0f; t <= d3; t += Time.deltaTime)
			{
				float t2 = t / d3;
				Vector2 vector = Vector2.Lerp(top, bottom, t2);
				vector += Random.insideUnitCircle * 0.025f;
				Player.transform.localPosition = vector;
				Player.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(-10f, 17f, t2));
				yield return null;
			}
			finalSinkCoroutine = CoFinalSink();
			StartCoroutine(finalSinkCoroutine);
		}
	}

	private IEnumerator CoFinalSink()
	{
		Vector2 bottom = Vector2.down * 2.81f;
		Vector2 paddedBottom = Vector2.down * 4f;
		float d4 = Duration / 1.8f;
		for (float t = 0f; t <= d4; t += Time.deltaTime)
		{
			float t2 = t / d4;
			Vector2 vector = Vector2.Lerp(bottom, paddedBottom, t2);
			Player.transform.localPosition = vector;
			yield return null;
		}
	}
}
