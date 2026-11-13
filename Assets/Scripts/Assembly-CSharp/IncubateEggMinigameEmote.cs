using System;
using Cpp2ILInjected;
using UnityEngine;

public class IncubateEggMinigameEmote : MonoBehaviour
{
	private const string IS_VISIBLE_BOOL = "IsVisible";

	[SerializeField]
	public Animator animator;

	public bool IsVisible => animator.GetBool("IsVisible");

	public void SetVisible(bool visible)
	{
		animator.SetBool("IsVisible", visible);
	}
}
