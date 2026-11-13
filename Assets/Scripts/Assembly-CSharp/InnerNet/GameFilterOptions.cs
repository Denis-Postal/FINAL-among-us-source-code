using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Hazel;

namespace InnerNet
{
	public class GameFilterOptions
	{
		[Serializable]
		private sealed class _003C_003Ec
		{
			public static readonly _003C_003Ec _003C_003E9;

			public static Func<StringNames, bool> _003C_003E9__2_0;

			public static Func<StringNames, string> _003C_003E9__2_1;

			static _003C_003Ec()
			{
				_003C_003Ec obj = new _003C_003Ec();
				_003C_003E9 = obj;
			}

			internal unsafe bool _003CResetToDefaults_003Eb__2_0(StringNames n)
			{
				return n.ToString().StartsWith("DefaultFilterTag_");
			}

			internal string _003CResetToDefaults_003Eb__2_1(StringNames n)
			{
				TranslationController instance = DestroyableSingleton<TranslationController>.Instance;
				object[] parts = Array.Empty<object>();
				if ((object)instance != null)
				{
					return instance.GetString(n, parts);
				}
				return (string)(object)new NullReferenceException();
			}
		}

		public DateTime LastRefreshTime = DateTime.MinValue;

		public HashSet<string> FilterTags= new HashSet<string>();

		public unsafe void ResetToDefaults(DateTime lastRefreshTime)
		{
			LastRefreshTime = lastRefreshTime;
			FilterTags = new HashSet<string>(from StringNames n in Enum.GetValues(typeof(StringNames))
				where n.ToString().StartsWith("DefaultFilterTag_")
				select DestroyableSingleton<TranslationController>.Instance.GetString(n));
		}

		public void Serialize(MessageWriter writer)
		{
			writer.WritePacked(FilterTags.Count);
			foreach (string filterTag in FilterTags)
			{
				writer.Write(filterTag);
			}
		}

		public void Deserialize(MessageReader reader)
		{
			FilterTags.Clear();
			int num = reader.ReadPackedInt32();
			if (num < 16)
			{
				for (int i = 0; i < num; i++)
				{
					FilterTags.Add(reader.ReadString());
				}
			}
		}

		public GameFilterOptions()
		{ }
	}
}
