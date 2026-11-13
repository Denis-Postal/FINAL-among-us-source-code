using System;

namespace Epic.OnlineServices.Platform
{
	public struct XboxOneGDKOptionsSystemSpecificOptions
	{
		public IntPtr OverlayGraphicsDevice { get; set; }

		internal void Set(ref XboxOneGDKOptionsSystemSpecificOptionsInternal other)
		{
		}
	}
}
