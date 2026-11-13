using System;

namespace Innersloth.DebugTool
{
	public class DebugCommandAttribute : Attribute
	{
		public string Path;

		public string Description;

		public bool CloseBeforeExecute;

		public DebugCommandAttribute(string path, string description = "", bool closeBeforeExecute = false)
		{
			Path = path;
			Description = description;
			CloseBeforeExecute = closeBeforeExecute;
		}
	}
}
