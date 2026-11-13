using System;

namespace Epic.OnlineServices.Platform
{
	public struct XSXOptionsSystemSpecificOptions
	{
		public IntPtr OverlayGraphicsDevice { get; set; }

		internal void Set(ref XSXOptionsSystemSpecificOptionsInternal other)
		{
		}
	}
}
