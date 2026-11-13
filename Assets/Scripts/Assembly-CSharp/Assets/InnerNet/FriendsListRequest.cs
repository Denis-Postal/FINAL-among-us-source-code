using Newtonsoft.Json;
using System;

namespace Assets.InnerNet
{
	public class FriendsListRequest
	{
		[JsonProperty("username")]
		public string Username;

		[JsonProperty("recipient_puid")]
		public string RecipientId;

		[JsonProperty("recipient_friendcode")]
		public string RecipientUsername;

		public static Request<FriendsListRequest> ChangeUsername(string username)
		{
			FriendsListRequest friendsListRequest = new FriendsListRequest();
			if (friendsListRequest != null)
			{
				friendsListRequest.Username = username;
				Request<FriendsListRequest> request = new Request<FriendsListRequest>();
				Request<FriendsListRequest>.RequestData requestData = new Request<FriendsListRequest>.RequestData();
				if (requestData != null)
				{
					requestData.Attributes = friendsListRequest;
					requestData.Type = "change_username";
					if (request != null)
					{
						request.Data = requestData;
						return request;
					}
				}
			}
			return (Request<FriendsListRequest>)(object)new NullReferenceException();
		}

		public static Request<FriendsListRequest> SendRequest(string recipientId, string type)
		{
			FriendsListRequest friendsListRequest = new FriendsListRequest();
			if (friendsListRequest != null)
			{
				friendsListRequest.RecipientId = recipientId;
				Request<FriendsListRequest> request = new Request<FriendsListRequest>();
				Request<FriendsListRequest>.RequestData requestData = new Request<FriendsListRequest>.RequestData();
				if (requestData != null)
				{
					requestData.Attributes = friendsListRequest;
					requestData.Type = type;
					if (request != null)
					{
						request.Data = requestData;
						return request;
					}
				}
			}
			return (Request<FriendsListRequest>)(object)new NullReferenceException();
		}

		public static Request<FriendsListRequest> SendRequestByUsername(string recipientName)
		{
			FriendsListRequest friendsListRequest = new FriendsListRequest();
			if (friendsListRequest != null)
			{
				friendsListRequest.RecipientUsername = recipientName;
				Request<FriendsListRequest> request = new Request<FriendsListRequest>();
				Request<FriendsListRequest>.RequestData requestData = new Request<FriendsListRequest>.RequestData();
				if (requestData != null)
				{
					requestData.Attributes = friendsListRequest;
					requestData.Type = "friend_request_by_friendcode";
					if (request != null)
					{
						request.Data = requestData;
						return request;
					}
				}
			}
			return (Request<FriendsListRequest>)(object)new NullReferenceException();
		}
	}
}
