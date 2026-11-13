using Cpp2ILInjected;
using UnityEngine;

public class MatchInfoHudButton : MonoBehaviour
{
	[SerializeField]
	private AspectPosition aspectPosition;

	private static Vector3 defaultDistanceFromEdge= new Vector3(2.15f, 0.505f, -400f);

	private static Vector3 adjustedDistanceFromEdge= new Vector3(2.75f, 0.505f, -400f);

	private void Update()
	{
		if ((bool)MeetingHud.Instance || PlayerControl.LocalPlayer.Data.IsDead)
		{
			aspectPosition.DistanceFromEdge = adjustedDistanceFromEdge;
		}
		else
		{
			aspectPosition.DistanceFromEdge = defaultDistanceFromEdge;
		}
	}

	static MatchInfoHudButton()
	{ }
}
