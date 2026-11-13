using System;
using System.Collections.Generic;
using Cpp2ILInjected;

namespace Innersloth.Assets
{
	public class CallbackGroup
	{
		public List<Action> callbacks= new List<Action>();

		public void AddCallback(Action callback)
		{
			if (callback != null)
			{
				callbacks.Add(callback);
			}
		}

		public void Call()
		{
			foreach (Action callback in callbacks)
			{
				callback();
			}
		}

		public void Clear()
		{
			// TODO(verify): reimplemented (decomp stub)
			callbacks?.Clear();
		}

		public void CallAndClear()
		{
			// TODO(verify): reimplemented (decomp stub)
			Call();
			Clear();
		}

		public void Destroy()
		{
			callbacks = null;
		}

		public CallbackGroup()
		{ }
	}
}
