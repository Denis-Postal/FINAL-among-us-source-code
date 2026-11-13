using System;

namespace AmongUs
{
	public class AmongUsDefaultDateTime : IAmongUsDateTimeRetriever
	{
		public DateTime Now()
		{
			return DateTime.Now;
		}

		public DateTime UtcNow()
		{
			return DateTime.UtcNow;
		}
	}
}
