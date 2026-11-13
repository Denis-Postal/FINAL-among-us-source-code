using System;

namespace Epic.OnlineServices
{
	internal class CachedTypeAllocationException : AllocationException
	{
		public CachedTypeAllocationException(IntPtr address, Type foundType, Type expectedType)
			: base(null)
		{
		}
	}
}
