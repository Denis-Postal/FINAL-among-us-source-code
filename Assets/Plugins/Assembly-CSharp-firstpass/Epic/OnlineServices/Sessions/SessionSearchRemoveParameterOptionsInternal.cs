using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct SessionSearchRemoveParameterOptionsInternal : ISettable<SessionSearchRemoveParameterOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Key;

		private ComparisonOp m_ComparisonOp;

		public Utf8String Key
		{
			set
			{
			}
		}

		public ComparisonOp ComparisonOp
		{
			set
			{
			}
		}

		public void Set(ref SessionSearchRemoveParameterOptions other)
		{
		}

		public void Set(ref SessionSearchRemoveParameterOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
