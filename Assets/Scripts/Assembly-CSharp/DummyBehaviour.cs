using System.Collections;
using UnityEngine;

public class DummyBehaviour : MonoBehaviour
{
	public int PlayerIdToVoteFor = -1;

	private PlayerControl myPlayer;

	private FloatRange voteTime = new FloatRange(3f, 8f);

	private bool voted;

	public PlayerControl Player
	{
		get
		{
			if (!(myPlayer == null))
			{
				return myPlayer;
			}
			return GetComponent<PlayerControl>();
		}
	}

	public void Start()
	{
		myPlayer = GetComponent<PlayerControl>();
	}

	public void Update()
	{
		NetworkedPlayerInfo data = myPlayer.Data;
		if (data == null || data.IsDead)
		{
			return;
		}
		if ((bool)MeetingHud.Instance)
		{
			if (!voted)
			{
				voted = true;
				StartCoroutine(DoVote());
			}
		}
		else
		{
			voted = false;
		}
	}

	private IEnumerator DoVote()
	{
		yield return new WaitForSeconds(voteTime.Next());
		byte suspectIdx = 253;
		if (PlayerIdToVoteFor == 253)
		{
			suspectIdx = 253;
		}
		if (PlayerIdToVoteFor >= 0)
		{
			suspectIdx = (byte)PlayerIdToVoteFor;
		}
		else
		{
			for (int i = 0; i < 100; i++)
			{
				int num = IntRange.Next(-1, GameData.Instance.PlayerCount);
				if (num < 0)
				{
					break;
				}
				NetworkedPlayerInfo networkedPlayerInfo = GameData.Instance.AllPlayers[num];
				if (!networkedPlayerInfo.IsDead)
				{
					suspectIdx = networkedPlayerInfo.PlayerId;
					break;
				}
			}
		}
		PlayerIdToVoteFor = -1;
		MeetingHud.Instance.CmdCastVote(myPlayer.PlayerId, suspectIdx);
	}
}
