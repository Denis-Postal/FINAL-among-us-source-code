using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugReportService
{
	public const string ENV_EDITOR = "editor";

	public const string ENV_DEVELOPMENT = "development";

	public const string ENV_STAGING = "staging";

	public const string ENV_PRODUCTION = "production";

	public const string TAG_ENVIRONMENT = "environment";

	public const string TAG_PLATFORM = "platform";

	public const string TAG_DISTRIBUTION = "distribution";

	public static BugReportService Instance = new BugReportService();

	private const float MinSecondsBetweenUserFeedbackSubmissions = 20f;

	private const string IssueTypeSentryTag = "among_us_issue_type";

	public static readonly StringNames[] BugReportCategories = new StringNames[5]
	{
		StringNames.BugReportCategoryGeneral,
		StringNames.BugReportCategoryGameplayIssue,
		StringNames.BugReportCategoryAccountManagement,
		StringNames.BugReportCategoryServerIssues,
		StringNames.BugReportCategoryBilling
	};

	private float timeOfLastSentUserFeedback = -20f;

	public static bool IsRuntimeProductionBuild()
	{
		return GetRuntimeEnvironmentString() == "production";
	}

	public static bool IsBuildTimeProductionBuild()
	{
		return GetBuildEnvironmentString() == "production";
	}

	public static string GetRuntimeEnvironmentString()
	{
		if (Application.isEditor)
		{
			return "editor";
		}
		return GetBuildEnvironmentString();
	}

	public static string GetBuildEnvironmentString()
	{
		return "production";
	}

	public static string GetPlatformString()
	{
		switch (Application.platform)
		{
		default:
			return "Unknown";
		case RuntimePlatform.OSXEditor:
			return "MacOS (Unity Editor)";
		case RuntimePlatform.WindowsEditor:
			return "Windows (Unity Editor)";
		case RuntimePlatform.OSXPlayer:
			return "MacOS";
		case RuntimePlatform.WindowsPlayer:
			return "Windows (Steam)";
		case RuntimePlatform.IPhonePlayer:
			return "iOS";
		case RuntimePlatform.Android:
			return "Android (Google Play)";
		case RuntimePlatform.LinuxEditor:
			return "Linux (Unity Editor)";
		case RuntimePlatform.LinuxPlayer:
			return "Linux";
		case RuntimePlatform.WebGLPlayer:
			return "WebGL";
		case RuntimePlatform.MetroPlayerX86:
			return "Windows Store (x86)";
		case RuntimePlatform.MetroPlayerX64:
			return "Windows Store (x64)";
		case RuntimePlatform.MetroPlayerARM:
			return "Windows Store (ARM)";
		case RuntimePlatform.Switch:
			return "Switch";
		case RuntimePlatform.PS4:
			return "PS4";
		case RuntimePlatform.PS5:
			return "PS5";
		case RuntimePlatform.XboxOne:
		case RuntimePlatform.GameCoreXboxOne:
			return "Xbox One";
		case RuntimePlatform.GameCoreXboxSeries:
			return "Xbox Series X/S";
		case RuntimePlatform.tvOS:
			return "tvOS";
		}
	}

	private static string GetDistributionString()
	{
		return "none";
	}

	// Offline remake: no Sentry/crash-report telemetry is ever sent. Breadcrumbs and errors are only written to the local Unity log.
	private BugReportService()
	{
	}

	public bool CanSubmitFeedback()
	{
		if (Time.time - timeOfLastSentUserFeedback < 20f)
		{
			return false;
		}
		return true;
	}

	public IEnumerator SubmitUserFeedbackWithScreenshot(StringNames bugReportCategory, string email, string comments)
	{
		if (!CanSubmitFeedback())
		{
			Error("Attempted to submit user feedback too quickly", "BugReportService");
			yield break;
		}
		yield return new WaitForEndOfFrame();
		SendFeedback(email, comments, bugReportCategory.ToString());
	}

	public void SubmitUserFeedback(StringNames bugReportCategory, string email, string comments)
	{
		if (!CanSubmitFeedback())
		{
			Error("Attempted to submit user feedback too quickly", "BugReportService");
			return;
		}
		SendFeedback(email, comments, bugReportCategory.ToString());
	}

	public void Info(string message, string category)
	{
		UnityEngine.Debug.Log("[" + category + "] " + message);
	}

	public void Debug(string message, string category)
	{
		UnityEngine.Debug.Log("[" + category + "] " + message);
	}

	public void Warning(string message, string category)
	{
		UnityEngine.Debug.LogWarning("[" + category + "] " + message);
	}

	public void Error(string message, string category)
	{
		UnityEngine.Debug.LogError("[" + category + "] " + message);
	}

	public void Exception(Exception exception)
	{
		UnityEngine.Debug.LogException(exception);
	}

	private void SendFeedback(string email, string comments, string category)
	{
		// Feedback stays on this machine (local log only).
		UnityEngine.Debug.Log("[BugReportService] user feedback (" + category + ") not sent: offline build");
		timeOfLastSentUserFeedback = Time.time;
	}
}
