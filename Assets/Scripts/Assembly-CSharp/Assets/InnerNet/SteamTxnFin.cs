using Newtonsoft.Json;
using System;

namespace Assets.InnerNet
{
	public class SteamTxnFin
	{
		[JsonProperty("order_id")]
		public string orderId;

		public static Request<SteamTxnFin> Create(string orderId)
		{
			SteamTxnFin steamTxnFin = new SteamTxnFin();
			if (steamTxnFin != null)
			{
				steamTxnFin.orderId = orderId;
				Request<SteamTxnFin> request = new Request<SteamTxnFin>();
				Request<SteamTxnFin>.RequestData requestData = new Request<SteamTxnFin>.RequestData();
				if (requestData != null)
				{
					requestData.Attributes = steamTxnFin;
					requestData.Type = "steam_txn_finalize";
					if (request != null)
					{
						request.Data = requestData;
						return request;
					}
				}
			}
			return (Request<SteamTxnFin>)(object)new NullReferenceException();
		}
	}
}
