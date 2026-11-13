using Cpp2ILInjected;
using UnityEngine;

public class SwitchVirtualKeyboardJob
{
	public string text = string.Empty;

	public bool active;

	public TouchScreenKeyboard.Status status = TouchScreenKeyboard.Status.Done;

	public SwitchVirtualKeyboardJob()
	{
		Cpp2ILHelpers.NoteDecompilerIssue("Method not found @180412670");
	}
}
