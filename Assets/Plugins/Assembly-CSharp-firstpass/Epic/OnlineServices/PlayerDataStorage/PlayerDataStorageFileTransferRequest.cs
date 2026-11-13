using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	public sealed class PlayerDataStorageFileTransferRequest : Handle
	{
		public PlayerDataStorageFileTransferRequest()
		{
		}

		public PlayerDataStorageFileTransferRequest(IntPtr innerHandle)
		{
		}

		public Result CancelRequest()
		{
			return Result.Success;
		}

		public Result GetFileRequestState()
		{
			return Result.Success;
		}

		public Result GetFilename(out Utf8String outStringBuffer)
		{
			outStringBuffer = null;
			return Result.Success;
		}

		public void Release()
		{
		}
	}
}
