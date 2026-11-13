using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class HideAndSeekTimerBar : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro timeText;

	[SerializeField]
	private Transform timerBar;

	[SerializeField]
	private MeshRenderer timerBarRenderer;

	[SerializeField]
	private Transform chunkBar;

	private float targetBarSize;

	private bool freezeChunk;

	private Coroutine chunkCoroutine;

	private void Update()
	{
		float t = Time.deltaTime * 10f;
		Vector3 localScale = timerBar.localScale;
		localScale.x = Mathf.Lerp(localScale.x, targetBarSize, t);
		timerBar.localScale = localScale;
		if (!freezeChunk)
		{
			localScale = chunkBar.localScale;
			localScale.x = Mathf.Lerp(localScale.x, targetBarSize, t);
			chunkBar.localScale = localScale;
		}
	}

	public void UpdateTimer(float time, float maxTime)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(time);
		timeText.text = timeSpan.ToString("m\\:ss");
		targetBarSize = Mathf.Clamp01(time / maxTime);
	}

	public void StartFinalHide()
	{
		timerBarRenderer.material = new Material(timerBarRenderer.material);
		timerBarRenderer.material.SetColor("_Color", Palette.ImpostorRed);
		StopChunkCoroutine();
		chunkBar.gameObject.SetActive(value: false);
	}

	public void TaskComplete()
	{
		StartChunkCoroutine();
	}

	private void StartChunkCoroutine()
	{
		StopChunkCoroutine();
		chunkCoroutine = StartCoroutine(ChunkCoroutine());
	}

	private void StopChunkCoroutine()
	{
		if (chunkCoroutine != null)
		{
			StopCoroutine(chunkCoroutine);
			chunkCoroutine = null;
		}
	}

	private IEnumerator ChunkCoroutine()
	{
		freezeChunk = true;
		yield return new WaitForSeconds(1f);
		freezeChunk = false;
		chunkCoroutine = null;
	}
}
