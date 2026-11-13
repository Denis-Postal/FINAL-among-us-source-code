using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

namespace InnerNet.GizmoHelpers
{
	public class NetTransformLogger : INetTransformLogger
	{
		private List<int> queue;

		private List<bool> slowbandLog;

		private Vector3 pos;

		public unsafe NetTransformLogger()
		{
			queue = new List<int>();
			slowbandLog = new List<bool>();
			float x = (float)Screen.width * 0.1f;
			float num = (float)Screen.height * 1.2f;
			pos = Camera.main.ScreenToWorldPoint(new Vector3(x, num, num));
		}

		public void SetQueueLength(int i)
		{
			if (queue.Count > 30)
			{
				queue.RemoveAt(0);
			}
			queue.Add(i);
		}

		public void SetSlowbandingLog(bool isSlowbanding)
		{
			if (slowbandLog.Count > 30)
			{
				slowbandLog.RemoveAt(0);
			}
			slowbandLog.Add(isSlowbanding);
		}

		public void Render(Vector3 pos2, float rubberBandMod)
		{
		}
	}
}
