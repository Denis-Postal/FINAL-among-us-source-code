using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TowelTask : NormalPlayerTask
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Func<byte, bool> _003C_003E9__0_1;

		public static Func<byte, bool> _003C_003E9__1_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003CValidConsole_003Eb__0_1(byte b)
		{
			//IL_000e: Expected Obj, but got I4
			object obj = b - 250;
			return obj == null;
		}

		internal bool _003CAppendTaskText_003Eb__1_0(byte b)
		{
			//IL_000e: Expected Obj, but got I4
			object obj = b - 250;
			return obj == null;
		}
	}

	private sealed class _003C_003Ec__DisplayClass0_0
	{
		public Console console;

		internal bool _003CValidConsole_003Eb__0(byte b)
		{
			return b == console.ConsoleId;
		}
	}

	public override bool ValidConsole(Console console)
	{
		if (TaskType == TaskTypes.PickUpTowels && console.TaskTypes.Contains(TaskTypes.PickUpTowels))
		{
			if (Data.IndexOf((byte b) => b == console.ConsoleId) != -1)
			{
				return true;
			}
			if (Data.All((byte b) => b == 250) && console.ConsoleId == 255)
			{
				return true;
			}
		}
		return false;
	}

	public override void AppendTaskText(StringBuilder sb)
	{
		int num = Data.Count((byte b) => b == 250);
		bool flag = num > 0;
		if (flag)
		{
			if (IsComplete)
			{
				sb.Append("<color=#00DD00FF>");
			}
			else
			{
				sb.Append("<color=#FFFF00FF>");
			}
		}
		sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(StartAt));
		sb.Append(": ");
		sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(TaskType));
		if (num < Data.Length)
		{
			sb.Append(" (");
			sb.Append(num);
			sb.Append("/");
			sb.Append(Data.Length);
			sb.Append(")");
		}
		if (flag)
		{
			sb.Append("</color>");
		}
		sb.AppendLine();
	}

	public TowelTask()
	{ }
}
