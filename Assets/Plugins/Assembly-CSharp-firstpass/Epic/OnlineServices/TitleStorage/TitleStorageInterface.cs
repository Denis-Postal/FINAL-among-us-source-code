using System;

namespace Epic.OnlineServices.TitleStorage
{
	public sealed class TitleStorageInterface : Handle
	{
		public const int CopyfilemetadataatindexApiLatest = 1;

		public const int CopyfilemetadataatindexoptionsApiLatest = 1;

		public const int CopyfilemetadatabyfilenameApiLatest = 1;

		public const int CopyfilemetadatabyfilenameoptionsApiLatest = 1;

		public const int DeletecacheApiLatest = 1;

		public const int DeletecacheoptionsApiLatest = 1;

		public const int FilemetadataApiLatest = 2;

		public const int FilenameMaxLengthBytes = 64;

		public const int GetfilemetadatacountApiLatest = 1;

		public const int GetfilemetadatacountoptionsApiLatest = 1;

		public const int QueryfileApiLatest = 1;

		public const int QueryfilelistApiLatest = 1;

		public const int QueryfilelistoptionsApiLatest = 1;

		public const int QueryfileoptionsApiLatest = 1;

		public const int ReadfileApiLatest = 2;

		public const int ReadfileoptionsApiLatest = 2;

		public TitleStorageInterface()
		{
		}

		public TitleStorageInterface(IntPtr innerHandle)
		{
		}

		public Result CopyFileMetadataAtIndex(ref CopyFileMetadataAtIndexOptions options, out FileMetadata? outMetadata)
		{
			outMetadata = null;
			return Result.Success;
		}

		public Result CopyFileMetadataByFilename(ref CopyFileMetadataByFilenameOptions options, out FileMetadata? outMetadata)
		{
			outMetadata = null;
			return Result.Success;
		}

		public Result DeleteCache(ref DeleteCacheOptions options, object clientData, OnDeleteCacheCompleteCallback completionCallback)
		{
			return Result.Success;
		}

		public uint GetFileMetadataCount(ref GetFileMetadataCountOptions options)
		{
			return 0u;
		}

		public void QueryFile(ref QueryFileOptions options, object clientData, OnQueryFileCompleteCallback completionCallback)
		{
		}

		public void QueryFileList(ref QueryFileListOptions options, object clientData, OnQueryFileListCompleteCallback completionCallback)
		{
		}

		public TitleStorageFileTransferRequest ReadFile(ref ReadFileOptions options, object clientData, OnReadFileCompleteCallback completionCallback)
		{
			return null;
		}

		[MonoPInvokeCallback(typeof(OnDeleteCacheCompleteCallbackInternal))]
		internal static void OnDeleteCacheCompleteCallbackInternalImplementation(ref DeleteCacheCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnFileTransferProgressCallbackInternal))]
		internal static void OnFileTransferProgressCallbackInternalImplementation(ref FileTransferProgressCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryFileCompleteCallbackInternal))]
		internal static void OnQueryFileCompleteCallbackInternalImplementation(ref QueryFileCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryFileListCompleteCallbackInternal))]
		internal static void OnQueryFileListCompleteCallbackInternalImplementation(ref QueryFileListCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnReadFileCompleteCallbackInternal))]
		internal static void OnReadFileCompleteCallbackInternalImplementation(ref ReadFileCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnReadFileDataCallbackInternal))]
		internal static ReadResult OnReadFileDataCallbackInternalImplementation(ref ReadFileDataCallbackInfoInternal data)
		{
			return (ReadResult)0;
		}
	}
}
