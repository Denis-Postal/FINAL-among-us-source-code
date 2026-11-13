using System;

namespace Innersloth.DebugTool
{
	public class DebugParameterAttribute : Attribute
	{
		public string Placeholder;

		public DebugParameterAttribute(string placeholder)
		{
			Placeholder = placeholder;
		}
	}
}
