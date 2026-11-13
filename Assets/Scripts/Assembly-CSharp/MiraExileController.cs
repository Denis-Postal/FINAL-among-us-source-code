using System.Collections;
using UnityEngine;

public class MiraExileController : ExileController
{
	public Transform BackgroundClouds;

	public Transform ForegroundClouds;

	protected override IEnumerator Animate()
	{
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			yield return DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.black, Color.clear);
		}
		yield return Effects.All(PlayerSpin(), HandleText(Duration * 0.5f, Duration * 0.5f), Effects.Slide2D(BackgroundClouds, new Vector2(0f, -3f), new Vector2(0f, 0.5f), Duration), Effects.Sequence(Effects.Wait(2f), Effects.Slide2D(ForegroundClouds, new Vector2(0f, -7f), new Vector2(0f, 2.5f))));
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
		WrapUp();
	}

	private IEnumerator PlayerSpin()
	{
		float num = Camera.main.orthographicSize + 1f;
		Vector2 top = Vector2.up * num;
		Vector2 bottom = Vector2.down * num;
		for (float t = 0f; t <= Duration; t += Time.deltaTime)
		{
			float t2 = t / Duration;
			Player.transform.localPosition = Vector2.Lerp(top, bottom, t2);
			float num2 = (t + 0.75f) * 25f / Mathf.Exp(t * 0.75f + 1f);
			Player.transform.Rotate(new Vector3(0f, 0f, num2 * Time.deltaTime * 5f));
			yield return null;
		}
	}
}
