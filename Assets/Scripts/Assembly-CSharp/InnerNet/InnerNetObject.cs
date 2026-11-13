using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using Hazel;
using UnityEngine;

namespace InnerNet
{
	public abstract class InnerNetObject : MonoBehaviour, IComparable<InnerNetObject>
	{
		public uint SpawnId;

		public uint NetId;

		protected uint DirtyBits;

		public SpawnFlags SpawnFlags;

		public SendOption sendMode = SendOption.Reliable;

		public int OwnerId;

		protected bool DespawnOnDestroy = true;

		public virtual bool IsDirty => DirtyBits != 0;

		public bool AmOwner => OwnerId == AmongUsClient.Instance.ClientId;

		public void Despawn()
		{
			AmongUsClient.Instance.Despawn(this);
			UnityEngine.Object.Destroy(gameObject);
		}

		public virtual void OnDestroy()
		{
			if ((bool)AmongUsClient.Instance && NetId != uint.MaxValue)
			{
				if (DespawnOnDestroy && (AmOwner || (OwnerId == -2 && AmongUsClient.Instance.AmHost)))
				{
					AmongUsClient.Instance.Despawn(this);
				}
				else
				{
					AmongUsClient.Instance.RemoveNetObject(this);
				}
			}
		}

		public abstract void HandleRpc(byte callId, MessageReader reader);

		public abstract void ClearOrDecrementDirt();

		public abstract bool Serialize(MessageWriter writer, bool initialState);

		public abstract void Deserialize(MessageReader reader, bool initialState);

		public int CompareTo(InnerNetObject other)
		{ /* spliced-from-2019 */
			if (NetId > other.NetId)
			{
				return 1;
			}
			if (NetId < other.NetId)
			{
				return -1;
			}
			return 0;
		}

		protected bool IsDirtyBitSet(int idx)
		{
			return (DirtyBits & (uint)(1 << idx)) != 0;
		}

		protected void ClearDirtyBits()
		{
			DirtyBits = 0u;
		}

		protected void UnsetDirtyBit(uint val)
		{
			DirtyBits &= ~val;
		}

		public void SetDirtyBit(uint val)
		{
			DirtyBits |= val;
		}
	}
}
