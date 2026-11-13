namespace Epic.OnlineServices
{
	public struct PageResult
	{
		public int StartIndex { get; set; }

		public int Count { get; set; }

		public int TotalCount { get; set; }

		internal void Set(ref PageResultInternal other)
		{
		}
	}
}
