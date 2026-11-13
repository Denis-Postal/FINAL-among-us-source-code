using System;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatServer
{
	public sealed class AntiCheatServerInterface : Handle
	{
		public const int AddnotifyclientactionrequiredApiLatest = 1;

		public const int AddnotifyclientauthstatuschangedApiLatest = 1;

		public const int AddnotifymessagetoclientApiLatest = 1;

		public const int BeginsessionApiLatest = 3;

		public const int BeginsessionMaxRegistertimeout = 120;

		public const int BeginsessionMinRegistertimeout = 10;

		public const int EndsessionApiLatest = 1;

		public const int GetprotectmessageoutputlengthApiLatest = 1;

		public const int OnmessagetoclientcallbackMaxMessageSize = 512;

		public const int ProtectmessageApiLatest = 1;

		public const int ReceivemessagefromclientApiLatest = 1;

		public const int RegisterclientApiLatest = 2;

		public const int SetclientnetworkstateApiLatest = 1;

		public const int UnprotectmessageApiLatest = 1;

		public const int UnregisterclientApiLatest = 1;

		public AntiCheatServerInterface()
		{
		}

		public AntiCheatServerInterface(IntPtr innerHandle)
		{
		}

		public ulong AddNotifyClientActionRequired(ref AddNotifyClientActionRequiredOptions options, object clientData, OnClientActionRequiredCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyClientAuthStatusChanged(ref AddNotifyClientAuthStatusChangedOptions options, object clientData, OnClientAuthStatusChangedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyMessageToClient(ref AddNotifyMessageToClientOptions options, object clientData, OnMessageToClientCallback notificationFn)
		{
			return 0uL;
		}

		public Result BeginSession(ref BeginSessionOptions options)
		{
			return Result.Success;
		}

		public Result EndSession(ref EndSessionOptions options)
		{
			return Result.Success;
		}

		public Result GetProtectMessageOutputLength(ref GetProtectMessageOutputLengthOptions options, out uint outBufferSizeBytes)
		{
			outBufferSizeBytes = default;
			return Result.Success;
		}

		public Result LogEvent(ref LogEventOptions options)
		{
			return Result.Success;
		}

		public Result LogGameRoundEnd(ref LogGameRoundEndOptions options)
		{
			return Result.Success;
		}

		public Result LogGameRoundStart(ref LogGameRoundStartOptions options)
		{
			return Result.Success;
		}

		public Result LogPlayerDespawn(ref LogPlayerDespawnOptions options)
		{
			return Result.Success;
		}

		public Result LogPlayerRevive(ref LogPlayerReviveOptions options)
		{
			return Result.Success;
		}

		public Result LogPlayerSpawn(ref LogPlayerSpawnOptions options)
		{
			return Result.Success;
		}

		public Result LogPlayerTakeDamage(ref LogPlayerTakeDamageOptions options)
		{
			return Result.Success;
		}

		public Result LogPlayerTick(ref LogPlayerTickOptions options)
		{
			return Result.Success;
		}

		public Result LogPlayerUseAbility(ref LogPlayerUseAbilityOptions options)
		{
			return Result.Success;
		}

		public Result LogPlayerUseWeapon(ref LogPlayerUseWeaponOptions options)
		{
			return Result.Success;
		}

		public Result ProtectMessage(ref ProtectMessageOptions options, ArraySegment<byte> outBuffer, out uint outBytesWritten)
		{
			outBytesWritten = default;
			return Result.Success;
		}

		public Result ReceiveMessageFromClient(ref ReceiveMessageFromClientOptions options)
		{
			return Result.Success;
		}

		public Result RegisterClient(ref RegisterClientOptions options)
		{
			return Result.Success;
		}

		public Result RegisterEvent(ref RegisterEventOptions options)
		{
			return Result.Success;
		}

		public void RemoveNotifyClientActionRequired(ulong notificationId)
		{
		}

		public void RemoveNotifyClientAuthStatusChanged(ulong notificationId)
		{
		}

		public void RemoveNotifyMessageToClient(ulong notificationId)
		{
		}

		public Result SetClientDetails(ref SetClientDetailsOptions options)
		{
			return Result.Success;
		}

		public Result SetClientNetworkState(ref SetClientNetworkStateOptions options)
		{
			return Result.Success;
		}

		public Result SetGameSessionId(ref SetGameSessionIdOptions options)
		{
			return Result.Success;
		}

		public Result UnprotectMessage(ref UnprotectMessageOptions options, ArraySegment<byte> outBuffer, out uint outBytesWritten)
		{
			outBytesWritten = default;
			return Result.Success;
		}

		public Result UnregisterClient(ref UnregisterClientOptions options)
		{
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(OnClientActionRequiredCallbackInternal))]
		internal static void OnClientActionRequiredCallbackInternalImplementation(ref OnClientActionRequiredCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnClientAuthStatusChangedCallbackInternal))]
		internal static void OnClientAuthStatusChangedCallbackInternalImplementation(ref OnClientAuthStatusChangedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnMessageToClientCallbackInternal))]
		internal static void OnMessageToClientCallbackInternalImplementation(ref OnMessageToClientCallbackInfoInternal data)
		{
		}
	}
}
