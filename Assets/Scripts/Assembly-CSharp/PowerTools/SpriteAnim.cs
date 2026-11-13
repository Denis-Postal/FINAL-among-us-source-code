using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.UI;

namespace PowerTools
{
	[RequireComponent(typeof(Animator))]
	[DisallowMultipleComponent]
	public class SpriteAnim : SpriteAnimEventHandler
	{
		private static readonly string STATE_NAME = "a";

		private static readonly string CONTROLLER_PATH = "SpriteAnimController";

		[SerializeField]
		private AnimationClip m_defaultAnim;

		private static RuntimeAnimatorController m_sharedAnimatorController = null;

		public Animator m_animator;

		private AnimatorOverrideController m_controller;

		private SpriteAnimNodes m_nodes;

		private List<KeyValuePair<AnimationClip, AnimationClip>> m_clipPairList= new List<KeyValuePair<AnimationClip, AnimationClip>>(1);

		public AnimationClip m_currAnim;

		private float m_speed= 1f;

		private bool m_initialized;

		public bool Playing => IsPlaying();

		public bool Paused
		{
			get
			{
				return IsPaused();
			}
			set
			{
				if (value)
				{
					Pause();
				}
				else
				{
					Resume();
				}
			}
		}

		public float Speed
		{
			get
			{
				return m_speed;
			}
			set
			{
				SetSpeed(value);
			}
		}

		// TODO(verify): getter/setter were lost (cvttss2si / imul); assumed frame index = time * clip frame rate
		public int FrameTime
		{
			get
			{
				return (int)(GetTime() * 24f);
			}
			set
			{
				SetTime(value / 24);
			}
		}

		public float Time
		{
			get
			{
				return GetTime();
			}
			set
			{
				SetTime(value);
			}
		}

		public float NormalizedTime
		{
			get
			{
				return GetNormalisedTime();
			}
			set
			{
				SetNormalizedTime(value);
			}
		}

		public AnimationClip Clip => m_currAnim;

		public string ClipName
		{
			get
			{
				if (!(m_currAnim != null))
				{
					return string.Empty;
				}
				return m_currAnim.name;
			}
		}

		public unsafe void Play(AnimationClip anim = null, float speed = 1f)
		{
			if (anim == null)
			{
				anim = (m_currAnim ? m_currAnim : m_defaultAnim);
				if (anim == null)
				{
					return;
				}
			}
			if (!(m_animator == null))
			{
				if (!m_animator.enabled)
				{
					m_animator.enabled = true;
				}
				if (m_nodes != null)
				{
					m_nodes.Reset();
				}
				m_clipPairList[0] = new KeyValuePair<AnimationClip, AnimationClip>(m_clipPairList[0].Key, anim);
				m_controller.ApplyOverrides(m_clipPairList);
				m_animator.Update(0f);
				m_animator.Play(STATE_NAME, 0, 0f);
				m_speed = Mathf.Max(0f, speed);
				m_animator.speed = m_speed;
				m_currAnim = anim;
				m_animator.Update(0f);
			}
		}

		public void Stop()
		{
			m_animator.enabled = false;
		}

		public void Pause()
		{
			m_animator.speed = 0f;
		}

		public void Resume()
		{
			m_animator.speed = m_speed;
		}

		public AnimationClip GetCurrentAnimation()
		{
			return m_currAnim;
		}

		public bool IsPlaying(AnimationClip clip = null)
		
		{ // TODO(verify): from 2019 baseline
			if (clip == null || m_currAnim == clip)
			{
				return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
			}
			return false;
		}

		public bool IsPlaying(string animName)
		
		{ // TODO(verify): from 2019 baseline
			if (m_currAnim == null)
			{
				return false;
			}
			if (m_currAnim.name == animName)
			{
				return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
			}
			return false;
		}

		public bool IsPaused()
		{ /* spliced-from-2019 */
			if (m_currAnim == null)
			{
				return false;
			}
			return m_animator.speed == 0f;
		}

		public void SetSpeed(float speed)
		{
			m_speed = Mathf.Max(0f, speed);
			m_animator.speed = m_speed;
		}

		public float GetSpeed()
		{
			return m_speed;
		}

		public float GetTime()
		{
			if (m_currAnim != null)
			{
				return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime * m_currAnim.length;
			}
			return 0f;
		}

		public void SetTime(float time)
		{
			if (!(m_currAnim == null) && !(m_currAnim.length <= 0f))
			{
				SetNormalizedTime(time / m_currAnim.length);
			}
		}

		public float GetNormalisedTime()
		{
			// TODO(verify): 2019 baseline
			if (m_currAnim != null)
			{
				return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
			}
			return 0f;
		}

		public void SetNormalizedTime(float ratio)
		{
			if (!(m_currAnim == null))
			{
				m_animator.Play(STATE_NAME, 0, m_currAnim.isLooping ? Mathf.Repeat(ratio, 1f) : Mathf.Clamp01(ratio));
			}
		}

		public void Initialize()
		{
			if (!m_initialized)
			{
				m_controller = new AnimatorOverrideController();
				if (m_sharedAnimatorController == null)
				{
					m_sharedAnimatorController = Resources.Load<RuntimeAnimatorController>(CONTROLLER_PATH);
				}
				m_controller.runtimeAnimatorController = m_sharedAnimatorController;
				m_animator = GetComponent<Animator>();
				m_animator.runtimeAnimatorController = m_controller;
				m_controller.GetOverrides(m_clipPairList);
				m_nodes = GetComponent<SpriteAnimNodes>();
				m_initialized = true;
			}
		}

		private void Awake()
		{
			Initialize();
			Play(m_defaultAnim);
		}

		private void Reset()
		{
			if (GetComponent<RectTransform>() == null)
			{
				if (GetComponent<Sprite>() == null)
				{
					gameObject.AddComponent<SpriteRenderer>();
				}
			}
			else if (GetComponent<Image>() == null)
			{
				gameObject.AddComponent<Image>();
			}
		}

		public SpriteAnim()
		{ }
	}
}
