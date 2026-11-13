using System;
using System.Collections;
using System.Collections.Generic;
using Cpp2ILInjected;
using Innersloth.DebugTool;
using UnityEngine;

public class DebugToolKeyboardInputProvider : IDebugToolInputProvider
{
	private sealed class _003CCoInitialize_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CCoInitialize_003Ed__3(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			//IL_0031: Expected I4, but got I8
			//IL_007a: Expected I4, but got I8
			if (_003C_003E1__state == 0)
			{
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			if (_003C_003E1__state == 1)
			{
				_003C_003E1__state = -1;
			}
			return false;
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

	private const float VERTICAL_REPEAT_RATE = 0.25f;

	private float keyDownStartTime;

	private float keyUpStartTime;

	public IEnumerator CoInitialize()
	{
		yield return null;
	}

	public bool GetToggle()
	{
		return Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.F3);
	}

	public bool GetCopy()
	{
		if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
		{
			return Input.GetKeyUp(KeyCode.C);
		}
		return false;
	} // TODO(verify): key binding (BackQuote toggle confirmed in decomp9; others inferred)

	public bool GetClear()
	{ return Input.GetKeyUp(KeyCode.Delete); } // TODO(verify): key binding (BackQuote toggle confirmed in decomp9; others inferred)

	public bool GetConfirm()
	{
		return Input.GetKeyUp(KeyCode.Return);
	} // TODO(verify): key binding (BackQuote toggle confirmed in decomp9; others inferred)

	public bool GetCancel()
	{ return Input.GetKeyUp(KeyCode.Escape); } // TODO(verify): key binding (BackQuote toggle confirmed in decomp9; others inferred)

	public bool GetPagePrevious()
	{
		return Input.GetKeyUp(KeyCode.Q);
	} // TODO(verify): key binding (BackQuote toggle confirmed in decomp9; others inferred)

	public bool GetPageNext()
	{
		return Input.GetKeyUp(KeyCode.E);
	} // TODO(verify): key binding (BackQuote toggle confirmed in decomp9; others inferred)

	public bool GetSubPagePrevious()
	{
		return Input.GetKeyUp(KeyCode.Z);
	} // TODO(verify): key binding (BackQuote toggle confirmed in decomp9; others inferred)

	public bool GetSubPageNext()
	{
		return Input.GetKeyUp(KeyCode.X);
	} // TODO(verify): key binding (BackQuote toggle confirmed in decomp9; others inferred)

	public bool GetNavLeft()
	{ return Input.GetKeyUp(KeyCode.LeftArrow); } // TODO(verify): key binding (BackQuote toggle confirmed in decomp9; others inferred)

	public bool GetNavRight()
	{ return Input.GetKeyUp(KeyCode.RightArrow); } // TODO(verify): key binding (BackQuote toggle confirmed in decomp9; others inferred)

	public bool GetNavUp()
	{ return Input.GetKeyUp(KeyCode.UpArrow); } // TODO(verify): key binding (BackQuote toggle confirmed in decomp9; others inferred)

	public bool GetNavDown()
	{ return Input.GetKeyUp(KeyCode.DownArrow); } // TODO(verify): key binding (BackQuote toggle confirmed in decomp9; others inferred)
}
