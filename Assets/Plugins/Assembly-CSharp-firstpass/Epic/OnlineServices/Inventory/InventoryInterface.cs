using System;

namespace Epic.OnlineServices.Inventory
{
	public sealed class InventoryInterface : Handle
	{
		public const int CheckpointApiLatest = 1;

		public const int CloseApiLatest = 1;

		public const int CreateApiLatest = 1;

		public const int OpenApiLatest = 1;

		public InventoryInterface()
		{
		}

		public InventoryInterface(IntPtr innerHandle)
		{
		}

		public void Checkpoint(ref CheckpointOptions options, object clientData, OnCheckpointCompleteCallback completionDelegate)
		{
		}

		public void Close(ref CloseOptions options, object clientData, OnCloseCompleteCallback completionDelegate)
		{
		}

		public void Create(ref CreateOptions options, object clientData, OnCreateCompleteCallback completionDelegate)
		{
		}

		public void Open(ref OpenOptions options, object clientData, OnOpenCompleteCallback completionDelegate)
		{
			if (completionDelegate != null)
			{
				OnOpenCompleteCallbackInfo data = new OnOpenCompleteCallbackInfo
				{
					ResultCode = Result.Success,
					ClientData = clientData
				};
				completionDelegate(ref data);
			}
		}

		[MonoPInvokeCallback(typeof(OnCheckpointCompleteCallbackInternal))]
		internal static void OnCheckpointCompleteCallbackInternalImplementation(ref OnCheckpointCompleteCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnCloseCompleteCallbackInternal))]
		internal static void OnCloseCompleteCallbackInternalImplementation(ref OnCloseCompleteCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnCreateCompleteCallbackInternal))]
		internal static void OnCreateCompleteCallbackInternalImplementation(ref OnCreateCompleteCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnOpenCompleteCallbackInternal))]
		internal static void OnOpenCompleteCallbackInternalImplementation(ref OnOpenCompleteCallbackInfoInternal data)
		{
		}
	}
}
