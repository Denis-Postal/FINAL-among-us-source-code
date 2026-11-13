using System;

namespace Epic.OnlineServices.Mods
{
	public sealed class ModsInterface : Handle
	{
		public const int CopymodinfoApiLatest = 1;

		public const int EnumeratemodsApiLatest = 1;

		public const int InstallmodApiLatest = 1;

		public const int ModIdentifierApiLatest = 1;

		public const int ModinfoApiLatest = 1;

		public const int UninstallmodApiLatest = 1;

		public const int UpdatemodApiLatest = 1;

		public ModsInterface()
		{
		}

		public ModsInterface(IntPtr innerHandle)
		{
		}

		public Result CopyModInfo(ref CopyModInfoOptions options, out ModInfo? outEnumeratedMods)
		{
			outEnumeratedMods = null;
			return Result.Success;
		}

		public void EnumerateMods(ref EnumerateModsOptions options, object clientData, OnEnumerateModsCallback completionDelegate)
		{
		}

		public void InstallMod(ref InstallModOptions options, object clientData, OnInstallModCallback completionDelegate)
		{
		}

		public void UninstallMod(ref UninstallModOptions options, object clientData, OnUninstallModCallback completionDelegate)
		{
		}

		public void UpdateMod(ref UpdateModOptions options, object clientData, OnUpdateModCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnEnumerateModsCallbackInternal))]
		internal static void OnEnumerateModsCallbackInternalImplementation(ref EnumerateModsCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnInstallModCallbackInternal))]
		internal static void OnInstallModCallbackInternalImplementation(ref InstallModCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUninstallModCallbackInternal))]
		internal static void OnUninstallModCallbackInternalImplementation(ref UninstallModCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUpdateModCallbackInternal))]
		internal static void OnUpdateModCallbackInternalImplementation(ref UpdateModCallbackInfoInternal data)
		{
		}
	}
}
