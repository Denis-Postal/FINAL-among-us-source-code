using Object = UnityEngine.Object;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class PingTracker : MonoBehaviour
{
	public TextMeshPro text;

	[SerializeField]
	private AspectPosition aspectPosition;

	private Vector3 lobbyPos= new Vector3(-0.21f, 0.65f, 0f);

	private Vector3 gamePos= new Vector3(-0.21f, 0.32f, 0f);

	private void Update()
	{
		if ((bool)AmongUsClient.Instance)
		{
			if (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay)
			{
				gameObject.SetActive(value: false);
			}
			if (AmongUsClient.Instance.IsGameStarted)
			{
				aspectPosition.DistanceFromEdge = gamePos;
			}
			else
			{
				aspectPosition.DistanceFromEdge = lobbyPos;
			}
			text.text = $"PING: <b>{AmongUsClient.Instance.Ping} ms</b>";
		}
	}

	public PingTracker()
	{ }
}
