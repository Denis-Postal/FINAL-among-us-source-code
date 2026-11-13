using System.Collections;
using UnityEngine;

public class SkeldExileController : ExileController
{
	protected override IEnumerator Animate()
	{
		float num = Camera.main.orthographicSize * Camera.main.aspect + 1f;
		Vector2 left = Vector2.left * num;
		Vector2 right = Vector2.right * num;
		Player.transform.localPosition = left;
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			yield return DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.black, Color.clear);
		}
		yield return new WaitForSeconds(0.2f);
		if (initData != null && initData.outfit != null && (bool)EjectSound)
		{
			SoundManager.Instance.PlayDynamicSound("PlayerEjected", EjectSound, loop: true, SoundDynamics, SoundManager.Instance.SfxChannel);
		}
		yield return new WaitForSeconds(0.8f);
		yield return Effects.All(PlayerSpin(left, right), HandleText(Duration * 0.3f, Duration * 0.5f));
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

	private IEnumerator PlayerSpin(Vector2 left, Vector2 right)
	{
		for (float t = 0f; t <= Duration; t += Time.deltaTime)
		{
			float time = t / Duration;
			Player.transform.localPosition = Vector2.Lerp(left, right, LerpCurve.Evaluate(time));
			float num = (t + 0.75f) * 25f / Mathf.Exp(t * 0.75f + 1f);
			Player.transform.Rotate(new Vector3(0f, 0f, num * Time.deltaTime * 30f));
			yield return null;
		}
	}

	private void SoundDynamics(AudioSource source, float dt)
	{
		source.volume = 0.8f;
		source.loop = false;
		source.panStereo = -1f;
	}
}
