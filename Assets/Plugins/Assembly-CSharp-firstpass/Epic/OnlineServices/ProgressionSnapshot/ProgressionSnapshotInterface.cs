using System;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	public sealed class ProgressionSnapshotInterface : Handle
	{
		public const int AddprogressionApiLatest = 1;

		public const int BeginsnapshotApiLatest = 1;

		public const int DeletesnapshotApiLatest = 1;

		public const int EndsnapshotApiLatest = 1;

		public const int InvalidProgressionsnapshotid = 0;

		public const int SubmitsnapshotApiLatest = 1;

		public ProgressionSnapshotInterface()
		{
		}

		public ProgressionSnapshotInterface(IntPtr innerHandle)
		{
		}

		public Result AddProgression(ref AddProgressionOptions options)
		{
			return Result.Success;
		}

		public Result BeginSnapshot(ref BeginSnapshotOptions options, out uint outSnapshotId)
		{
			outSnapshotId = default;
			return Result.Success;
		}

		public void DeleteSnapshot(ref DeleteSnapshotOptions options, object clientData, OnDeleteSnapshotCallback completionDelegate)
		{
		}

		public Result EndSnapshot(ref EndSnapshotOptions options)
		{
			return Result.Success;
		}

		public void SubmitSnapshot(ref SubmitSnapshotOptions options, object clientData, OnSubmitSnapshotCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnDeleteSnapshotCallbackInternal))]
		internal static void OnDeleteSnapshotCallbackInternalImplementation(ref DeleteSnapshotCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnSubmitSnapshotCallbackInternal))]
		internal static void OnSubmitSnapshotCallbackInternalImplementation(ref SubmitSnapshotCallbackInfoInternal data)
		{
		}
	}
}
