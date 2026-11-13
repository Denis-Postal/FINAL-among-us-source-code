using System;

namespace AmongUs
{
	public interface IAmongUsDateTimeRetriever
	{
		DateTime Now();

		DateTime UtcNow();
	}
}
