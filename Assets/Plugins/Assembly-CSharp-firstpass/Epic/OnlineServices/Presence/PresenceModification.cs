using System;

namespace Epic.OnlineServices.Presence
{
	public sealed class PresenceModification : Handle
	{
		public const int PresencemodificationDatarecordidApiLatest = 1;

		public const int PresencemodificationDeletedataApiLatest = 1;

		public const int PresencemodificationJoininfoMaxLength = 255;

		public const int PresencemodificationSetdataApiLatest = 1;

		public const int PresencemodificationSetjoininfoApiLatest = 1;

		public const int PresencemodificationSetrawrichtextApiLatest = 1;

		public const int PresencemodificationSetstatusApiLatest = 1;

		public PresenceModification()
		{
		}

		public PresenceModification(IntPtr innerHandle)
		{
		}

		public Result DeleteData(ref PresenceModificationDeleteDataOptions options)
		{
			return Result.Success;
		}

		public void Release()
		{
		}

		public Result SetData(ref PresenceModificationSetDataOptions options)
		{
			return Result.Success;
		}

		public Result SetJoinInfo(ref PresenceModificationSetJoinInfoOptions options)
		{
			return Result.Success;
		}

		public Result SetRawRichText(ref PresenceModificationSetRawRichTextOptions options)
		{
			return Result.Success;
		}

		public Result SetStatus(ref PresenceModificationSetStatusOptions options)
		{
			return Result.Success;
		}
	}
}
