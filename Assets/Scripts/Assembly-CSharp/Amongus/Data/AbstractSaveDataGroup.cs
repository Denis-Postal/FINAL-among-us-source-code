using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;

namespace AmongUs.Data
{
	public abstract class AbstractSaveDataGroup : ISaveDataGroupControl
	{
		private Action m_OnChangedEvent;

		public event Action OnChangedEvent
		{
			add
			{
				m_OnChangedEvent = (Action)Delegate.Combine(m_OnChangedEvent, value);
			}
			remove
			{
				m_OnChangedEvent = (Action)Delegate.Remove(m_OnChangedEvent, value);
			}
		}

		protected unsafe virtual void SetValue<T>(ref T field, T value, Action onChangedCallback)
		{
			if (!field.Equals(value))
			{
				field = value;
				onChangedCallback?.Invoke();
				HandleChange();
			}
		}

		protected virtual void HandleChange()
		{
			// TODO(verify): reimplemented (decomp stub)
			m_OnChangedEvent?.Invoke();
		}

		protected virtual void OnSaveStart()
		{
		}

		protected virtual void OnSaveComplete()
		{
		}

		protected virtual void OnLoadStart()
		{
		}

		protected virtual void OnLoadComplete()
		{
		}

		void ISaveDataGroupControl.SaveStart()
		{
			// TODO(verify): reimplemented (decomp stub)
			OnSaveStart();
		}

		void ISaveDataGroupControl.SaveComplete()
		{
			// TODO(verify): reimplemented (decomp stub)
			OnSaveComplete();
		}

		void ISaveDataGroupControl.LoadStart()
		{
			// TODO(verify): reimplemented (decomp stub)
			OnLoadStart();
		}

		void ISaveDataGroupControl.LoadComplete()
		{
			// TODO(verify): reimplemented (decomp stub)
			OnLoadComplete();
		}
	}
}
