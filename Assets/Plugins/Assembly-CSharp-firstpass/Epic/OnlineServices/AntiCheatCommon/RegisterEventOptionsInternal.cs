using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct RegisterEventOptionsInternal : ISettable<RegisterEventOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_EventId;

		private IntPtr m_EventName;

		private AntiCheatCommonEventType m_EventType;

		private uint m_ParamDefsCount;

		private IntPtr m_ParamDefs;

		public uint EventId
		{
			set
			{
			}
		}

		public Utf8String EventName
		{
			set
			{
			}
		}

		public AntiCheatCommonEventType EventType
		{
			set
			{
			}
		}

		public RegisterEventParamDef[] ParamDefs
		{
			set
			{
			}
		}

		public void Set(ref RegisterEventOptions other)
		{
		}

		public void Set(ref RegisterEventOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
