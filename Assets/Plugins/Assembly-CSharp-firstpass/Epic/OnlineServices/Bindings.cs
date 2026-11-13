using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.Achievements;
using Epic.OnlineServices.AntiCheatClient;
using Epic.OnlineServices.AntiCheatCommon;
using Epic.OnlineServices.AntiCheatServer;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.CustomInvites;
using Epic.OnlineServices.Ecom;
using Epic.OnlineServices.Friends;
using Epic.OnlineServices.IntegratedPlatform;
using Epic.OnlineServices.Inventory;
using Epic.OnlineServices.KWS;
using Epic.OnlineServices.Leaderboards;
using Epic.OnlineServices.Lobby;
using Epic.OnlineServices.Logging;
using Epic.OnlineServices.Metrics;
using Epic.OnlineServices.Mods;
using Epic.OnlineServices.P2P;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.PlayerDataStorage;
using Epic.OnlineServices.Presence;
using Epic.OnlineServices.ProgressionSnapshot;
using Epic.OnlineServices.RTC;
using Epic.OnlineServices.RTCAdmin;
using Epic.OnlineServices.RTCAudio;
using Epic.OnlineServices.RTCData;
using Epic.OnlineServices.ReceiptValidator;
using Epic.OnlineServices.Reports;
using Epic.OnlineServices.Sanctions;
using Epic.OnlineServices.Sessions;
using Epic.OnlineServices.Stats;
using Epic.OnlineServices.TitleStorage;
using Epic.OnlineServices.UI;
using Epic.OnlineServices.UserInfo;

namespace Epic.OnlineServices
{
	public static class Bindings
	{
		[PreserveSig]
		internal static extern ulong EOS_Achievements_AddNotifyAchievementsUnlocked(IntPtr handle, ref AddNotifyAchievementsUnlockedOptionsInternal options, IntPtr clientData, OnAchievementsUnlockedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Achievements_AddNotifyAchievementsUnlockedV2(IntPtr handle, ref AddNotifyAchievementsUnlockedV2OptionsInternal options, IntPtr clientData, OnAchievementsUnlockedCallbackV2Internal notificationFn);

		[PreserveSig]
		internal static extern Result EOS_Achievements_CopyAchievementDefinitionByAchievementId(IntPtr handle, ref CopyAchievementDefinitionByAchievementIdOptionsInternal options, ref IntPtr outDefinition);

		[PreserveSig]
		internal static extern Result EOS_Achievements_CopyAchievementDefinitionByIndex(IntPtr handle, ref CopyAchievementDefinitionByIndexOptionsInternal options, ref IntPtr outDefinition);

		[PreserveSig]
		internal static extern Result EOS_Achievements_CopyAchievementDefinitionV2ByAchievementId(IntPtr handle, ref CopyAchievementDefinitionV2ByAchievementIdOptionsInternal options, ref IntPtr outDefinition);

		[PreserveSig]
		internal static extern Result EOS_Achievements_CopyAchievementDefinitionV2ByIndex(IntPtr handle, ref CopyAchievementDefinitionV2ByIndexOptionsInternal options, ref IntPtr outDefinition);

		[PreserveSig]
		internal static extern Result EOS_Achievements_CopyPlayerAchievementByAchievementId(IntPtr handle, ref CopyPlayerAchievementByAchievementIdOptionsInternal options, ref IntPtr outAchievement);

		[PreserveSig]
		internal static extern Result EOS_Achievements_CopyPlayerAchievementByIndex(IntPtr handle, ref CopyPlayerAchievementByIndexOptionsInternal options, ref IntPtr outAchievement);

		[PreserveSig]
		internal static extern Result EOS_Achievements_CopyUnlockedAchievementByAchievementId(IntPtr handle, ref CopyUnlockedAchievementByAchievementIdOptionsInternal options, ref IntPtr outAchievement);

		[PreserveSig]
		internal static extern Result EOS_Achievements_CopyUnlockedAchievementByIndex(IntPtr handle, ref CopyUnlockedAchievementByIndexOptionsInternal options, ref IntPtr outAchievement);

		[PreserveSig]
		internal static extern void EOS_Achievements_DefinitionV2_Release(IntPtr achievementDefinition);

		[PreserveSig]
		internal static extern void EOS_Achievements_Definition_Release(IntPtr achievementDefinition);

		[PreserveSig]
		internal static extern uint EOS_Achievements_GetAchievementDefinitionCount(IntPtr handle, ref GetAchievementDefinitionCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_Achievements_GetPlayerAchievementCount(IntPtr handle, ref GetPlayerAchievementCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_Achievements_GetUnlockedAchievementCount(IntPtr handle, ref GetUnlockedAchievementCountOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_Achievements_PlayerAchievement_Release(IntPtr achievement);

		[PreserveSig]
		internal static extern void EOS_Achievements_QueryDefinitions(IntPtr handle, ref QueryDefinitionsOptionsInternal options, IntPtr clientData, OnQueryDefinitionsCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Achievements_QueryPlayerAchievements(IntPtr handle, ref QueryPlayerAchievementsOptionsInternal options, IntPtr clientData, OnQueryPlayerAchievementsCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Achievements_RemoveNotifyAchievementsUnlocked(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Achievements_UnlockAchievements(IntPtr handle, ref UnlockAchievementsOptionsInternal options, IntPtr clientData, OnUnlockAchievementsCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Achievements_UnlockedAchievement_Release(IntPtr achievement);

		[PreserveSig]
		internal static extern Result EOS_ActiveSession_CopyInfo(IntPtr handle, ref ActiveSessionCopyInfoOptionsInternal options, ref IntPtr outActiveSessionInfo);

		[PreserveSig]
		internal static extern IntPtr EOS_ActiveSession_GetRegisteredPlayerByIndex(IntPtr handle, ref ActiveSessionGetRegisteredPlayerByIndexOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_ActiveSession_GetRegisteredPlayerCount(IntPtr handle, ref ActiveSessionGetRegisteredPlayerCountOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_ActiveSession_Info_Release(IntPtr activeSessionInfo);

		[PreserveSig]
		internal static extern void EOS_ActiveSession_Release(IntPtr activeSessionHandle);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatClient_AddExternalIntegrityCatalog(IntPtr handle, ref AddExternalIntegrityCatalogOptionsInternal options);

		[PreserveSig]
		internal static extern ulong EOS_AntiCheatClient_AddNotifyClientIntegrityViolated(IntPtr handle, ref AddNotifyClientIntegrityViolatedOptionsInternal options, IntPtr clientData, OnClientIntegrityViolatedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_AntiCheatClient_AddNotifyMessageToPeer(IntPtr handle, ref AddNotifyMessageToPeerOptionsInternal options, IntPtr clientData, OnMessageToPeerCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_AntiCheatClient_AddNotifyMessageToServer(IntPtr handle, ref AddNotifyMessageToServerOptionsInternal options, IntPtr clientData, OnMessageToServerCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_AntiCheatClient_AddNotifyPeerActionRequired(IntPtr handle, ref AddNotifyPeerActionRequiredOptionsInternal options, IntPtr clientData, OnPeerActionRequiredCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_AntiCheatClient_AddNotifyPeerAuthStatusChanged(IntPtr handle, ref AddNotifyPeerAuthStatusChangedOptionsInternal options, IntPtr clientData, OnPeerAuthStatusChangedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatClient_BeginSession(IntPtr handle, ref Epic.OnlineServices.AntiCheatClient.BeginSessionOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatClient_EndSession(IntPtr handle, ref Epic.OnlineServices.AntiCheatClient.EndSessionOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatClient_GetProtectMessageOutputLength(IntPtr handle, ref Epic.OnlineServices.AntiCheatClient.GetProtectMessageOutputLengthOptionsInternal options, ref uint outBufferSizeBytes);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatClient_PollStatus(IntPtr handle, ref PollStatusOptionsInternal options, ref AntiCheatClientViolationType outViolationType, IntPtr outMessage);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatClient_ProtectMessage(IntPtr handle, ref Epic.OnlineServices.AntiCheatClient.ProtectMessageOptionsInternal options, IntPtr outBuffer, ref uint outBytesWritten);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatClient_ReceiveMessageFromPeer(IntPtr handle, ref ReceiveMessageFromPeerOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatClient_ReceiveMessageFromServer(IntPtr handle, ref ReceiveMessageFromServerOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatClient_RegisterPeer(IntPtr handle, ref RegisterPeerOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolated(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_AntiCheatClient_RemoveNotifyMessageToPeer(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_AntiCheatClient_RemoveNotifyMessageToServer(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_AntiCheatClient_RemoveNotifyPeerActionRequired(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChanged(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatClient_UnprotectMessage(IntPtr handle, ref Epic.OnlineServices.AntiCheatClient.UnprotectMessageOptionsInternal options, IntPtr outBuffer, ref uint outBytesWritten);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatClient_UnregisterPeer(IntPtr handle, ref UnregisterPeerOptionsInternal options);

		[PreserveSig]
		internal static extern ulong EOS_AntiCheatServer_AddNotifyClientActionRequired(IntPtr handle, ref AddNotifyClientActionRequiredOptionsInternal options, IntPtr clientData, OnClientActionRequiredCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_AntiCheatServer_AddNotifyClientAuthStatusChanged(IntPtr handle, ref AddNotifyClientAuthStatusChangedOptionsInternal options, IntPtr clientData, OnClientAuthStatusChangedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_AntiCheatServer_AddNotifyMessageToClient(IntPtr handle, ref AddNotifyMessageToClientOptionsInternal options, IntPtr clientData, OnMessageToClientCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_BeginSession(IntPtr handle, ref Epic.OnlineServices.AntiCheatServer.BeginSessionOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_EndSession(IntPtr handle, ref Epic.OnlineServices.AntiCheatServer.EndSessionOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_GetProtectMessageOutputLength(IntPtr handle, ref Epic.OnlineServices.AntiCheatServer.GetProtectMessageOutputLengthOptionsInternal options, ref uint outBufferSizeBytes);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_LogEvent(IntPtr handle, ref LogEventOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_LogGameRoundEnd(IntPtr handle, ref LogGameRoundEndOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_LogGameRoundStart(IntPtr handle, ref LogGameRoundStartOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_LogPlayerDespawn(IntPtr handle, ref LogPlayerDespawnOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_LogPlayerRevive(IntPtr handle, ref LogPlayerReviveOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_LogPlayerSpawn(IntPtr handle, ref LogPlayerSpawnOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_LogPlayerTakeDamage(IntPtr handle, ref LogPlayerTakeDamageOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_LogPlayerTick(IntPtr handle, ref LogPlayerTickOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_LogPlayerUseAbility(IntPtr handle, ref LogPlayerUseAbilityOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_LogPlayerUseWeapon(IntPtr handle, ref LogPlayerUseWeaponOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_ProtectMessage(IntPtr handle, ref Epic.OnlineServices.AntiCheatServer.ProtectMessageOptionsInternal options, IntPtr outBuffer, ref uint outBytesWritten);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_ReceiveMessageFromClient(IntPtr handle, ref ReceiveMessageFromClientOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_RegisterClient(IntPtr handle, ref RegisterClientOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_RegisterEvent(IntPtr handle, ref RegisterEventOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_AntiCheatServer_RemoveNotifyClientActionRequired(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChanged(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_AntiCheatServer_RemoveNotifyMessageToClient(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_SetClientDetails(IntPtr handle, ref SetClientDetailsOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_SetClientNetworkState(IntPtr handle, ref SetClientNetworkStateOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_SetGameSessionId(IntPtr handle, ref SetGameSessionIdOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_UnprotectMessage(IntPtr handle, ref Epic.OnlineServices.AntiCheatServer.UnprotectMessageOptionsInternal options, IntPtr outBuffer, ref uint outBytesWritten);

		[PreserveSig]
		internal static extern Result EOS_AntiCheatServer_UnregisterClient(IntPtr handle, ref UnregisterClientOptionsInternal options);

		[PreserveSig]
		internal static extern ulong EOS_Auth_AddNotifyLoginStatusChanged(IntPtr handle, ref Epic.OnlineServices.Auth.AddNotifyLoginStatusChangedOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Auth.OnLoginStatusChangedCallbackInternal notification);

		[PreserveSig]
		internal static extern Result EOS_Auth_CopyIdToken(IntPtr handle, ref Epic.OnlineServices.Auth.CopyIdTokenOptionsInternal options, ref IntPtr outIdToken);

		[PreserveSig]
		internal static extern Result EOS_Auth_CopyUserAuthToken(IntPtr handle, ref CopyUserAuthTokenOptionsInternal options, IntPtr localUserId, ref IntPtr outUserAuthToken);

		[PreserveSig]
		internal static extern void EOS_Auth_DeletePersistentAuth(IntPtr handle, ref DeletePersistentAuthOptionsInternal options, IntPtr clientData, OnDeletePersistentAuthCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern IntPtr EOS_Auth_GetLoggedInAccountByIndex(IntPtr handle, int index);

		[PreserveSig]
		internal static extern int EOS_Auth_GetLoggedInAccountsCount(IntPtr handle);

		[PreserveSig]
		internal static extern LoginStatus EOS_Auth_GetLoginStatus(IntPtr handle, IntPtr localUserId);

		[PreserveSig]
		internal static extern IntPtr EOS_Auth_GetMergedAccountByIndex(IntPtr handle, IntPtr localUserId, uint index);

		[PreserveSig]
		internal static extern uint EOS_Auth_GetMergedAccountsCount(IntPtr handle, IntPtr localUserId);

		[PreserveSig]
		internal static extern Result EOS_Auth_GetSelectedAccountId(IntPtr handle, IntPtr localUserId, ref IntPtr outSelectedAccountId);

		[PreserveSig]
		internal static extern void EOS_Auth_IdToken_Release(IntPtr idToken);

		[PreserveSig]
		internal static extern void EOS_Auth_LinkAccount(IntPtr handle, ref Epic.OnlineServices.Auth.LinkAccountOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Auth.OnLinkAccountCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Auth_Login(IntPtr handle, ref Epic.OnlineServices.Auth.LoginOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Auth.OnLoginCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Auth_Logout(IntPtr handle, ref Epic.OnlineServices.Auth.LogoutOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Auth.OnLogoutCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Auth_QueryIdToken(IntPtr handle, ref QueryIdTokenOptionsInternal options, IntPtr clientData, OnQueryIdTokenCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Auth_RemoveNotifyLoginStatusChanged(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Auth_Token_Release(IntPtr authToken);

		[PreserveSig]
		internal static extern void EOS_Auth_VerifyIdToken(IntPtr handle, ref Epic.OnlineServices.Auth.VerifyIdTokenOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Auth.OnVerifyIdTokenCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Auth_VerifyUserAuth(IntPtr handle, ref VerifyUserAuthOptionsInternal options, IntPtr clientData, OnVerifyUserAuthCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_ByteArray_ToString(IntPtr byteArray, uint length, IntPtr outBuffer, ref uint inOutBufferLength);

		[PreserveSig]
		internal static extern ulong EOS_Connect_AddNotifyAuthExpiration(IntPtr handle, ref AddNotifyAuthExpirationOptionsInternal options, IntPtr clientData, OnAuthExpirationCallbackInternal notification);

		[PreserveSig]
		internal static extern ulong EOS_Connect_AddNotifyLoginStatusChanged(IntPtr handle, ref Epic.OnlineServices.Connect.AddNotifyLoginStatusChangedOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Connect.OnLoginStatusChangedCallbackInternal notification);

		[PreserveSig]
		internal static extern Result EOS_Connect_CopyIdToken(IntPtr handle, ref Epic.OnlineServices.Connect.CopyIdTokenOptionsInternal options, ref IntPtr outIdToken);

		[PreserveSig]
		internal static extern Result EOS_Connect_CopyProductUserExternalAccountByAccountId(IntPtr handle, ref CopyProductUserExternalAccountByAccountIdOptionsInternal options, ref IntPtr outExternalAccountInfo);

		[PreserveSig]
		internal static extern Result EOS_Connect_CopyProductUserExternalAccountByAccountType(IntPtr handle, ref CopyProductUserExternalAccountByAccountTypeOptionsInternal options, ref IntPtr outExternalAccountInfo);

		[PreserveSig]
		internal static extern Result EOS_Connect_CopyProductUserExternalAccountByIndex(IntPtr handle, ref CopyProductUserExternalAccountByIndexOptionsInternal options, ref IntPtr outExternalAccountInfo);

		[PreserveSig]
		internal static extern Result EOS_Connect_CopyProductUserInfo(IntPtr handle, ref CopyProductUserInfoOptionsInternal options, ref IntPtr outExternalAccountInfo);

		[PreserveSig]
		internal static extern void EOS_Connect_CreateDeviceId(IntPtr handle, ref CreateDeviceIdOptionsInternal options, IntPtr clientData, OnCreateDeviceIdCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Connect_CreateUser(IntPtr handle, ref Epic.OnlineServices.Connect.CreateUserOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Connect.OnCreateUserCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Connect_DeleteDeviceId(IntPtr handle, ref DeleteDeviceIdOptionsInternal options, IntPtr clientData, OnDeleteDeviceIdCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Connect_ExternalAccountInfo_Release(IntPtr externalAccountInfo);

		[PreserveSig]
		internal static extern IntPtr EOS_Connect_GetExternalAccountMapping(IntPtr handle, ref GetExternalAccountMappingsOptionsInternal options);

		[PreserveSig]
		internal static extern IntPtr EOS_Connect_GetLoggedInUserByIndex(IntPtr handle, int index);

		[PreserveSig]
		internal static extern int EOS_Connect_GetLoggedInUsersCount(IntPtr handle);

		[PreserveSig]
		internal static extern LoginStatus EOS_Connect_GetLoginStatus(IntPtr handle, IntPtr localUserId);

		[PreserveSig]
		internal static extern uint EOS_Connect_GetProductUserExternalAccountCount(IntPtr handle, ref GetProductUserExternalAccountCountOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_Connect_GetProductUserIdMapping(IntPtr handle, ref GetProductUserIdMappingOptionsInternal options, IntPtr outBuffer, ref int inOutBufferLength);

		[PreserveSig]
		internal static extern void EOS_Connect_IdToken_Release(IntPtr idToken);

		[PreserveSig]
		internal static extern void EOS_Connect_LinkAccount(IntPtr handle, ref Epic.OnlineServices.Connect.LinkAccountOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Connect.OnLinkAccountCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Connect_Login(IntPtr handle, ref Epic.OnlineServices.Connect.LoginOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Connect.OnLoginCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Connect_Logout(IntPtr handle, ref Epic.OnlineServices.Connect.LogoutOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Connect.OnLogoutCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Connect_QueryExternalAccountMappings(IntPtr handle, ref QueryExternalAccountMappingsOptionsInternal options, IntPtr clientData, OnQueryExternalAccountMappingsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Connect_QueryProductUserIdMappings(IntPtr handle, ref QueryProductUserIdMappingsOptionsInternal options, IntPtr clientData, OnQueryProductUserIdMappingsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Connect_RemoveNotifyAuthExpiration(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Connect_RemoveNotifyLoginStatusChanged(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Connect_TransferDeviceIdAccount(IntPtr handle, ref TransferDeviceIdAccountOptionsInternal options, IntPtr clientData, OnTransferDeviceIdAccountCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Connect_UnlinkAccount(IntPtr handle, ref UnlinkAccountOptionsInternal options, IntPtr clientData, OnUnlinkAccountCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Connect_VerifyIdToken(IntPtr handle, ref Epic.OnlineServices.Connect.VerifyIdTokenOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Connect.OnVerifyIdTokenCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_ContinuanceToken_ToString(IntPtr continuanceToken, IntPtr outBuffer, ref int inOutBufferLength);

		[PreserveSig]
		internal static extern void EOS_CustomInvites_AcceptRequestToJoin(IntPtr handle, ref AcceptRequestToJoinOptionsInternal options, IntPtr clientData, OnAcceptRequestToJoinCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern ulong EOS_CustomInvites_AddNotifyCustomInviteAccepted(IntPtr handle, ref AddNotifyCustomInviteAcceptedOptionsInternal options, IntPtr clientData, OnCustomInviteAcceptedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_CustomInvites_AddNotifyCustomInviteReceived(IntPtr handle, ref AddNotifyCustomInviteReceivedOptionsInternal options, IntPtr clientData, OnCustomInviteReceivedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_CustomInvites_AddNotifyCustomInviteRejected(IntPtr handle, ref AddNotifyCustomInviteRejectedOptionsInternal options, IntPtr clientData, OnCustomInviteRejectedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_CustomInvites_AddNotifyRequestToJoinAccepted(IntPtr handle, ref AddNotifyRequestToJoinAcceptedOptionsInternal options, IntPtr clientData, OnRequestToJoinAcceptedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_CustomInvites_AddNotifyRequestToJoinReceived(IntPtr handle, ref AddNotifyRequestToJoinReceivedOptionsInternal options, IntPtr clientData, OnRequestToJoinReceivedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_CustomInvites_AddNotifyRequestToJoinRejected(IntPtr handle, ref AddNotifyRequestToJoinRejectedOptionsInternal options, IntPtr clientData, OnRequestToJoinRejectedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_CustomInvites_AddNotifyRequestToJoinResponseReceived(IntPtr handle, ref AddNotifyRequestToJoinResponseReceivedOptionsInternal options, IntPtr clientData, OnRequestToJoinResponseReceivedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_CustomInvites_AddNotifySendCustomNativeInviteRequested(IntPtr handle, ref AddNotifySendCustomNativeInviteRequestedOptionsInternal options, IntPtr clientData, OnSendCustomNativeInviteRequestedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern Result EOS_CustomInvites_FinalizeInvite(IntPtr handle, ref FinalizeInviteOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_CustomInvites_RejectRequestToJoin(IntPtr handle, ref RejectRequestToJoinOptionsInternal options, IntPtr clientData, OnRejectRequestToJoinCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_CustomInvites_RemoveNotifyCustomInviteAccepted(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_CustomInvites_RemoveNotifyCustomInviteReceived(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_CustomInvites_RemoveNotifyCustomInviteRejected(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_CustomInvites_RemoveNotifyRequestToJoinAccepted(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_CustomInvites_RemoveNotifyRequestToJoinReceived(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_CustomInvites_RemoveNotifyRequestToJoinRejected(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceived(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequested(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_CustomInvites_SendCustomInvite(IntPtr handle, ref SendCustomInviteOptionsInternal options, IntPtr clientData, OnSendCustomInviteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_CustomInvites_SendRequestToJoin(IntPtr handle, ref SendRequestToJoinOptionsInternal options, IntPtr clientData, OnSendRequestToJoinCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_CustomInvites_SetCustomInvite(IntPtr handle, ref SetCustomInviteOptionsInternal options);

		[PreserveSig]
		internal static extern IntPtr EOS_EApplicationStatus_ToString(ApplicationStatus applicationStatus);

		[PreserveSig]
		internal static extern IntPtr EOS_ENetworkStatus_ToString(NetworkStatus networkStatus);

		[PreserveSig]
		internal static extern int EOS_EResult_IsOperationComplete(Result result);

		[PreserveSig]
		internal static extern IntPtr EOS_EResult_ToString(Result result);

		[PreserveSig]
		internal static extern void EOS_Ecom_CatalogItem_Release(IntPtr catalogItem);

		[PreserveSig]
		internal static extern void EOS_Ecom_CatalogOffer_Release(IntPtr catalogOffer);

		[PreserveSig]
		internal static extern void EOS_Ecom_CatalogRelease_Release(IntPtr catalogRelease);

		[PreserveSig]
		internal static extern void EOS_Ecom_Checkout(IntPtr handle, ref CheckoutOptionsInternal options, IntPtr clientData, OnCheckoutCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_Ecom_CopyEntitlementById(IntPtr handle, ref CopyEntitlementByIdOptionsInternal options, ref IntPtr outEntitlement);

		[PreserveSig]
		internal static extern Result EOS_Ecom_CopyEntitlementByIndex(IntPtr handle, ref CopyEntitlementByIndexOptionsInternal options, ref IntPtr outEntitlement);

		[PreserveSig]
		internal static extern Result EOS_Ecom_CopyEntitlementByNameAndIndex(IntPtr handle, ref CopyEntitlementByNameAndIndexOptionsInternal options, ref IntPtr outEntitlement);

		[PreserveSig]
		internal static extern Result EOS_Ecom_CopyItemById(IntPtr handle, ref CopyItemByIdOptionsInternal options, ref IntPtr outItem);

		[PreserveSig]
		internal static extern Result EOS_Ecom_CopyItemImageInfoByIndex(IntPtr handle, ref CopyItemImageInfoByIndexOptionsInternal options, ref IntPtr outImageInfo);

		[PreserveSig]
		internal static extern Result EOS_Ecom_CopyItemReleaseByIndex(IntPtr handle, ref CopyItemReleaseByIndexOptionsInternal options, ref IntPtr outRelease);

		[PreserveSig]
		internal static extern Result EOS_Ecom_CopyLastRedeemedEntitlementByIndex(IntPtr handle, ref CopyLastRedeemedEntitlementByIndexOptionsInternal options, IntPtr outRedeemedEntitlementId, ref int inOutRedeemedEntitlementIdLength);

		[PreserveSig]
		internal static extern Result EOS_Ecom_CopyOfferById(IntPtr handle, ref CopyOfferByIdOptionsInternal options, ref IntPtr outOffer);

		[PreserveSig]
		internal static extern Result EOS_Ecom_CopyOfferByIndex(IntPtr handle, ref CopyOfferByIndexOptionsInternal options, ref IntPtr outOffer);

		[PreserveSig]
		internal static extern Result EOS_Ecom_CopyOfferImageInfoByIndex(IntPtr handle, ref CopyOfferImageInfoByIndexOptionsInternal options, ref IntPtr outImageInfo);

		[PreserveSig]
		internal static extern Result EOS_Ecom_CopyOfferItemByIndex(IntPtr handle, ref CopyOfferItemByIndexOptionsInternal options, ref IntPtr outItem);

		[PreserveSig]
		internal static extern Result EOS_Ecom_CopyTransactionById(IntPtr handle, ref CopyTransactionByIdOptionsInternal options, ref IntPtr outTransaction);

		[PreserveSig]
		internal static extern Result EOS_Ecom_CopyTransactionByIndex(IntPtr handle, ref CopyTransactionByIndexOptionsInternal options, ref IntPtr outTransaction);

		[PreserveSig]
		internal static extern void EOS_Ecom_Entitlement_Release(IntPtr entitlement);

		[PreserveSig]
		internal static extern uint EOS_Ecom_GetEntitlementsByNameCount(IntPtr handle, ref GetEntitlementsByNameCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_Ecom_GetEntitlementsCount(IntPtr handle, ref GetEntitlementsCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_Ecom_GetItemImageInfoCount(IntPtr handle, ref GetItemImageInfoCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_Ecom_GetItemReleaseCount(IntPtr handle, ref GetItemReleaseCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_Ecom_GetLastRedeemedEntitlementsCount(IntPtr handle, ref GetLastRedeemedEntitlementsCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_Ecom_GetOfferCount(IntPtr handle, ref GetOfferCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_Ecom_GetOfferImageInfoCount(IntPtr handle, ref GetOfferImageInfoCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_Ecom_GetOfferItemCount(IntPtr handle, ref GetOfferItemCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_Ecom_GetTransactionCount(IntPtr handle, ref GetTransactionCountOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_Ecom_KeyImageInfo_Release(IntPtr keyImageInfo);

		[PreserveSig]
		internal static extern void EOS_Ecom_QueryEntitlementToken(IntPtr handle, ref QueryEntitlementTokenOptionsInternal options, IntPtr clientData, OnQueryEntitlementTokenCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Ecom_QueryEntitlements(IntPtr handle, ref QueryEntitlementsOptionsInternal options, IntPtr clientData, OnQueryEntitlementsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Ecom_QueryOffers(IntPtr handle, ref QueryOffersOptionsInternal options, IntPtr clientData, OnQueryOffersCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Ecom_QueryOwnership(IntPtr handle, ref QueryOwnershipOptionsInternal options, IntPtr clientData, OnQueryOwnershipCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Ecom_QueryOwnershipBySandboxIds(IntPtr handle, ref QueryOwnershipBySandboxIdsOptionsInternal options, IntPtr clientData, OnQueryOwnershipBySandboxIdsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Ecom_QueryOwnershipToken(IntPtr handle, ref QueryOwnershipTokenOptionsInternal options, IntPtr clientData, OnQueryOwnershipTokenCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Ecom_RedeemEntitlements(IntPtr handle, ref RedeemEntitlementsOptionsInternal options, IntPtr clientData, OnRedeemEntitlementsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_Ecom_Transaction_CopyEntitlementByIndex(IntPtr handle, ref TransactionCopyEntitlementByIndexOptionsInternal options, ref IntPtr outEntitlement);

		[PreserveSig]
		internal static extern uint EOS_Ecom_Transaction_GetEntitlementsCount(IntPtr handle, ref TransactionGetEntitlementsCountOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_Ecom_Transaction_GetTransactionId(IntPtr handle, IntPtr outBuffer, ref int inOutBufferLength);

		[PreserveSig]
		internal static extern void EOS_Ecom_Transaction_Release(IntPtr transaction);

		[PreserveSig]
		internal static extern IntPtr EOS_EpicAccountId_FromString(IntPtr accountIdString);

		[PreserveSig]
		internal static extern int EOS_EpicAccountId_IsValid(IntPtr accountId);

		[PreserveSig]
		internal static extern Result EOS_EpicAccountId_ToString(IntPtr accountId, IntPtr outBuffer, ref int inOutBufferLength);

		[PreserveSig]
		internal static extern void EOS_Friends_AcceptInvite(IntPtr handle, ref AcceptInviteOptionsInternal options, IntPtr clientData, OnAcceptInviteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern ulong EOS_Friends_AddNotifyBlockedUsersUpdate(IntPtr handle, ref AddNotifyBlockedUsersUpdateOptionsInternal options, IntPtr clientData, OnBlockedUsersUpdateCallbackInternal blockedUsersUpdateHandler);

		[PreserveSig]
		internal static extern ulong EOS_Friends_AddNotifyFriendsUpdate(IntPtr handle, ref AddNotifyFriendsUpdateOptionsInternal options, IntPtr clientData, OnFriendsUpdateCallbackInternal friendsUpdateHandler);

		[PreserveSig]
		internal static extern IntPtr EOS_Friends_GetBlockedUserAtIndex(IntPtr handle, ref GetBlockedUserAtIndexOptionsInternal options);

		[PreserveSig]
		internal static extern int EOS_Friends_GetBlockedUsersCount(IntPtr handle, ref GetBlockedUsersCountOptionsInternal options);

		[PreserveSig]
		internal static extern IntPtr EOS_Friends_GetFriendAtIndex(IntPtr handle, ref GetFriendAtIndexOptionsInternal options);

		[PreserveSig]
		internal static extern int EOS_Friends_GetFriendsCount(IntPtr handle, ref GetFriendsCountOptionsInternal options);

		[PreserveSig]
		internal static extern FriendsStatus EOS_Friends_GetStatus(IntPtr handle, ref GetStatusOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_Friends_QueryFriends(IntPtr handle, ref QueryFriendsOptionsInternal options, IntPtr clientData, OnQueryFriendsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Friends_RejectInvite(IntPtr handle, ref Epic.OnlineServices.Friends.RejectInviteOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Friends.OnRejectInviteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Friends_RemoveNotifyBlockedUsersUpdate(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_Friends_RemoveNotifyFriendsUpdate(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_Friends_SendInvite(IntPtr handle, ref Epic.OnlineServices.Friends.SendInviteOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Friends.OnSendInviteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern IntPtr EOS_GetVersion();

		[PreserveSig]
		internal static extern Result EOS_Initialize(ref InitializeOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_IntegratedPlatformOptionsContainer_Add(IntPtr handle, ref IntegratedPlatformOptionsContainerAddOptionsInternal inOptions);

		[PreserveSig]
		internal static extern void EOS_IntegratedPlatformOptionsContainer_Release(IntPtr integratedPlatformOptionsContainerHandle);

		[PreserveSig]
		internal static extern ulong EOS_IntegratedPlatform_AddNotifyUserLoginStatusChanged(IntPtr handle, ref AddNotifyUserLoginStatusChangedOptionsInternal options, IntPtr clientData, OnUserLoginStatusChangedCallbackInternal callbackFunction);

		[PreserveSig]
		internal static extern void EOS_IntegratedPlatform_ClearUserPreLogoutCallback(IntPtr handle, ref ClearUserPreLogoutCallbackOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainer(ref CreateIntegratedPlatformOptionsContainerOptionsInternal options, ref IntPtr outIntegratedPlatformOptionsContainerHandle);

		[PreserveSig]
		internal static extern Result EOS_IntegratedPlatform_FinalizeDeferredUserLogout(IntPtr handle, ref FinalizeDeferredUserLogoutOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChanged(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern Result EOS_IntegratedPlatform_SetUserLoginStatus(IntPtr handle, ref SetUserLoginStatusOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_IntegratedPlatform_SetUserPreLogoutCallback(IntPtr handle, ref SetUserPreLogoutCallbackOptionsInternal options, IntPtr clientData, OnUserPreLogoutCallbackInternal callbackFunction);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_CreateSubItem(IntPtr handle, ref InventoryItemCreateSubItemOptionsInternal options, ref IntPtr outInventoryItemHandle);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_DeleteSubItem(IntPtr handle, ref InventoryItemDeleteSubItemOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_GetDoubleSetValueByIndex(IntPtr handle, ref InventoryItemGetDoubleSetValueByIndexOptionsInternal options, ref double outValue);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_GetInt64SetValueByIndex(IntPtr handle, ref InventoryItemGetInt64SetValueByIndexOptionsInternal options, ref long outValue);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_GetKey(IntPtr handle, ref InventoryItemGetKeyOptionsInternal options, IntPtr outKey, ref uint inOutKeyLength);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_GetStringSetValueByIndex(IntPtr handle, ref InventoryItemGetStringSetValueByIndexOptionsInternal options, IntPtr outValue, ref uint inOutStringLength);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_GetSubItem(IntPtr handle, ref InventoryItemGetSubItemOptionsInternal options, ref IntPtr outInventoryItemHandle);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_GetSubItemByIndex(IntPtr handle, ref InventoryItemGetSubItemByIndexOptionsInternal options, ref IntPtr outInventoryItemHandle);

		[PreserveSig]
		internal static extern uint EOS_InventoryItem_GetSubItemsCount(IntPtr handle, ref InventoryItemGetSubItemsCountOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_GetValueAsBool(IntPtr handle, ref InventoryItemGetValueAsBoolOptionsInternal options, ref int outValue);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_GetValueAsDouble(IntPtr handle, ref InventoryItemGetValueAsDoubleOptionsInternal options, ref double outValue);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_GetValueAsInt64(IntPtr handle, ref InventoryItemGetValueAsInt64OptionsInternal options, ref long outValue);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_GetValueAsString(IntPtr handle, ref InventoryItemGetValueAsStringOptionsInternal options, IntPtr outValue, ref uint inOutValueLength);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_GetValueAsTimestamp(IntPtr handle, ref InventoryItemGetValueAsTimestampOptionsInternal options, ref long outValue);

		[PreserveSig]
		internal static extern uint EOS_InventoryItem_GetValueCount(IntPtr handle, ref InventoryItemGetValueCountOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_GetValueType(IntPtr handle, ref InventoryItemGetValueTypeOptionsInternal options, ref Epic.OnlineServices.Inventory.ValueType outValueType);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_SetKey(IntPtr handle, ref InventoryItemSetKeyOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_SetValueAsBool(IntPtr handle, ref InventoryItemSetValueAsBoolOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_SetValueAsDouble(IntPtr handle, ref InventoryItemSetValueAsDoubleOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_SetValueAsDoubleSet(IntPtr handle, ref InventoryItemSetValueAsDoubleSetOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_SetValueAsInt64(IntPtr handle, ref InventoryItemSetValueAsInt64OptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_SetValueAsInt64Set(IntPtr handle, ref InventoryItemSetValueAsInt64SetOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_SetValueAsString(IntPtr handle, ref InventoryItemSetValueAsStringOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_SetValueAsStringSet(IntPtr handle, ref InventoryItemSetValueAsStringSetOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_InventoryItem_SetValueAsTimestamp(IntPtr handle, ref InventoryItemSetValueAsTimestampOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_Inventory_Checkpoint(IntPtr handle, ref CheckpointOptionsInternal options, IntPtr clientData, OnCheckpointCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Inventory_Close(IntPtr handle, ref CloseOptionsInternal options, IntPtr clientData, OnCloseCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Inventory_Create(IntPtr handle, ref CreateOptionsInternal options, IntPtr clientData, OnCreateCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Inventory_Open(IntPtr handle, ref OpenOptionsInternal options, IntPtr clientData, OnOpenCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern ulong EOS_KWS_AddNotifyPermissionsUpdateReceived(IntPtr handle, ref AddNotifyPermissionsUpdateReceivedOptionsInternal options, IntPtr clientData, OnPermissionsUpdateReceivedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern Result EOS_KWS_CopyPermissionByIndex(IntPtr handle, ref CopyPermissionByIndexOptionsInternal options, ref IntPtr outPermission);

		[PreserveSig]
		internal static extern void EOS_KWS_CreateUser(IntPtr handle, ref Epic.OnlineServices.KWS.CreateUserOptionsInternal options, IntPtr clientData, Epic.OnlineServices.KWS.OnCreateUserCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_KWS_GetPermissionByKey(IntPtr handle, ref GetPermissionByKeyOptionsInternal options, ref KWSPermissionStatus outPermission);

		[PreserveSig]
		internal static extern int EOS_KWS_GetPermissionsCount(IntPtr handle, ref GetPermissionsCountOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_KWS_PermissionStatus_Release(IntPtr permissionStatus);

		[PreserveSig]
		internal static extern void EOS_KWS_QueryAgeGate(IntPtr handle, ref QueryAgeGateOptionsInternal options, IntPtr clientData, OnQueryAgeGateCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_KWS_QueryPermissions(IntPtr handle, ref QueryPermissionsOptionsInternal options, IntPtr clientData, OnQueryPermissionsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_KWS_RemoveNotifyPermissionsUpdateReceived(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_KWS_RequestPermissions(IntPtr handle, ref RequestPermissionsOptionsInternal options, IntPtr clientData, OnRequestPermissionsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_KWS_UpdateParentEmail(IntPtr handle, ref UpdateParentEmailOptionsInternal options, IntPtr clientData, OnUpdateParentEmailCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_Leaderboards_CopyLeaderboardDefinitionByIndex(IntPtr handle, ref CopyLeaderboardDefinitionByIndexOptionsInternal options, ref IntPtr outLeaderboardDefinition);

		[PreserveSig]
		internal static extern Result EOS_Leaderboards_CopyLeaderboardDefinitionByLeaderboardId(IntPtr handle, ref CopyLeaderboardDefinitionByLeaderboardIdOptionsInternal options, ref IntPtr outLeaderboardDefinition);

		[PreserveSig]
		internal static extern Result EOS_Leaderboards_CopyLeaderboardRecordByIndex(IntPtr handle, ref CopyLeaderboardRecordByIndexOptionsInternal options, ref IntPtr outLeaderboardRecord);

		[PreserveSig]
		internal static extern Result EOS_Leaderboards_CopyLeaderboardRecordByUserId(IntPtr handle, ref CopyLeaderboardRecordByUserIdOptionsInternal options, ref IntPtr outLeaderboardRecord);

		[PreserveSig]
		internal static extern Result EOS_Leaderboards_CopyLeaderboardUserScoreByIndex(IntPtr handle, ref CopyLeaderboardUserScoreByIndexOptionsInternal options, ref IntPtr outLeaderboardUserScore);

		[PreserveSig]
		internal static extern Result EOS_Leaderboards_CopyLeaderboardUserScoreByUserId(IntPtr handle, ref CopyLeaderboardUserScoreByUserIdOptionsInternal options, ref IntPtr outLeaderboardUserScore);

		[PreserveSig]
		internal static extern void EOS_Leaderboards_Definition_Release(IntPtr leaderboardDefinition);

		[PreserveSig]
		internal static extern uint EOS_Leaderboards_GetLeaderboardDefinitionCount(IntPtr handle, ref GetLeaderboardDefinitionCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_Leaderboards_GetLeaderboardRecordCount(IntPtr handle, ref GetLeaderboardRecordCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_Leaderboards_GetLeaderboardUserScoreCount(IntPtr handle, ref GetLeaderboardUserScoreCountOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_Leaderboards_LeaderboardDefinition_Release(IntPtr leaderboardDefinition);

		[PreserveSig]
		internal static extern void EOS_Leaderboards_LeaderboardRecord_Release(IntPtr leaderboardRecord);

		[PreserveSig]
		internal static extern void EOS_Leaderboards_LeaderboardUserScore_Release(IntPtr leaderboardUserScore);

		[PreserveSig]
		internal static extern void EOS_Leaderboards_QueryLeaderboardDefinitions(IntPtr handle, ref QueryLeaderboardDefinitionsOptionsInternal options, IntPtr clientData, OnQueryLeaderboardDefinitionsCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Leaderboards_QueryLeaderboardRanks(IntPtr handle, ref QueryLeaderboardRanksOptionsInternal options, IntPtr clientData, OnQueryLeaderboardRanksCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Leaderboards_QueryLeaderboardUserScores(IntPtr handle, ref QueryLeaderboardUserScoresOptionsInternal options, IntPtr clientData, OnQueryLeaderboardUserScoresCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_LobbyDetails_CopyAttributeByIndex(IntPtr handle, ref LobbyDetailsCopyAttributeByIndexOptionsInternal options, ref IntPtr outAttribute);

		[PreserveSig]
		internal static extern Result EOS_LobbyDetails_CopyAttributeByKey(IntPtr handle, ref LobbyDetailsCopyAttributeByKeyOptionsInternal options, ref IntPtr outAttribute);

		[PreserveSig]
		internal static extern Result EOS_LobbyDetails_CopyInfo(IntPtr handle, ref LobbyDetailsCopyInfoOptionsInternal options, ref IntPtr outLobbyDetailsInfo);

		[PreserveSig]
		internal static extern Result EOS_LobbyDetails_CopyMemberAttributeByIndex(IntPtr handle, ref LobbyDetailsCopyMemberAttributeByIndexOptionsInternal options, ref IntPtr outAttribute);

		[PreserveSig]
		internal static extern Result EOS_LobbyDetails_CopyMemberAttributeByKey(IntPtr handle, ref LobbyDetailsCopyMemberAttributeByKeyOptionsInternal options, ref IntPtr outAttribute);

		[PreserveSig]
		internal static extern Result EOS_LobbyDetails_CopyMemberInfo(IntPtr handle, ref LobbyDetailsCopyMemberInfoOptionsInternal options, ref IntPtr outLobbyDetailsMemberInfo);

		[PreserveSig]
		internal static extern uint EOS_LobbyDetails_GetAttributeCount(IntPtr handle, ref LobbyDetailsGetAttributeCountOptionsInternal options);

		[PreserveSig]
		internal static extern IntPtr EOS_LobbyDetails_GetLobbyOwner(IntPtr handle, ref LobbyDetailsGetLobbyOwnerOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_LobbyDetails_GetMemberAttributeCount(IntPtr handle, ref LobbyDetailsGetMemberAttributeCountOptionsInternal options);

		[PreserveSig]
		internal static extern IntPtr EOS_LobbyDetails_GetMemberByIndex(IntPtr handle, ref LobbyDetailsGetMemberByIndexOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_LobbyDetails_GetMemberCount(IntPtr handle, ref LobbyDetailsGetMemberCountOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_LobbyDetails_Info_Release(IntPtr lobbyDetailsInfo);

		[PreserveSig]
		internal static extern void EOS_LobbyDetails_MemberInfo_Release(IntPtr lobbyDetailsMemberInfo);

		[PreserveSig]
		internal static extern void EOS_LobbyDetails_Release(IntPtr lobbyHandle);

		[PreserveSig]
		internal static extern Result EOS_LobbyModification_AddAttribute(IntPtr handle, ref LobbyModificationAddAttributeOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_LobbyModification_AddMemberAttribute(IntPtr handle, ref LobbyModificationAddMemberAttributeOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_LobbyModification_Release(IntPtr lobbyModificationHandle);

		[PreserveSig]
		internal static extern Result EOS_LobbyModification_RemoveAttribute(IntPtr handle, ref LobbyModificationRemoveAttributeOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_LobbyModification_RemoveMemberAttribute(IntPtr handle, ref LobbyModificationRemoveMemberAttributeOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_LobbyModification_SetAllowedPlatformIds(IntPtr handle, ref LobbyModificationSetAllowedPlatformIdsOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_LobbyModification_SetBucketId(IntPtr handle, ref LobbyModificationSetBucketIdOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_LobbyModification_SetInvitesAllowed(IntPtr handle, ref LobbyModificationSetInvitesAllowedOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_LobbyModification_SetMaxMembers(IntPtr handle, ref LobbyModificationSetMaxMembersOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_LobbyModification_SetPermissionLevel(IntPtr handle, ref LobbyModificationSetPermissionLevelOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_LobbySearch_CopySearchResultByIndex(IntPtr handle, ref LobbySearchCopySearchResultByIndexOptionsInternal options, ref IntPtr outLobbyDetailsHandle);

		[PreserveSig]
		internal static extern void EOS_LobbySearch_Find(IntPtr handle, ref LobbySearchFindOptionsInternal options, IntPtr clientData, LobbySearchOnFindCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern uint EOS_LobbySearch_GetSearchResultCount(IntPtr handle, ref LobbySearchGetSearchResultCountOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_LobbySearch_Release(IntPtr lobbySearchHandle);

		[PreserveSig]
		internal static extern Result EOS_LobbySearch_RemoveParameter(IntPtr handle, ref LobbySearchRemoveParameterOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_LobbySearch_SetLobbyId(IntPtr handle, ref LobbySearchSetLobbyIdOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_LobbySearch_SetMaxResults(IntPtr handle, ref LobbySearchSetMaxResultsOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_LobbySearch_SetParameter(IntPtr handle, ref LobbySearchSetParameterOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_LobbySearch_SetTargetUserId(IntPtr handle, ref LobbySearchSetTargetUserIdOptionsInternal options);

		[PreserveSig]
		internal static extern ulong EOS_Lobby_AddNotifyJoinLobbyAccepted(IntPtr handle, ref AddNotifyJoinLobbyAcceptedOptionsInternal options, IntPtr clientData, OnJoinLobbyAcceptedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Lobby_AddNotifyLeaveLobbyRequested(IntPtr handle, ref AddNotifyLeaveLobbyRequestedOptionsInternal options, IntPtr clientData, OnLeaveLobbyRequestedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Lobby_AddNotifyLobbyInviteAccepted(IntPtr handle, ref AddNotifyLobbyInviteAcceptedOptionsInternal options, IntPtr clientData, OnLobbyInviteAcceptedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Lobby_AddNotifyLobbyInviteReceived(IntPtr handle, ref AddNotifyLobbyInviteReceivedOptionsInternal options, IntPtr clientData, OnLobbyInviteReceivedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Lobby_AddNotifyLobbyInviteRejected(IntPtr handle, ref AddNotifyLobbyInviteRejectedOptionsInternal options, IntPtr clientData, OnLobbyInviteRejectedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Lobby_AddNotifyLobbyMemberStatusReceived(IntPtr handle, ref AddNotifyLobbyMemberStatusReceivedOptionsInternal options, IntPtr clientData, OnLobbyMemberStatusReceivedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Lobby_AddNotifyLobbyMemberUpdateReceived(IntPtr handle, ref AddNotifyLobbyMemberUpdateReceivedOptionsInternal options, IntPtr clientData, OnLobbyMemberUpdateReceivedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Lobby_AddNotifyLobbyUpdateReceived(IntPtr handle, ref AddNotifyLobbyUpdateReceivedOptionsInternal options, IntPtr clientData, OnLobbyUpdateReceivedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Lobby_AddNotifyRTCRoomConnectionChanged(IntPtr handle, ref AddNotifyRTCRoomConnectionChangedOptionsInternal options, IntPtr clientData, OnRTCRoomConnectionChangedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Lobby_AddNotifySendLobbyNativeInviteRequested(IntPtr handle, ref AddNotifySendLobbyNativeInviteRequestedOptionsInternal options, IntPtr clientData, OnSendLobbyNativeInviteRequestedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern void EOS_Lobby_Attribute_Release(IntPtr lobbyAttribute);

		[PreserveSig]
		internal static extern Result EOS_Lobby_CopyLobbyDetailsHandle(IntPtr handle, ref CopyLobbyDetailsHandleOptionsInternal options, ref IntPtr outLobbyDetailsHandle);

		[PreserveSig]
		internal static extern Result EOS_Lobby_CopyLobbyDetailsHandleByInviteId(IntPtr handle, ref CopyLobbyDetailsHandleByInviteIdOptionsInternal options, ref IntPtr outLobbyDetailsHandle);

		[PreserveSig]
		internal static extern Result EOS_Lobby_CopyLobbyDetailsHandleByUiEventId(IntPtr handle, ref CopyLobbyDetailsHandleByUiEventIdOptionsInternal options, ref IntPtr outLobbyDetailsHandle);

		[PreserveSig]
		internal static extern void EOS_Lobby_CreateLobby(IntPtr handle, ref CreateLobbyOptionsInternal options, IntPtr clientData, OnCreateLobbyCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_Lobby_CreateLobbySearch(IntPtr handle, ref CreateLobbySearchOptionsInternal options, ref IntPtr outLobbySearchHandle);

		[PreserveSig]
		internal static extern void EOS_Lobby_DestroyLobby(IntPtr handle, ref DestroyLobbyOptionsInternal options, IntPtr clientData, OnDestroyLobbyCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_Lobby_GetConnectString(IntPtr handle, ref GetConnectStringOptionsInternal options, IntPtr outBuffer, ref uint inOutBufferLength);

		[PreserveSig]
		internal static extern uint EOS_Lobby_GetInviteCount(IntPtr handle, ref Epic.OnlineServices.Lobby.GetInviteCountOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_Lobby_GetInviteIdByIndex(IntPtr handle, ref Epic.OnlineServices.Lobby.GetInviteIdByIndexOptionsInternal options, IntPtr outBuffer, ref int inOutBufferLength);

		[PreserveSig]
		internal static extern Result EOS_Lobby_GetRTCRoomName(IntPtr handle, ref GetRTCRoomNameOptionsInternal options, IntPtr outBuffer, ref uint inOutBufferLength);

		[PreserveSig]
		internal static extern void EOS_Lobby_HardMuteMember(IntPtr handle, ref HardMuteMemberOptionsInternal options, IntPtr clientData, OnHardMuteMemberCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_Lobby_IsRTCRoomConnected(IntPtr handle, ref IsRTCRoomConnectedOptionsInternal options, ref int bOutIsConnected);

		[PreserveSig]
		internal static extern void EOS_Lobby_JoinLobby(IntPtr handle, ref JoinLobbyOptionsInternal options, IntPtr clientData, OnJoinLobbyCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Lobby_JoinLobbyById(IntPtr handle, ref JoinLobbyByIdOptionsInternal options, IntPtr clientData, OnJoinLobbyByIdCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Lobby_KickMember(IntPtr handle, ref KickMemberOptionsInternal options, IntPtr clientData, OnKickMemberCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Lobby_LeaveLobby(IntPtr handle, ref LeaveLobbyOptionsInternal options, IntPtr clientData, OnLeaveLobbyCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_Lobby_ParseConnectString(IntPtr handle, ref ParseConnectStringOptionsInternal options, IntPtr outBuffer, ref uint inOutBufferLength);

		[PreserveSig]
		internal static extern void EOS_Lobby_PromoteMember(IntPtr handle, ref PromoteMemberOptionsInternal options, IntPtr clientData, OnPromoteMemberCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Lobby_QueryInvites(IntPtr handle, ref Epic.OnlineServices.Lobby.QueryInvitesOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Lobby.OnQueryInvitesCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Lobby_RejectInvite(IntPtr handle, ref Epic.OnlineServices.Lobby.RejectInviteOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Lobby.OnRejectInviteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Lobby_RemoveNotifyJoinLobbyAccepted(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Lobby_RemoveNotifyLeaveLobbyRequested(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Lobby_RemoveNotifyLobbyInviteAccepted(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Lobby_RemoveNotifyLobbyInviteReceived(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Lobby_RemoveNotifyLobbyInviteRejected(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Lobby_RemoveNotifyLobbyMemberStatusReceived(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceived(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Lobby_RemoveNotifyLobbyUpdateReceived(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Lobby_RemoveNotifyRTCRoomConnectionChanged(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequested(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Lobby_SendInvite(IntPtr handle, ref Epic.OnlineServices.Lobby.SendInviteOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Lobby.OnSendInviteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Lobby_UpdateLobby(IntPtr handle, ref UpdateLobbyOptionsInternal options, IntPtr clientData, OnUpdateLobbyCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_Lobby_UpdateLobbyModification(IntPtr handle, ref UpdateLobbyModificationOptionsInternal options, ref IntPtr outLobbyModificationHandle);

		[PreserveSig]
		internal static extern Result EOS_Logging_SetCallback(LogMessageFuncInternal callback);

		[PreserveSig]
		internal static extern Result EOS_Logging_SetLogLevel(LogCategory logCategory, LogLevel logLevel);

		[PreserveSig]
		internal static extern Result EOS_Metrics_BeginPlayerSession(IntPtr handle, ref BeginPlayerSessionOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_Metrics_EndPlayerSession(IntPtr handle, ref EndPlayerSessionOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_Mods_CopyModInfo(IntPtr handle, ref CopyModInfoOptionsInternal options, ref IntPtr outEnumeratedMods);

		[PreserveSig]
		internal static extern void EOS_Mods_EnumerateMods(IntPtr handle, ref EnumerateModsOptionsInternal options, IntPtr clientData, OnEnumerateModsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Mods_InstallMod(IntPtr handle, ref InstallModOptionsInternal options, IntPtr clientData, OnInstallModCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Mods_ModInfo_Release(IntPtr modInfo);

		[PreserveSig]
		internal static extern void EOS_Mods_UninstallMod(IntPtr handle, ref UninstallModOptionsInternal options, IntPtr clientData, OnUninstallModCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Mods_UpdateMod(IntPtr handle, ref UpdateModOptionsInternal options, IntPtr clientData, OnUpdateModCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_P2P_AcceptConnection(IntPtr handle, ref AcceptConnectionOptionsInternal options);

		[PreserveSig]
		internal static extern ulong EOS_P2P_AddNotifyIncomingPacketQueueFull(IntPtr handle, ref AddNotifyIncomingPacketQueueFullOptionsInternal options, IntPtr clientData, OnIncomingPacketQueueFullCallbackInternal incomingPacketQueueFullHandler);

		[PreserveSig]
		internal static extern ulong EOS_P2P_AddNotifyPeerConnectionClosed(IntPtr handle, ref AddNotifyPeerConnectionClosedOptionsInternal options, IntPtr clientData, OnRemoteConnectionClosedCallbackInternal connectionClosedHandler);

		[PreserveSig]
		internal static extern ulong EOS_P2P_AddNotifyPeerConnectionEstablished(IntPtr handle, ref AddNotifyPeerConnectionEstablishedOptionsInternal options, IntPtr clientData, OnPeerConnectionEstablishedCallbackInternal connectionEstablishedHandler);

		[PreserveSig]
		internal static extern ulong EOS_P2P_AddNotifyPeerConnectionInterrupted(IntPtr handle, ref AddNotifyPeerConnectionInterruptedOptionsInternal options, IntPtr clientData, OnPeerConnectionInterruptedCallbackInternal connectionInterruptedHandler);

		[PreserveSig]
		internal static extern ulong EOS_P2P_AddNotifyPeerConnectionRequest(IntPtr handle, ref AddNotifyPeerConnectionRequestOptionsInternal options, IntPtr clientData, OnIncomingConnectionRequestCallbackInternal connectionRequestHandler);

		[PreserveSig]
		internal static extern Result EOS_P2P_ClearPacketQueue(IntPtr handle, ref ClearPacketQueueOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_P2P_CloseConnection(IntPtr handle, ref CloseConnectionOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_P2P_CloseConnections(IntPtr handle, ref CloseConnectionsOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_P2P_GetNATType(IntPtr handle, ref GetNATTypeOptionsInternal options, ref NATType outNATType);

		[PreserveSig]
		internal static extern Result EOS_P2P_GetNextReceivedPacketSize(IntPtr handle, ref GetNextReceivedPacketSizeOptionsInternal options, ref uint outPacketSizeBytes);

		[PreserveSig]
		internal static extern Result EOS_P2P_GetPacketQueueInfo(IntPtr handle, ref GetPacketQueueInfoOptionsInternal options, ref PacketQueueInfoInternal outPacketQueueInfo);

		[PreserveSig]
		internal static extern Result EOS_P2P_GetPortRange(IntPtr handle, ref GetPortRangeOptionsInternal options, ref ushort outPort, ref ushort outNumAdditionalPortsToTry);

		[PreserveSig]
		internal static extern Result EOS_P2P_GetRelayControl(IntPtr handle, ref GetRelayControlOptionsInternal options, ref RelayControl outRelayControl);

		[PreserveSig]
		internal static extern void EOS_P2P_QueryNATType(IntPtr handle, ref QueryNATTypeOptionsInternal options, IntPtr clientData, OnQueryNATTypeCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_P2P_RemoveNotifyIncomingPacketQueueFull(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_P2P_RemoveNotifyPeerConnectionClosed(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_P2P_RemoveNotifyPeerConnectionEstablished(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_P2P_RemoveNotifyPeerConnectionInterrupted(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_P2P_RemoveNotifyPeerConnectionRequest(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern Result EOS_P2P_SendPacket(IntPtr handle, ref SendPacketOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_P2P_SetPacketQueueSize(IntPtr handle, ref SetPacketQueueSizeOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_P2P_SetPortRange(IntPtr handle, ref SetPortRangeOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_P2P_SetRelayControl(IntPtr handle, ref SetRelayControlOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_Platform_CheckForLauncherAndRestart(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_Create(ref Epic.OnlineServices.Platform.OptionsInternal options);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetAchievementsInterface(IntPtr handle);

		[PreserveSig]
		internal static extern Result EOS_Platform_GetActiveCountryCode(IntPtr handle, IntPtr localUserId, IntPtr outBuffer, ref int inOutBufferLength);

		[PreserveSig]
		internal static extern Result EOS_Platform_GetActiveLocaleCode(IntPtr handle, IntPtr localUserId, IntPtr outBuffer, ref int inOutBufferLength);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetAntiCheatClientInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetAntiCheatServerInterface(IntPtr handle);

		[PreserveSig]
		internal static extern ApplicationStatus EOS_Platform_GetApplicationStatus(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetAuthInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetConnectInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetCustomInvitesInterface(IntPtr handle);

		[PreserveSig]
		internal static extern Result EOS_Platform_GetDesktopCrossplayStatus(IntPtr handle, ref GetDesktopCrossplayStatusOptionsInternal options, ref DesktopCrossplayStatusInfoInternal outDesktopCrossplayStatusInfo);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetEcomInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetFriendsInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetIntegratedPlatformInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetInventoryInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetKWSInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetLeaderboardsInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetLobbyInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetMetricsInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetModsInterface(IntPtr handle);

		[PreserveSig]
		internal static extern NetworkStatus EOS_Platform_GetNetworkStatus(IntPtr handle);

		[PreserveSig]
		internal static extern Result EOS_Platform_GetOverrideCountryCode(IntPtr handle, IntPtr outBuffer, ref int inOutBufferLength);

		[PreserveSig]
		internal static extern Result EOS_Platform_GetOverrideLocaleCode(IntPtr handle, IntPtr outBuffer, ref int inOutBufferLength);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetP2PInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetPlayerDataStorageInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetPresenceInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetProgressionSnapshotInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetRTCAdminInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetRTCInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetReceiptValidatorInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetReportsInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetSanctionsInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetSessionsInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetStatsInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetTitleStorageInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetUIInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_GetUserInfoInterface(IntPtr handle);

		[PreserveSig]
		internal static extern void EOS_Platform_Release(IntPtr handle);

		[PreserveSig]
		internal static extern Result EOS_Platform_SetApplicationStatus(IntPtr handle, ApplicationStatus newStatus);

		[PreserveSig]
		internal static extern Result EOS_Platform_SetNetworkStatus(IntPtr handle, NetworkStatus newStatus);

		[PreserveSig]
		internal static extern Result EOS_Platform_SetOverrideCountryCode(IntPtr handle, IntPtr newCountryCode);

		[PreserveSig]
		internal static extern Result EOS_Platform_SetOverrideLocaleCode(IntPtr handle, IntPtr newLocaleCode);

		[PreserveSig]
		internal static extern void EOS_Platform_Tick(IntPtr handle);

		[PreserveSig]
		internal static extern Result EOS_PlayerDataStorageFileTransferRequest_CancelRequest(IntPtr handle);

		[PreserveSig]
		internal static extern Result EOS_PlayerDataStorageFileTransferRequest_GetFileRequestState(IntPtr handle);

		[PreserveSig]
		internal static extern Result EOS_PlayerDataStorageFileTransferRequest_GetFilename(IntPtr handle, uint filenameStringBufferSizeBytes, IntPtr outStringBuffer, ref int outStringLength);

		[PreserveSig]
		internal static extern void EOS_PlayerDataStorageFileTransferRequest_Release(IntPtr playerDataStorageFileTransferHandle);

		[PreserveSig]
		internal static extern Result EOS_PlayerDataStorage_CopyFileMetadataAtIndex(IntPtr handle, ref Epic.OnlineServices.PlayerDataStorage.CopyFileMetadataAtIndexOptionsInternal copyFileMetadataOptions, ref IntPtr outMetadata);

		[PreserveSig]
		internal static extern Result EOS_PlayerDataStorage_CopyFileMetadataByFilename(IntPtr handle, ref Epic.OnlineServices.PlayerDataStorage.CopyFileMetadataByFilenameOptionsInternal copyFileMetadataOptions, ref IntPtr outMetadata);

		[PreserveSig]
		internal static extern Result EOS_PlayerDataStorage_DeleteCache(IntPtr handle, ref Epic.OnlineServices.PlayerDataStorage.DeleteCacheOptionsInternal options, IntPtr clientData, Epic.OnlineServices.PlayerDataStorage.OnDeleteCacheCompleteCallbackInternal completionCallback);

		[PreserveSig]
		internal static extern void EOS_PlayerDataStorage_DeleteFile(IntPtr handle, ref DeleteFileOptionsInternal deleteOptions, IntPtr clientData, OnDeleteFileCompleteCallbackInternal completionCallback);

		[PreserveSig]
		internal static extern void EOS_PlayerDataStorage_DuplicateFile(IntPtr handle, ref DuplicateFileOptionsInternal duplicateOptions, IntPtr clientData, OnDuplicateFileCompleteCallbackInternal completionCallback);

		[PreserveSig]
		internal static extern void EOS_PlayerDataStorage_FileMetadata_Release(IntPtr fileMetadata);

		[PreserveSig]
		internal static extern Result EOS_PlayerDataStorage_GetFileMetadataCount(IntPtr handle, ref Epic.OnlineServices.PlayerDataStorage.GetFileMetadataCountOptionsInternal getFileMetadataCountOptions, ref int outFileMetadataCount);

		[PreserveSig]
		internal static extern void EOS_PlayerDataStorage_QueryFile(IntPtr handle, ref Epic.OnlineServices.PlayerDataStorage.QueryFileOptionsInternal queryFileOptions, IntPtr clientData, Epic.OnlineServices.PlayerDataStorage.OnQueryFileCompleteCallbackInternal completionCallback);

		[PreserveSig]
		internal static extern void EOS_PlayerDataStorage_QueryFileList(IntPtr handle, ref Epic.OnlineServices.PlayerDataStorage.QueryFileListOptionsInternal queryFileListOptions, IntPtr clientData, Epic.OnlineServices.PlayerDataStorage.OnQueryFileListCompleteCallbackInternal completionCallback);

		[PreserveSig]
		internal static extern IntPtr EOS_PlayerDataStorage_ReadFile(IntPtr handle, ref Epic.OnlineServices.PlayerDataStorage.ReadFileOptionsInternal readOptions, IntPtr clientData, Epic.OnlineServices.PlayerDataStorage.OnReadFileCompleteCallbackInternal completionCallback);

		[PreserveSig]
		internal static extern IntPtr EOS_PlayerDataStorage_WriteFile(IntPtr handle, ref WriteFileOptionsInternal writeOptions, IntPtr clientData, OnWriteFileCompleteCallbackInternal completionCallback);

		[PreserveSig]
		internal static extern Result EOS_PresenceModification_DeleteData(IntPtr handle, ref PresenceModificationDeleteDataOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_PresenceModification_Release(IntPtr presenceModificationHandle);

		[PreserveSig]
		internal static extern Result EOS_PresenceModification_SetData(IntPtr handle, ref PresenceModificationSetDataOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_PresenceModification_SetJoinInfo(IntPtr handle, ref PresenceModificationSetJoinInfoOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_PresenceModification_SetRawRichText(IntPtr handle, ref PresenceModificationSetRawRichTextOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_PresenceModification_SetStatus(IntPtr handle, ref PresenceModificationSetStatusOptionsInternal options);

		[PreserveSig]
		internal static extern ulong EOS_Presence_AddNotifyJoinGameAccepted(IntPtr handle, ref AddNotifyJoinGameAcceptedOptionsInternal options, IntPtr clientData, OnJoinGameAcceptedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Presence_AddNotifyOnPresenceChanged(IntPtr handle, ref AddNotifyOnPresenceChangedOptionsInternal options, IntPtr clientData, OnPresenceChangedCallbackInternal notificationHandler);

		[PreserveSig]
		internal static extern Result EOS_Presence_CopyPresence(IntPtr handle, ref CopyPresenceOptionsInternal options, ref IntPtr outPresence);

		[PreserveSig]
		internal static extern Result EOS_Presence_CreatePresenceModification(IntPtr handle, ref CreatePresenceModificationOptionsInternal options, ref IntPtr outPresenceModificationHandle);

		[PreserveSig]
		internal static extern Result EOS_Presence_GetJoinInfo(IntPtr handle, ref GetJoinInfoOptionsInternal options, IntPtr outBuffer, ref int inOutBufferLength);

		[PreserveSig]
		internal static extern int EOS_Presence_HasPresence(IntPtr handle, ref HasPresenceOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_Presence_Info_Release(IntPtr presenceInfo);

		[PreserveSig]
		internal static extern void EOS_Presence_QueryPresence(IntPtr handle, ref QueryPresenceOptionsInternal options, IntPtr clientData, OnQueryPresenceCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Presence_RemoveNotifyJoinGameAccepted(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Presence_RemoveNotifyOnPresenceChanged(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_Presence_SetPresence(IntPtr handle, ref SetPresenceOptionsInternal options, IntPtr clientData, SetPresenceCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern IntPtr EOS_ProductUserId_FromString(IntPtr productUserIdString);

		[PreserveSig]
		internal static extern int EOS_ProductUserId_IsValid(IntPtr accountId);

		[PreserveSig]
		internal static extern Result EOS_ProductUserId_ToString(IntPtr accountId, IntPtr outBuffer, ref int inOutBufferLength);

		[PreserveSig]
		internal static extern Result EOS_ProgressionSnapshot_AddProgression(IntPtr handle, ref AddProgressionOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_ProgressionSnapshot_BeginSnapshot(IntPtr handle, ref BeginSnapshotOptionsInternal options, ref uint outSnapshotId);

		[PreserveSig]
		internal static extern void EOS_ProgressionSnapshot_DeleteSnapshot(IntPtr handle, ref DeleteSnapshotOptionsInternal options, IntPtr clientData, OnDeleteSnapshotCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_ProgressionSnapshot_EndSnapshot(IntPtr handle, ref EndSnapshotOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_ProgressionSnapshot_SubmitSnapshot(IntPtr handle, ref SubmitSnapshotOptionsInternal options, IntPtr clientData, OnSubmitSnapshotCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_RTCAdmin_CopyUserTokenByIndex(IntPtr handle, ref CopyUserTokenByIndexOptionsInternal options, ref IntPtr outUserToken);

		[PreserveSig]
		internal static extern Result EOS_RTCAdmin_CopyUserTokenByUserId(IntPtr handle, ref CopyUserTokenByUserIdOptionsInternal options, ref IntPtr outUserToken);

		[PreserveSig]
		internal static extern void EOS_RTCAdmin_Kick(IntPtr handle, ref KickOptionsInternal options, IntPtr clientData, OnKickCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTCAdmin_QueryJoinRoomToken(IntPtr handle, ref QueryJoinRoomTokenOptionsInternal options, IntPtr clientData, OnQueryJoinRoomTokenCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTCAdmin_SetParticipantHardMute(IntPtr handle, ref SetParticipantHardMuteOptionsInternal options, IntPtr clientData, OnSetParticipantHardMuteCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTCAdmin_UserToken_Release(IntPtr userToken);

		[PreserveSig]
		internal static extern ulong EOS_RTCAudio_AddNotifyAudioBeforeRender(IntPtr handle, ref AddNotifyAudioBeforeRenderOptionsInternal options, IntPtr clientData, OnAudioBeforeRenderCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern ulong EOS_RTCAudio_AddNotifyAudioBeforeSend(IntPtr handle, ref AddNotifyAudioBeforeSendOptionsInternal options, IntPtr clientData, OnAudioBeforeSendCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern ulong EOS_RTCAudio_AddNotifyAudioDevicesChanged(IntPtr handle, ref AddNotifyAudioDevicesChangedOptionsInternal options, IntPtr clientData, OnAudioDevicesChangedCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern ulong EOS_RTCAudio_AddNotifyAudioInputState(IntPtr handle, ref AddNotifyAudioInputStateOptionsInternal options, IntPtr clientData, OnAudioInputStateCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern ulong EOS_RTCAudio_AddNotifyAudioOutputState(IntPtr handle, ref AddNotifyAudioOutputStateOptionsInternal options, IntPtr clientData, OnAudioOutputStateCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern ulong EOS_RTCAudio_AddNotifyParticipantUpdated(IntPtr handle, ref Epic.OnlineServices.RTCAudio.AddNotifyParticipantUpdatedOptionsInternal options, IntPtr clientData, Epic.OnlineServices.RTCAudio.OnParticipantUpdatedCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_RTCAudio_CopyInputDeviceInformationByIndex(IntPtr handle, ref CopyInputDeviceInformationByIndexOptionsInternal options, ref IntPtr outInputDeviceInformation);

		[PreserveSig]
		internal static extern Result EOS_RTCAudio_CopyOutputDeviceInformationByIndex(IntPtr handle, ref CopyOutputDeviceInformationByIndexOptionsInternal options, ref IntPtr outOutputDeviceInformation);

		[PreserveSig]
		internal static extern IntPtr EOS_RTCAudio_GetAudioInputDeviceByIndex(IntPtr handle, ref GetAudioInputDeviceByIndexOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_RTCAudio_GetAudioInputDevicesCount(IntPtr handle, ref GetAudioInputDevicesCountOptionsInternal options);

		[PreserveSig]
		internal static extern IntPtr EOS_RTCAudio_GetAudioOutputDeviceByIndex(IntPtr handle, ref GetAudioOutputDeviceByIndexOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_RTCAudio_GetAudioOutputDevicesCount(IntPtr handle, ref GetAudioOutputDevicesCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_RTCAudio_GetInputDevicesCount(IntPtr handle, ref GetInputDevicesCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_RTCAudio_GetOutputDevicesCount(IntPtr handle, ref GetOutputDevicesCountOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_InputDeviceInformation_Release(IntPtr deviceInformation);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_OutputDeviceInformation_Release(IntPtr deviceInformation);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_QueryInputDevicesInformation(IntPtr handle, ref QueryInputDevicesInformationOptionsInternal options, IntPtr clientData, OnQueryInputDevicesInformationCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_QueryOutputDevicesInformation(IntPtr handle, ref QueryOutputDevicesInformationOptionsInternal options, IntPtr clientData, OnQueryOutputDevicesInformationCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_RTCAudio_RegisterPlatformAudioUser(IntPtr handle, ref RegisterPlatformAudioUserOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_RegisterPlatformUser(IntPtr handle, ref RegisterPlatformUserOptionsInternal options, IntPtr clientData, OnRegisterPlatformUserCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_RemoveNotifyAudioBeforeRender(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_RemoveNotifyAudioBeforeSend(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_RemoveNotifyAudioDevicesChanged(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_RemoveNotifyAudioInputState(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_RemoveNotifyAudioOutputState(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_RemoveNotifyParticipantUpdated(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern Result EOS_RTCAudio_SendAudio(IntPtr handle, ref SendAudioOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_RTCAudio_SetAudioInputSettings(IntPtr handle, ref SetAudioInputSettingsOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_RTCAudio_SetAudioOutputSettings(IntPtr handle, ref SetAudioOutputSettingsOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_SetInputDeviceSettings(IntPtr handle, ref SetInputDeviceSettingsOptionsInternal options, IntPtr clientData, OnSetInputDeviceSettingsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_SetOutputDeviceSettings(IntPtr handle, ref SetOutputDeviceSettingsOptionsInternal options, IntPtr clientData, OnSetOutputDeviceSettingsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_RTCAudio_UnregisterPlatformAudioUser(IntPtr handle, ref UnregisterPlatformAudioUserOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_UnregisterPlatformUser(IntPtr handle, ref UnregisterPlatformUserOptionsInternal options, IntPtr clientData, OnUnregisterPlatformUserCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_UpdateParticipantVolume(IntPtr handle, ref UpdateParticipantVolumeOptionsInternal options, IntPtr clientData, OnUpdateParticipantVolumeCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_UpdateReceiving(IntPtr handle, ref Epic.OnlineServices.RTCAudio.UpdateReceivingOptionsInternal options, IntPtr clientData, Epic.OnlineServices.RTCAudio.OnUpdateReceivingCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_UpdateReceivingVolume(IntPtr handle, ref UpdateReceivingVolumeOptionsInternal options, IntPtr clientData, OnUpdateReceivingVolumeCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_UpdateSending(IntPtr handle, ref Epic.OnlineServices.RTCAudio.UpdateSendingOptionsInternal options, IntPtr clientData, Epic.OnlineServices.RTCAudio.OnUpdateSendingCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTCAudio_UpdateSendingVolume(IntPtr handle, ref UpdateSendingVolumeOptionsInternal options, IntPtr clientData, OnUpdateSendingVolumeCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern ulong EOS_RTCData_AddNotifyDataReceived(IntPtr handle, ref AddNotifyDataReceivedOptionsInternal options, IntPtr clientData, OnDataReceivedCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern ulong EOS_RTCData_AddNotifyParticipantUpdated(IntPtr handle, ref Epic.OnlineServices.RTCData.AddNotifyParticipantUpdatedOptionsInternal options, IntPtr clientData, Epic.OnlineServices.RTCData.OnParticipantUpdatedCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTCData_RemoveNotifyDataReceived(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_RTCData_RemoveNotifyParticipantUpdated(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern Result EOS_RTCData_SendData(IntPtr handle, ref SendDataOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_RTCData_UpdateReceiving(IntPtr handle, ref Epic.OnlineServices.RTCData.UpdateReceivingOptionsInternal options, IntPtr clientData, Epic.OnlineServices.RTCData.OnUpdateReceivingCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTCData_UpdateSending(IntPtr handle, ref Epic.OnlineServices.RTCData.UpdateSendingOptionsInternal options, IntPtr clientData, Epic.OnlineServices.RTCData.OnUpdateSendingCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern ulong EOS_RTC_AddNotifyDisconnected(IntPtr handle, ref AddNotifyDisconnectedOptionsInternal options, IntPtr clientData, OnDisconnectedCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern ulong EOS_RTC_AddNotifyParticipantStatusChanged(IntPtr handle, ref AddNotifyParticipantStatusChangedOptionsInternal options, IntPtr clientData, OnParticipantStatusChangedCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern ulong EOS_RTC_AddNotifyRoomStatisticsUpdated(IntPtr handle, ref AddNotifyRoomStatisticsUpdatedOptionsInternal options, IntPtr clientData, OnRoomStatisticsUpdatedCallbackInternal statisticsUpdateHandler);

		[PreserveSig]
		internal static extern void EOS_RTC_BlockParticipant(IntPtr handle, ref BlockParticipantOptionsInternal options, IntPtr clientData, OnBlockParticipantCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern IntPtr EOS_RTC_GetAudioInterface(IntPtr handle);

		[PreserveSig]
		internal static extern IntPtr EOS_RTC_GetDataInterface(IntPtr handle);

		[PreserveSig]
		internal static extern void EOS_RTC_JoinRoom(IntPtr handle, ref JoinRoomOptionsInternal options, IntPtr clientData, OnJoinRoomCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTC_LeaveRoom(IntPtr handle, ref LeaveRoomOptionsInternal options, IntPtr clientData, OnLeaveRoomCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_RTC_RemoveNotifyDisconnected(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_RTC_RemoveNotifyParticipantStatusChanged(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern void EOS_RTC_RemoveNotifyRoomStatisticsUpdated(IntPtr handle, ulong notificationId);

		[PreserveSig]
		internal static extern Result EOS_RTC_SetRoomSetting(IntPtr handle, ref SetRoomSettingOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_RTC_SetSetting(IntPtr handle, ref SetSettingOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_ReceiptValidator_VerifyPurchase(IntPtr handle, IntPtr options, IntPtr clientData, OnVerifyPurchaseCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Reports_SendPlayerBehaviorReport(IntPtr handle, ref SendPlayerBehaviorReportOptionsInternal options, IntPtr clientData, OnSendPlayerBehaviorReportCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_Sanctions_CopyPlayerSanctionByIndex(IntPtr handle, ref CopyPlayerSanctionByIndexOptionsInternal options, ref IntPtr outSanction);

		[PreserveSig]
		internal static extern void EOS_Sanctions_CreatePlayerSanctionAppeal(IntPtr handle, ref CreatePlayerSanctionAppealOptionsInternal options, IntPtr clientData, CreatePlayerSanctionAppealCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern uint EOS_Sanctions_GetPlayerSanctionCount(IntPtr handle, ref GetPlayerSanctionCountOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_Sanctions_PlayerSanction_Release(IntPtr sanction);

		[PreserveSig]
		internal static extern void EOS_Sanctions_QueryActivePlayerSanctions(IntPtr handle, ref QueryActivePlayerSanctionsOptionsInternal options, IntPtr clientData, OnQueryActivePlayerSanctionsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_SessionDetails_Attribute_Release(IntPtr sessionAttribute);

		[PreserveSig]
		internal static extern Result EOS_SessionDetails_CopyInfo(IntPtr handle, ref SessionDetailsCopyInfoOptionsInternal options, ref IntPtr outSessionInfo);

		[PreserveSig]
		internal static extern Result EOS_SessionDetails_CopySessionAttributeByIndex(IntPtr handle, ref SessionDetailsCopySessionAttributeByIndexOptionsInternal options, ref IntPtr outSessionAttribute);

		[PreserveSig]
		internal static extern Result EOS_SessionDetails_CopySessionAttributeByKey(IntPtr handle, ref SessionDetailsCopySessionAttributeByKeyOptionsInternal options, ref IntPtr outSessionAttribute);

		[PreserveSig]
		internal static extern uint EOS_SessionDetails_GetSessionAttributeCount(IntPtr handle, ref SessionDetailsGetSessionAttributeCountOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_SessionDetails_Info_Release(IntPtr sessionInfo);

		[PreserveSig]
		internal static extern void EOS_SessionDetails_Release(IntPtr sessionHandle);

		[PreserveSig]
		internal static extern Result EOS_SessionModification_AddAttribute(IntPtr handle, ref SessionModificationAddAttributeOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_SessionModification_Release(IntPtr sessionModificationHandle);

		[PreserveSig]
		internal static extern Result EOS_SessionModification_RemoveAttribute(IntPtr handle, ref SessionModificationRemoveAttributeOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_SessionModification_SetAllowedPlatformIds(IntPtr handle, ref SessionModificationSetAllowedPlatformIdsOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_SessionModification_SetBucketId(IntPtr handle, ref SessionModificationSetBucketIdOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_SessionModification_SetHostAddress(IntPtr handle, ref SessionModificationSetHostAddressOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_SessionModification_SetInvitesAllowed(IntPtr handle, ref SessionModificationSetInvitesAllowedOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_SessionModification_SetJoinInProgressAllowed(IntPtr handle, ref SessionModificationSetJoinInProgressAllowedOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_SessionModification_SetMaxPlayers(IntPtr handle, ref SessionModificationSetMaxPlayersOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_SessionModification_SetPermissionLevel(IntPtr handle, ref SessionModificationSetPermissionLevelOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_SessionSearch_CopySearchResultByIndex(IntPtr handle, ref SessionSearchCopySearchResultByIndexOptionsInternal options, ref IntPtr outSessionHandle);

		[PreserveSig]
		internal static extern void EOS_SessionSearch_Find(IntPtr handle, ref SessionSearchFindOptionsInternal options, IntPtr clientData, SessionSearchOnFindCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern uint EOS_SessionSearch_GetSearchResultCount(IntPtr handle, ref SessionSearchGetSearchResultCountOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_SessionSearch_Release(IntPtr sessionSearchHandle);

		[PreserveSig]
		internal static extern Result EOS_SessionSearch_RemoveParameter(IntPtr handle, ref SessionSearchRemoveParameterOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_SessionSearch_SetMaxResults(IntPtr handle, ref SessionSearchSetMaxResultsOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_SessionSearch_SetParameter(IntPtr handle, ref SessionSearchSetParameterOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_SessionSearch_SetSessionId(IntPtr handle, ref SessionSearchSetSessionIdOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_SessionSearch_SetTargetUserId(IntPtr handle, ref SessionSearchSetTargetUserIdOptionsInternal options);

		[PreserveSig]
		internal static extern ulong EOS_Sessions_AddNotifyJoinSessionAccepted(IntPtr handle, ref AddNotifyJoinSessionAcceptedOptionsInternal options, IntPtr clientData, OnJoinSessionAcceptedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Sessions_AddNotifyLeaveSessionRequested(IntPtr handle, ref AddNotifyLeaveSessionRequestedOptionsInternal options, IntPtr clientData, OnLeaveSessionRequestedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Sessions_AddNotifySendSessionNativeInviteRequested(IntPtr handle, ref AddNotifySendSessionNativeInviteRequestedOptionsInternal options, IntPtr clientData, OnSendSessionNativeInviteRequestedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Sessions_AddNotifySessionInviteAccepted(IntPtr handle, ref AddNotifySessionInviteAcceptedOptionsInternal options, IntPtr clientData, OnSessionInviteAcceptedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Sessions_AddNotifySessionInviteReceived(IntPtr handle, ref AddNotifySessionInviteReceivedOptionsInternal options, IntPtr clientData, OnSessionInviteReceivedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_Sessions_AddNotifySessionInviteRejected(IntPtr handle, ref AddNotifySessionInviteRejectedOptionsInternal options, IntPtr clientData, OnSessionInviteRejectedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern Result EOS_Sessions_CopyActiveSessionHandle(IntPtr handle, ref CopyActiveSessionHandleOptionsInternal options, ref IntPtr outSessionHandle);

		[PreserveSig]
		internal static extern Result EOS_Sessions_CopySessionHandleByInviteId(IntPtr handle, ref CopySessionHandleByInviteIdOptionsInternal options, ref IntPtr outSessionHandle);

		[PreserveSig]
		internal static extern Result EOS_Sessions_CopySessionHandleByUiEventId(IntPtr handle, ref CopySessionHandleByUiEventIdOptionsInternal options, ref IntPtr outSessionHandle);

		[PreserveSig]
		internal static extern Result EOS_Sessions_CopySessionHandleForPresence(IntPtr handle, ref CopySessionHandleForPresenceOptionsInternal options, ref IntPtr outSessionHandle);

		[PreserveSig]
		internal static extern Result EOS_Sessions_CreateSessionModification(IntPtr handle, ref CreateSessionModificationOptionsInternal options, ref IntPtr outSessionModificationHandle);

		[PreserveSig]
		internal static extern Result EOS_Sessions_CreateSessionSearch(IntPtr handle, ref CreateSessionSearchOptionsInternal options, ref IntPtr outSessionSearchHandle);

		[PreserveSig]
		internal static extern void EOS_Sessions_DestroySession(IntPtr handle, ref DestroySessionOptionsInternal options, IntPtr clientData, OnDestroySessionCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_Sessions_DumpSessionState(IntPtr handle, ref DumpSessionStateOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_Sessions_EndSession(IntPtr handle, ref Epic.OnlineServices.Sessions.EndSessionOptionsInternal options, IntPtr clientData, OnEndSessionCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern uint EOS_Sessions_GetInviteCount(IntPtr handle, ref Epic.OnlineServices.Sessions.GetInviteCountOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_Sessions_GetInviteIdByIndex(IntPtr handle, ref Epic.OnlineServices.Sessions.GetInviteIdByIndexOptionsInternal options, IntPtr outBuffer, ref int inOutBufferLength);

		[PreserveSig]
		internal static extern Result EOS_Sessions_IsUserInSession(IntPtr handle, ref IsUserInSessionOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_Sessions_JoinSession(IntPtr handle, ref JoinSessionOptionsInternal options, IntPtr clientData, OnJoinSessionCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Sessions_QueryInvites(IntPtr handle, ref Epic.OnlineServices.Sessions.QueryInvitesOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Sessions.OnQueryInvitesCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Sessions_RegisterPlayers(IntPtr handle, ref RegisterPlayersOptionsInternal options, IntPtr clientData, OnRegisterPlayersCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Sessions_RejectInvite(IntPtr handle, ref Epic.OnlineServices.Sessions.RejectInviteOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Sessions.OnRejectInviteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Sessions_RemoveNotifyJoinSessionAccepted(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Sessions_RemoveNotifyLeaveSessionRequested(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Sessions_RemoveNotifySendSessionNativeInviteRequested(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Sessions_RemoveNotifySessionInviteAccepted(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Sessions_RemoveNotifySessionInviteReceived(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Sessions_RemoveNotifySessionInviteRejected(IntPtr handle, ulong inId);

		[PreserveSig]
		internal static extern void EOS_Sessions_SendInvite(IntPtr handle, ref Epic.OnlineServices.Sessions.SendInviteOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Sessions.OnSendInviteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Sessions_StartSession(IntPtr handle, ref StartSessionOptionsInternal options, IntPtr clientData, OnStartSessionCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Sessions_UnregisterPlayers(IntPtr handle, ref UnregisterPlayersOptionsInternal options, IntPtr clientData, OnUnregisterPlayersCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Sessions_UpdateSession(IntPtr handle, ref UpdateSessionOptionsInternal options, IntPtr clientData, OnUpdateSessionCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern Result EOS_Sessions_UpdateSessionModification(IntPtr handle, ref UpdateSessionModificationOptionsInternal options, ref IntPtr outSessionModificationHandle);

		[PreserveSig]
		internal static extern Result EOS_Shutdown();

		[PreserveSig]
		internal static extern Result EOS_Stats_CopyStatByIndex(IntPtr handle, ref CopyStatByIndexOptionsInternal options, ref IntPtr outStat);

		[PreserveSig]
		internal static extern Result EOS_Stats_CopyStatByName(IntPtr handle, ref CopyStatByNameOptionsInternal options, ref IntPtr outStat);

		[PreserveSig]
		internal static extern uint EOS_Stats_GetStatsCount(IntPtr handle, ref GetStatCountOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_Stats_IngestStat(IntPtr handle, ref IngestStatOptionsInternal options, IntPtr clientData, OnIngestStatCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Stats_QueryStats(IntPtr handle, ref QueryStatsOptionsInternal options, IntPtr clientData, OnQueryStatsCompleteCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_Stats_Stat_Release(IntPtr stat);

		[PreserveSig]
		internal static extern Result EOS_TitleStorageFileTransferRequest_CancelRequest(IntPtr handle);

		[PreserveSig]
		internal static extern Result EOS_TitleStorageFileTransferRequest_GetFileRequestState(IntPtr handle);

		[PreserveSig]
		internal static extern Result EOS_TitleStorageFileTransferRequest_GetFilename(IntPtr handle, uint filenameStringBufferSizeBytes, IntPtr outStringBuffer, ref int outStringLength);

		[PreserveSig]
		internal static extern void EOS_TitleStorageFileTransferRequest_Release(IntPtr titleStorageFileTransferHandle);

		[PreserveSig]
		internal static extern Result EOS_TitleStorage_CopyFileMetadataAtIndex(IntPtr handle, ref Epic.OnlineServices.TitleStorage.CopyFileMetadataAtIndexOptionsInternal options, ref IntPtr outMetadata);

		[PreserveSig]
		internal static extern Result EOS_TitleStorage_CopyFileMetadataByFilename(IntPtr handle, ref Epic.OnlineServices.TitleStorage.CopyFileMetadataByFilenameOptionsInternal options, ref IntPtr outMetadata);

		[PreserveSig]
		internal static extern Result EOS_TitleStorage_DeleteCache(IntPtr handle, ref Epic.OnlineServices.TitleStorage.DeleteCacheOptionsInternal options, IntPtr clientData, Epic.OnlineServices.TitleStorage.OnDeleteCacheCompleteCallbackInternal completionCallback);

		[PreserveSig]
		internal static extern void EOS_TitleStorage_FileMetadata_Release(IntPtr fileMetadata);

		[PreserveSig]
		internal static extern uint EOS_TitleStorage_GetFileMetadataCount(IntPtr handle, ref Epic.OnlineServices.TitleStorage.GetFileMetadataCountOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_TitleStorage_QueryFile(IntPtr handle, ref Epic.OnlineServices.TitleStorage.QueryFileOptionsInternal options, IntPtr clientData, Epic.OnlineServices.TitleStorage.OnQueryFileCompleteCallbackInternal completionCallback);

		[PreserveSig]
		internal static extern void EOS_TitleStorage_QueryFileList(IntPtr handle, ref Epic.OnlineServices.TitleStorage.QueryFileListOptionsInternal options, IntPtr clientData, Epic.OnlineServices.TitleStorage.OnQueryFileListCompleteCallbackInternal completionCallback);

		[PreserveSig]
		internal static extern IntPtr EOS_TitleStorage_ReadFile(IntPtr handle, ref Epic.OnlineServices.TitleStorage.ReadFileOptionsInternal options, IntPtr clientData, Epic.OnlineServices.TitleStorage.OnReadFileCompleteCallbackInternal completionCallback);

		[PreserveSig]
		internal static extern Result EOS_UI_AcknowledgeEventId(IntPtr handle, ref AcknowledgeEventIdOptionsInternal options);

		[PreserveSig]
		internal static extern ulong EOS_UI_AddNotifyDisplaySettingsUpdated(IntPtr handle, ref AddNotifyDisplaySettingsUpdatedOptionsInternal options, IntPtr clientData, OnDisplaySettingsUpdatedCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern ulong EOS_UI_AddNotifyMemoryMonitor(IntPtr handle, ref AddNotifyMemoryMonitorOptionsInternal options, IntPtr clientData, OnMemoryMonitorCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern int EOS_UI_GetFriendsExclusiveInput(IntPtr handle, ref GetFriendsExclusiveInputOptionsInternal options);

		[PreserveSig]
		internal static extern int EOS_UI_GetFriendsVisible(IntPtr handle, ref GetFriendsVisibleOptionsInternal options);

		[PreserveSig]
		internal static extern NotificationLocation EOS_UI_GetNotificationLocationPreference(IntPtr handle);

		[PreserveSig]
		internal static extern InputStateButtonFlags EOS_UI_GetToggleFriendsButton(IntPtr handle, ref GetToggleFriendsButtonOptionsInternal options);

		[PreserveSig]
		internal static extern KeyCombination EOS_UI_GetToggleFriendsKey(IntPtr handle, ref GetToggleFriendsKeyOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_UI_HideFriends(IntPtr handle, ref HideFriendsOptionsInternal options, IntPtr clientData, OnHideFriendsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern int EOS_UI_IsSocialOverlayPaused(IntPtr handle, ref IsSocialOverlayPausedOptionsInternal options);

		[PreserveSig]
		internal static extern int EOS_UI_IsValidButtonCombination(IntPtr handle, InputStateButtonFlags buttonCombination);

		[PreserveSig]
		internal static extern int EOS_UI_IsValidKeyCombination(IntPtr handle, KeyCombination keyCombination);

		[PreserveSig]
		internal static extern Result EOS_UI_PauseSocialOverlay(IntPtr handle, ref PauseSocialOverlayOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_UI_PrePresent(IntPtr handle, ref PrePresentOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_UI_RemoveNotifyDisplaySettingsUpdated(IntPtr handle, ulong id);

		[PreserveSig]
		internal static extern void EOS_UI_RemoveNotifyMemoryMonitor(IntPtr handle, ulong id);

		[PreserveSig]
		internal static extern Result EOS_UI_ReportInputState(IntPtr handle, ref ReportInputStateOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_UI_SetDisplayPreference(IntPtr handle, ref SetDisplayPreferenceOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_UI_SetToggleFriendsButton(IntPtr handle, ref SetToggleFriendsButtonOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_UI_SetToggleFriendsKey(IntPtr handle, ref SetToggleFriendsKeyOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_UI_ShowBlockPlayer(IntPtr handle, ref ShowBlockPlayerOptionsInternal options, IntPtr clientData, OnShowBlockPlayerCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_UI_ShowFriends(IntPtr handle, ref ShowFriendsOptionsInternal options, IntPtr clientData, OnShowFriendsCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_UI_ShowNativeProfile(IntPtr handle, ref ShowNativeProfileOptionsInternal options, IntPtr clientData, OnShowNativeProfileCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_UI_ShowReportPlayer(IntPtr handle, ref ShowReportPlayerOptionsInternal options, IntPtr clientData, OnShowReportPlayerCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_UserInfo_BestDisplayName_Release(IntPtr bestDisplayName);

		[PreserveSig]
		internal static extern Result EOS_UserInfo_CopyBestDisplayName(IntPtr handle, ref CopyBestDisplayNameOptionsInternal options, ref IntPtr outBestDisplayName);

		[PreserveSig]
		internal static extern Result EOS_UserInfo_CopyBestDisplayNameWithPlatform(IntPtr handle, ref CopyBestDisplayNameWithPlatformOptionsInternal options, ref IntPtr outBestDisplayName);

		[PreserveSig]
		internal static extern Result EOS_UserInfo_CopyExternalUserInfoByAccountId(IntPtr handle, ref CopyExternalUserInfoByAccountIdOptionsInternal options, ref IntPtr outExternalUserInfo);

		[PreserveSig]
		internal static extern Result EOS_UserInfo_CopyExternalUserInfoByAccountType(IntPtr handle, ref CopyExternalUserInfoByAccountTypeOptionsInternal options, ref IntPtr outExternalUserInfo);

		[PreserveSig]
		internal static extern Result EOS_UserInfo_CopyExternalUserInfoByIndex(IntPtr handle, ref CopyExternalUserInfoByIndexOptionsInternal options, ref IntPtr outExternalUserInfo);

		[PreserveSig]
		internal static extern Result EOS_UserInfo_CopyUserInfo(IntPtr handle, ref CopyUserInfoOptionsInternal options, ref IntPtr outUserInfo);

		[PreserveSig]
		internal static extern void EOS_UserInfo_ExternalUserInfo_Release(IntPtr externalUserInfo);

		[PreserveSig]
		internal static extern uint EOS_UserInfo_GetExternalUserInfoCount(IntPtr handle, ref GetExternalUserInfoCountOptionsInternal options);

		[PreserveSig]
		internal static extern uint EOS_UserInfo_GetLocalPlatformType(IntPtr handle, ref GetLocalPlatformTypeOptionsInternal options);

		[PreserveSig]
		internal static extern void EOS_UserInfo_QueryUserInfo(IntPtr handle, ref QueryUserInfoOptionsInternal options, IntPtr clientData, OnQueryUserInfoCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_UserInfo_QueryUserInfoByDisplayName(IntPtr handle, ref QueryUserInfoByDisplayNameOptionsInternal options, IntPtr clientData, OnQueryUserInfoByDisplayNameCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_UserInfo_QueryUserInfoByExternalAccount(IntPtr handle, ref QueryUserInfoByExternalAccountOptionsInternal options, IntPtr clientData, OnQueryUserInfoByExternalAccountCallbackInternal completionDelegate);

		[PreserveSig]
		internal static extern void EOS_UserInfo_Release(IntPtr userInfo);

		[PreserveSig]
		internal static extern Result EOS_P2P_ReceivePacket(IntPtr handle, ref ReceivePacketOptionsInternal options, ref IntPtr outPeerId, IntPtr outSocketId, ref byte outChannel, IntPtr outData, ref uint outBytesWritten);
	}
}
