using System;

namespace Epic.OnlineServices
{
	internal class CachedArrayAllocationException : AllocationException
	{
		public CachedArrayAllocationException(IntPtr address, int foundLength, int expectedLength)
			: base(null)
		{
		}
	}
}
