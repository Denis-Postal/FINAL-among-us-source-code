using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Discord
{
	public class UserManager
	{
		internal struct FFIEvents
		{
			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void CurrentUserUpdateHandler(IntPtr ptr);

			internal CurrentUserUpdateHandler OnCurrentUserUpdate;
		}

		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result GetCurrentUserMethod(IntPtr methodsPtr, ref User currentUser);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void GetUserCallback(IntPtr ptr, Result result, ref User user);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void GetUserMethod(IntPtr methodsPtr, long userId, IntPtr callbackData, GetUserCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result GetCurrentUserPremiumTypeMethod(IntPtr methodsPtr, ref PremiumType premiumType);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result CurrentUserHasFlagMethod(IntPtr methodsPtr, UserFlag flag, ref bool hasFlag);

			internal GetCurrentUserMethod GetCurrentUser;

			internal GetUserMethod GetUser;

			internal GetCurrentUserPremiumTypeMethod GetCurrentUserPremiumType;

			internal CurrentUserHasFlagMethod CurrentUserHasFlag;
		}

		public delegate void GetUserHandler(Result result, ref User user);

		public delegate void CurrentUserUpdateHandler();

		private IntPtr MethodsPtr;

		private object MethodsStructure;

		[CompilerGenerated]
		private CurrentUserUpdateHandler m_OnCurrentUserUpdate;

		private FFIMethods Methods => default;

		public event CurrentUserUpdateHandler OnCurrentUserUpdate
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

		internal UserManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
		{
		}

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
		}

		public User GetCurrentUser()
		{
			return default;
		}

		[MonoPInvokeCallback]
		private static void GetUserCallbackImpl(IntPtr ptr, Result result, ref User user)
		{
		}

		public void GetUser(long userId, GetUserHandler callback)
		{
		}

		public PremiumType GetCurrentUserPremiumType()
		{
			return PremiumType.None;
		}

		public bool CurrentUserHasFlag(UserFlag flag)
		{
			return false;
		}

		[MonoPInvokeCallback]
		private static void OnCurrentUserUpdateImpl(IntPtr ptr)
		{
		}
	}
}
