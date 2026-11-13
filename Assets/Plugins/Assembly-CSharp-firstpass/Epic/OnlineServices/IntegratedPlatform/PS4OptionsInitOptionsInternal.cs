using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct PS4OptionsInitOptionsInternal : IGettable<PS4OptionsInitOptions>, ISettable<PS4OptionsInitOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ManagedBySdkClientData;

		private IntPtr m_ManagedBySdkCallback;

		private int m_IsPsPlusRequired;

		private static PS4ManagedBySdkNotificationMessagesCallbackInternal s_ManagedBySdkCallback;

		public IntPtr ManagedBySdkClientData
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public static PS4ManagedBySdkNotificationMessagesCallbackInternal ManagedBySdkCallback => null;

		public bool IsPsPlusRequired
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public void Set(ref PS4OptionsInitOptions other)
		{
		}

		public void Set(ref PS4OptionsInitOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS4OptionsInitOptions output)
		{
			output = default;
		}
	}
}
