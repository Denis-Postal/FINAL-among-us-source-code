using Newtonsoft.Json;
using System;
using InnerNet;

namespace Assets.InnerNet
{
	public class PlayerReportRequest
	{
		[JsonProperty("game_server_id")]
		public string GameServerId;

		[JsonProperty("game_id")]
		public int GameId;

		[JsonProperty("reportee_client_id")]
		public int ReporteeCliendId;

		[JsonProperty("reason_id")]
		public int ReasonId;

		public static Request<PlayerReportRequest> SendPlayerReportOutsideOfLobby(string gameServerId, int reporteeClientId, int roomId, ReportReasons reason)
		{
			PlayerReportRequest playerReportRequest = new PlayerReportRequest();
			playerReportRequest.GameServerId = gameServerId;
			playerReportRequest.ReporteeCliendId = reporteeClientId;
			playerReportRequest.GameId = roomId;
			playerReportRequest.ReasonId = (int)reason;
			return new Request<PlayerReportRequest>
			{
				Data = new Request<PlayerReportRequest>.RequestData
				{
					Attributes = playerReportRequest,
					Type = "player_report"
				}
			};
		}
	}
}
