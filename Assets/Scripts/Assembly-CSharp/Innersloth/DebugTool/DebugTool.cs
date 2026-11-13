using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Innersloth.DebugTool
{
	public static class DebugTool
	{
		public delegate void LogHandler(string message, DebugCommandMessageType type);

		private static readonly List<DebugCommand> AllCommands = new List<DebugCommand>();

		private static readonly DebugCircleBuffer<DebugLogMessage> AllLogMessages = new DebugCircleBuffer<DebugLogMessage>(1000);

		private static readonly Dictionary<Type, Func<string, object>> CustomParsers = new Dictionary<Type, Func<string, object>>();

		private static readonly Dictionary<Type, Func<string[]>> CustomDropdownCollectors = new Dictionary<Type, Func<string[]>>();

		private static Dictionary<Type, string> simpleTypeNames = new Dictionary<Type, string>
		{
			{
				typeof(bool),
				"bool"
			},
			{
				typeof(int),
				"int"
			},
			{
				typeof(float),
				"float"
			},
			{
				typeof(long),
				"long"
			},
			{
				typeof(string),
				"string"
			}
		};

		public static IReadOnlyList<DebugCommand> Commands => AllCommands;

		public static IEnumerator<DebugLogMessage> Logs => AllLogMessages.GetEnumerator();

		public static bool ListeningToLogs { get; private set; }

		public static event LogHandler OnLogMessageReceived;

		public static void RegisterCommandHandler(object obj)
		{
			RegisterCommandHandler(obj.GetType(), obj);
		}

		public static void RegisterCommandHandler(Type t, object target = null)
		{
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | (target != null ? BindingFlags.Instance : BindingFlags.Static);
			MethodInfo[] methods = t.GetMethods(flags);
			for (int i = 0; i < methods.Length; i++)
			{
				DebugCommandAttribute customAttribute = methods[i].GetCustomAttribute<DebugCommandAttribute>();
				if (customAttribute != null)
				{
					DebugCommand cmd = new DebugCommand(customAttribute.Path, target, methods[i])
					{
						Description = customAttribute.Description,
						CloseBeforeExecute = customAttribute.CloseBeforeExecute
					};
					AddCommand(cmd);
				}
			}
		}

		public static void UnregisterCommandHandler(Type t)
		{
			MethodInfo[] methods = t.GetMethods(BindingFlags.Static | BindingFlags.Public);
			for (int i = 0; i < methods.Length; i++)
			{
				DebugCommandAttribute customAttribute = methods[i].GetCustomAttribute<DebugCommandAttribute>();
				if (customAttribute != null)
				{
					RemoveCommand(customAttribute.Path);
				}
			}
		}

		public static void UnregisterCommandHandler(object obj)
		{
			UnregisterCommandHandler(obj.GetType());
		}

		public static DebugCommand AddCommand(string commandPath, Action commandAction, bool closeBeforeExecute = false)
		{
			DebugCommand debugCommand = new DebugCommand(commandPath, commandAction.Target, commandAction.Method)
			{
				CloseBeforeExecute = closeBeforeExecute
			};
			AddCommand(debugCommand);
			return debugCommand;
		}

		public static DebugCommand AddCommand<T>(string commandPath, Action<T> commandAction, bool closeBeforeExecute = false)
		{
			DebugCommand debugCommand = new DebugCommand(commandPath, commandAction.Target, commandAction.Method)
			{
				CloseBeforeExecute = closeBeforeExecute
			};
			AddCommand(debugCommand);
			return debugCommand;
		}

		public static DebugCommand AddCommand<T1, T2>(string commandPath, Action<T1, T2> commandAction, bool closeBeforeExecute = false)
		{
			DebugCommand debugCommand = new DebugCommand(commandPath, commandAction.Target, commandAction.Method)
			{
				CloseBeforeExecute = closeBeforeExecute
			};
			AddCommand(debugCommand);
			return debugCommand;
		}

		public static void AddCommand(DebugCommand command)
		{
			if (command != null && !AllCommands.Contains(command))
			{
				AllCommands.Add(command);
			}
		}

		public static void RemoveCommand(string commandPath)
		{
			AllCommands.RemoveAll((DebugCommand c) => c.CommandPath == commandPath);
		}

		public static void AddCustomParser<T>(Func<string, T> parser)
		{
			Type typeFromHandle = typeof(T);
			if (!CustomParsers.ContainsKey(typeFromHandle))
			{
				CustomParsers.Add(typeFromHandle, (string str) => parser(str));
			}
			else
			{
				Debug.LogError($"DebugTool already contained a parser for type {typeFromHandle}");
			}
		}

		public static object ParseInputToType(string rawValue, string fieldName, Type type)
		{
			try
			{
				if (CustomParsers.ContainsKey(type))
				{
					return CustomParsers[type](rawValue);
				}
				if (type == typeof(int))
				{
					return int.Parse(rawValue);
				}
				if (type == typeof(float))
				{
					return float.Parse(rawValue);
				}
				if (type == typeof(bool))
				{
					return bool.Parse(rawValue);
				}
			}
			catch (FormatException)
			{
				Debug.LogError(string.Format("[{0}] Format error: Expected type {1} in field '{2}' and was not able to parse it from the raw value: '{3}'", "DebugTool", type, fieldName, rawValue));
				return null;
			}
			if (type == typeof(string))
			{
				return rawValue;
			}
			Debug.LogError(string.Format("{0} Not implemented: Command parameters of type {1} are not supported.", "DebugTool", type));
			return null;
		}

		public static void AddCustomDropdownValueCollector<T>(Func<string[]> collector)
		{
			Type typeFromHandle = typeof(T);
			if (!CustomDropdownCollectors.ContainsKey(typeFromHandle))
			{
				CustomDropdownCollectors.Add(typeFromHandle, collector);
			}
			else
			{
				Debug.LogError($"DebugTool already contained a dropdown value collector for type {typeFromHandle}");
			}
		}

		public static bool HasDropdownValueCollector(Type type)
		{
			return CustomDropdownCollectors.ContainsKey(type);
		}

		public static string[] GetDropdownValues(Type type)
		{
			if (CustomDropdownCollectors.ContainsKey(type))
			{
				return CustomDropdownCollectors[type]();
			}
			Debug.LogError("DebugTool Dropdown collector not found for type '" + type.Name + "'. Make sure you're checking if it exists first.");
			return null;
		}

		public static void ListenToLogs()
		{
		}

		private static void LogMessageReceived(string log, string stacktrace, LogType type)
		{
			AllLogMessages.Add(new DebugLogMessage(log, stacktrace, type));
		}

		public static void ClearLogs()
		{
			AllLogMessages.Clear();
		}

		public static void LogCommandInfo(string log)
		{
			Log(log, DebugCommandMessageType.Info);
		}

		public static void LogCommandSuccess(string log)
		{
			Log(log, DebugCommandMessageType.Success);
		}

		public static void LogCommandWarning(string log)
		{
			Log(log, DebugCommandMessageType.Warning);
		}

		public static void LogCommandError(string log)
		{
			Log(log, DebugCommandMessageType.Error);
		}

		private static void Log(string message, DebugCommandMessageType type)
		{
			OnLogMessageReceived?.Invoke(message, type);
			Debug.Log(string.Format("[{0}] Command {1}: {2}", "DebugTool", type, message));
		}

		public static string GetTypeName(Type type)
		{
			if (simpleTypeNames.ContainsKey(type))
			{
				return simpleTypeNames[type];
			}
			return type.Name;
		}
	}
}
