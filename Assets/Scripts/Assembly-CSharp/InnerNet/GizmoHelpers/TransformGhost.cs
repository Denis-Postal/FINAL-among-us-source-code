using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

namespace InnerNet.GizmoHelpers
{
	public class TransformGhost : ITransformGhost
	{
		private const int MaxCount = 30;

		private const float HalfSize = 0.05f;

		private List<Vector2> positions= new List<Vector2>();

		public void AddPt(Vector2 pt)
		{
			positions.Add(pt);
			if (positions.Count >= 30)
			{
				positions.RemoveAt(0);
			}
		}

		public unsafe void Render(Color c, bool crosses)
		{
			Gizmos.color = c;
			for (int i = 0; i < positions.Count; i++)
			{
				Vector3 vector = positions[i];
				if (crosses)
				{
					vector.z = -101f;
					Gizmos.DrawLine(vector + Vector3.left * 0.05f, vector + Vector3.right * 0.05f);
					Gizmos.DrawLine(vector + Vector3.up * 0.05f, vector + Vector3.down * 0.05f);
				}
				else
				{
					vector.z = -100f;
					Gizmos.DrawCube(vector, Vector3.one * 0.05f * 2f);
				}
			}
			Gizmos.color = Color.white;
		}

		public TransformGhost()
		{ }
	}
}
