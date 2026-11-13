using System;
using System.Globalization;
using System.IO;
using Cpp2ILInjected;
using Innersloth.IO;

public class PersistentAccountLogger : DestroyableSingleton<PersistentAccountLogger>
{
	private StreamWriter sw;

	private void Start()
	{
		string persistentDataPath = PlatformPaths.persistentDataPath;
		string path = Path.Combine(persistentDataPath, "persLog.log");
		if (FileIO.Exists(path))
		{
			StreamWriter streamWriter = File.AppendText(path);
			sw = streamWriter;
		}
		else
		{
			StreamWriter streamWriter2 = File.CreateText(path);
			sw = streamWriter2;
		}
		sw.WriteLine("----------------------------------------------------------");
		if (sw != null)
		{
			DateTime now = DateTime.Now;
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			DateTime dateTime = default;
			string text = dateTime.ToString("dd-mm-yyyy-hh-mm-ss", invariantCulture);
			string value = text + ": " + "App Start";
			sw.WriteLine(value);
		}
	}

	public void AddStringToLog(string str)
	{
		if (sw != null)
		{
			DateTime now = DateTime.Now;
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			DateTime dateTime = default;
			string text = dateTime.ToString("dd-mm-yyyy-hh-mm-ss", invariantCulture);
			string value = text + ": " + str;
			sw.WriteLine(value);
		}
	}

	private void OnApplicationQuit()
	{
		// TODO(verify): reimplemented (decomp stub)
		// TODO(verify): original flushed the account log; nothing to flush offline
	}
}
