using System;
using Cpp2ILInjected;
using UnityEngine;

public class HowToPlaySceneMovementPageDummyConsole : MonoBehaviour
{
	public int ConsoleId;

	public HowToPlaySceneMovementPagePlayerAnimator[] Players;

	public float UseDistance;

	[HideInInspector]
	private SpriteRenderer rend;

	public void Start()
	{
		rend = GetComponent<SpriteRenderer>();
	}

	public unsafe void FixedUpdate()
	{
		rend.material.SetColor("_OutlineColor", Color.yellow);
		float num = float.MaxValue;
		for (int i = 0; i < Players.Length; i++)
		{
			HowToPlaySceneMovementPagePlayerAnimator howToPlaySceneMovementPagePlayerAnimator = Players[i];
			Vector2 vector = transform.position - howToPlaySceneMovementPagePlayerAnimator.transform.position;
			vector.y += 0.3636f;
			float magnitude = vector.magnitude;
			if (magnitude < num)
			{
				num = magnitude;
			}
			if (magnitude < UseDistance)
			{
				howToPlaySceneMovementPagePlayerAnimator.NearbyConsoles |= 1 << ConsoleId;
			}
			else
			{
				howToPlaySceneMovementPagePlayerAnimator.NearbyConsoles &= ~(1 << ConsoleId);
			}
		}
		if (num < UseDistance * 2f)
		{
			rend.material.SetFloat("_Outline", 1f);
			if (num < UseDistance)
			{
				rend.material.SetColor("_AddColor", Color.yellow);
			}
			else
			{
				rend.material.SetColor("_AddColor", Color.clear);
			}
		}
		else
		{
			rend.material.SetFloat("_Outline", 0f);
			rend.material.SetColor("_AddColor", Color.clear);
		}
	}
}
