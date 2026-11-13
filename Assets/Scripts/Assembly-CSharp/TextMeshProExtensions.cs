using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public static class TextMeshProExtensions
{
	public static float GetNotDumbRenderedHeight(this TextMeshPro self)
	{
		if ((double)self.renderedHeight > 100000000.0 || (double)self.renderedHeight < -100000000.0)
		{
			return 0f;
		}
		return self.renderedHeight;
	}

	public static Vector2 CursorPos(this TextMeshPro self, int caretPos, float yOffset)
	{
		if (self.textInfo == null || self.textInfo.lineCount == 0 || self.textInfo.lineInfo[0].characterCount <= 0)
		{
			return new Vector2(0f, yOffset);
		}
		if (self.textInfo.lineCount == 2)
		{
			return new Vector2(self.textInfo.characterInfo[caretPos - 1].topRight.x - 0.01f, (float)Mathf.Abs(self.textInfo.characterInfo[caretPos - 1].lineNumber - 1) * (yOffset * 2f));
		}
		if (caretPos == 0)
		{
			return new Vector2(self.textInfo.characterInfo[0].topLeft.x - 0.01f, yOffset);
		}
		return new Vector2(self.textInfo.characterInfo[caretPos - 1].topRight.x - 0.01f, yOffset);
	}

	public static Vector2 CursorPos(this TextMeshPro self)
	{
		if (self.textInfo == null || self.textInfo.lineCount == 0 || self.textInfo.lineInfo[0].characterCount <= 0)
		{
			return Vector2.zero;
		}
		return self.textInfo.lineInfo.Last((TMP_LineInfo l) => l.characterCount > 0).lineExtents.max;
	}

	private static int TextInfoIndexOf(this TextMeshPro self, string str, int wordOffset = 0)
	{
		TMP_CharacterInfo[] characterInfo = self.textInfo.characterInfo;
		for (int i = wordOffset; i < characterInfo.Length; i++)
		{
			int j = 0;
			int num = 0;
			for (; j < str.Length && i + j < characterInfo.Length && characterInfo[i + j].character == str[j]; j++)
			{
				num++;
			}
			if (num == str.Length)
			{
				return i;
			}
		}
		return -1;
	}

	public static bool GetLinkPosition(this TextMeshPro self, int linkIndex, out Vector3 bottomLeft, out Vector3 topRight)
	{
		TMP_TextInfo textInfo = self.textInfo;
		if (linkIndex >= textInfo.linkCount)
		{
			bottomLeft = Vector3.zero;
			topRight = Vector3.zero;
			return false;
		}
		TMP_LinkInfo tMP_LinkInfo = textInfo.linkInfo[linkIndex];
		TMP_CharacterInfo tMP_CharacterInfo = self.textInfo.characterInfo[tMP_LinkInfo.linkTextfirstCharacterIndex];
		TMP_CharacterInfo tMP_CharacterInfo2 = self.textInfo.characterInfo[tMP_LinkInfo.linkTextfirstCharacterIndex + tMP_LinkInfo.linkTextLength];
		bottomLeft = tMP_CharacterInfo.bottomLeft;
		topRight = tMP_CharacterInfo2.topRight;
		for (int i = tMP_LinkInfo.linkTextfirstCharacterIndex + 1; i < tMP_LinkInfo.linkTextfirstCharacterIndex + tMP_LinkInfo.linkTextLength; i++)
		{
			Vector3 bottomLeft2 = self.textInfo.characterInfo[i].bottomLeft;
			Vector3 topRight2 = self.textInfo.characterInfo[i].topRight;
			bottomLeft.y = Mathf.Min(bottomLeft.y, bottomLeft2.y);
			topRight.y = Mathf.Max(topRight.y, topRight2.y);
		}
		bottomLeft.z = (topRight.z = self.transform.localPosition.z);
		return true;
	}

	public static bool GetWordPosition(this TextMeshPro self, string str, List<string> wordsPositioned, out Vector3 bottomLeft, out Vector3 topRight, out int lineBreakCount)
	{
		int num = wordsPositioned.Count((string text) => text == str);
		int num2 = self.TextInfoIndexOf(str);
		for (int num3 = 0; num3 < num; num3++)
		{
			num2 = self.TextInfoIndexOf(str, num2 + 1);
		}
		if (num2 != -1)
		{
			int num4 = num2 + str.Length - 1;
			if (num4 < self.textInfo.characterInfo.Length)
			{
				TMP_CharacterInfo tMP_CharacterInfo = self.textInfo.characterInfo[num2];
				TMP_CharacterInfo tMP_CharacterInfo2 = self.textInfo.characterInfo[num4];
				bottomLeft = tMP_CharacterInfo.bottomLeft;
				topRight = tMP_CharacterInfo2.topRight;
				lineBreakCount = 0;
				float x = tMP_CharacterInfo.bottomLeft.x;
				float num5 = 0f;
				float num6 = 0f;
				for (int num7 = num2 + 1; num7 < num4; num7++)
				{
					Vector3 bottomLeft2 = self.textInfo.characterInfo[num7].bottomLeft;
					Vector3 topRight2 = self.textInfo.characterInfo[num7].topRight;
					num6 = bottomLeft2.x - x;
					x = bottomLeft2.x;
					if (num5 * num6 < 0f)
					{
						lineBreakCount++;
					}
					num5 = num6;
					bottomLeft.y = Mathf.Min(bottomLeft.y, bottomLeft2.y);
					topRight.y = Mathf.Max(topRight.y, topRight2.y);
				}
				bottomLeft.z = (topRight.z = self.transform.localPosition.z);
				lineBreakCount /= 2;
				return true;
			}
		}
		bottomLeft = Vector3.zero;
		topRight = Vector3.zero;
		lineBreakCount = 0;
		return false;
	}

	public static int GetSplitWordPositions(this TextMeshPro self, string str, List<Vector3> bottomLeft, List<Vector3> topRight)
	{
		int num = self.TextInfoIndexOf(str);
		bottomLeft.Clear();
		topRight.Clear();
		if (num != -1)
		{
			int num2 = num + str.Length - 1;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			if (num2 < self.textInfo.characterInfo.Length)
			{
				TMP_CharacterInfo tMP_CharacterInfo = self.textInfo.characterInfo[num];
				zero = tMP_CharacterInfo.bottomLeft;
				zero2 = tMP_CharacterInfo.topRight;
				float x = tMP_CharacterInfo.bottomLeft.x;
				float num3 = 0f;
				float num4 = 0f;
				bool flag = false;
				for (int i = num + 1; i <= num2; i++)
				{
					Vector3 bottomLeft2 = self.textInfo.characterInfo[i].bottomLeft;
					Vector3 topRight2 = self.textInfo.characterInfo[i].topRight;
					num4 = bottomLeft2.x - x;
					x = bottomLeft2.x;
					if (!flag && num3 * num4 < 0f)
					{
						flag = true;
						zero.z = (zero2.z = self.transform.localPosition.z);
						bottomLeft.Add(zero);
						topRight.Add(zero2);
						zero2 = topRight2;
						zero = bottomLeft2;
					}
					else if (flag)
					{
						flag = false;
					}
					num3 = num4;
					zero.x = Mathf.Min(zero.x, bottomLeft2.x);
					zero.y = Mathf.Min(zero.y, bottomLeft2.y);
					zero2.y = Mathf.Max(zero2.y, topRight2.y);
					zero2.x = Mathf.Max(zero2.x, topRight2.x);
				}
				zero.z = (zero2.z = self.transform.localPosition.z);
				bottomLeft.Add(zero);
				topRight.Add(zero2);
			}
		}
		return bottomLeft.Count;
	}
}
