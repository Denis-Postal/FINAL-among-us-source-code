using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct ModInfoInternal : IGettable<ModInfo>, ISettable<ModInfo>, IDisposable
	{
		private int m_ApiVersion;

		private int m_ModsCount;

		private IntPtr m_Mods;

		private ModEnumerationType m_Type;

		public ModIdentifier[] Mods
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ModEnumerationType Type
		{
			get
			{
				return ModEnumerationType.Installed;
			}
			set
			{
			}
		}

		public void Set(ref ModInfo other)
		{
		}

		public void Set(ref ModInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out ModInfo output)
		{
			output = default;
		}
	}
}
