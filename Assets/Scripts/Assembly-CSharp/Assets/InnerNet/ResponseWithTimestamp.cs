namespace Assets.InnerNet
{
	public class ResponseWithTimestamp<T>
	{
		public class ResponseData
		{
			public string Type;

			public T Attributes;

			public string Timestamp;
		}

		public ResponseData Data;

		public ResponseError[] Errors;

		public ResponseError[] Warnings;
	}
}
