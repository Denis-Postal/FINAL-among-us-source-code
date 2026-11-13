using Object = UnityEngine.Object;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public abstract class AbstractQuickChatMenuPage : MonoBehaviour
	{
		public QuickChatMenu menu;

		public float width;

		public float height;

		public QuickChatMenu Menu => menu;

		public float Width => width;

		public float Height => height;

		public virtual PassiveButton DefaultSelection => null;

		public virtual void Initialize(QuickChatMenu menu, float width)
		{
			this.menu = menu;
			this.width = width;
		}

		public virtual void Show(float height)
		{
			this.height = height;
			gameObject.SetActive(value: true);
		}

		public virtual void Hide()
		{
			gameObject.SetActive(value: false);
		}
	}
}
