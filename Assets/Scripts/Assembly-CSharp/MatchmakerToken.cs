using System;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class MatchmakerToken
{
	public readonly MatchmakerTokenPayload Content;

	public readonly string Hash;

	[JsonIgnore]
	public bool IsValid
	{
		get
		{
			if (Content.ExpiresAt >= DateTime.UtcNow)
			{
				return Content.ClientVersion == Constants.GetBroadcastVersion();
			}
			return false;
		}
	}

	[JsonConstructor]
	public MatchmakerToken(MatchmakerTokenPayload content, string hash)
	{
		Content = content;
		Hash = hash;
	}

	public static bool TryParse(string base64Json, out MatchmakerToken token)
	{
		try
		{
			string value = Encoding.UTF8.GetString(Convert.FromBase64String(base64Json));
			token = JsonConvert.DeserializeObject<MatchmakerToken>(value);
			return true;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		token = null;
		return false;
	}
}
