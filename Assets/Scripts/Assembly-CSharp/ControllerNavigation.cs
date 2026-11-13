using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;

[Serializable]
public struct ControllerNavigation
{
	public enum Mode
	{
		Automatic,
		Explicit
	}

	private static readonly ControllerNavigation defaultNavigation_bf;

	public Mode mode;

	public UiElement selectOnUp;

	public UiElement selectOnDown;

	public UiElement selectOnLeft;

	public UiElement selectOnRight;

	public unsafe static ControllerNavigation defaultNavigation { get; }
}
