using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct IOSCredentialsSystemAuthCredentialsOptionsInternal : IGettable<IOSCredentialsSystemAuthCredentialsOptions>, ISettable<IOSCredentialsSystemAuthCredentialsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PresentationContextProviding;

		private IntPtr m_CreateBackgroundSnapshotView;

		private IntPtr m_CreateBackgroundSnapshotViewContext;

		private static IOSCreateBackgroundSnapshotViewInternal s_CreateBackgroundSnapshotView;

		public IntPtr PresentationContextProviding
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public static IOSCreateBackgroundSnapshotViewInternal CreateBackgroundSnapshotView => null;

		public IntPtr CreateBackgroundSnapshotViewContext
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public void Set(ref IOSCredentialsSystemAuthCredentialsOptions other)
		{
		}

		public void Set(ref IOSCredentialsSystemAuthCredentialsOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out IOSCredentialsSystemAuthCredentialsOptions output)
		{
			output = default;
		}
	}
}
