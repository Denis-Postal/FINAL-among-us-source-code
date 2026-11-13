using UnityEngine;

public class SkinViewData : ScriptableObject
{
	public Sprite IdleFrame;

	public Sprite EjectFrame;

	public HatData RelatedHat;

	public VisorData RelatedVisor;

	public bool MatchPlayerColor;

	public AnimationClip IdleAnim;

	public AnimationClip IdleLeftAnim;

	public AnimationClip RunAnim;

	public AnimationClip RunLeftAnim;

	public AnimationClip EnterVentAnim;

	public AnimationClip ExitVentAnim;

	public AnimationClip EnterLeftVentAnim;

	public AnimationClip ExitLeftVentAnim;

	public AnimationClip ClimbAnim;

	public AnimationClip ClimbDownAnim;

	public AnimationClip SpawnAnim;

	public AnimationClip SpawnLeftAnim;

	public AnimationClip KillTongueImpostor;

	public AnimationClip KillTongueVictim;

	public AnimationClip KillShootImpostor;

	public AnimationClip KillShootVictim;

	public AnimationClip KillNeckImpostor;

	public AnimationClip KillNeckVictim;

	public AnimationClip KillStabImpostor;

	public AnimationClip KillStabVictim;

	public AnimationClip KillRHMVictim;

	public int CustomKillAnimID;

	public string VisorID_KillAnim;

	public string HatID_KillAnim;
}
