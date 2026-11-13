using System.Collections;

namespace Innersloth.DebugTool
{
	public interface IDebugToolInputProvider
	{
		IEnumerator CoInitialize();

		bool GetToggle();

		bool GetNavUp();

		bool GetNavDown();

		bool GetNavLeft();

		bool GetNavRight();

		bool GetConfirm();

		bool GetCancel();

		bool GetPagePrevious();

		bool GetPageNext();

		bool GetSubPagePrevious();

		bool GetSubPageNext();

		bool GetCopy();

		bool GetClear();
	}
}
