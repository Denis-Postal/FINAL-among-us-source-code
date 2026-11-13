using System;
using System.Runtime.InteropServices;
using Cpp2ILInjected;
using UnityEngine;

public static class ClipboardHelper
{
	private const uint CF_TEXT = 1u;

	[PreserveSig]
	private static extern bool IsClipboardFormatAvailable(uint format);

	[PreserveSig]
	private static extern bool OpenClipboard(IntPtr hWndNewOwner);

	[PreserveSig]
	private static extern bool CloseClipboard();

	[PreserveSig]
	private static extern IntPtr GetClipboardData(uint format);

	[PreserveSig]
	private static extern IntPtr GlobalLock(IntPtr hMem);

	[PreserveSig]
	private static extern bool GlobalUnlock(IntPtr hMem);

	[PreserveSig]
	private static extern int GlobalSize(IntPtr hMem);

	public static string GetClipboardString()
	{
		if (!IsClipboardFormatAvailable(1u))
		{
			return null;
		}
		try
		{
			if (!OpenClipboard(IntPtr.Zero))
			{
				return null;
			}
			IntPtr clipboardData = GetClipboardData(1u);
			if (clipboardData == IntPtr.Zero)
			{
				return null;
			}
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = GlobalLock(clipboardData);
				int len = GlobalSize(clipboardData);
				return Marshal.PtrToStringAnsi(clipboardData, len);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					GlobalUnlock(intPtr);
				}
			}
		}
		finally
		{
			CloseClipboard();
		}
	}

	public static void PutClipboardString(string str)
	{
		GUIUtility.systemCopyBuffer = str;
	}
}
