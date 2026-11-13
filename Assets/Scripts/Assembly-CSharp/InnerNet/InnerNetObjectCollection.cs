using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using Hazel;
using UnityEngine;

namespace InnerNet
{
	public class InnerNetObjectCollection
	{
		public readonly List<InnerNetObject> allObjects= new List<InnerNetObject>();

		public readonly List<InnerNetObject> allReliableObjects= new List<InnerNetObject>();

		public readonly List<InnerNetObject> allUnreliableObjects= new List<InnerNetObject>();

		public readonly Dictionary<uint, InnerNetObject> allObjectsFast= new Dictionary<uint, InnerNetObject>();

		public readonly Logger logger= new Logger(Logger.Category.Server);

		public IReadOnlyList<InnerNetObject> AllObjects => allObjects;

		public IReadOnlyList<InnerNetObject> AllReliableObjects => allReliableObjects;

		public IReadOnlyList<InnerNetObject> AllUnreliableObjects => allUnreliableObjects;

		public IReadOnlyDictionary<uint, InnerNetObject> AllObjectsFast => allObjectsFast;

		public void Clear()
		{
			allObjects.Clear();
			allReliableObjects.Clear();
			allUnreliableObjects.Clear();
			allObjectsFast.Clear();
		}

		public bool TryAddNetObject(InnerNetObject obj)
		{
			if (obj == null)
			{
				logger.Error("Attempt to add null net object to InnerNetObjectCollection");
				return false;
			}
			if (!allObjectsFast.ContainsKey(obj.NetId))
			{
				allObjects.Add(obj);
				allObjectsFast.Add(obj.NetId, obj);
				switch (obj.sendMode)
				{
				case SendOption.Reliable:
					allReliableObjects.Add(obj);
					break;
				case SendOption.None:
					allUnreliableObjects.Add(obj);
					break;
				default:
					logger.Warning(obj.name + " has SendMode set to Everything, and likely should be Reliable. We are making an assumption to add it to the reliable objects queue here but SendModes should not have the [Flags] attribute.");
					allReliableObjects.Add(obj);
					break;
				}
				return true;
			}
			logger.Error(string.Format("Attempt to add duplicate net object to {0} - Type: {1}, NetId: {2}", "InnerNetObjectCollection", obj.GetType(), obj.NetId));
			return false;
		}

		public void RemoveNetObject(InnerNetObject obj)
		{
			int num = allObjects.IndexOf(obj);
			if (num > -1)
			{
				allObjects.RemoveAt(num);
			}
			allObjectsFast.Remove(obj.NetId);
			switch (obj.sendMode)
			{
			case SendOption.Reliable:
			{
				int num3 = allReliableObjects.IndexOf(obj);
				if (num3 > -1)
				{
					allReliableObjects.RemoveAt(num3);
				}
				break;
			}
			case SendOption.None:
			{
				int num2 = allUnreliableObjects.IndexOf(obj);
				if (num2 > -1)
				{
					allUnreliableObjects.RemoveAt(num2);
				}
				break;
			}
			}
			obj.NetId = uint.MaxValue;
		}

		public void RemoveNullObjects()
		{
			for (int num = allObjects.Count - 1; num > -1; num--)
			{
				if (!allObjects[num])
				{
					allObjects.RemoveAt(num);
				}
			}
			for (int num2 = allReliableObjects.Count - 1; num2 > -1; num2--)
			{
				if (!allReliableObjects[num2])
				{
					allReliableObjects.RemoveAt(num2);
				}
			}
			for (int num3 = allUnreliableObjects.Count - 1; num3 > -1; num3--)
			{
				if (!allUnreliableObjects[num3])
				{
					allUnreliableObjects.RemoveAt(num3);
				}
			}
		}

		public void RemoveUnownedObjects(HashSet<int> clientIds, int ownerClientId, Action<GameObject> removalAction)
		{
			for (int num = allObjects.Count - 1; num > -1; num--)
			{
				InnerNetObject innerNetObject = allObjects[num];
				if (!innerNetObject)
				{
					allObjects.RemoveAt(num);
					switch (innerNetObject.sendMode)
					{
					case SendOption.Reliable:
					{
						int num3 = allReliableObjects.IndexOf(innerNetObject);
						if (num3 > -1)
						{
							allReliableObjects.RemoveAt(num3);
						}
						break;
					}
					case SendOption.None:
					{
						int num2 = allUnreliableObjects.IndexOf(innerNetObject);
						if (num2 > -1)
						{
							allUnreliableObjects.RemoveAt(num2);
						}
						break;
					}
					}
				}
				else if (!clientIds.Contains(innerNetObject.OwnerId))
				{
					innerNetObject.OwnerId = ownerClientId;
					removalAction?.Invoke(innerNetObject.gameObject);
				}
			}
		}

		public void DestroyPlayerInfoObjects(Action<GameObject> destroyAction)
		{
			for (int num = allObjects.Count - 1; num > -1; num--)
			{
				InnerNetObject innerNetObject = allObjects[num];
				if (!innerNetObject)
				{
					allObjects.RemoveAt(num);
					switch (innerNetObject.sendMode)
					{
					case SendOption.Reliable:
					{
						int num3 = allReliableObjects.IndexOf(innerNetObject);
						if (num3 > -1)
						{
							allReliableObjects.RemoveAt(num3);
						}
						break;
					}
					case SendOption.None:
					{
						int num2 = allUnreliableObjects.IndexOf(innerNetObject);
						if (num2 > -1)
						{
							allUnreliableObjects.RemoveAt(num2);
						}
						break;
					}
					}
				}
				else if (innerNetObject is NetworkedPlayerInfo)
				{
					destroyAction?.Invoke(innerNetObject.gameObject);
					allObjects.RemoveAt(num);
				}
			}
		}

		private void RemoveFromSendModeList(InnerNetObject obj)
		{
			List<InnerNetObject> list = null;
			if (obj.sendMode == SendOption.None)
			{
				list = allUnreliableObjects;
			}
			else if (obj.sendMode == SendOption.Reliable)
			{
				list = allReliableObjects;
			}
			if (list != null)
			{
				int num = list.IndexOf(obj);
				if (num > -1)
				{
					list.RemoveAt(num);
				}
			}
		}

		public InnerNetObjectCollection()
		{ }
	}
}
