#if UNITY_EDITOR
// Headless Windows x64 Mono build:
//   Unity -batchmode -nographics -quit -projectPath AmongUsGame -executeMethod AmongUsGameTools.BuildWin.Run -logFile x.log
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace AmongUsGameTools
{
	public static class BuildWin
	{
		// The standalone player logged "Default UI Material doesn't have _Stencil property": the built-in UI shader was stripped, so every uGUI
		// element (debug overlay, SpiritGuide image buttons) rendered with the error shader (pink). Force the built-in UI/sprite/TMP shaders in.
		static void EnsureAlwaysIncludedShaders()
		{
			var so = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset")[0]);
			var list = so.FindProperty("m_AlwaysIncludedShaders");
			foreach (var name in new[] { "UI/Default", "UI/Default Font", "UI/DefaultETC1", "Sprites/Default", "Sprites/Mask", "TextMeshPro/Distance Field", "TextMeshPro/Mobile/Distance Field", "TextMeshPro/Sprite", "TextMeshPro/Bitmap", "TextMeshPro/Mobile/Bitmap" })
			{
				var sh = Shader.Find(name);
				if (sh == null) { Debug.Log("[Build] always-include shader not found: " + name); continue; }
				bool have = false;
				for (int i = 0; i < list.arraySize; i++) if (list.GetArrayElementAtIndex(i).objectReferenceValue == sh) have = true;
				if (have) continue;
				list.InsertArrayElementAtIndex(list.arraySize);
				list.GetArrayElementAtIndex(list.arraySize - 1).objectReferenceValue = sh;
				Debug.Log("[Build] always-include shader added: " + name);
			}
			so.ApplyModifiedProperties();
			AssetDatabase.SaveAssets();
		}

		public static void Run()
		{
			// never spawn external editors from batch builds
			EditorPrefs.SetString("kScriptsDefaultApp", "");
			try { BuildRegistry.Run(); } catch (System.Exception e) { Debug.LogError("BuildRegistry failed: " + e); }
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EnsureAlwaysIncludedShaders();
			PlayerSettings.SetScriptingBackend(UnityEditor.Build.NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);
			var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
			string outDir = System.Environment.GetEnvironmentVariable("AG_BUILD_DIR") ?? "C:/spacemafia-game/Build"; // AG_BUILD_DIR: build next to the running player instead of over it
			Directory.CreateDirectory(outDir);
			var opts = new BuildPlayerOptions { scenes = scenes, locationPathName = outDir + "/AmongUs.exe", target = BuildTarget.StandaloneWindows64, options = BuildOptions.None };
			var rep = BuildPipeline.BuildPlayer(opts);
			Debug.Log("BUILD RESULT: " + rep.summary.result + " errors=" + rep.summary.totalErrors + " size=" + rep.summary.totalSize);
			EditorApplication.Exit(rep.summary.result == BuildResult.Succeeded ? 0 : 1);
		}
	}
}
#endif
