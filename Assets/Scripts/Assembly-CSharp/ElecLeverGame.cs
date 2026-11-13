using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class ElecLeverGame : Minigame
{
	private sealed class _003CFinishUp_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		public ElecLeverGame _003C_003E4__this;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CFinishUp_003Ed__16(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			// rebuilt from decomp9 (TODO(verify): handle/bars reset arithmetic inferred)
			ElecLeverGame self = _003C_003E4__this;
			switch (_003C_003E1__state)
			{
			case 0:
			{
				_003C_003E1__state = -1;
				NormalPlayerTask task = self.MyNormTask;
				if (task.Data[task.taskStep] != self.ConsoleId)
				{
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(self.incorrectSound, loop: false);
					}
					self.SetLights(on: false);
					_003C_003E2__current = Effects.Wait(0.1f);
					_003C_003E1__state = 2;
					return true;
				}
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(self.correctSound, loop: false);
				}
				self.Lights[task.taskStep].sprite = self.LightOn;
				task.NextStep();
				_003C_003E2__current = Effects.Wait(0.25f);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			case 4:
				_003C_003E1__state = -1;
				self.Close();
				return false;
			case 2:
			{
				_003C_003E1__state = -1;
				Vector3 hs = self.Handle.transform.localScale;
				self.Handle.transform.localScale = new Vector3(hs.x, 1f, hs.z);
				Vector3 hp = self.Handle.transform.localPosition;
				self.Handle.transform.localPosition = new Vector3(hp.x, self.HandleRange.max, hp.z);
				Vector3 bs = self.Bars.transform.localScale;
				self.Bars.transform.localScale = new Vector3(bs.x, self.HandleRange.ChangeRange(self.HandleRange.max, 0.25f, 1f), bs.z);
				self.SetLights(on: true);
				_003C_003E2__current = Effects.Wait(0.5f);
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
				_003C_003E1__state = -1;
				self.ResetLights();
				_003C_003E2__current = Effects.Wait(0.25f);
				_003C_003E1__state = 4;
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

	public Sprite[] NumberIcons;

	public SpriteRenderer NumberImage;

	public SpriteRenderer[] Lights;

	public Sprite LightOn;

	public Sprite LightOff;

	public Collider2D Handle;

	public SpriteRenderer Bars;

	private FloatRange HandleRange= new FloatRange(-2.75f, 0.87f);

	private bool finished;

	private Controller controller= new Controller();

	public AudioClip correctSound;

	public AudioClip incorrectSound;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		NumberImage.sprite = NumberIcons[MyNormTask.Data.IndexOf((byte b) => b == ConsoleId)];
		ResetLights();
		SetupInput(disableCursor: true);
	}

	private void ResetLights()
	{
		int taskStep = MyNormTask.TaskStep;
		for (int i = 0; i < Lights.Length; i++)
		{
			Lights[i].sprite = ((i < taskStep) ? LightOn : LightOff);
		}
	}

	private void SetLights(bool on)
	{
		_ = MyNormTask.Data[0];
		for (int i = 0; i < Lights.Length; i++)
		{
			Lights[i].sprite = (on ? LightOn : LightOff);
		}
	}

	private void SetHandle(float y)
	{
		Vector3 hp = Handle.transform.localPosition;
		Handle.transform.localPosition = new Vector3(hp.x, y, hp.z);
		Vector3 bs = Bars.transform.localScale;
		Bars.transform.localScale = new Vector3(bs.x, HandleRange.ChangeRange(y, 0.25f, 1f), bs.z);
	}

	private void SetHandleFlip(float sign)
	{
		Vector3 hs = Handle.transform.localScale;
		Handle.transform.localScale = new Vector3(hs.x, sign, hs.z);
	}

	private void Update()
	{
		if (amClosing != CloseState.None)
		{
			return;
		}
		controller.Update();
		Vector3 localPosition = Handle.transform.localPosition;
		float num = HandleRange.ReverseLerp(localPosition.y);
		if (!finished)
		{
			if (Controller.currentTouchType == Controller.TouchType.Joystick)
			{
				Player player = ReInput.players.GetPlayer(0);
				float axisRaw = player.GetAxisRaw(14);
				float axisRaw2 = player.GetAxisRaw(17);
				axisRaw = Mathf.Clamp01(0f - axisRaw);
				axisRaw2 = Mathf.Clamp01(0f - axisRaw2);
				Vector2 vector = 0.15f * new Vector2((axisRaw > 0.7f) ? axisRaw : 0f, (axisRaw2 > 0.7f) ? axisRaw2 : 0f);
				int num2 = 0;
				if (axisRaw > 0.7f)
				{
					num2++;
				}
				if (axisRaw2 > 0.7f)
				{
					num2++;
				}
				switch (num2)
				{
				case 0:
				{
					Vector3 localScale4 = Bars.transform.localScale;
					localScale4.y = 1f;
					Handle.transform.localScale = localScale4;
					localPosition.y = Mathf.Lerp(localPosition.y, HandleRange.max, num + Time.deltaTime * 15f);
					break;
				}
				case 1:
				{
					float num4 = Mathf.Max(axisRaw, axisRaw2);
					if (num > 0.9f)
					{
						num -= num4 * Time.deltaTime;
						if (num < 0.9f)
						{
							num = 0.9f;
						}
						else
						{
							VibrationManager.Vibrate(vector.x, vector.y, 1E-05f);
						}
						localPosition.y = HandleRange.Lerp(num);
						Handle.transform.localPosition = localPosition;
						Vector3 localScale3 = Bars.transform.localScale;
						localScale3.y = HandleRange.ChangeRange(localPosition.y, 0.25f, 1f);
						Handle.transform.localScale = localScale3;
					}
					break;
				}
				case 2:
				{
					float num3 = (axisRaw + axisRaw2) * 0.5f;
					if (num > 0.7f)
					{
						num -= num3 * Time.deltaTime;
						localPosition.y = HandleRange.Lerp(num);
						VibrationManager.Vibrate(vector.x, vector.y, 1E-05f);
						Handle.transform.localPosition = localPosition;
						Vector3 localScale = Bars.transform.localScale;
						localScale.y = HandleRange.ChangeRange(localPosition.y, 0.25f, 1f);
						Handle.transform.localScale = localScale;
					}
					else
					{
						Vector3 localScale2 = Bars.transform.localScale;
						localScale2.y = -1f;
						Handle.transform.localScale = localScale2;
						VibrationManager.Vibrate(0.5f, 0.5f, 0.2f, VibrationManager.VibrationFalloff.Linear);
						localPosition.y = HandleRange.min;
						finished = true;
						StartCoroutine(FinishUp());
					}
					break;
				}
				}
			}
			else
			{
				switch (controller.CheckDrag(Handle))
				{
				case DragState.Dragging:
					if (num > 0.7f)
					{
						Vector2 vector2 = controller.DragPosition - (Vector2)transform.position;
						float num5 = HandleRange.ReverseLerp(HandleRange.Clamp(vector2.y));
						localPosition.y = HandleRange.Lerp(num5 / 2f + 0.5f);
						Handle.transform.localPosition = localPosition;
						Vector3 localScale6 = Bars.transform.localScale;
						localScale6.y = HandleRange.ChangeRange(localPosition.y, 0.25f, 1f);
						Handle.transform.localScale = localScale6;
					}
					else
					{
						Vector3 localScale7 = Bars.transform.localScale;
						localScale7.y = -1f;
						Handle.transform.localScale = localScale7;
						localPosition.y = HandleRange.min;
					}
					break;
				case DragState.NoTouch:
					if (num > 0.7f)
					{
						Vector3 localScale5 = Bars.transform.localScale;
						localScale5.y = 1f;
						Handle.transform.localScale = localScale5;
						localPosition.y = Mathf.Lerp(localPosition.y, HandleRange.max, num + Time.deltaTime * 15f);
					}
					else
					{
						finished = true;
						StartCoroutine(FinishUp());
					}
					break;
				}
			}
		}
		Handle.transform.localPosition = localPosition;
		Vector3 localScale8 = Bars.transform.localScale;
		localScale8.y = HandleRange.ChangeRange(localPosition.y, -1f, 1f);
		Bars.transform.localScale = localScale8;
	}

	private IEnumerator FinishUp()
	{
		if (MyNormTask.Data[MyNormTask.taskStep] == ConsoleId)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(correctSound, loop: false);
			}
			Lights[MyNormTask.taskStep].sprite = LightOn;
			MyNormTask?.NextStep();
			yield return Effects.Wait(0.25f);
			Close();
			yield break;
		}
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(incorrectSound, loop: false);
		}
		SetLights(on: false);
		yield return Effects.Wait(0.1f);
		Vector3 localScale = Bars.transform.localScale;
		localScale.y = 1f;
		Handle.transform.localScale = localScale;
		Vector3 localPosition = Handle.transform.localPosition;
		localPosition.y = HandleRange.max;
		Handle.transform.localPosition = localPosition;
		Vector3 localScale2 = Bars.transform.localScale;
		localScale2.y = HandleRange.ChangeRange(localPosition.y, -1f, 1f);
		Bars.transform.localScale = localScale2;
		SetLights(on: true);
		yield return Effects.Wait(0.5f);
		ResetLights();
		yield return Effects.Wait(0.25f);
		Close();
	}

	public ElecLeverGame()
	{ }

	private bool _003CBegin_003Eb__12_0(byte b)
	{
		//IL_0017: Expected Obj, but got I4
		int consoleId = ConsoleId;
		object obj = b - consoleId;
		return obj == null;
	}
}
