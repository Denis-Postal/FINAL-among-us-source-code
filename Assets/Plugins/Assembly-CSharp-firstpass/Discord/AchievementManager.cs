using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Discord
{
	public class AchievementManager
	{
		internal struct FFIEvents
		{
			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void UserAchievementUpdateHandler(IntPtr ptr, ref UserAchievement userAchievement);

			internal UserAchievementUpdateHandler OnUserAchievementUpdate;
		}

		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void SetUserAchievementCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void SetUserAchievementMethod(IntPtr methodsPtr, long achievementId, byte percentComplete, IntPtr callbackData, SetUserAchievementCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void FetchUserAchievementsCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void FetchUserAchievementsMethod(IntPtr methodsPtr, IntPtr callbackData, FetchUserAchievementsCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void CountUserAchievementsMethod(IntPtr methodsPtr, ref int count);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result GetUserAchievementMethod(IntPtr methodsPtr, long userAchievementId, ref UserAchievement userAchievement);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result GetUserAchievementAtMethod(IntPtr methodsPtr, int index, ref UserAchievement userAchievement);

			internal SetUserAchievementMethod SetUserAchievement;

			internal FetchUserAchievementsMethod FetchUserAchievements;

			internal CountUserAchievementsMethod CountUserAchievements;

			internal GetUserAchievementMethod GetUserAchievement;

			internal GetUserAchievementAtMethod GetUserAchievementAt;
		}

		public delegate void SetUserAchievementHandler(Result result);

		public delegate void FetchUserAchievementsHandler(Result result);

		public delegate void UserAchievementUpdateHandler(ref UserAchievement userAchievement);

		private IntPtr MethodsPtr;

		private object MethodsStructure;

		[CompilerGenerated]
		private UserAchievementUpdateHandler m_OnUserAchievementUpdate;

		private FFIMethods Methods => default;

		public event UserAchievementUpdateHandler OnUserAchievementUpdate
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

		internal AchievementManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
		{
		}

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
		}

		[MonoPInvokeCallback]
		private static void SetUserAchievementCallbackImpl(IntPtr ptr, Result result)
		{
		}

		public void SetUserAchievement(long achievementId, byte percentComplete, SetUserAchievementHandler callback)
		{
		}

		[MonoPInvokeCallback]
		private static void FetchUserAchievementsCallbackImpl(IntPtr ptr, Result result)
		{
		}

		public void FetchUserAchievements(FetchUserAchievementsHandler callback)
		{
		}

		public int CountUserAchievements()
		{
			return 0;
		}

		public UserAchievement GetUserAchievement(long userAchievementId)
		{
			return default;
		}

		public UserAchievement GetUserAchievementAt(int index)
		{
			return default;
		}

		[MonoPInvokeCallback]
		private static void OnUserAchievementUpdateImpl(IntPtr ptr, ref UserAchievement userAchievement)
		{
		}
	}
}
