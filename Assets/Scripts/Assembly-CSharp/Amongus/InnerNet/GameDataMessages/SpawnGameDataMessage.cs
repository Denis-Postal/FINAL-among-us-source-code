using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using Hazel;
using InnerNet;
using UnityEngine;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class SpawnGameDataMessage : BaseGameDataMessage
	{
		private readonly uint spawnTypeId;

		private readonly int ownerId;

		private readonly SpawnFlags flags;

		private readonly InnerNetObject[] childNetObjects;

		public override GameDataTypes GameDataType { get; } = GameDataTypes.SpawnFlag;

		public Type NetObjectType { get; private set; }

		public SpawnGameDataMessage(InnerNetObject parentNetObject, int ownerId, SpawnFlags flags, InnerNetObject[] childNetObjects)
		{
			NetObjectType = parentNetObject.GetType();
			spawnTypeId = parentNetObject.SpawnId;
			this.ownerId = ownerId;
			this.flags = flags;
			this.childNetObjects = childNetObjects;
		}

		public void ClearOrDecrementChildObjectDirt()
		{
			int num = childNetObjects.Length;
			for (int i = 0; i < num; i++)
			{
				childNetObjects[i]?.ClearOrDecrementDirt();
			}
		}

		protected override void SerializeValues(MessageWriter msg)
		{
			msg.WritePacked(spawnTypeId);
			msg.WritePacked(ownerId);
			msg.Write((byte)flags);
			int num = childNetObjects.Length;
			msg.WritePacked(num);
			for (int i = 0; i < num; i++)
			{
				InnerNetObject innerNetObject = childNetObjects[i];
				if (innerNetObject == null)
				{
					Logger.GlobalInstance.Error($"WHAT!? child net obj on spawn with id {spawnTypeId}, owner id {ownerId}, and flags {flags} is now null during serialize values");
					continue;
				}
				msg.WritePacked(innerNetObject.NetId);
				msg.StartMessage(1);
				innerNetObject.Serialize(msg, initialState: true);
				msg.EndMessage();
			}
		}
	}
}
