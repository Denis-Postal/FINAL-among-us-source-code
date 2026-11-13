using System;
using UnityEngine;
using UnityEngine.Audio;

public class TowelTaskConsole : AutoTaskConsole
{
	// TODO(verify): NextStep() and the final colour are assumed (the decompile shows only the sound, the 250 marker write and a colour assignment).
	protected override void AfterUse(NormalPlayerTask task)
	{
		if ((bool)useSound && Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(useSound, loop: false);
		}
		int num = task.Data.IndexOf((byte b) => b == ConsoleId);
		task.Data[num] = 250;
		Image.color = Palette.ClearWhite;
	}

	private bool _003CAfterUse_003Eb__0_0(byte b)
	{
		//IL_000f: Expected Obj, but got I4
		object obj = b - ConsoleId;
		return obj == null;
	}
}
