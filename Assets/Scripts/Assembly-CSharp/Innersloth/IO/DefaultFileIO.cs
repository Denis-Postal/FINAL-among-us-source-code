using System;
using System.IO;
using Steamworks;
using UnityEngine;

namespace Innersloth.IO
{
	public class DefaultFileIO : IPlatformFileIO
	{
		private string userName;

		private string platformName;

		public virtual string GetUserName()
		{
			if (string.IsNullOrWhiteSpace(userName))
			{
				userName = "offline"; // TODO(verify): Steam user id in the original
			}
			return userName;
		}

		public unsafe virtual string GetPlatformName()
		{
			// TODO(verify): reimplemented (decomp stub)
			platformName = "Windows"; // TODO(verify): offline build
			return platformName;
		}

		public string GetRootDataPath()
		{
			return PlatformPaths.persistentDataPath;
		}

		public virtual bool Exists(string path)
		{
			return File.Exists(path);
		}

		public virtual string ReadAllText(string path)
		{
			return File.ReadAllText(path);
		}

		public virtual void WriteAllText(string path, string contents)
		{
			string directoryName = Path.GetDirectoryName(path);
			DirectoryInfo directoryInfo = Directory.CreateDirectory(directoryName);
			File.WriteAllText(path, contents);
		}

		public virtual byte[] ReadAllBytes(string path)
		{
			return File.ReadAllBytes(path);
		}

		public virtual void WriteAllBytes(string path, byte[] bytes)
		{
			string directoryName = Path.GetDirectoryName(path);
			DirectoryInfo directoryInfo = Directory.CreateDirectory(directoryName);
			File.WriteAllBytes(path, bytes);
		}

		public virtual void Delete(string path)
		{
			File.Delete(path);
		}
	}
}
