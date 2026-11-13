using System;
using System.Collections.Generic;
using System.Text;
using Cpp2ILInjected;
using Sentry;
using UnityEngine;

public class Logger
{
	public enum Category
	{
		None,
		Addressables,
		Assets,
		Data,
		Gameplay,
		Input,
		Network,
		Server,
		UI,
		Ads
	}

	private static readonly Dictionary<Category, string> ColorDict= new Dictionary<Category, string>
	{
		{
			Category.Data,
			"#FC8C03"
		},
		{
			Category.Gameplay,
			"lime"
		},
		{
			Category.Network,
			"magenta"
		},
		{
			Category.Server,
			"#9159e4"
		},
		{
			Category.UI,
			"yellow"
		}
	} ;

	public static readonly Logger GlobalInstance= new Logger();

	private static StringBuilder builder= new StringBuilder();

	public readonly Category category;

	public readonly string[] subCategories= new string[0];

	public Logger()
	{
		category = Category.None;
		subCategories = new string[0];
	}

	public Logger(Category category)
	{
		this.category = category;
		subCategories = new string[0];
	}

	public Logger(Category category, params string[] subCategories)
	{
		this.category = category;
		if (subCategories == null || subCategories.Length < 1)
		{
			this.subCategories = new string[0];
		}
		else
		{
			this.subCategories = subCategories;
		}
	}

	public void Debug(object message, UnityEngine.Object context = null)
	{
		Debug(null, message, context);
	}

	public void Debug(string[] path, object message, UnityEngine.Object context = null)
	{
		UnityEngine.Debug.Log(FormatMessageForDebugConsole(message, path), context);
		BugReportService.Instance.Debug(FormatMessageForBreadcrumb(message), FormatCategoryAndSubCategoryForBreadcrumb(path));
	}

	public void Info(object message, UnityEngine.Object context = null)
	{
		Info(null, message, context);
	}

	public void Info(string[] path, object message, UnityEngine.Object context = null)
	{
		UnityEngine.Debug.Log(FormatMessageForDebugConsole(message, path), context);
		BugReportService.Instance.Info(FormatMessageForBreadcrumb(message), FormatCategoryAndSubCategoryForBreadcrumb(path));
	}

	public void Warning(object message, UnityEngine.Object context = null)
	{
		Warning(null, message, context);
	}

	public void Warning(string[] path, object message, UnityEngine.Object context = null)
	{
		UnityEngine.Debug.LogWarning(FormatMessageForDebugConsole(message, path), context);
		BugReportService.Instance.Warning(FormatMessageForBreadcrumb(message), FormatCategoryAndSubCategoryForBreadcrumb(path));
	}

	public void Error(object message, UnityEngine.Object context = null)
	{
		Error(null, message, context);
	}

	public void Error(string[] path, object message, UnityEngine.Object context = null)
	{
		string text = FormatCategoryAndSubCategoryForBreadcrumb(path);
		if (!string.IsNullOrWhiteSpace(text))
		{
			text += " > ";
		}
		UnityEngine.Debug.LogError($"{text}{message}", context);
	}

	public void Exception(Exception e, UnityEngine.Object context = null)
	{
		UnityEngine.Debug.LogException(e, context);
	}

	private string FormatMessageForDebugConsole(object message, string[] path)
	{
		string arg = BuildDebugConsolePath(path);
		return $"{arg}{message}";
	}

	private unsafe string BuildDebugConsolePath(string[] path)
	{
		builder.Clear();
		if (category != Category.None)
		{
			Add(category.ToString());
		}
		if (subCategories != null)
		{
			for (int i = 0; i < subCategories.Length; i++)
			{
				Add(subCategories[i]);
			}
		}
		if (path != null)
		{
			for (int j = 0; j < path.Length; j++)
			{
				Add(path[j]);
			}
		}
		Add(null);
		return builder.ToString();
		static void Add(string value)
		{
			if (builder.Length > 0)
			{
				builder.Append(" > ");
			}
			if (!string.IsNullOrWhiteSpace(value))
			{
				builder.Append("[" + value + "]");
			}
		}
	}

	private string FormatCategoryAndSubCategoryForBreadcrumb(string[] path)
	{
		return BuildBreadcrumbPath(path);
	}

	private unsafe string BuildBreadcrumbPath(string[] path)
	{
		builder.Clear();
		if (category != Category.None)
		{
			Add(category.ToString());
		}
		if (subCategories != null)
		{
			for (int i = 0; i < subCategories.Length; i++)
			{
				Add(subCategories[i]);
			}
		}
		if (path != null)
		{
			for (int j = 0; j < path.Length; j++)
			{
				Add(path[j]);
			}
		}
		return builder.ToString();
		static void Add(string value)
		{
			if (builder.Length > 0)
			{
				builder.Append(" > ");
			}
			if (!string.IsNullOrWhiteSpace(value))
			{
				builder.Append(value);
			}
		}
	}

	private string FormatMessageForBreadcrumb(object message)
	{
		return $"{message}";
	}

	static Logger()
	{ }

	internal static void _003CBuildDebugConsolePath_003Eg__Add_007C19_0(string value)
	{
		int length = builder.Length;
		if (length > 0)
		{
			StringBuilder stringBuilder = builder.Append(" > ");
		}
		if (!string.IsNullOrWhiteSpace(value))
		{
			string value2 = "[" + value + "]";
			StringBuilder stringBuilder2 = builder.Append(value2);
		}
	}

	internal static void _003CBuildBreadcrumbPath_003Eg__Add_007C21_0(string value)
	{
		int length = builder.Length;
		if (length > 0)
		{
			StringBuilder stringBuilder = builder.Append(" > ");
		}
		if (!string.IsNullOrWhiteSpace(value))
		{
			StringBuilder stringBuilder2 = builder.Append(value);
		}
	}
}
