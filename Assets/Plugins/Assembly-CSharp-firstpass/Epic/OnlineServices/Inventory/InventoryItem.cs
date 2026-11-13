using System;

namespace Epic.OnlineServices.Inventory
{
	public sealed class InventoryItem : Handle
	{
		public const int InventoryitemCreatesubitemApiLatest = 1;

		public const int InventoryitemDeletesubitemApiLatest = 1;

		public const int InventoryitemGetdoublesetvaluebyindexApiLatest = 1;

		public const int InventoryitemGetint64SetvaluebyindexApiLatest = 1;

		public const int InventoryitemGetkeyApiLatest = 1;

		public const int InventoryitemGetstringsetvaluebyindexApiLatest = 1;

		public const int InventoryitemGetsubitemApiLatest = 1;

		public const int InventoryitemGetsubitembyindexApiLatest = 1;

		public const int InventoryitemGetsubitemscountApiLatest = 1;

		public const int InventoryitemGetvalueasboolApiLatest = 1;

		public const int InventoryitemGetvalueasdoubleApiLatest = 1;

		public const int InventoryitemGetvalueasint64ApiLatest = 1;

		public const int InventoryitemGetvalueasstringApiLatest = 1;

		public const int InventoryitemGetvalueastimestampApiLatest = 1;

		public const int InventoryitemGetvaluecountApiLatest = 1;

		public const int InventoryitemGetvaluetypeApiLatest = 1;

		public const int InventoryitemKeyMaxLength = 256;

		public const int InventoryitemSetkeyApiLatest = 1;

		public const int InventoryitemSetvalueasboolApiLatest = 1;

		public const int InventoryitemSetvalueasdoubleApiLatest = 1;

		public const int InventoryitemSetvalueasdoublesetApiLatest = 1;

		public const int InventoryitemSetvalueasint64ApiLatest = 1;

		public const int InventoryitemSetvalueasint64SetApiLatest = 1;

		public const int InventoryitemSetvalueasstringApiLatest = 1;

		public const int InventoryitemSetvalueasstringsetApiLatest = 1;

		public const int InventoryitemSetvalueastimestampApiLatest = 1;

		public const int InventoryitemStringValueMaxLength = 393216;

		public InventoryItem()
		{
		}

		public InventoryItem(IntPtr innerHandle)
		{
		}

		public Result CreateSubItem(ref InventoryItemCreateSubItemOptions options, out InventoryItem outInventoryItemHandle)
		{
			outInventoryItemHandle = null;
			return Result.Success;
		}

		public Result DeleteSubItem(ref InventoryItemDeleteSubItemOptions options)
		{
			return Result.Success;
		}

		public Result GetDoubleSetValueByIndex(ref InventoryItemGetDoubleSetValueByIndexOptions options, out double outValue)
		{
			outValue = default;
			return Result.Success;
		}

		public Result GetInt64SetValueByIndex(ref InventoryItemGetInt64SetValueByIndexOptions options, out long outValue)
		{
			outValue = default;
			return Result.Success;
		}

		public Result GetKey(ref InventoryItemGetKeyOptions options, out Utf8String outKey)
		{
			outKey = null;
			return Result.Success;
		}

		public Result GetStringSetValueByIndex(ref InventoryItemGetStringSetValueByIndexOptions options, out Utf8String outValue)
		{
			outValue = null;
			return Result.Success;
		}

		public Result GetSubItem(ref InventoryItemGetSubItemOptions options, out InventoryItem outInventoryItemHandle)
		{
			outInventoryItemHandle = null;
			return Result.Success;
		}

		public Result GetSubItemByIndex(ref InventoryItemGetSubItemByIndexOptions options, out InventoryItem outInventoryItemHandle)
		{
			outInventoryItemHandle = null;
			return Result.Success;
		}

		public uint GetSubItemsCount(ref InventoryItemGetSubItemsCountOptions options)
		{
			return 0u;
		}

		public Result GetValueAsBool(ref InventoryItemGetValueAsBoolOptions options, out bool outValue)
		{
			outValue = default;
			return Result.Success;
		}

		public Result GetValueAsDouble(ref InventoryItemGetValueAsDoubleOptions options, out double outValue)
		{
			outValue = default;
			return Result.Success;
		}

		public Result GetValueAsInt64(ref InventoryItemGetValueAsInt64Options options, out long outValue)
		{
			outValue = default;
			return Result.Success;
		}

		public Result GetValueAsString(ref InventoryItemGetValueAsStringOptions options, out Utf8String outValue)
		{
			outValue = null;
			return Result.Success;
		}

		public Result GetValueAsTimestamp(ref InventoryItemGetValueAsTimestampOptions options, out long outValue)
		{
			outValue = default;
			return Result.Success;
		}

		public uint GetValueCount(ref InventoryItemGetValueCountOptions options)
		{
			return 0u;
		}

		public Result GetValueType(ref InventoryItemGetValueTypeOptions options, out ValueType outValueType)
		{
			outValueType = default;
			return Result.Success;
		}

		public Result SetKey(ref InventoryItemSetKeyOptions options)
		{
			return Result.Success;
		}

		public Result SetValueAsBool(ref InventoryItemSetValueAsBoolOptions options)
		{
			return Result.Success;
		}

		public Result SetValueAsDouble(ref InventoryItemSetValueAsDoubleOptions options)
		{
			return Result.Success;
		}

		public Result SetValueAsDoubleSet(ref InventoryItemSetValueAsDoubleSetOptions options)
		{
			return Result.Success;
		}

		public Result SetValueAsInt64(ref InventoryItemSetValueAsInt64Options options)
		{
			return Result.Success;
		}

		public Result SetValueAsInt64Set(ref InventoryItemSetValueAsInt64SetOptions options)
		{
			return Result.Success;
		}

		public Result SetValueAsString(ref InventoryItemSetValueAsStringOptions options)
		{
			return Result.Success;
		}

		public Result SetValueAsStringSet(ref InventoryItemSetValueAsStringSetOptions options)
		{
			return Result.Success;
		}

		public Result SetValueAsTimestamp(ref InventoryItemSetValueAsTimestampOptions options)
		{
			return Result.Success;
		}
	}
}
