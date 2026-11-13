using System;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatFavoriteIcon : MonoBehaviour
	{
		private const string STATE_ACTIVE_IDLE = "ActiveIdle";

		private const string STATE_INACTIVE_IDLE = "InactiveIdle";

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private BoxCollider2D collider;

		[SerializeField]
		private PassiveButton button;

		public Vector2 Size => collider.size;

		public event Action OnPressEvent;

		private void Awake()
		{
			button.AddOnClickListeners(OnButtonPressed);
		}

		public void Setup(bool active)
		{
			animator.Play(active ? "ActiveIdle" : "InactiveIdle");
			SetFavorite(active);
		}

		public void SetFavorite(bool isFavorite)
		{
			animator.SetBool("Active", isFavorite);
		}

		public void SetHeight(float height)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition.y = (0f - height) / 2f;
			transform.localPosition = localPosition;
			collider.size = new Vector2(collider.size.x, height);
		}

		private void OnButtonPressed()
		{
			OnPressEvent?.Invoke();
		}
	}
}
