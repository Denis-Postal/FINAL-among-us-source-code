using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Metrics
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct BeginPlayerSessionOptionsInternal : ISettable<BeginPlayerSessionOptions>, IDisposable
	{
		private int m_ApiVersion;

		private BeginPlayerSessionOptionsAccountIdInternal m_AccountId;

		private IntPtr m_DisplayName;

		private UserControllerType m_ControllerType;

		private IntPtr m_ServerIp;

		private IntPtr m_GameSessionId;

		public BeginPlayerSessionOptionsAccountId AccountId
		{
			set
			{
			}
		}

		public Utf8String DisplayName
		{
			set
			{
			}
		}

		public UserControllerType ControllerType
		{
			set
			{
			}
		}

		public Utf8String ServerIp
		{
			set
			{
			}
		}

		public Utf8String GameSessionId
		{
			set
			{
			}
		}

		public void Set(ref BeginPlayerSessionOptions other)
		{
		}

		public void Set(ref BeginPlayerSessionOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
