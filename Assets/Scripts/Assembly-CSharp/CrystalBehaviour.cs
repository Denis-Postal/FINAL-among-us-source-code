using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

public class CrystalBehaviour : UiElement
{
	private sealed class _003CFlash_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		public float delay;

		public CrystalBehaviour c;

		private Color _003Ccol_003E5__2;

		private float _003Ctime_003E5__3;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CFlash_003Ed__12(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			// real iterator from the 2019 baseline Flash (TODO(verify))
			switch (_003C_003E1__state)
			{
			case 0:
				_003C_003E1__state = -1;
				_003Ctime_003E5__3 = 0f;
				goto case 1;
			case 1:
				if (_003C_003E1__state == 1) { _003Ctime_003E5__3 += Time.deltaTime; }
				_003C_003E1__state = -1;
				if (_003Ctime_003E5__3 < delay)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003Ccol_003E5__2 = Color.clear;
				_003Ctime_003E5__3 = 0f;
				goto case 2;
			case 2:
			{
				if (_003C_003E1__state == 2) { _003Ctime_003E5__3 += Time.deltaTime; }
				_003C_003E1__state = -1;
				if (_003Ctime_003E5__3 < 0.1f)
				{
					float t = _003Ctime_003E5__3 / 0.1f;
					float v = Mathf.Lerp(0f, 1f, t);
					_003Ccol_003E5__2.r = v; _003Ccol_003E5__2.g = v; _003Ccol_003E5__2.b = v;
					c.Renderer.material.SetColor("_AddColor", _003Ccol_003E5__2);
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				_003Ctime_003E5__3 = 0f;
				goto case 3;
			}
			case 3:
			{
				if (_003C_003E1__state == 3) { _003Ctime_003E5__3 += Time.deltaTime; }
				_003C_003E1__state = -1;
				if (_003Ctime_003E5__3 < 0.1f)
				{
					float t = _003Ctime_003E5__3 / 0.1f;
					float v = Mathf.Lerp(1f, 0f, t);
					_003Ccol_003E5__2.r = v; _003Ccol_003E5__2.g = v; _003Ccol_003E5__2.b = v;
					c.Renderer.material.SetColor("_AddColor", _003Ccol_003E5__2);
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				_003Ccol_003E5__2.r = 0f; _003Ccol_003E5__2.g = 0f; _003Ccol_003E5__2.b = 0f;
				c.Renderer.material.SetColor("_AddColor", _003Ccol_003E5__2);
				return false;
			}
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

	public Transform TargetPosition;

	public SpriteRenderer Renderer;

	public BoxCollider2D Collider;

	public bool CanMove = true;

	public FloatRange Padding;

	private const float Speed = 15f;

	public float XFloatMag = 0.01f;

	private const float FloatMag = 0.05f;

	private const float FloatSpeed = 0.35f;

	public float PieceIndex;

	private unsafe void Update()
	{
		if ((bool)TargetPosition)
		{
			Vector3 localPosition = transform.localPosition;
			if ((double)Vector2.Distance(TargetPosition.localPosition, transform.localPosition) > 0.01)
			{
				localPosition = Vector3.Lerp(TargetPosition.localPosition, transform.localPosition, Time.deltaTime);
			}
			float num = Time.time * 0.35f;
			localPosition.x += (Mathf.PerlinNoise(num, PieceIndex * 100f) * 2f - 1f) * XFloatMag;
			localPosition.y += (Mathf.PerlinNoise(PieceIndex * 100f, num) * 2f - 1f) * 0.05f;
			transform.localPosition = localPosition;
		}
	}

	public void Flash(float delay = 0f)
	{
		StopAllCoroutines();
		StartCoroutine(Flash(this, delay));
	}

	private static IEnumerator Flash(CrystalBehaviour c, float delay)
	{
		for (float time = 0f; time < delay; time += Time.deltaTime)
		{
			yield return null;
		}
		Color col = Color.clear;
		for (float time = 0f; time < 0.1f; time += Time.deltaTime)
		{
			float t = time / 0.1f;
			col.r = (col.g = (col.b = Mathf.Lerp(0f, 1f, t)));
			c.Renderer.material.SetColor("_AddColor", col);
			yield return null;
		}
		for (float time = 0f; time < 0.1f; time += Time.deltaTime)
		{
			float t2 = time / 0.1f;
			col.r = (col.g = (col.b = Mathf.Lerp(1f, 0f, t2)));
			c.Renderer.material.SetColor("_AddColor", col);
			yield return null;
		}
		col.r = (col.g = (col.b = 0f));
		c.Renderer.material.SetColor("_AddColor", col);
	}
}
