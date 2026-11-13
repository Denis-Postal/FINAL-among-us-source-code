#if UNITY_EDITOR
// Builds Assets/Resources/AssetGuidRegistry.asset: guid -> asset for every Addressables-style AssetReference
// (m_AssetGUID: <guid>) found in project YAML. Used by the offline Addressables shim (unity_shims/Addressables.cs).
// Run: Unity -batchmode -nographics -quit -projectPath AmongUsGame -executeMethod AmongUsGameTools.BuildRegistry.Run
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class AmongUsGameTools_BuildRegistry { }
public static class BuildRegistryMenu
{
	[MenuItem("AmongUs/Build Asset Registry")]
	public static void Menu() { AmongUsGameTools.BuildRegistry.Run(); }
}
namespace AmongUsGameTools
{
	public static class BuildRegistry
	{
		[Serializable]
		private sealed class LocalCatalog
		{
			public string[] m_InternalIds;
			public string m_KeyDataString;
			public string m_BucketDataString;
			public string m_EntryDataString;
		}

		// Local adapter: AssetRipper generates new .meta GUIDs, while serialized
		// AssetReferences retain the original GUIDs. The shipped catalogue connects
		// each original GUID to its exact asset path, without remote bundle loading.
		private static Dictionary<string, List<string>> ReadCatalogPaths()
		{
			string path = Path.Combine(Application.dataPath, "StreamingAssets/aa/catalog.json");
			if (!File.Exists(path))
				path = "C:/2026AmongUsAssets/build/Among Us_Data/StreamingAssets/aa/catalog.json";
			if (!File.Exists(path))
				throw new FileNotFoundException("Local Addressables catalogue is required to map original GUIDs", path);
			LocalCatalog catalog = JsonUtility.FromJson<LocalCatalog>(File.ReadAllText(path));
			byte[] keys = Convert.FromBase64String(catalog.m_KeyDataString);
			byte[] buckets = Convert.FromBase64String(catalog.m_BucketDataString);
			byte[] entries = Convert.FromBase64String(catalog.m_EntryDataString);
			int count = BitConverter.ToInt32(buckets, 0);
			int cursor = 4;
			var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
			var guidPattern = new Regex(@"\A[0-9a-fA-F]{32}\z");
			for (int bucket = 0; bucket < count; bucket++)
			{
				int keyOffset = BitConverter.ToInt32(buckets, cursor);
				int entryCount = BitConverter.ToInt32(buckets, cursor + 4);
				cursor += 8;
				// ObjectSerialization type 0 is the ASCII string used for GUID keys.
				string key = keys[keyOffset] == 0
					? Encoding.ASCII.GetString(keys, keyOffset + 5, BitConverter.ToInt32(keys, keyOffset + 1)) : null;
				if (key != null && guidPattern.IsMatch(key))
				{
					for (int item = 0; item < entryCount; item++)
					{
						int entryIndex = BitConverter.ToInt32(buckets, cursor + item * 4);
						// Each location has seven Int32 fields. The first indexes m_InternalIds.
						int internalIndex = BitConverter.ToInt32(entries, 4 + entryIndex * 28);
						string assetPath = catalog.m_InternalIds[internalIndex].Replace('\\', '/');
						if (!assetPath.StartsWith("Assets/", StringComparison.Ordinal)) continue;
						if (!result.TryGetValue(key, out var paths)) result[key] = paths = new List<string>();
						if (!paths.Contains(assetPath)) paths.Add(assetPath);
					}
				}
				cursor += entryCount * 4;
			}
			Debug.Log("[BuildRegistry] local catalogue=" + path + " GUID paths=" + result.Count);
			return result;
		}

		public static void Run()
		{
			var rx = new Regex(@"m_AssetGUID: ([0-9a-f]{32})");
			var set = new HashSet<string>();
			string root = Application.dataPath;
			foreach (var f in Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories))
			{
				if (!(f.EndsWith(".prefab") || f.EndsWith(".asset") || f.EndsWith(".unity"))) continue;
				string txt;
				try { using (var sr = new StreamReader(f)) { var h = new char[10]; sr.Read(h, 0, 10); if (new string(h).IndexOf("%YAML") < 0) continue; } txt = File.ReadAllText(f); } catch { continue; }
				foreach (Match m in rx.Matches(txt)) set.Add(m.Groups[1].Value);
			}
			set.Add("55c71aa2de7bbe043be164fd8fc79d78"); // ReferenceData.asset (loaded by ReferenceDataManager offline init)
			var catalogPaths = ReadCatalogPaths();
			const string registryPath = "Assets/Resources/AssetGuidRegistry.asset";
			var reg = AssetDatabase.LoadAssetAtPath<AssetGuidRegistry>(registryPath);
			bool create = reg == null;
			if (create) reg = ScriptableObject.CreateInstance<AssetGuidRegistry>();
			reg.guids.Clear();
			reg.objects.Clear();
			int missing = 0;
			int mapped = 0;
			var sorted = new List<string>(set);
			sorted.Sort(StringComparer.Ordinal);
			var unresolved = new List<string>();
			foreach (var g in sorted)
			{
				string p = AssetDatabase.GUIDToAssetPath(g);
				var o = string.IsNullOrEmpty(p) ? null : AssetDatabase.LoadMainAssetAtPath(p);
				if (o == null && catalogPaths.TryGetValue(g, out var paths))
				{
					foreach (string candidate in paths)
					{
						o = AssetDatabase.LoadMainAssetAtPath(candidate);
						if (o != null) { p = candidate; mapped++; break; }
					}
				}
				if (o == null) { missing++; unresolved.Add(g); continue; }
				reg.guids.Add(g); reg.objects.Add(o);
				// AssetReference with a sub-object name (e.g. a Sprite inside a texture): also register "guid#spriteName" (and "guid#" = first sprite)
				if (o is Texture2D)
				{
					bool first = true;
					foreach (var sub in AssetDatabase.LoadAllAssetsAtPath(p))
					{
						if (!(sub is Sprite sp)) continue;
						reg.guids.Add(g + "#" + sp.name); reg.objects.Add(sp);
						if (first) { reg.guids.Add(g + "#"); reg.objects.Add(sp); first = false; }
					}
				}
			}
			Directory.CreateDirectory(Path.Combine(root, "Resources"));
			if (create) AssetDatabase.CreateAsset(reg, registryPath);
			else EditorUtility.SetDirty(reg);
			AssetDatabase.SaveAssets();
			string reportPath = Path.GetFullPath(Path.Combine(root, "../../scratch/reports/registry_unresolved.txt"));
			Directory.CreateDirectory(Path.GetDirectoryName(reportPath));
			File.WriteAllLines(reportPath, unresolved);
			Debug.Log("[BuildRegistry] guids referenced=" + set.Count + " resolved=" + reg.guids.Count + " catalogue mapped=" + mapped + " missing=" + missing);
		}
	}
}
#endif
