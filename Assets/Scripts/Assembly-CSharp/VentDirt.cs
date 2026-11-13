using System;
using System.Collections;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

public class VentDirt : PoolableBehavior
{
	private sealed class _003C_003Ec__DisplayClass4_0
	{
		public SpriteRenderer rend;

		internal void _003CCoDisappear_003Eb__0(float t)
		{
			// TODO(verify): shrink to nothing
			rend.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
		}
	}

	private sealed class _003CCoDisappear_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		public VentDirt _003C_003E4__this;

		private _003C_003Ec__DisplayClass4_0 _003C_003E8__1;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CCoDisappear_003Ed__4(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			VentDirt self = _003C_003E4__this;
			switch (_003C_003E1__state)
			{
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass4_0();
				if (!self.enabled)
				{
					return false;
				}
				self.enabled = false;
				ButtonBehavior component = self.GetComponent<ButtonBehavior>();
				component.enabled = false;
				VibrationManager.Vibrate(0.5f, 0.5f, 0.35f, VibrationManager.VibrationFalloff.Linear, null, false, null);
				self.CleanedEffect.Play();
				_003C_003E8__1.rend = self.GetComponent<SpriteRenderer>();
				_003C_003E2__current = Effects.Lerp(0.2f, _003C_003E8__1._003CCoDisappear_003Eb__0);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.rend.sprite = null;
				_003C_003E2__current = Effects.Wait(0.1f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				self.OwnerPool.Reclaim(self);
				return false;
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

	public Sprite[] DirtImages;

	public ParticleSystem CleanedEffect;

	private int imgIdx;

	public override void Reset()
	{
		enabled = true;
		SpriteRenderer component = GetComponent<SpriteRenderer>();
		imgIdx = DirtImages.RandomIdx();
		component.sprite = DirtImages[imgIdx];
		component.enabled = true;
		ButtonBehavior component2 = GetComponent<ButtonBehavior>();
		component2.enabled = true;
		component2.OnClick.RemoveAllListeners();
		base.Reset();
	}

	public IEnumerator CoDisappear()
	{
		if (enabled)
		{
			enabled = false;
			GetComponent<ButtonBehavior>().enabled = false;
			VibrationManager.Vibrate(0.5f, 0.5f, 0.35f, VibrationManager.VibrationFalloff.Linear);
			CleanedEffect.Play();
			SpriteRenderer rend = GetComponent<SpriteRenderer>();
			yield return Effects.Lerp(0.2f, (float t) =>
			{
				rend.transform.localScale = new Vector3(1f - t, 1f - t, 1f - t);
			});
			yield return Effects.Wait(0.1f);
			rend.sprite = null;
			OwnerPool.Reclaim(this);
		}
	}
}
