using System;

namespace Epic.OnlineServices.TitleStorage
{
	public sealed class TitleStorageFileTransferRequest : Handle
	{
		public TitleStorageFileTransferRequest()
		{
		}

		public TitleStorageFileTransferRequest(IntPtr innerHandle)
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
