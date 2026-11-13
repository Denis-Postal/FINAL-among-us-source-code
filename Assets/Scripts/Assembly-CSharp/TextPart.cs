internal class TextPart : ITextPart
{
	public string m_text;

	public TextPart(string text)
	{
		m_text = text;
	}

	public string GetText()
	{
		return m_text;
	}
}
