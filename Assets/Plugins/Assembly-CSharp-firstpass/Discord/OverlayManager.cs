using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Discord
{
	public class OverlayManager
	{
		internal struct FFIEvents
		{
			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void ToggleHandler(IntPtr ptr, bool locked);

			internal ToggleHandler OnToggle;
		}

		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void IsEnabledMethod(IntPtr methodsPtr, ref bool enabled);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void IsLockedMethod(IntPtr methodsPtr, ref bool locked);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void SetLockedCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void SetLockedMethod(IntPtr methodsPtr, bool locked, IntPtr callbackData, SetLockedCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void OpenActivityInviteCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void OpenActivityInviteMethod(IntPtr methodsPtr, ActivityActionType type, IntPtr callbackData, OpenActivityInviteCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void OpenGuildInviteCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void OpenGuildInviteMethod(IntPtr methodsPtr, string code, IntPtr callbackData, OpenGuildInviteCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void OpenVoiceSettingsCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void OpenVoiceSettingsMethod(IntPtr methodsPtr, IntPtr callbackData, OpenVoiceSettingsCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result InitDrawingDxgiMethod(IntPtr methodsPtr, IntPtr swapchain, bool useMessageForwarding);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void OnPresentMethod(IntPtr methodsPtr);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void ForwardMessageMethod(IntPtr methodsPtr, IntPtr message);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void KeyEventMethod(IntPtr methodsPtr, bool down, string keyCode, KeyVariant variant);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void CharEventMethod(IntPtr methodsPtr, string character);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void MouseButtonEventMethod(IntPtr methodsPtr, byte down, int clickCount, MouseButton which, int x, int y);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void MouseMotionEventMethod(IntPtr methodsPtr, int x, int y);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void ImeCommitTextMethod(IntPtr methodsPtr, string text);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void ImeSetCompositionMethod(IntPtr methodsPtr, string text, ref ImeUnderline underlines, int from, int to);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void ImeCancelCompositionMethod(IntPtr methodsPtr);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void SetImeCompositionRangeCallbackCallback(IntPtr ptr, int from, int to, ref Rect bounds);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void SetImeCompositionRangeCallbackMethod(IntPtr methodsPtr, IntPtr callbackData, SetImeCompositionRangeCallbackCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void SetImeSelectionBoundsCallbackCallback(IntPtr ptr, Rect anchor, Rect focus, bool isAnchorFirst);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void SetImeSelectionBoundsCallbackMethod(IntPtr methodsPtr, IntPtr callbackData, SetImeSelectionBoundsCallbackCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate bool IsPointInsideClickZoneMethod(IntPtr methodsPtr, int x, int y);

			internal IsEnabledMethod IsEnabled;

			internal IsLockedMethod IsLocked;

			internal SetLockedMethod SetLocked;

			internal OpenActivityInviteMethod OpenActivityInvite;

			internal OpenGuildInviteMethod OpenGuildInvite;

			internal OpenVoiceSettingsMethod OpenVoiceSettings;

			internal InitDrawingDxgiMethod InitDrawingDxgi;

			internal OnPresentMethod OnPresent;

			internal ForwardMessageMethod ForwardMessage;

			internal KeyEventMethod KeyEvent;

			internal CharEventMethod CharEvent;

			internal MouseButtonEventMethod MouseButtonEvent;

			internal MouseMotionEventMethod MouseMotionEvent;

			internal ImeCommitTextMethod ImeCommitText;

			internal ImeSetCompositionMethod ImeSetComposition;

			internal ImeCancelCompositionMethod ImeCancelComposition;

			internal SetImeCompositionRangeCallbackMethod SetImeCompositionRangeCallback;

			internal SetImeSelectionBoundsCallbackMethod SetImeSelectionBoundsCallback;

			internal IsPointInsideClickZoneMethod IsPointInsideClickZone;
		}

		public delegate void SetLockedHandler(Result result);

		public delegate void OpenActivityInviteHandler(Result result);

		public delegate void OpenGuildInviteHandler(Result result);

		public delegate void OpenVoiceSettingsHandler(Result result);

		public delegate void SetImeCompositionRangeCallbackHandler(int from, int to, ref Rect bounds);

		public delegate void SetImeSelectionBoundsCallbackHandler(Rect anchor, Rect focus, bool isAnchorFirst);

		public delegate void ToggleHandler(bool locked);

		private IntPtr MethodsPtr;

		private object MethodsStructure;

		[CompilerGenerated]
		private ToggleHandler m_OnToggle;

		private FFIMethods Methods => default;

		public event ToggleHandler OnToggle
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		internal OverlayManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
		{
		}

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
		}

		public bool IsEnabled()
		{
			return false;
		}

		public bool IsLocked()
		{
			return false;
		}

		[MonoPInvokeCallback]
		private static void SetLockedCallbackImpl(IntPtr ptr, Result result)
		{
		}

		public void SetLocked(bool locked, SetLockedHandler callback)
		{
		}

		[MonoPInvokeCallback]
		private static void OpenActivityInviteCallbackImpl(IntPtr ptr, Result result)
		{
		}

		public void OpenActivityInvite(ActivityActionType type, OpenActivityInviteHandler callback)
		{
		}

		[MonoPInvokeCallback]
		private static void OpenGuildInviteCallbackImpl(IntPtr ptr, Result result)
		{
		}

		public void OpenGuildInvite(string code, OpenGuildInviteHandler callback)
		{
		}

		[MonoPInvokeCallback]
		private static void OpenVoiceSettingsCallbackImpl(IntPtr ptr, Result result)
		{
		}

		public void OpenVoiceSettings(OpenVoiceSettingsHandler callback)
		{
		}

		public void InitDrawingDxgi(IntPtr swapchain, bool useMessageForwarding)
		{
		}

		public void OnPresent()
		{
		}

		public void ForwardMessage(IntPtr message)
		{
		}

		public void KeyEvent(bool down, string keyCode, KeyVariant variant)
		{
		}

		public void CharEvent(string character)
		{
		}

		public void MouseButtonEvent(byte down, int clickCount, MouseButton which, int x, int y)
		{
		}

		public void MouseMotionEvent(int x, int y)
		{
		}

		public void ImeCommitText(string text)
		{
		}

		public void ImeSetComposition(string text, ImeUnderline underlines, int from, int to)
		{
		}

		public void ImeCancelComposition()
		{
		}

		[MonoPInvokeCallback]
		private static void SetImeCompositionRangeCallbackCallbackImpl(IntPtr ptr, int from, int to, ref Rect bounds)
		{
		}

		public void SetImeCompositionRangeCallback(SetImeCompositionRangeCallbackHandler callback)
		{
		}

		[MonoPInvokeCallback]
		private static void SetImeSelectionBoundsCallbackCallbackImpl(IntPtr ptr, Rect anchor, Rect focus, bool isAnchorFirst)
		{
		}

		public void SetImeSelectionBoundsCallback(SetImeSelectionBoundsCallbackHandler callback)
		{
		}

		public bool IsPointInsideClickZone(int x, int y)
		{
			return false;
		}

		[MonoPInvokeCallback]
		private static void OnToggleImpl(IntPtr ptr, bool locked)
		{
		}
	}
}
