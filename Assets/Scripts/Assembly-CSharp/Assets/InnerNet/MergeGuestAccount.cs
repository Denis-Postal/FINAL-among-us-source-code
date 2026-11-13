using Newtonsoft.Json;
using System;

namespace Assets.InnerNet
{
	public class MergeGuestAccount
	{
		[JsonProperty("merge_to_id_token")]
		public string MergeToToken;

		[JsonProperty("merge_from_id_token")]
		public string MergeFromToken;

		public static Request<MergeGuestAccount> ToMergeGuestAccount(string mergeToToken, string mergeFromToken)
		{
			MergeGuestAccount mergeGuestAccount = new MergeGuestAccount();
			if (mergeGuestAccount != null)
			{
				mergeGuestAccount.MergeToToken = mergeToToken;
				mergeGuestAccount.MergeFromToken = mergeFromToken;
				Request<MergeGuestAccount> request = new Request<MergeGuestAccount>();
				Request<MergeGuestAccount>.RequestData requestData = new Request<MergeGuestAccount>.RequestData();
				if (requestData != null)
				{
					requestData.Attributes = mergeGuestAccount;
					requestData.Type = "guest";
					if (request != null)
					{
						request.Data = requestData;
						return request;
					}
				}
			}
			return (Request<MergeGuestAccount>)(object)new NullReferenceException();
		}
	}
}
