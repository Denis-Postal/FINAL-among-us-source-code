using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;

public class ConditionalHide : MonoBehaviour
{
	public RuntimePlatform[] HideForPlatforms= new RuntimePlatform[1] { RuntimePlatform.WindowsPlayer };

	public RuntimePlatform[] OnlyShowForPlatforms= new RuntimePlatform[0];

	public bool ShowIfXboxTouchEnabled;

	private void Awake()
	{
		Hide();
	}

	public void Hide()
	{
		for (int i = 0; i < HideForPlatforms.Length; i++)
		{
			if (HideForPlatforms[i] == RuntimePlatform.WindowsPlayer)
			{
				gameObject.SetActive(value: false);
			}
		}
		for (int j = 0; j < OnlyShowForPlatforms.Length; j++)
		{
			RuntimePlatform num = OnlyShowForPlatforms[j];
			bool flag = false;
			if (num == RuntimePlatform.WindowsPlayer)
			{
				flag = true;
			}
			if (flag)
			{
				gameObject.SetActive(value: true);
				break;
			}
			gameObject.SetActive(value: false);
		}
	}

	public ConditionalHide()
	{ }
}
