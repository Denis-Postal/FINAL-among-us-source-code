using Cpp2ILInjected;
using Hazel;
using InnerNet;

public class JudgeOverrule
{
	public PlayerId OverruledPlayerId = PlayerId.Invalid;

	public PlayerId JudgePlayerId = PlayerId.Invalid;

	public ushort OverruleNonce;

	public JudgeOverrule()
	{
	}

	public JudgeOverrule(PlayerId overridenPlayerID, PlayerId judgePlayerID, ushort overruleNonce)
	{
		OverruledPlayerId = overridenPlayerID;
		JudgePlayerId = judgePlayerID;
		OverruleNonce = overruleNonce;
	}

	public void Deserialize(MessageReader reader)
	{
		MessageReader messageReader = reader.ReadMessage();
		JudgePlayerId = messageReader.Tag;
		OverruledPlayerId = messageReader.ReadByte();
		OverruleNonce = messageReader.ReadUInt16();
	}

	public void Serialize(MessageWriter writer)
	{
		writer.StartMessage(JudgePlayerId);
		writer.Write(OverruledPlayerId);
		writer.Write(OverruleNonce);
		writer.EndMessage();
	}
}
