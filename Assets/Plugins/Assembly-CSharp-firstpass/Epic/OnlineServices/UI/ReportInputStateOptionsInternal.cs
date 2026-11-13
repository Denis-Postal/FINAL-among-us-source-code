using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 52)]
	internal struct ReportInputStateOptionsInternal : ISettable<ReportInputStateOptions>, IDisposable
	{
		private int m_ApiVersion;

		private InputStateButtonFlags m_ButtonDownFlags;

		private int m_AcceptIsFaceButtonRight;

		private int m_MouseButtonDown;

		private uint m_MousePosX;

		private uint m_MousePosY;

		private uint m_GamepadIndex;

		private float m_LeftStickX;

		private float m_LeftStickY;

		private float m_RightStickX;

		private float m_RightStickY;

		private float m_LeftTrigger;

		private float m_RightTrigger;

		public InputStateButtonFlags ButtonDownFlags
		{
			set
			{
			}
		}

		public bool AcceptIsFaceButtonRight
		{
			set
			{
			}
		}

		public bool MouseButtonDown
		{
			set
			{
			}
		}

		public uint MousePosX
		{
			set
			{
			}
		}

		public uint MousePosY
		{
			set
			{
			}
		}

		public uint GamepadIndex
		{
			set
			{
			}
		}

		public float LeftStickX
		{
			set
			{
			}
		}

		public float LeftStickY
		{
			set
			{
			}
		}

		public float RightStickX
		{
			set
			{
			}
		}

		public float RightStickY
		{
			set
			{
			}
		}

		public float LeftTrigger
		{
			set
			{
			}
		}

		public float RightTrigger
		{
			set
			{
			}
		}

		public void Set(ref ReportInputStateOptions other)
		{
		}

		public void Set(ref ReportInputStateOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
