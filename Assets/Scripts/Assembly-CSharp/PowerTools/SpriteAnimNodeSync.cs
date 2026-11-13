using Cpp2ILInjected;
using UnityEngine;

namespace PowerTools
{
	[ExecuteInEditMode]
	public class SpriteAnimNodeSync : MonoBehaviour
	{
		public int NodeId;

		public SpriteAnimNodes Parent;

		public SpriteRenderer ParentRenderer;

		public SpriteRenderer Renderer;

		[HideInInspector]
		public Vector3 flipOffset= Vector3.zero;

		[HideInInspector]
		public Vector3 normalOffset= Vector3.zero;

		// TODO(verify): rebuilt from decomp9 + 2019 baseline. The decompile adds normalOffset (or flipOffset when flipped) to the local position
		// and negates the angle when flipped; the per-NodeId (<=9) jump table was not recovered.
		public void LateUpdate()
		{
			bool flag = false;
			if ((bool)ParentRenderer)
			{
				flag = ParentRenderer.flipX;
				if ((bool)Renderer)
				{
					Renderer.flipX = ParentRenderer.flipX;
				}
			}
			else if ((bool)Renderer)
			{
				flag = Renderer.flipX;
			}
			Vector3 localPosition = transform.localPosition;
			Vector3 localPosition2 = Parent.GetLocalPosition(NodeId);
			localPosition.x = localPosition2.x;
			localPosition.y = localPosition2.y;
			transform.localPosition = localPosition;
			float angle = Parent.GetAngle(NodeId);
			if (flag)
			{
				transform.eulerAngles = new Vector3(0f, 0f, 0f - angle);
				transform.localPosition += flipOffset;
			}
			else
			{
				transform.eulerAngles = new Vector3(0f, 0f, angle);
				transform.localPosition += normalOffset;
			}
		}

		public SpriteAnimNodeSync()
		{ }
	}
}
