using System;
using System.Collections.Generic;
using System.Linq;

public static class TaskTypesHelpers
{
	public static readonly TaskTypes[] AllTypes= Enum.GetValues(typeof(TaskTypes)).Cast<TaskTypes>().ToArray();

	static TaskTypesHelpers()
	{ }
}
