using System;
using UnityEngine;

namespace PowerTools
{
	[DisallowMultipleComponent]
	public class SpriteAnimEventHandler : MonoBehaviour
	{
		public static class EventParser
		{
			public static readonly char MESSAGE_DELIMITER = '\t';

			public static readonly string MESSAGE_NOPARAM = "_Anim";

			public static readonly string MESSAGE_INT = "_AnimInt";

			public static readonly string MESSAGE_FLOAT = "_AnimFloat";

			public static readonly string MESSAGE_STRING = "_AnimString";

			public static readonly string MESSAGE_OBJECT_FUNCNAME = "_AnimObjectFunc";

			public static readonly string MESSAGE_OBJECT_DATA = "_AnimObjectData";

			public unsafe static int ParseInt(ref string messageString)
			{ /* spliced-from-2019 */
				int num = messageString.IndexOf(MESSAGE_DELIMITER);
				int result = 0;
				int.TryParse(messageString.Substring(num + 1), out result);
				messageString = messageString.Substring(0, num);
				return result;
			}

			public unsafe static float ParseFloat(ref string messageString)
			{ /* spliced-from-2019 */
				int num = messageString.IndexOf(MESSAGE_DELIMITER);
				float result = 0f;
				float.TryParse(messageString.Substring(num + 1), out result);
				messageString = messageString.Substring(0, num);
				return result;
			}

			public unsafe static string ParseString(ref string messageString)
			{ /* spliced-from-2019 */
				int num = messageString.IndexOf(MESSAGE_DELIMITER);
				string result = messageString.Substring(num + 1);
				messageString = messageString.Substring(0, num);
				return result;
			}
		}

		private string m_eventWithObjectMessage;

		private object m_eventWithObjectData;

		private void _Anim(string function)
		{
			SendMessageUpwards(function, SendMessageOptions.DontRequireReceiver);
		}

		private void _AnimInt(string messageString)
		{
			//IL_007a: Expected I4, but got Obj
			int num = messageString.IndexOf(EventParser.MESSAGE_DELIMITER);
			int startIndex = num + 1;
			string s = messageString.Substring(startIndex);
			bool flag = int.TryParse(s, out var _);
			string text = messageString.Substring(0, num);
			object obj = default;
			object value = (int)obj;
			string methodName = default;
			SendMessageUpwards(methodName, value, SendMessageOptions.DontRequireReceiver);
		}

		private void _AnimFloat(string messageString)
		{
			//IL_007a: Expected F4, but got Obj
			int num = messageString.IndexOf(EventParser.MESSAGE_DELIMITER);
			int startIndex = num + 1;
			string s = messageString.Substring(startIndex);
			bool flag = float.TryParse(s, out var _);
			string text = messageString.Substring(0, num);
			object obj = default;
			object value = (float)obj;
			string methodName = default;
			SendMessageUpwards(methodName, value, SendMessageOptions.DontRequireReceiver);
		}

		private void _AnimString(string messageString)
		{
			int num = messageString.IndexOf(EventParser.MESSAGE_DELIMITER);
			int startIndex = num + 1;
			string value = messageString.Substring(startIndex);
			string text = messageString.Substring(0, num);
			string methodName = default;
			SendMessageUpwards(methodName, value, SendMessageOptions.DontRequireReceiver);
		}

		private void _AnimObjectFunc(string funcName)
		{
			if (m_eventWithObjectData == null)
			{
				if (!string.IsNullOrEmpty(m_eventWithObjectMessage))
				{
					Debug.LogError("Animation event with object parameter had no object");
				}
				m_eventWithObjectMessage = funcName;
			}
			else
			{
				SendMessageUpwards(funcName, m_eventWithObjectData, SendMessageOptions.DontRequireReceiver);
				m_eventWithObjectMessage = null;
				m_eventWithObjectData = null;
			}
		}

		private void _AnimObjectData(UnityEngine.Object data)
		{
			UnityEngine.Object eventWithObjectData;
			if (string.IsNullOrEmpty(m_eventWithObjectMessage))
			{
				if (m_eventWithObjectData != null)
				{
					Debug.LogError("Animation event with object parameter had no object");
				}
				eventWithObjectData = data;
			}
			else
			{
				SendMessageUpwards(m_eventWithObjectMessage, data, SendMessageOptions.DontRequireReceiver);
				m_eventWithObjectMessage = null;
				eventWithObjectData = null;
			}
			m_eventWithObjectData = eventWithObjectData;
		}
	}
}
