namespace Epic.OnlineServices.P2P
{
	public struct SocketId
	{
		public static readonly SocketId Empty;

		private const int MaxSocketNameLength = 32;

		private const int ApiVersionLength = 4;

		private const int NullTerminatorSpace = 1;

		private const int TotalSizeInBytes = 37;

		private bool m_CacheValid;

		private string m_CachedSocketName;

		internal byte[] m_AllBytes;

		internal byte[] m_SwapBuffer;

		public string SocketName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		internal void Set(ref SocketIdInternal other)
		{
		}

		internal bool PrepareForUpdate()
		{
			return false;
		}

		internal void CheckIfChanged(bool wasCacheValid)
		{
		}

		private void RebuildStringFromBuffer()
		{
		}

		private void EnsureStorage()
		{
		}

		private void CopyIdToSwapBuffer()
		{
		}
	}
}
