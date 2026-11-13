using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class TelescopeGame : Minigame
{
	private sealed class _003CCoBlinky_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		public TelescopeGame _003C_003E4__this;

		private int _003Ci_003E5__2;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CCoBlinky_003Ed__15(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			// rebuilt from decomp9; TODO(verify): reticle colours (static Color loads lost)
			TelescopeGame self = _003C_003E4__this;
			switch (_003C_003E1__state)
			{
			case 0:
				_003Ci_003E5__2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				self.ReticleImage.color = Color.white;
				_003C_003E2__current = Effects.Wait(0.2f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			case 3:
				_003C_003E1__state = -1;
				return false;
			default:
				return false;
			}
			_003C_003E1__state = -1;
			if (_003Ci_003E5__2 < 3)
			{
				self.ReticleImage.color = Color.green;
				_003C_003E2__current = Effects.Wait(0.1f);
				_003C_003E1__state = 1;
				return true;
			}
			self.ReticleImage.color = Color.green;
			self.MyNormTask.NextStep();
			_003C_003E2__current = self.CoStartClose();
			_003C_003E1__state = 3;
			return true;
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

	private sealed class _003CRunBlipSound_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		public TelescopeGame _003C_003E4__this;

		private float _003Ctime_003E5__2;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CRunBlipSound_003Ed__13(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			TelescopeGame self = _003C_003E4__this;
			if (_003C_003E1__state == 0)
			{
				_003C_003E1__state = -1;
				_003Ctime_003E5__2 = 0f;
			}
			else
			{
				if (_003C_003E1__state != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003Ctime_003E5__2 += Time.deltaTime;
			}
			while (self.TargetItem != null && self.Reticle != null)
			{
				float v = Vector2.Distance(self.TargetItem.transform.position, self.Reticle.transform.position) / 10f;
				float delay = self.BlipDelay.Lerp(v);
				if (!(delay > _003Ctime_003E5__2))
				{
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySoundImmediate(self.BlipSound, loop: false, 1f, 1f, null);
					}
					VibrationManager.Vibrate(0.3f, 0.3f, 0.01f, VibrationManager.VibrationFalloff.Linear, null, false, null);
					_003Ctime_003E5__2 = 0f;
					continue;
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
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

	private bool grabbed;

	public Transform Background;

	public SpriteRenderer ItemDisplay;

	public BoxCollider2D[] Items;

	private BoxCollider2D TargetItem;

	public BoxCollider2D Reticle;

	public SpriteRenderer ReticleImage;

	private Coroutine blinky;

	public AudioClip BlipSound;

	public FloatRange BlipDelay= new FloatRange(0.01f, 1f);

	private TouchpadBehavior touchpad;

	private Vector3 initialPos;

	public void Start()
	{
		TargetItem = Items.Random();
		ItemDisplay.sprite = TargetItem.GetComponent<SpriteRenderer>().sprite;
		StartCoroutine(RunBlipSound());
		touchpad = GetComponent<TouchpadBehavior>();
		SetupInput(disableCursor: true);
	}

	private IEnumerator RunBlipSound()
	{
		while (true)
		{
			for (float time = 0f; time < BlipDelay.Lerp(Vector2.Distance(TargetItem.transform.position, Reticle.transform.position) / 10f); time += Time.deltaTime)
			{
				yield return null;
			}
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySoundImmediate(BlipSound, loop: false);
			}
			VibrationManager.Vibrate(0.3f, 0.3f, 0.01f, VibrationManager.VibrationFalloff.Linear);
		}
	}

	public void Update()
	{
		NormalPlayerTask myNormTask = MyNormTask;
		if ((object)myNormTask != null && myNormTask.IsComplete)
		{
			return;
		}
		Vector3 zero = Vector3.zero;
		Player player = ReInput.players.GetPlayer(0);
		Vector2 vector = new Vector2(player.GetAxis(13), player.GetAxis(14));
		if (vector.magnitude > 0.01f)
		{
			Vector2 vector2 = -vector * Time.deltaTime * 3.5f;
			zero = Background.transform.localPosition;
			zero.x = Mathf.Clamp(zero.x + vector2.x, -6f, 6f);
			zero.y = Mathf.Clamp(zero.y + vector2.y, -7f, 7f);
			Background.transform.localPosition = zero;
		}
		else if (touchpad.IsTouching())
		{
			if (touchpad.IsFirstTouch())
			{
				initialPos = Background.transform.localPosition;
			}
			Vector2 touchVector = touchpad.GetTouchVector();
			zero.x = Mathf.Clamp(initialPos.x + touchVector.x, -6f, 6f);
			zero.y = Mathf.Clamp(initialPos.y + touchVector.y, -7f, 7f);
			Background.transform.localPosition = zero;
		}
		if (grabbed)
		{
			Controller controller = DestroyableSingleton<PassiveButtonManager>.Instance.controller;
			Vector2 vector3 = controller.DragPosition - controller.DragStartPosition;
			zero = Background.transform.localPosition;
			zero.x = Mathf.Clamp(zero.x + vector3.x, -6f, 6f);
			zero.y = Mathf.Clamp(zero.y + vector3.y, -7f, 7f);
			Background.transform.localPosition = zero;
			controller.ResetDragPosition();
		}
		if (Reticle.IsTouching(TargetItem))
		{
			if (blinky == null)
			{
				blinky = StartCoroutine(CoBlinky());
			}
		}
		else if (blinky != null)
		{
			StopCoroutine(blinky);
			blinky = null;
			ReticleImage.color = Color.white;
		}
	}

	private IEnumerator CoBlinky()
	{
		int i = 0;
		while (i < 3)
		{
			ReticleImage.color = Color.green;
			yield return Effects.Wait(0.1f);
			ReticleImage.color = Color.white;
			yield return Effects.Wait(0.2f);
			int num = i + 1;
			i = num;
		}
		blinky = null;
		ReticleImage.color = Color.green;
		MyNormTask.NextStep();
		yield return CoStartClose();
	}

	public void Grab()
	{
		grabbed = !grabbed;
	}

	public TelescopeGame()
	{ }
}
