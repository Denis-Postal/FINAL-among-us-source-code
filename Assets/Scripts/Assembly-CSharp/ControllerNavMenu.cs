using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

public class ControllerNavMenu : MonoBehaviour
{
	private sealed class _003COpenCoroutine_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		public ControllerNavMenu _003C_003E4__this;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003COpenCoroutine_003Ed__9(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false; // TODO(verify): default, original logic unrecoverable
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		void IEnumerator.Reset()
		{
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			NotSupportedException ex = new NotSupportedException();
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			throw ex;
		}
	}

	public bool openInOnEnable;

	public bool gridNavigation;

	public bool trySelectAny;

	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	private bool isOpen;

	private void Start()
	{
		if (!openInOnEnable)
		{
			OpenMenu();
		}
	}

	private unsafe void OnEnable()
	{
		if (!openInOnEnable)
		{
			return;
		}
		foreach (UiElement item in ControllerSelectable)
		{
			if (!item.isActiveAndEnabled && item.enabled && item.gameObject.activeSelf)
			{
				item.gameObject.SetActive(value: false);
				item.gameObject.SetActive(value: true);
			}
		}
		OpenMenu();
	}

	private IEnumerator OpenCoroutine()
	{
		yield return null;
		OpenMenu();
	}

	private void OnDisable()
	{
		StopAllCoroutines();
		CloseMenu();
	}

	public void OpenMenu(bool force = false)
	{
		if (!(!isOpen | force) || !ControllerManager.Instance)
		{
			return;
		}
		UiElement uiElement = DefaultButtonSelected;
		if ((!uiElement || !uiElement.isActiveAndEnabled) && trySelectAny)
		{
			foreach (UiElement item in ControllerSelectable)
			{
				if ((bool)item && item.isActiveAndEnabled)
				{
					uiElement = item;
				}
			}
		}
		ControllerManager.Instance.OpenOverlayMenu(gameObject.name, BackButton, uiElement, ControllerSelectable, gridNavigation);
		isOpen = true;
	}

	public void CloseMenu()
	{
		if (isOpen)
		{
			ControllerManager.Instance.CloseOverlayMenu(gameObject.name);
			isOpen = false;
		}
	}
}
