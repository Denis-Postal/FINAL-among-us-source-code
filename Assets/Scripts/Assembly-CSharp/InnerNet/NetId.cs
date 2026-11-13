namespace InnerNet
{
	public struct NetId
	{
		public static readonly NetId InvalidNetId = new NetId(uint.MaxValue);

		public readonly uint Value;

		public NetId(uint value)
		{
			Value = value;
		}

		public static implicit operator uint(NetId netId)
		{
			return netId.Value;
		}

		public static implicit operator NetId(uint value)
		{
			return new NetId(value);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", "NetId", Value);
		}

		static NetId()
		{
			//IL_0013: Expected VT, but got I8
		}
	}
}
