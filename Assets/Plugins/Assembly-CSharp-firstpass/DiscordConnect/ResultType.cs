namespace DiscordConnect
{
	public enum ResultType
	{
		Ok = 0,
		InternalError = 1,
		Unauthorized = 2,
		BadRequest = 3,
		JsonError = 4,
		RuntimeCreateFailed = 5,
		OAuth2StateMismatch = 6,
		EventSendFailed = 7,
		NotFound = 8,
		GenericApiError = 9,
		NetworkError = 10,
		UrlParseError = 11,
		MissingAction = 12,
		UnsupportedAction = 13,
		MissingSecret = 14,
		InvalidActionUrl = 15,
		ActivityUpdateCancelled = 16
	}
}
