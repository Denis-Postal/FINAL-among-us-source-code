using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AmongUs.Data;
using AmongUs.GameOptions;
using AmongUs.HTTP;
using AmongUs.Matchmaking;
using InnerNet;
using Newtonsoft.Json;
using UnityEngine;

public class HttpMatchmakerManager : DestroyableSingleton<HttpMatchmakerManager>
{
	public class MatchmakerResponse
	{
		public MatchmakerError[] Errors;
	}

	public class MatchmakerError
	{
		public DisconnectReasons Reason;

		public SanctionReasons SanctionReason;

		public DateTimeOffset EndsAt;
	}

	public class UserTokenRequestData
	{
		public string Puid { get; set; }

		public string Username { get; set; }

		public int ClientVersion { get; set; }

		public SupportedLangs Language { get; set; }
	}

	public class HostServer
	{
		public readonly string Ip;

		public readonly ushort Port;

		public HostServer(long ip, ushort port)
		{
			Ip = new IPAddress(ip).ToString();
			Port = port;
		}
	}

	public class MatchmakerFailure
	{
		public DisconnectReasons Reason { get; set; }

		public string CustomDisconnect { get; set; }

		public MatchmakerError MatchmakerError { get; set; }

		public bool ShouldGoOffline { get; set; }
	}

	[Serializable]
	public class Error
	{
		public DisconnectReasons Reason;
	}

	[Serializable]
	public class FindGameByCodeResponse
	{
		public List<Error> Errors;

		public GameListing Game;

		public StringNames Region;

		public string UntranslatedRegion;
	}

	[Serializable]
	public class FindGamesListFilteredResponse
	{
		[JsonProperty("games")]
		public List<GameListing> Games;

		[JsonProperty("metadata")]
		public GamesListMetadata Metadata;
	}

	[Serializable]
	public class GamesListMetadata
	{
		[JsonProperty("allGamesCount")]
		public int AllGamesCount;

		[JsonProperty("matchingGamesCount")]
		public int MatchingGamesCount;
	}

	private const string CONTEXT_AUTHENTICATE = "authenticate";

	private const string CONTEXT_REQUEST_GAME_CODE = "request gamecode server";

	private const string CONTEXT_FIND_HOST_SERVER = "find host server";

	private const string CONTEXT_REQUEST_GAMES_LIST = "request games list";

	private const string CONTEXT_GET_FILTER_TAGS = "get filter tags";

	private const string CONTEXT_GET_PERMITTED_FILTERS = "get permitted filters";

	private readonly Logger logger = new Logger(Logger.Category.Network, "HttpMatchmakerManager");

	private readonly GameOptionsFactory optionsFactory = new GameOptionsFactory(new UnityLogger());

	private JsonSerializerSettings jsonSettings;

	private static readonly Dictionary<string, StringNames> ContextMessages = new Dictionary<string, StringNames>
	{
		{
			"authenticate",
			StringNames.HttpErrorContextAuthenticate
		},
		{
			"request gamecode server",
			StringNames.HttpErrorContextRequestGameCode
		},
		{
			"find host server",
			StringNames.HttpErrorContextFindHostServer
		},
		{
			"request games list",
			StringNames.HttpErrorContextRequestGamesList
		}
	};

	private static readonly Dictionary<long, StringNames> ErrorMessages = new Dictionary<long, StringNames>
	{
		{
			0L,
			StringNames.HttpErrorUnknown
		},
		{
			400L,
			StringNames.HttpErrorBadRequest
		},
		{
			401L,
			StringNames.HttpErrorUnauthorized
		},
		{
			403L,
			StringNames.HttpErrorForbidden
		},
		{
			404L,
			StringNames.HttpErrorNotFound
		},
		{
			405L,
			StringNames.HttpErrorMethodNotAllowed
		},
		{
			408L,
			StringNames.HttpErrorRequestTimeout
		},
		{
			429L,
			StringNames.HttpErrorTooManyRequests
		},
		{
			500L,
			StringNames.HttpErrorInternalServerError
		},
		{
			502L,
			StringNames.HttpErrorBadGateway
		},
		{
			503L,
			StringNames.HttpErrorServiceUnavailable
		},
		{
			504L,
			StringNames.HttpErrorGatewayTimeout
		}
	};

	private MatchmakerToken token;

	private string base64Token;

	public override void Awake()
	{
		base.Awake();
		GameOptionsJsonConverter item = new GameOptionsJsonConverter(optionsFactory);
		jsonSettings = new JsonSerializerSettings
		{
			Converters = { (JsonConverter)item }
		};
	}

	public IEnumerator CoGetOrRefreshToken(Action<string> onGetToken)
	{
		if (TryReadCachedToken(out var matchmakerToken))
		{
			onGetToken(matchmakerToken);
		}
		else
		{
			yield return CoRefreshTokenInternal(onGetToken);
		}
	}

	private bool TryReadCachedToken(out string matchmakerToken)
	{
		if (token != null && token.IsValid)
		{
			matchmakerToken = base64Token;
			return true;
		}
		base64Token = null;
		token = null;
		matchmakerToken = null;
		return false;
	}

	private IEnumerator CoRefreshTokenInternal(Action<string> onGetToken)
	{
		if (string.IsNullOrEmpty(DestroyableSingleton<EOSManager>.Instance.UserIDToken))
		{
			AmongUsClient.Instance.LastCustomDisconnect = "User is not logged in - can not authenticate";
			AmongUsClient.Instance.LastDisconnectReason = DisconnectReasons.Custom;
			DestroyableSingleton<DisconnectPopup>.Instance.Show();
			yield break;
		}
		string url = DestroyableSingleton<ServerManager>.Instance.CurrentUdpServer.HttpUrl + "api/user";
		string body = JsonConvert.SerializeObject(new UserTokenRequestData
		{
			Puid = DestroyableSingleton<EOSManager>.Instance.ProductUserId,
			Username = DataManager.Player.Customization.Name,
			ClientVersion = Constants.GetBroadcastVersion(),
			Language = DataManager.Settings.Language.CurrentLanguage
		});
		RetryableWebRequest retryableWebRequest = RetryableWebRequest.Post(url, body);
		retryableWebRequest.SetOrReplaceRequestHeader("Content-Type", "application/json");
		retryableWebRequest.SetOrReplaceRequestHeader("Accept", "text/plain");
		retryableWebRequest.SetOrReplaceRequestHeader("Authorization", "Bearer " + DestroyableSingleton<EOSManager>.Instance.UserIDToken);
		bool tokenReceived = false;
		retryableWebRequest.SetOrReplaceSuccessCallback((string encodedToken) =>
		{
			if (!MatchmakerToken.TryParse(encodedToken, out var matchmakerToken))
			{
				logger.Warning("Hmm, the server sent a corrupt token: " + encodedToken);
			}
			else
			{
				base64Token = encodedToken;
				token = matchmakerToken;
				onGetToken(encodedToken);
			}
		});
		logger.Info("Sent UserIDToken Request");
		yield return CoSendRequest(retryableWebRequest, "authenticate");
	}

	public IEnumerator CoFindGameCode(int gameId, Action<HostServer, string> onGameHost)
	{
		string uri = DestroyableSingleton<ServerManager>.Instance.CurrentUdpServer.HttpUrl + "api/games?gameId=" + gameId;
		string matchmakerToken = string.Empty;
		yield return CoGetOrRefreshToken((string token) =>
		{
			matchmakerToken = token;
		});
		if (string.IsNullOrEmpty(matchmakerToken))
		{
			yield break;
		}
		RetryableWebRequest retryableWebRequest = RetryableWebRequest.Post(uri, "");
		retryableWebRequest.SetOrReplaceRequestHeader("Accept", "application/json");
		retryableWebRequest.SetOrReplaceRequestHeader("Authorization", "Bearer " + matchmakerToken);
		retryableWebRequest.SetOrReplaceSuccessCallback((string server) =>
		{
			try
			{
				onGameHost(JsonConvert.DeserializeObject<HostServer>(server), matchmakerToken);
			}
			catch (JsonException ex)
			{
				logger.Warning("CoFindGameCode failed to deserialize response: " + ex.Message);
				logger.Warning("\t" + server);
			}
		});
		logger.Info("Sent FindGameCode Request");
		yield return CoSendRequest(retryableWebRequest, "request gamecode server");
	}

	public IEnumerator CoFindGameInfo(int gameId, Action<FindGameByCodeResponse, string> onGameInfo)
	{
		IRegionInfo initialRegion = DestroyableSingleton<ServerManager>.Instance.CurrentRegion;
		List<IRegionInfo> regions = DestroyableSingleton<ServerManager>.Instance.AvailableRegions.ToList();
		regions.Remove(initialRegion);
		regions.Insert(0, initialRegion);
		bool foundGame = false;
		logger.Info($"Iterating through out {regions.Count} regions");
		for (int i = 0; i < regions.Count; i++)
		{
			IRegionInfo region = regions[i];
			DestroyableSingleton<ServerManager>.Instance.SetRegion(region);
			string uri = $"{DestroyableSingleton<ServerManager>.Instance.CurrentUdpServer.HttpUrl}api/games/{gameId}";
			string matchmakerToken = string.Empty;
			logger.Info("Selecting region: " + region.Name + ", with uri: " + uri);
			yield return CoGetOrRefreshToken((string token) =>
			{
				matchmakerToken = token;
			});
			if (string.IsNullOrEmpty(matchmakerToken))
			{
				DestroyableSingleton<ServerManager>.Instance.SetRegion(initialRegion);
				yield break;
			}
			RetryableWebRequest retryableWebRequest = RetryableWebRequest.Get(uri);
			retryableWebRequest.SetOrReplaceRequestHeader("Accept", "application/json");
			retryableWebRequest.SetOrReplaceRequestHeader("Authorization", "Bearer " + matchmakerToken);
			retryableWebRequest.SetOrReplaceSuccessCallback((string response) =>
			{
				try
				{
					FindGameByCodeResponse findGameByCodeResponse = JsonConvert.DeserializeObject<FindGameByCodeResponse>(response, jsonSettings);
					findGameByCodeResponse.Region = region.TranslateName;
					findGameByCodeResponse.UntranslatedRegion = region.Name;
					onGameInfo(findGameByCodeResponse, matchmakerToken);
					foundGame = true;
					DestroyableSingleton<ServerManager>.Instance.SetRegion(initialRegion);
				}
				catch (JsonException ex)
				{
					logger.Info("CoFindGameCode failed to deserialize response: " + ex.Message);
					logger.Info("\t" + response);
				}
			});
			yield return CoSendRequest(retryableWebRequest, "request gamecode server", 2, (MatchmakerFailure failure) =>
			{
				logger.Info("Unable to find game in region: " + DestroyableSingleton<ServerManager>.Instance.CurrentRegion.Name);
			});
			if (foundGame)
			{
				yield break;
			}
		}
		DestroyableSingleton<ServerManager>.Instance.SetRegion(initialRegion);
		SetDisconnectInfoAndShowPopup(new MatchmakerFailure
		{
			Reason = DisconnectReasons.GameNotFound,
			CustomDisconnect = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ErrorNotFoundGame),
			MatchmakerError = null,
			ShouldGoOffline = false
		});
	}

	public IEnumerator CoFindHostServer(Action<HostServer, string> onGameHost)
	{
		string uri = DestroyableSingleton<ServerManager>.Instance.CurrentUdpServer.HttpUrl + "api/games";
		if (!DestroyableSingleton<ServerManager>.Instance.TargetServer.IsNullOrWhiteSpace())
		{
			uri = uri + "?targetServer=" + GameCode.GameNameToInt("QQ" + DestroyableSingleton<ServerManager>.Instance.TargetServer);
		}
		string matchmakerToken = string.Empty;
		yield return CoGetOrRefreshToken((string token) =>
		{
			matchmakerToken = token;
		});
		if (string.IsNullOrEmpty(matchmakerToken))
		{
			yield break;
		}
		RetryableWebRequest retryableWebRequest = RetryableWebRequest.Put(uri, new byte[1]);
		retryableWebRequest.SetOrReplaceRequestHeader("Accept", "application/json");
		retryableWebRequest.SetOrReplaceRequestHeader("Authorization", "Bearer " + matchmakerToken);
		retryableWebRequest.SetOrReplaceSuccessCallback((string server) =>
		{
			try
			{
				onGameHost(JsonConvert.DeserializeObject<HostServer>(server), matchmakerToken);
			}
			catch (JsonException ex)
			{
				logger.Warning("CoFindHostServer failed to deserialize response: " + ex.Message);
				logger.Warning("\t" + server);
			}
		});
		logger.Info("Sent FindHost Request");
		yield return CoSendRequest(retryableWebRequest, "find host server");
	}

	public IEnumerator CoRequestGameListFiltered(IGameOptions gameSearchOptions, GameFilterOptions filterOpts, Action<InnerNetClient.TotalGameData, FindGamesListFilteredResponse> onGameList)
	{
		_ = gameSearchOptions.GameMode;
		if (AprilFoolsMode.IsAprilFoolsModeToggledOn)
		{
			GameModes aprilFoolsOnMode = gameSearchOptions.AprilFoolsOnMode;
			logger.Info($"Overriding matchmaking search to gameMode = {aprilFoolsOnMode}");
		}
		GameFiltersList gameFiltersList = new GameFiltersList();
		GameFilterSet filterSet = DestroyableSingleton<FindAGameManager>.Instance.GetFilterSet();
		CurrentModRegistration.UpdateFilterSetWithModRegistrationSettings(filterSet);
		gameFiltersList.FilterSets = new List<GameFilterSet> { filterSet };
		Dictionary<string, object> uriParams = new Dictionary<string, object> { 
		{
			"filter",
			JsonConvert.SerializeObject(gameFiltersList)
		} };
		string uri = DestroyableSingleton<ServerManager>.Instance.CurrentUdpServer.HttpUrl + "api/games/filtered?" + HttpUtils.ToUriQuery(uriParams);
		logger.Info("Trying to send request to: " + uri);
		string matchmakerToken = string.Empty;
		yield return CoGetOrRefreshToken((string token) =>
		{
			matchmakerToken = token;
		});
		if (string.IsNullOrEmpty(matchmakerToken))
		{
			yield break;
		}
		RetryableWebRequest retryableWebRequest = RetryableWebRequest.Get(uri);
		retryableWebRequest.SetOrReplaceRequestHeader("Accept", "application/json");
		retryableWebRequest.SetOrReplaceRequestHeader("Authorization", "Bearer " + matchmakerToken);
		retryableWebRequest.SetOrReplaceSuccessCallback((string gamesResponse) =>
		{
			InnerNetClient.TotalGameData arg = new InnerNetClient.TotalGameData();
			try
			{
				onGameList(arg, JsonConvert.DeserializeObject<FindGamesListFilteredResponse>(gamesResponse, jsonSettings));
			}
			catch (JsonException ex)
			{
				logger.Warning("CoRequestGameListFiltered failed to deserialize response: " + ex.Message);
				logger.Warning("\t" + gamesResponse);
			}
		});
		logger.Info("Sent GetGamesRequest");
		yield return CoSendRequest(retryableWebRequest, "request games list", 1, (MatchmakerFailure failure) =>
		{
			logger.Info("CoRequestGameListFiltered failed: " + failure.Reason);
			onGameList(new InnerNetClient.TotalGameData(), new FindGamesListFilteredResponse { Games = new List<GameListing>() });
		});
	}

	public IEnumerator CoRefreshTags(Action<HashSet<string>> onTags)
	{
		string matchmakerToken = string.Empty;
		yield return CoGetOrRefreshToken((string token) =>
		{
			matchmakerToken = token;
		});
		if (string.IsNullOrEmpty(matchmakerToken))
		{
			yield break;
		}
		Dictionary<string, object> uriParams = new Dictionary<string, object> { 
		{
			"lang",
			(uint)DataManager.Settings.Language.CurrentLanguage
		} };
		RetryableWebRequest retryableWebRequest = RetryableWebRequest.Get(DestroyableSingleton<ServerManager>.Instance.CurrentUdpServer.HttpUrl + "api/filtertags?" + HttpUtils.ToUriQuery(uriParams));
		retryableWebRequest.SetOrReplaceRequestHeader("Accept", "application/json");
		retryableWebRequest.SetOrReplaceRequestHeader("Authorization", "Bearer " + matchmakerToken);
		retryableWebRequest.SetOrReplaceSuccessCallback((string tags) =>
		{
			if (string.IsNullOrEmpty(tags))
			{
				onTags(new HashSet<string>());
				return;
			}
			try
			{
				onTags(JsonConvert.DeserializeObject<HashSet<string>>(tags));
			}
			catch (JsonException ex)
			{
				logger.Warning("CoRefreshTags failed to deserialize response: " + ex.Message);
				logger.Warning("\t" + tags);
			}
		});
		logger.Info("Sent GetFilterTags");
		yield return CoSendRequest(retryableWebRequest, "get filter tags", 2, (MatchmakerFailure failure) =>
		{
			logger.Warning("CoRefreshTags failed");
		});
	}

	public IEnumerator CoRefreshFilters(Action<PermittedFilters> onRefreshFilters)
	{
		string matchmakerToken = string.Empty;
		yield return CoGetOrRefreshToken((string token) =>
		{
			matchmakerToken = token;
		});
		if (string.IsNullOrEmpty(matchmakerToken))
		{
			yield break;
		}
		RetryableWebRequest retryableWebRequest = RetryableWebRequest.Get(DestroyableSingleton<ServerManager>.Instance.CurrentUdpServer.HttpUrl + "api/filters");
		retryableWebRequest.SetOrReplaceRequestHeader("Accept", "application/json");
		retryableWebRequest.SetOrReplaceRequestHeader("Authorization", "Bearer " + matchmakerToken);
		retryableWebRequest.SetOrReplaceSuccessCallback((string filtersString) =>
		{
			try
			{
				PermittedFilters obj = JsonConvert.DeserializeObject<PermittedFilters>(filtersString);
				onRefreshFilters(obj);
			}
			catch (JsonException ex)
			{
				logger.Warning("CoRefreshFilters failed to deserialize response: " + ex.Message);
				logger.Warning("\t" + filtersString);
			}
		});
		logger.Info("Sent CoRefreshFilters");
		yield return CoSendRequest(retryableWebRequest, "get permitted filters", 2, (MatchmakerFailure failure) =>
		{
			onRefreshFilters(null);
			logger.Warning("CoRefreshFilters failed");
		});
	}

	private IEnumerator CoSendRequest(RetryableWebRequest request, string context)
	{
		return CoSendRequest(request, context, 2, SetDisconnectInfoAndShowPopup);
	}

	private IEnumerator CoSendRequest(RetryableWebRequest request, string context, int maxRetries, Action<MatchmakerFailure> onFailure)
	{
		request.SetOrReplaceErrorCallback((RetryableWebRequest r) =>
		{
			string message = string.Join(Environment.NewLine, "CoSendRequest failed:", $"\tStatus Code: {r.ResponseCode}", "\tContext: " + context, "\tRequest: " + r.Method + " at " + r.Url, "\tResponse: " + (r.ResponseText ?? "null"), "\tError: " + (r.Error ?? "null"));
			logger.Info(message);
		});
		yield return request.CoSend(logger);
		if (request.IsSuccess)
		{
			yield break;
		}
		if (request.IsAuthError && context == "authenticate")
		{
			onFailure?.Invoke(BundleFailureInfo(request, context));
			yield break;
		}
		if (request.IsAuthError)
		{
			logger.Info("Authorization failure. Attempting to refresh token.");
			bool didRefreshToken = false;
			yield return CoRefreshTokenInternal((string token) =>
			{
				request.SetOrReplaceRequestHeader("Authorization", "Bearer " + token);
				didRefreshToken = true;
			});
			if (!didRefreshToken)
			{
				logger.Info("Failed to refresh matchmaker token.");
				onFailure?.Invoke(BundleFailureInfo(request, context));
				yield break;
			}
		}
		if (request.IsAuthError || request.IsTransientError)
		{
			float retryIntervalSeconds = 0.2f;
			int retryBackoffFactor = 2;
			for (int retries = 1; retries <= maxRetries; retries++)
			{
				float seconds = (float)((double)retryIntervalSeconds * Math.Pow(retryBackoffFactor, retries));
				yield return new WaitForSeconds(seconds);
				yield return request.CoSend(logger);
				if (request.IsSuccess)
				{
					yield break;
				}
				if (!request.IsTransientError)
				{
					break;
				}
			}
		}
		onFailure?.Invoke(BundleFailureInfo(request, context));
	}

	private MatchmakerFailure BundleFailureInfo(RetryableWebRequest request, string context)
	{
		if (!ContextMessages.TryGetValue(context, out var value))
		{
			value = StringNames.HttpErrorContextNone;
		}
		if (!ErrorMessages.TryGetValue(request.ResponseCode, out var value2))
		{
			value2 = StringNames.HttpErrorUnknown;
		}
		bool flag = request.ResponseCode == 0;
		string customDisconnect = (flag ? DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ErrorGenericOnlineDisconnect) : (DestroyableSingleton<TranslationController>.Instance.GetString(value) + ".\n" + DestroyableSingleton<TranslationController>.Instance.GetString(value2)));
		MatchmakerFailure matchmakerFailure = new MatchmakerFailure
		{
			Reason = DisconnectReasons.Custom,
			CustomDisconnect = customDisconnect,
			MatchmakerError = null,
			ShouldGoOffline = flag
		};
		if (string.IsNullOrEmpty(request.ResponseText))
		{
			logger.Warning(string.Format("HTTP-MM request failed internally\nStatus Code: {0}\nContext: {1}\nRequest: {2} at {3}\nResponse: {4}\nError: {5}", request.ResponseCode, context, request.Method, request.Url, request.ResponseText ?? "null", request.Error ?? "null"));
			return matchmakerFailure;
		}
		try
		{
			MatchmakerResponse matchmakerResponse = JsonConvert.DeserializeObject<MatchmakerResponse>(request.ResponseText);
			if (matchmakerResponse != null && matchmakerResponse.Errors != null && matchmakerResponse.Errors.Length != 0)
			{
				MatchmakerError matchmakerError = matchmakerResponse.Errors[0];
				matchmakerFailure.Reason = matchmakerError.Reason;
				matchmakerFailure.MatchmakerError = matchmakerError;
			}
			logger.Warning($"HTTP-MM request failed with reason\nStatus Code: {request.ResponseCode}\nContext: {context}\nRequest: {request.Method} at {request.Url}\nReason: {(matchmakerResponse != null && matchmakerResponse.Errors != null && matchmakerResponse.Errors.Length > 0 ? matchmakerResponse.Errors[0].Reason.ToString() : request.ResponseText)}");
		}
		catch (Exception ex)
		{
			logger.Warning(string.Format("HTTP-MM request failed but could not parse response\nStatus Code: {0}\nContext: {1}\nRequest: {2} at {3}\nResponse: {4}\nError: {5}\nStacktrace = {6}", request.ResponseCode, context, request.Method, request.Url, request.ResponseText ?? "null", ex.Message, ex.StackTrace));
		}
		return matchmakerFailure;
	}

	private void SetDisconnectInfoAndShowPopup(MatchmakerFailure failure)
	{
		AmongUsClient.Instance.LastDisconnectReason = failure.Reason;
		AmongUsClient.Instance.LastCustomDisconnect = failure.CustomDisconnect;
		DestroyableSingleton<MatchMaker>.Instance.NotConnecting();
		if (failure.MatchmakerError != null)
		{
			AmongUsClient.Instance.LastMatchmakerError = failure.MatchmakerError;
		}
		// Do not force offline mode on matchmaking failures:
		// if (failure.ShouldGoOffline) { DestroyableSingleton<EOSManager>.Instance.ContinueInOfflineMode(); }
		DestroyableSingleton<DisconnectPopup>.Instance.Show();
	}
}
