using System;

namespace Epic.OnlineServices
{
	internal class ExternalAllocationException : AllocationException
	{
		public ExternalAllocationException(IntPtr address, Type type)
			: base(null)
		{
		}
	}
}
