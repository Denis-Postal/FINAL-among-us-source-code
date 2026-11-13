using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct GetAchievementDefinitionCountOptionsInternal : ISettable<GetAchievementDefinitionCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref GetAchievementDefinitionCountOptions other)
		{
		}

		public void Set(ref GetAchievementDefinitionCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
