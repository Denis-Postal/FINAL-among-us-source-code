using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Metrics
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct EndPlayerSessionOptionsInternal : ISettable<EndPlayerSessionOptions>, IDisposable
	{
		private int m_ApiVersion;

		private EndPlayerSessionOptionsAccountIdInternal m_AccountId;

		public EndPlayerSessionOptionsAccountId AccountId
		{
			set
			{
			}
		}

		public void Set(ref EndPlayerSessionOptions other)
		{
		}

		public void Set(ref EndPlayerSessionOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
