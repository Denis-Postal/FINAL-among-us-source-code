namespace Epic.OnlineServices.IntegratedPlatform
{
	public struct XboxOneGDKXBLOptionsInitOptions
	{
		public Utf8String ServiceConfigurationID { get; set; }

		public bool IsXboxGoldRequired { get; set; }

		public bool UseClassicGamertag { get; set; }

		internal void Set(ref XboxOneGDKXBLOptionsInitOptionsInternal other)
		{
		}
	}
}
