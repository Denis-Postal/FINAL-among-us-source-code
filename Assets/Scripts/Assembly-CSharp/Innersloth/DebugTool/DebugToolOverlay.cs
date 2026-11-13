using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Innersloth.DebugTool
{
	public class DebugToolOverlay : MonoBehaviour
	{
		[SerializeField]
		private GameObject rootUI;

		[SerializeField]
		private Button closeButton;

		[SerializeField]
		private Button backButton;

		[Header("Tabs")]
		[SerializeField]
		private DebugTabButton[] tabButtons;

		[SerializeField]
		private DebugToolOverlayPage[] pages;

		private readonly List<IDebugToolInputProvider> inputProviders = new List<IDebugToolInputProvider>();

		private int activeTabIndex;

		private int totalTabs;

		private bool UIVisible => rootUI.activeSelf;

		private bool SubpageShowingDetails => backButton.gameObject.activeSelf;

		private DebugToolOverlayPage ActivePage => (pages != null && tabButtons != null && activeTabIndex >= 0 && activeTabIndex < tabButtons.Length)
			? pages.FirstOrDefault((DebugToolOverlayPage page) => page.Name == tabButtons[activeTabIndex].PageName)
			: null;

		private bool SubpageConsumingNavActions => ActivePage != null && ActivePage.ConsumingNavActions;

		private event Action<bool> OnToggle;

		private void Awake()
		{
			if (rootUI != null)
			{
				rootUI.SetActive(false);
			}
			if (tabButtons != null)
			{
				totalTabs = tabButtons.Length;
			}
		}

		private void ShowPage(string pageName)
		{
			bool flag = false;
			for (int i = 0; i < tabButtons.Length; i++)
			{
				DebugTabButton obj = tabButtons[i];
				obj.ToggleSelect(obj.PageName == pageName);
				if (obj.PageName == pageName)
				{
					activeTabIndex = i;
					flag = true;
				}
			}
			DebugToolOverlayPage[] array = pages;
			foreach (DebugToolOverlayPage debugToolOverlayPage in array)
			{
				if (debugToolOverlayPage.Name == pageName)
				{
					debugToolOverlayPage.gameObject.SetActive(value: true);
					debugToolOverlayPage.Show();
				}
				else
				{
					debugToolOverlayPage.gameObject.SetActive(value: false);
				}
			}
			if (!flag)
			{
				Debug.LogError("No tab or page was found with page name " + pageName);
			}
			ToggleBackButtonActive(active: false);
		}

		private void Update()
		{
			if (UIVisible)
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}

			bool toggle = Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.F3);
			if (!toggle)
			{
				foreach (IDebugToolInputProvider inputProvider in inputProviders)
				{
					if (inputProvider.GetToggle())
					{
						toggle = true;
						break;
					}
				}
			}

			if (toggle)
			{
				ToggleUIVisible(!UIVisible);
				return;
			}

			foreach (IDebugToolInputProvider inputProvider in inputProviders)
			{
				if (UIVisible && !SubpageConsumingNavActions)
				{
					if (inputProvider.GetPagePrevious())
					{
						activeTabIndex--;
						if (activeTabIndex < 0)
						{
							activeTabIndex = totalTabs - 1;
						}
						ShowPage(tabButtons[activeTabIndex].PageName);
						break;
					}
					if (inputProvider.GetPageNext())
					{
						activeTabIndex++;
						if (activeTabIndex >= totalTabs)
						{
							activeTabIndex = 0;
						}
						ShowPage(tabButtons[activeTabIndex].PageName);
						break;
					}
				}
				if (!SubpageShowingDetails && inputProvider.GetCancel())
				{
					ToggleUIVisible(visible: false);
					break;
				}
			}
		}

		public void RegisterInputProvider(IDebugToolInputProvider provider)
		{
			inputProviders.Add(provider);
			DebugToolOverlayPage[] array = pages;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].AddInputProvider(provider);
			}
		}

		public void RegisterKeyboardInputProvider(IDebugToolKeyboardInputProvider provider)
		{
			DebugToolOverlayPage[] array = pages;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetKeyboardInputProvider(provider);
			}
		}

		public void AddToggleActiveCallback(Action<bool> action)
		{
			OnToggle += action;
		}

		public void Initialize()
		{
			foreach (IDebugToolInputProvider inputProvider in inputProviders)
			{
				StartCoroutine(inputProvider.CoInitialize());
			}
		}

		public void ToggleUIVisible(bool visible)
		{
			if (rootUI != null && rootUI.activeSelf != visible)
			{
				rootUI.SetActive(visible);
				if (visible)
				{
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
				}
				OnToggle?.Invoke(visible);
				if (visible && tabButtons != null && tabButtons.Length > 0)
				{
					ShowPage(tabButtons[activeTabIndex < tabButtons.Length ? activeTabIndex : 0].PageName);
				}
			}
		}

		private void ToggleBackButtonActive(bool active)
		{
			backButton.gameObject.SetActive(active);
			closeButton.gameObject.SetActive(!active);
		}
	}
}
