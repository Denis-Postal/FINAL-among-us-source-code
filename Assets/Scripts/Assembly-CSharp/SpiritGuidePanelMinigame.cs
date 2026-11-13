using UnityEngine;
public class SpiritGuidePanelMinigame : Minigame
{
	[SerializeField]
	private SpiritGuideRole spiritGuideRole;

	public override void Close()
	{
		Minigame.Instance = null;
		spiritGuideRole.CloseSpiritGuidePanel(playSFX: false);
	}

	public override void Begin(PlayerTask task)
	{
		Minigame.Instance = this;
	}

	public override void ForceClose()
	{
		Minigame.Instance = null;
		spiritGuideRole.CloseSpiritGuidePanel(playSFX: false);
	}

	public void RemoveInstance()
	{
		Minigame.Instance = null;
	}
}
