namespace Epic.OnlineServices.Version
{
	public sealed class VersionInterface
	{
		public static readonly Utf8String CompanyName;

		public static readonly Utf8String CopyrightString;

		public const int MajorVersion = 1;

		public const int MinorVersion = 16;

		public const int PatchVersion = 4;

		public static readonly Utf8String ProductIdentifier;

		public static readonly Utf8String ProductName;

		public static Utf8String GetVersion()
		{
			return null;
		}
	}
}
