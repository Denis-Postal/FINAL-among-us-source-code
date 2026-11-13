#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace AmongUsGameTools
{
	// roles3: editor-only frame-stall detector for capture / play runs.
	// A background thread watches a heartbeat that the main thread refreshes every EditorApplication.update.
	// If the main thread does not tick for HangSeconds while a capture/play session is running, the thread writes
	// scratch/logs/hang_<pid>.txt ("HANG DETECTED" + the last log lines + managed stack of the watchdog's view) and kills ONLY this Unity process
	// (the capture wrapper then releases the Unity lock). It also enforces a wall-clock limit for capture runs (soft: exit play mode; hard: kill).
	// Configure with env vars: AMONGUS_HANG_SECONDS (default 20; stalls shorter than this are normal scene loads), AMONGUS_CAPTURE_MAX_SECONDS (default 540).
	[InitializeOnLoad]
	public static class Roles3HangWatchdog
	{
		const string CaptureKey = "AmongUsGame.MenuCapture";
		static long lastBeatTicks;
		static int beatCount;
		static bool playing;
		static readonly Queue<string> lastLines = new Queue<string>();
		static readonly object linesLock = new object();
		static Thread thread;
		static volatile bool stop;
		static double startedAt;
		static string logDir = "C:/spacemafia-game/scratch/logs";

		static Roles3HangWatchdog()
		{
			if(Application.isBatchMode == false && Environment.GetEnvironmentVariable("AMONGUS_HANG_FORCE") != "1") { /* interactive editor: still safe, only fires during capture runs */ }
			Application.logMessageReceivedThreaded += OnLog;
			EditorApplication.update += Beat;
			EditorApplication.playModeStateChanged += st => { if(st == PlayModeStateChange.ExitingPlayMode || st == PlayModeStateChange.ExitingEditMode) beatCount = 0; };
			AssemblyReloadEvents.beforeAssemblyReload += () => { stop = true; };
			lastBeatTicks = Stopwatch.GetTimestamp();
			startedAt = EditorApplication.timeSinceStartup;
			stop = false;
			thread = new Thread(Watch) { IsBackground = true, Name = "Roles3HangWatchdog" };
			thread.Start();
		}

		static void OnLog(string condition, string stackTrace, LogType type)
		{
			lock(linesLock)
			{
				lastLines.Enqueue("[" + type + "] " + (condition.Length > 300 ? condition.Substring(0, 300) : condition));
				while(lastLines.Count > 60) lastLines.Dequeue();
			}
		}

		static void Beat()
		{
			Interlocked.Exchange(ref lastBeatTicks, Stopwatch.GetTimestamp());
			beatCount++;
		}

		static double EnvSeconds(string name, double fallback)
		{
			double v;
			return double.TryParse(Environment.GetEnvironmentVariable(name), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out v) && v > 0 ? v : fallback;
		}

		static void Watch()
		{
			double hangSeconds = EnvSeconds("AMONGUS_HANG_SECONDS", 20);
			double maxSeconds = EnvSeconds("AMONGUS_CAPTURE_MAX_SECONDS", 540);
			var wall = Stopwatch.StartNew();
			bool softSent = false;
			while(!stop)
			{
				Thread.Sleep(1000);
				bool captureRun = SafeCaptureFlag();
				if(!captureRun) { continue; }
				double stall = (Stopwatch.GetTimestamp() - Interlocked.Read(ref lastBeatTicks)) / (double)Stopwatch.Frequency;
				if(playing && beatCount > 30 && stall > hangSeconds)
				{
					Kill("main thread produced no editor tick for " + stall.ToString("F1") + " s (limit " + hangSeconds + " s)");
					return;
				}
				if(wall.Elapsed.TotalSeconds > maxSeconds)
				{
					if(!softSent && stall < 3)
					{
						softSent = true;
						// soft stop: ask the main thread to leave play mode and exit (the game view closes with play mode)
						EditorApplication.delayCall += () => { try { EditorApplication.isPlaying = false; EditorApplication.delayCall += () => EditorApplication.Exit(3); } catch { } };
					}
					else if(wall.Elapsed.TotalSeconds > maxSeconds + 30)
					{
						Kill("capture exceeded " + maxSeconds + " s wall clock and did not exit after the soft stop");
						return;
					}
				}
			}
		}

		static bool SafeCaptureFlag()
		{
			// EditorPrefs is main-thread only: use the flag file written by Beat-time polling instead
			return flag;
		}
		static bool flag;
		[InitializeOnLoadMethod]
		static void Poll()
		{
			EditorApplication.update += () => { playing = EditorApplication.isPlaying; flag = EditorPrefs.GetBool(CaptureKey, false) || (playing && Environment.GetEnvironmentVariable("AMONGUS_HANG_ALWAYS") == "1"); };
		}

		static void Kill(string reason)
		{
			try
			{
				var sb = new StringBuilder();
				sb.AppendLine("HANG DETECTED: " + reason);
				sb.AppendLine("pid=" + Process.GetCurrentProcess().Id + " time=" + DateTime.Now.ToString("s"));
				sb.AppendLine("last log lines:");
				lock(linesLock) { foreach(var l in lastLines) sb.AppendLine("  " + l); }
				Directory.CreateDirectory(logDir);
				File.WriteAllText(Path.Combine(logDir, "hang_" + Process.GetCurrentProcess().Id + ".txt"), sb.ToString());
			}
			catch { }
			try { Process.GetCurrentProcess().Kill(); } catch { Environment.Exit(4); }
		}
	}
}
#endif
