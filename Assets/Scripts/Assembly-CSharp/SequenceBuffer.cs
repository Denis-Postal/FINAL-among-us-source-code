using System;
using System.Collections;
using System.Collections.Generic;

// TODO(verify): rewritten as plain C# (ILSpy output was unusable); semantics inferred from decomp9 usage in VentilationSystem
public class SequenceBuffer<T>
{
	private struct SequencedData : IComparable<SequencedData>
	{
		public readonly ushort Sid;

		public readonly T Data;

		public SequencedData(ushort sid, T data)
		{
			Sid = sid;
			Data = data;
		}

		public int CompareTo(SequencedData other)
		{
			if (Sid == other.Sid)
			{
				return 0;
			}
			return NetHelpers.SidGreaterThan(Sid, other.Sid) ? 1 : -1;
		}
	}

	private readonly List<SequencedData> buffer;

	public ushort LastSid { get; set; }

	public SequenceBuffer(ushort sidStart = 0)
	{
		buffer = new List<SequencedData>();
		LastSid = sidStart;
	}

	public void Add(ushort sid, T info)
	{
		buffer.Add(new SequencedData(sid, info));
		Sort();
	}

	public void BumpSid()
	{
		LastSid = (ushort)(LastSid + 1);
	}

	public bool IsInvalidSid(ushort sid)
	{
		return !NetHelpers.SidGreaterThan(sid, LastSid);
	}

	public bool IsNextSid(ushort sid)
	{
		return sid == (ushort)(LastSid + 1);
	}

	public IEnumerable<T> SubsequentObjs()
	{
		while (HasElements() && IsNextSid(Peek().Sid))
		{
			yield return Pop().Data;
		}
	}

	private void Sort()
	{
		buffer.Sort();
	}

	private bool HasElements()
	{
		return buffer.Count > 0;
	}

	private SequencedData Pop()
	{
		SequencedData result = buffer[0];
		buffer.RemoveAt(0);
		return result;
	}

	private SequencedData Peek()
	{
		return buffer[0];
	}
}
