using System;
using System.Collections;
using AmongUs.Data;
using TMPro;
using UnityEngine;

public class ProgressionScreen : MonoBehaviour
{
	private const float TimeToFillSeconds = 0.5f;

	private const float TimeToGlowSeconds = 0.75f;

	public PoolablePlayer Player;

	[Header("Xp")]
	public AudioClip XpSound;

	public AudioClip LevelUpSound;

	public ProgressBar XpBar;

	public TextMeshPro XpEarnedNowText;

	public AnimationCurve XpEarnedNowCurve;

	public TextMeshPro LevelText;

	public Transform LevelCircle;

	public TextMeshPro FutureLevelText;

	[Header("Pods")]
	public CurrencyEarned PodsEarned;

	[Header("Beans")]
	public CurrencyEarned BeansEarned;

	public void Activate()
	{
		gameObject.SetActive(value: true);
		ProgressionManager.XpGrantResult cachedXpGrantResult = EndGameResult.CachedXpGrantResult;
		uint oldLevel = cachedXpGrantResult.OldLevel;
		uint newLevel = cachedXpGrantResult.NewLevel;
		XpBar.Value = cachedXpGrantResult.OldXpAmount;
		XpBar.CapValue = cachedXpGrantResult.OldXpAmount;
		XpBar.MaxValue = cachedXpGrantResult.XpRequiredToLevelUp;
		XpBar.GlowAlpha = 0f;
		XpEarnedNowText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.XpGainedValue, cachedXpGrantResult.GrantedXp);
		XpEarnedNowText.color = Palette.ClearWhite;
		if (oldLevel == cachedXpGrantResult.MaxLevel)
		{
			XpBar.Value = 1f;
			XpBar.CapValue = 1f;
			XpBar.MaxValue = 1f;
			LevelText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Max);
			FutureLevelText.color = Palette.ClearWhite;
		}
		else if (newLevel == cachedXpGrantResult.MaxLevel)
		{
			FutureLevelText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Max);
			LevelText.text = ProgressionManager.FormatVisualLevel(oldLevel);
		}
		else
		{
			LevelText.text = ProgressionManager.FormatVisualLevel(oldLevel);
			string text = ProgressionManager.FormatVisualLevel((newLevel == oldLevel) ? (oldLevel + 1) : newLevel);
			FutureLevelText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LevelShorthand, text);
		}
		ProgressionManager.CurrencyGrantResult cachedPodsGrantResult = EndGameResult.CachedPodsGrantResult;
		CosmicubeData dataForPodId = DestroyableSingleton<CosmicubeManager>.Instance.GetDataForPodId(cachedPodsGrantResult.PodId);
		if (!(dataForPodId == null) && !(dataForPodId.GetCosmicubeName() == "Empty Cosmicube") && cachedPodsGrantResult.GrantedPodsWithMultiplierApplied != 0)
		{
			PodsEarned.Initialize(cachedPodsGrantResult);
		}
		else
		{
			PodsEarned.gameObject.SetActive(value: false);
		}
		ProgressionManager.CurrencyGrantResult cachedBeansGrantResult = EndGameResult.CachedBeansGrantResult;
		if (cachedBeansGrantResult.GrantedPodsWithMultiplierApplied != 0)
		{
			BeansEarned.Initialize(cachedBeansGrantResult);
		}
		else
		{
			BeansEarned.gameObject.SetActive(value: false);
		}
		if (BeansEarned.gameObject.activeSelf != PodsEarned.gameObject.activeSelf)
		{
			Transform transform = (BeansEarned.gameObject.activeSelf ? BeansEarned.transform : PodsEarned.transform);
			transform.localPosition = new Vector3(0f, transform.localPosition.y, base.transform.localPosition.z);
		}
		Player.UpdateFromPlayerOutfit(EndGameResult.CachedLocalPlayer.Outfit, PlayerMaterial.MaskType.None, isDead: false, includePet: true);
		Player.ToggleName(active: false);
		StartCoroutine(DoAnimations(cachedXpGrantResult));
	}

	private IEnumerator DoAnimations(ProgressionManager.XpGrantResult xpGainedResult)
	{
		yield return Effects.Wait(0.2f);
		if ((xpGainedResult.MaxLevel != DataManager.Player.Stats.Level || xpGainedResult.OldLevel != xpGainedResult.NewLevel) && xpGainedResult.GrantedXp != 0)
		{
			yield return AnimateXpAndLevelUp(xpGainedResult);
		}
		yield return AnimatePodsAndBeans();
	}

	private IEnumerator AnimateXpAndLevelUp(ProgressionManager.XpGrantResult xpGainedResult)
	{
		ulong newXpAmount = Math.Min(xpGainedResult.XpRequiredToLevelUp, xpGainedResult.OldXpAmount + xpGainedResult.GrantedXp);
		uint oldXpAmount = xpGainedResult.OldXpAmount;
		XpBar.CapValue = newXpAmount;
		StartCoroutine(Effects.Lerp(1.25f, (float t) =>
		{
			XpEarnedNowText.transform.SetLocalX(0.9f + XpEarnedNowCurve.Evaluate(t));
		}));
		StartCoroutine(Effects.ColorFade(XpEarnedNowText, Palette.ClearWhite, Color.white, 0.625f));
		StartCoroutine(AnimateXpBarFill(oldXpAmount, newXpAmount));
		yield return Effects.Wait(0.625f);
		yield return Effects.ColorFade(XpEarnedNowText, Color.white, Palette.ClearWhite, 0.625f);
		if (xpGainedResult.LevelledUp)
		{
			if (DataManager.Player.Stats.Level >= xpGainedResult.MaxLevel)
			{
				LevelText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MaxLevel);
				FutureLevelText.color = Palette.ClearWhite;
			}
			else
			{
				LevelText.text = ProgressionManager.Instance.CurrentVisualLevel;
				XpBar.MaxValue = xpGainedResult.XpRequiredToLevelUpNextLevel;
			}
			newXpAmount = xpGainedResult.GrantedXp + xpGainedResult.OldXpAmount - xpGainedResult.XpRequiredToLevelUp;
			XpBar.Value = 0f;
			XpBar.GlowAlpha = 1f;
			XpBar.CapValue = xpGainedResult.XpRequiredToLevelUpNextLevel;
			XpBar.MaxValue = xpGainedResult.XpRequiredToLevelUpNextLevel;
			XpEarnedNowText.gameObject.SetActive(value: false);
			SoundManager.Instance.PlaySound(LevelUpSound, loop: false, 0.5f);
			yield return Effects.ScaleIn(LevelCircle, 2f, 1f, 0.5f);
			yield return Effects.All(XpBar.ScaleDownY(0.75f), Effects.Lerp(0.75f, (float t) =>
			{
				Transform transform = FutureLevelText.transform;
				Vector3 localPosition = transform.localPosition;
				localPosition = new Vector3(localPosition.x, Mathf.Lerp(0f, -0.3f, t), localPosition.z);
				transform.localPosition = localPosition;
				FutureLevelText.color = FutureLevelText.color.SetAlpha(1f - t);
			}));
			yield return Effects.Wait(0.2f);
			uint num = DataManager.Player.Stats.Level + 1;
			if (num >= xpGainedResult.MaxLevel)
			{
				FutureLevelText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Max);
			}
			else
			{
				FutureLevelText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LevelShorthand, ProgressionManager.FormatVisualLevel(num));
			}
			StartCoroutine(Effects.Lerp(0.75f, (float t) =>
			{
				Transform transform = FutureLevelText.transform;
				Vector3 localPosition = transform.localPosition;
				localPosition = new Vector3(localPosition.x, Mathf.Lerp(0.3f, 0f, t), localPosition.z);
				transform.localPosition = localPosition;
				FutureLevelText.color = FutureLevelText.color.SetAlpha(t);
			}));
			yield return AnimateXpBarFill(0u, newXpAmount);
		}
	}

	private IEnumerator AnimateXpBarFill(uint oldXpAmount, ulong newXpAmount)
	{
		float startTime = Time.time;
		if (Mathf.Approximately(XpBar.GlowAlpha, 0f))
		{
			while (Time.time - startTime < 0.75f)
			{
				XpBar.GlowAlpha = Mathf.Lerp(0f, 1f, (Time.time - startTime) / 0.75f);
				yield return null;
			}
		}
		XpBar.ResetScale();
		XpBar.GlowAlpha = 0f;
		startTime = Time.time;
		SoundManager.Instance.PlaySound(XpSound, loop: false);
		while (Time.time - startTime < 0.5f)
		{
			float t = (Time.time - startTime) / 0.5f;
			XpBar.Value = Mathf.Lerp(oldXpAmount, newXpAmount, Effects.ExpOut(t));
			yield return null;
		}
		if (Mathf.Approximately(XpBar.GlowAlpha, 1f))
		{
			while (Time.time - startTime < 0.75f)
			{
				XpBar.GlowAlpha = Mathf.Lerp(1f, 0f, (Time.time - startTime) / 0.75f);
				yield return null;
			}
		}
	}

	private IEnumerator AnimatePodsAndBeans()
	{
		if (BeansEarned.gameObject.activeSelf)
		{
			yield return BeansEarned.ShowAmount();
		}
		if (PodsEarned.gameObject.activeSelf)
		{
			yield return PodsEarned.ShowAmount();
		}
		if (BeansEarned.gameObject.activeSelf)
		{
			ProgressionManager.CurrencyGrantResult cachedBeansGrantResult = EndGameResult.CachedBeansGrantResult;
			if (cachedBeansGrantResult.GrantedPodsWithMultiplierApplied != cachedBeansGrantResult.GrantedPodsPreMultiplier)
			{
				yield return BeansEarned.ShowMultiplier();
			}
			StartCoroutine(BeansEarned.EnterWallet());
		}
		if (PodsEarned.gameObject.activeSelf)
		{
			ProgressionManager.CurrencyGrantResult cachedPodsGrantResult = EndGameResult.CachedPodsGrantResult;
			if (cachedPodsGrantResult.GrantedPodsWithMultiplierApplied != cachedPodsGrantResult.GrantedPodsPreMultiplier)
			{
				yield return PodsEarned.ShowMultiplier();
			}
			StartCoroutine(PodsEarned.EnterWallet());
		}
	}
}
