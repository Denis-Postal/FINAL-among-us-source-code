using System;
using System.Runtime.CompilerServices;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using Hazel;

namespace InnerNet
{
	[Serializable]
	public struct GameListing
	{
		public uint IP;

		public ushort Port;

		public int GameId;

		public byte PlayerCount;

		public string HostName;

		public string TrueHostName;

		public string HostPlatformName;

		public QuickChatModes QuickChat;

		public Platforms Platform;

		public int Age;

		public int MaxPlayers;

		public int NumImpostors;

		public byte MapId;

		public uint Language;

		public IGameOptions Options;

		public string IPString => InnerNetClient.AddressToString(IP);

		public unsafe static GameListing DeserializeV2(MessageReader reader)
		{
			return new GameListing
			{
				IP = reader.ReadUInt32(),
				Port = reader.ReadUInt16(),
				GameId = reader.ReadInt32(),
				HostName = reader.ReadString(),
				PlayerCount = reader.ReadByte(),
				Age = reader.ReadPackedInt32(),
				MapId = reader.ReadByte(),
				NumImpostors = reader.ReadByte(),
				MaxPlayers = reader.ReadByte(),
				Platform = (Platforms)reader.ReadByte(),
				HostPlatformName = reader.ReadString(),
				Language = reader.ReadUInt32()
			};
		}
	}
}
