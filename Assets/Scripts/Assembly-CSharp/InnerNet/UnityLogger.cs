using Hazel;
using UnityEngine;

namespace InnerNet
{
	public class UnityLogger : Hazel.ILogger
	{
		public void WriteVerbose(string msg)
		{
		}

		public void WriteWarning(string msg)
		{
			Debug.LogWarning(msg);
		}

		public void WriteError(string msg)
		{
			Debug.LogError(msg);
		}

		public void WriteInfo(string msg)
		{
			Debug.Log(msg);
		}

		public UnityLogger()
		{
			WriteVerbose(null);
		}
	}
}
