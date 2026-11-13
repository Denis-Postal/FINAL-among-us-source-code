using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct PS5OptionsInitOptionsInternal : IGettable<PS5OptionsInitOptions>, ISettable<PS5OptionsInitOptions>, IDisposable
	{
		private int m_ApiVersion;

		private int m_IsPsPlusRequired;

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

		public void Set(ref PS5OptionsInitOptions other)
		{
		}

		public void Set(ref PS5OptionsInitOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS5OptionsInitOptions output)
		{
			output = default;
		}
	}
}
