using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 72)]
	internal struct InitializeThreadAffinityInternal : IGettable<InitializeThreadAffinity>, ISettable<InitializeThreadAffinity>, IDisposable
	{
		private int m_ApiVersion;

		private ulong m_NetworkWork;

		private ulong m_StorageIo;

		private ulong m_WebSocketIo;

		private ulong m_P2PIo;

		private ulong m_HttpRequestIo;

		private ulong m_RTCIo;

		private ulong m_EmbeddedOverlayMainThread;

		private ulong m_EmbeddedOverlayWorkerThreads;

		public ulong NetworkWork
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public ulong StorageIo
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public ulong WebSocketIo
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public ulong P2PIo
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public ulong HttpRequestIo
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public ulong RTCIo
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public ulong EmbeddedOverlayMainThread
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public ulong EmbeddedOverlayWorkerThreads
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public void Set(ref InitializeThreadAffinity other)
		{
		}

		public void Set(ref InitializeThreadAffinity? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out InitializeThreadAffinity output)
		{
			output = default;
		}
	}
}
