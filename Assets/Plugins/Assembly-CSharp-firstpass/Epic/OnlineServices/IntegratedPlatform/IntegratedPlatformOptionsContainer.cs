using System;

namespace Epic.OnlineServices.IntegratedPlatform
{
	public sealed class IntegratedPlatformOptionsContainer : Handle
	{
		public const int IntegratedplatformoptionscontainerAddApiLatest = 1;

		public IntegratedPlatformOptionsContainer()
		{
		}

		public IntegratedPlatformOptionsContainer(IntPtr innerHandle)
		{
		}

		public Result Add(ref IntegratedPlatformOptionsContainerAddOptions inOptions)
		{
			return Result.Success;
		}

		public void Release()
		{
		}

		public Result Add(ref PS4IntegratedPlatformOptionsContainerAddOptions inOptions)
		{
			return Result.Success;
		}

		public Result Add(ref PS5IntegratedPlatformOptionsContainerAddOptions inOptions)
		{
			return Result.Success;
		}

		public Result Add(ref XboxOneGDKIntegratedPlatformOptionsContainerAddOptions inOptions)
		{
			return Result.Success;
		}

		public Result Add(ref XSXIntegratedPlatformOptionsContainerAddOptions inOptions)
		{
			return Result.Success;
		}
	}
}
