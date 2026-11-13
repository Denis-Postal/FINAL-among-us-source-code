using System;
using System.Collections.Generic;
using System.Linq;
using Cpp2ILInjected;
using UnityEngine;

public class VoteSpreader : MonoBehaviour
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Func<SpriteRenderer, bool> _003C_003E9__5_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003CUpdate_003Eb__5_0(SpriteRenderer v)
		{
			return v.transform.localScale.magnitude > 0.0001f;
		}
	}

	public List<SpriteRenderer> Votes= new List<SpriteRenderer>();

	public FloatRange VoteRange= new FloatRange(-0.5f, 1.15f);

	public int maxVotesBeforeSmoosh= 7;

	public float CounterY= -0.16f;

	public float adjustRate= 4f;

	private unsafe void Update()
	{
		int num = Votes.Count((SpriteRenderer v) => v.transform.localScale.magnitude > 0.0001f);
		for (int num2 = 0; num2 < num; num2++)
		{
			SpriteRenderer spriteRenderer = Votes[num2];
			Vector2 b = new Vector3(0f, CounterY);
			b.x = VoteRange.SpreadToEdges(num2, Mathf.Max(num, maxVotesBeforeSmoosh));
			Vector3 localPosition = spriteRenderer.transform.localPosition;
			localPosition = Vector2.Lerp(localPosition, b, Time.deltaTime * adjustRate);
			localPosition.z = (float)(-num2) / 50f;
			spriteRenderer.transform.localPosition = localPosition;
		}
	}

	// TODO(verify): the shipped AddVote start position was unrecoverable; y = CounterY is inferred from the field name.
	public unsafe void AddVote(SpriteRenderer newVote)
	{
		newVote.transform.localPosition = new Vector3(VoteRange.max, CounterY, 0f);
		Votes.Add(newVote);
	}

	public VoteSpreader()
	{ }
}
