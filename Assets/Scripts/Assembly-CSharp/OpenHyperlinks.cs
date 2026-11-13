using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class OpenHyperlinks : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro pTextMeshPro;

	[SerializeField]
	private Color linkColor;

	public string Text
	{
		get
		{
			return pTextMeshPro.text;
		}
		set
		{
			pTextMeshPro.text = value;
		}
	}

	public unsafe Color LinkColor
	{
		get
		{
			return linkColor;
		}
		set
		{
			linkColor = value;
		}
	}

	public bool GetWordPosition(string text, List<string> wordsPositioned, out Vector3 bottomLeft, out Vector3 topRight, out int lineBreakCount)
	{
		return pTextMeshPro.GetWordPosition(text, wordsPositioned, out bottomLeft, out topRight, out lineBreakCount);
	}

	public void UpdateTMPMesh()
	{
		pTextMeshPro.ForceMeshUpdate();
		pTextMeshPro.UpdateVertexData();
	}
}
