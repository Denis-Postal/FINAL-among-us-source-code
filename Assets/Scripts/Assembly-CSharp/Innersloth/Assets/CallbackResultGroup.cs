using System;
using System.Collections.Generic;
using Cpp2ILInjected;

namespace Innersloth.Assets
{
	public class CallbackResultGroup
	{
		public CallbackGroup onError= new CallbackGroup();

		public CallbackGroup onSuccess= new CallbackGroup();

		public CallbackGroup onFinished= new CallbackGroup();

		public void AddOnSuccess(Action callback)
		{
			onSuccess.AddCallback(callback);
		}

		public void AddOnError(Action callback)
		{
			onError.AddCallback(callback);
		}

		public void AddOnFinished(Action callback)
		{
			onFinished.AddCallback(callback);
		}

		public void CallSuccessAndClear()
		{
			onSuccess.CallAndClear();
			onError.Clear();
			onFinished.CallAndClear();
		}

		public void CallErrorAndClear()
		{
			onError.CallAndClear();
			onSuccess.Clear();
			onFinished.CallAndClear();
		}

		public void CallFinishedAndClear()
		{
			onSuccess.Clear();
			onError.Clear();
			onFinished.CallAndClear();
		}

		public void Clear()
		{
			onSuccess?.Clear();
			onError?.Clear();
			onFinished?.Clear();
		}

		public void Destroy()
		{
			onSuccess?.Destroy();
			onSuccess = null;
			onError?.Destroy();
			onError = null;
			onFinished?.Destroy();
			onFinished = null;
		}

		public CallbackResultGroup()
		{ }
	}
}
