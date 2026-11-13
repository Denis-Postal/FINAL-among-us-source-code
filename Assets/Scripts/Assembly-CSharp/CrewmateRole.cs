using System;
using System.Text;
using Cpp2ILInjected;

public class CrewmateRole : RoleBehaviour
{
	public override bool IsDead => false;

	public override bool CanUse(IUsable console)
	{
		if (console is ZiplineConsole || console is Ladder || console is PlatformConsole)
		{
			return true;
		}
		if (!(console is Console))
		{
			return console is DoorConsole;
		}
		return true;
	}

	public override bool DidWin(GameOverReason gameOverReason)
	{
		return GameManager.Instance.DidHumansWin(gameOverReason);
	}

	public override void AppendTaskHint(StringBuilder taskStringBuilder)
	{
	}
}
