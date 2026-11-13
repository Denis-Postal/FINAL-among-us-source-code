using System;
using Cpp2ILInjected;
using Innersloth.IO;

namespace AmongUs.Data
{
	public abstract class AbstractUserSaveData : AbstractSaveData
	{
		protected abstract string FileName { get; }

		protected override void HandleSave()
		{
			string fileName = FileName;
			if (!AbstractSaveData.TrySaveToJsonFile(this, fileName))
			{
				Cpp2ILHelpers.NoteDecompilerIssue("Unknown call target operand: \"il2cpp_vm_object_is_inst\"");
				object obj2 = default;
				object obj = obj2;
				Cpp2ILHelpers.NoteDecompilerIssue("Indirect call: [v182 @ rdx_v7+1B8] (should have been resolved before IL gen)");
				string text = default;
				string message = "[" + text + "] Failed to save.";
				DataManager.logger.Error(message);
			}
		}

		protected override void HandleLoad()
		{
			if (HandleDoesFileExist())
			{
				string fileName = FileName;
				if (!AbstractSaveData.TryLoadAndOverwriteFromJsonFile(this, fileName))
				{
					Cpp2ILHelpers.NoteDecompilerIssue("Unknown call target operand: \"il2cpp_vm_object_is_inst\"");
					object obj2 = default;
					object obj = obj2;
					Cpp2ILHelpers.NoteDecompilerIssue("Indirect call: [v489 @ rdx_v25+1B8] (should have been resolved before IL gen)");
					string fileName2 = FileName;
					object obj3 = default;
					string message = "[" + (string)obj3 + "] Loading file '" + fileName2 + "' failed.";
					DataManager.logger.Error(message);
				}
			}
			else
			{
				Cpp2ILHelpers.NoteDecompilerIssue("Unknown call target operand: \"il2cpp_vm_object_is_inst\"");
				object obj5 = default;
				object obj4 = obj5;
				Cpp2ILHelpers.NoteDecompilerIssue("Indirect call: [v485 @ rdx_v8+1B8] (should have been resolved before IL gen)");
				string fileName3 = FileName;
				object obj6 = default;
				string message2 = "[" + (string)obj6 + "] File '" + fileName3 + "' does not exist. Using default value.";
				DataManager.logger.Info(message2);
			}
		}

		protected override void HandleDelete()
		{
			if (HandleDoesFileExist())
			{
				string[] array = new string[1];
				string fileName = FileName;
				array[0] = fileName;
				string dataPathTo = FileIO.GetDataPathTo(array);
				FileIO.Delete(dataPathTo);
			}
		}

		protected override bool HandleDoesFileExist()
		{
			// TODO(verify): reimplemented (decomp stub)
			return System.IO.File.Exists(System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, FileName));
		}

		private string GetFileName()
		{
			// TODO(verify): reimplemented (decomp stub)
			return FileName;
		}

		private bool ShouldIncludeUserIdInPath()
		{
			return false;
		}

		protected AbstractUserSaveData()
		{
			// TODO(verify): reimplemented (decomp stub)
			// base ctor subscribes groups
		}
	}
}
