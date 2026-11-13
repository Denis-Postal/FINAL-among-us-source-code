using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cpp2ILInjected;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class WaterPlantsGame : Minigame
{
	private sealed class _003CCoGrowPlant_003Ed__25 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		public WaterPlantsGame _003C_003E4__this;

		public int num;

		private SpriteRenderer _003Cplant_003E5__2;

		private float _003Ctimer_003E5__3;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CCoGrowPlant_003Ed__25(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private unsafe bool MoveNext()
		{
			// real iterator rebuilt from the 2019 baseline CoGrowPlant (TODO(verify) constants)
			WaterPlantsGame self = _003C_003E4__this;
			switch (_003C_003E1__state)
			{
			case 0:
				_003C_003E1__state = -1;
				_003Cplant_003E5__2 = self.Plants[num];
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(Extensions.Random(self.PlantGrowSounds), loop: false).pitch = FloatRange.Next(0.9f, 1.1f);
				}
				_003Ctimer_003E5__3 = 0f;
				_003C_003E1__state = 1; goto case 1;
			case 1:
				if (_003C_003E1__state == 1 && _003Ctimer_003E5__3 < 1f)
				{
					float t = _003Ctimer_003E5__3 / 1f;
					_003Cplant_003E5__2.material.SetFloat("_Desat", 1f - t);
					_003Cplant_003E5__2.transform.localScale = new Vector3(0.8f, Mathf.Lerp(0.8f, 1.1f, t), 1f);
					_003Ctimer_003E5__3 += Time.deltaTime;
					_003C_003E2__current = null;
					return true;
				}
				_003Cplant_003E5__2.material.SetFloat("_Desat", 0f);
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(Extensions.Random(self.PlantFinishedSounds), loop: false).pitch = FloatRange.Next(0.9f, 1.1f);
				}
				_003Ctimer_003E5__3 = 0f;
				_003C_003E1__state = 2; goto case 2;
			case 2:
				if (_003Ctimer_003E5__3 < 0.1f)
				{
					float t = _003Ctimer_003E5__3 / 0.1f;
					_003Cplant_003E5__2.transform.localScale = new Vector3(Mathf.Lerp(0.8f, 1.1f, t), Mathf.Lerp(1.1f, 0.95f, t), 1f);
					_003Ctimer_003E5__3 += Time.deltaTime;
					_003C_003E2__current = null;
					return true;
				}
				_003Ctimer_003E5__3 = 0f;
				_003C_003E1__state = 3; goto case 3;
			case 3:
				if (_003Ctimer_003E5__3 < 0.1f)
				{
					float t = _003Ctimer_003E5__3 / 0.1f;
					_003Cplant_003E5__2.transform.localScale = new Vector3(Mathf.Lerp(1.1f, 1f, t), Mathf.Lerp(0.95f, 1f, t), 1f);
					_003Ctimer_003E5__3 += Time.deltaTime;
					_003C_003E2__current = null;
					return true;
				}
				_003C_003E1__state = -1;
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

	private sealed class _003CCoPickWaterCan_003Ed__23 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public WaterPlantsGame _003C_003E4__this;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CCoPickWaterCan_003Ed__23(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private unsafe bool MoveNext()
		{
			// real iterator rebuilt from the 2019 baseline CoPickWaterCan + decomp9 (TextMeshPro FloatText)
			WaterPlantsGame self = _003C_003E4__this;
			switch (_003C_003E1__state)
			{
			case 0:
				_003C_003E1__state = -1;
				self.FloatText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.WaterPlantsGetCan, Array.Empty<object>());
				self.FloatText.gameObject.SetActive(value: true);
				_003C_003E2__current = Effects.All(new IEnumerator[] { Effects.ColorFade(self.WaterCan.GetComponent<SpriteRenderer>(), Color.white, Palette.ClearWhite, 0.25f), Effects.Slide2D(self.FloatText.transform, self.WaterCan.transform.localPosition + new Vector3(0f, 0.1f, 0f), self.WaterCan.transform.localPosition + new Vector3(0f, 0.5f, 0f)), Effects.ColorFade(self.FloatText, Color.white, Palette.ClearWhite, 0.75f) });
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = 2;
				_003C_003E2__current = self.CoStartClose();
				return true;
			case 2:
				_003C_003E1__state = -1;
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

	public GameObject stage1;

	public GameObject stage2;

	public AudioClip CanGrabSound;

	public PassiveButton WaterCan;

	public SpriteRenderer[] Plants;

	public AudioClip WaterPlantSound;

	public AudioClip[] PlantGrowSounds;

	public AudioClip[] PlantFinishedSounds;

	public TextMeshPro FloatText;

	public Transform[] Locations;

	public Transform selectorObject;

	public GameObject grabCanSubObject;

	public GameObject holdingCanSubObject;

	public GameObject waterPlantsSubObject;

	private Controller c= new Controller();

	public SpriteRenderer[] playerHandObjects;

	public ParticleSystem waterParticles;

	public GameObject greenDot;

	private bool Watered(int x)
	{ /* spliced-from-2019 */
		return MyNormTask.Data[x] != 0;
	}

	private void Watered(int x, bool b)
	{
		MyNormTask.Data[x] = (byte)(b ? 1u : 0u);
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		if (MyNormTask.taskStep == 0)
		{
			WaterCan.transform.localPosition = Locations.Random().localPosition;
			WaterCan.GetComponent<SpriteRenderer>().flipX = BoolRange.Next();
			stage1.gameObject.SetActive(value: true);
			stage2.gameObject.SetActive(value: false);
			SetupInput(disableCursor: false);
			grabCanSubObject.SetActive(value: true);
			holdingCanSubObject.SetActive(value: false);
			waterPlantsSubObject.SetActive(value: false);
			SpriteRenderer[] array = playerHandObjects;
			foreach (SpriteRenderer playerMaterialColors in array)
			{
				PlayerControl.LocalPlayer.SetPlayerMaterialColors(playerMaterialColors);
			}
			greenDot.SetActive(value: false);
		}
		else
		{
			if (MyNormTask.taskStep != 1)
			{
				return;
			}
			stage1.gameObject.SetActive(value: false);
			stage2.gameObject.SetActive(value: true);
			for (int j = 0; j < Plants.Length; j++)
			{
				if (Watered(j))
				{
					SpriteRenderer obj = Plants[j];
					obj.material.SetFloat("_Desat", 0f);
					obj.transform.localScale = Vector3.one;
				}
			}
			SetupInput(disableCursor: false);
			grabCanSubObject.SetActive(value: false);
			holdingCanSubObject.SetActive(value: false);
			waterPlantsSubObject.SetActive(value: true);
			SpriteRenderer[] array = playerHandObjects;
			foreach (SpriteRenderer playerMaterialColors2 in array)
			{
				PlayerControl.LocalPlayer.SetPlayerMaterialColors(playerMaterialColors2);
			}
			greenDot.SetActive(value: true);
		}
	}

	private void Update()
	{
		c.Update();
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			ReInput.players.GetPlayer(0).GetButton(11);
			Vector3 position = selectorObject.transform.position;
			position.x = VirtualCursor.currentPosition.x;
			position.y = VirtualCursor.currentPosition.y;
			selectorObject.transform.position = position;
		}
	}

	public void PickWaterCan()
	{
		grabCanSubObject.SetActive(value: false);
		holdingCanSubObject.SetActive(value: true);
		WaterCan.enabled = false;
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(CanGrabSound, loop: false);
		}
		MyNormTask.NextStep();
		StartCoroutine(CoPickWaterCan());
	}

	private IEnumerator CoPickWaterCan()
	{
		FloatText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.WaterPlantsGetCan);
		FloatText.gameObject.SetActive(value: true);
		yield return Effects.All(Effects.ColorFade(WaterCan.GetComponent<SpriteRenderer>(), Color.white, Palette.ClearWhite, 0.25f), Effects.Slide2D(FloatText.transform, WaterCan.transform.localPosition + new Vector3(0f, 0.1f, 0f), WaterCan.transform.localPosition + new Vector3(0f, 0.5f, 0f)), Effects.ColorFade(FloatText, Color.white, Palette.ClearWhite, 0.75f));
		yield return CoStartClose();
	}

	public void WaterPlant(int num)
	{
		if (!Watered(num))
		{
			Watered(num, b: true);
			if (Enumerable.Range(0, 4).All(Watered))
			{
				MyNormTask.NextStep();
				StartCoroutine(CoStartClose());
			}
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(WaterPlantSound, loop: false);
			}
			StartCoroutine(CoGrowPlant(num));
			if (Controller.currentTouchType == Controller.TouchType.Joystick)
			{
				waterParticles.Play();
			}
		}
	}

	private IEnumerator CoGrowPlant(int num)
	{
		SpriteRenderer plant = Plants[num];
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(PlantGrowSounds.Random(), loop: false).pitch = FloatRange.Next(0.9f, 1.1f);
		}
		for (float timer = 0f; timer < 1f; timer += Time.deltaTime)
		{
			float num2 = timer / 1f;
			plant.material.SetFloat("_Desat", (1f - num2) * 0.8f);
			plant.transform.localScale = new Vector3(0.8f, Mathf.Lerp(0.8f, 1.1f, num2), 1f);
			yield return null;
		}
		plant.material.SetFloat("_Desat", 0f);
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(PlantFinishedSounds.Random(), loop: false).pitch = FloatRange.Next(0.9f, 1.1f);
		}
		for (float timer = 0f; timer < 0.1f; timer += Time.deltaTime)
		{
			float t = timer / 0.1f;
			plant.transform.localScale = new Vector3(Mathf.Lerp(0.8f, 1.1f, t), Mathf.Lerp(1.1f, 0.95f, t), 1f);
			yield return null;
		}
		for (float timer = 0f; timer < 0.1f; timer += Time.deltaTime)
		{
			float t2 = timer / 0.1f;
			plant.transform.localScale = new Vector3(Mathf.Lerp(1.1f, 1f, t2), Mathf.Lerp(0.95f, 1f, t2), 1f);
			yield return null;
		}
	}

	public WaterPlantsGame()
	{ }
}
