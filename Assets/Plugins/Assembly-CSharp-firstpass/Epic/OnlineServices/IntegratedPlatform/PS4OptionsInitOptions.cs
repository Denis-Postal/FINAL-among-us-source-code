using System;

namespace Epic.OnlineServices.IntegratedPlatform
{
	public struct PS4OptionsInitOptions
	{
		public IntPtr ManagedBySdkClientData { get; set; }

		public PS4ManagedBySdkNotificationMessagesCallback ManagedBySdkCallback { get; set; }

		public bool IsPsPlusRequired { get; set; }

		internal void Set(ref PS4OptionsInitOptionsInternal other)
		{
		}
	}
}
