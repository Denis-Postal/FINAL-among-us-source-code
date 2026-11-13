// No-op Sentry shim (offline build). TODO(verify)
using System;
using System.Collections.Generic;
namespace Sentry
{
	public enum BreadcrumbLevel { Debug, Info, Warning, Error, Critical }
	public enum AttachmentType { Default, EventAttachment, Minidump, AppleCrashReport, UnrealContext, UnrealLogs, ViewHierarchy }
	public static class ScopeExtensions
	{
		public static void AddAttachment(this Scope scope, byte[] data, string name, AttachmentType type = AttachmentType.Default, string contentType = null) { }
	}
	public static class SentrySdk
	{
		public static void ConfigureScope(Action<Scope> cb) { }
		public static void AddBreadcrumb(string message, string category = null, string type = null, IDictionary<string, string> data = null, BreadcrumbLevel level = BreadcrumbLevel.Info) { }
		public static SentryId CaptureMessage(string message, Action<Scope> cb, SentryLevel level = SentryLevel.Info) => default;
		public static SentryId CaptureException(Exception e, Action<Scope> cb) => default;
		public static void CaptureUserFeedback(SentryId id, string email, string comments, string name = null) { }
		public static bool IsEnabled => false;
	}
}
