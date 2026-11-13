using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PolyAndCode.UI
{
	public class RecyclableScrollRect : ScrollRect
	{
		public enum DirectionType
		{
			Vertical,
			Horizontal
		}

		[HideInInspector]
		public IRecyclableScrollRectDataSource DataSource;

		public bool IsGrid;

		public RectTransform PrototypeCell;

		public bool SelfInitialize = true;

		public DirectionType Direction;

		[SerializeField]
		private int _segments;

		private RecyclingSystem _recyclingSystem;

		private Vector2 _prevAnchoredPos;

		public int Segments
		{
			get
			{
				return _segments;
			}
			set
			{
				_segments = Math.Max(value, 2);
			}
		}

		public IEnumerator<ICell> GetCells()
		{
			return _recyclingSystem.GetCells();
		}

		protected override void Start()
		{
			vertical = true;
			horizontal = false;
			if (Application.isPlaying && SelfInitialize)
			{
				Initialize();
			}
		}

		private void Initialize()
		{
			if (Direction == DirectionType.Vertical)
			{
				_recyclingSystem = new VerticalRecyclingSystem(PrototypeCell, viewport, content, DataSource, IsGrid, Segments);
			}
			else if (Direction == DirectionType.Horizontal)
			{
				_recyclingSystem = new HorizontalRecyclingSystem(PrototypeCell, viewport, content, DataSource, IsGrid, Segments);
			}
			vertical = Direction == DirectionType.Vertical;
			horizontal = Direction == DirectionType.Horizontal;
			_prevAnchoredPos = content.anchoredPosition;
			onValueChanged.RemoveListener(OnValueChangedListener);
			StartCoroutine(_recyclingSystem.InitCoroutine(() =>
			{
				onValueChanged.AddListener(OnValueChangedListener);
			}));
		}

		public void Initialize(IRecyclableScrollRectDataSource dataSource)
		{
			DataSource = dataSource;
			Initialize();
		}

		public void OnValueChangedListener(Vector2 normalizedPos)
		{
			Vector2 direction = content.anchoredPosition - _prevAnchoredPos;
			m_ContentStartPosition += _recyclingSystem.OnValueChangedListener(direction);
			_prevAnchoredPos = content.anchoredPosition;
		}

		public void ReloadData()
		{
			ReloadData(DataSource);
		}

		public void ReloadData(IRecyclableScrollRectDataSource dataSource)
		{
			if (_recyclingSystem != null)
			{
				StopMovement();
				onValueChanged.RemoveListener(OnValueChangedListener);
				_recyclingSystem.DataSource = dataSource;
				StartCoroutine(_recyclingSystem.InitCoroutine(() =>
				{
					onValueChanged.AddListener(OnValueChangedListener);
				}));
				_prevAnchoredPos = content.anchoredPosition;
			}
		}

		public void ScrollToBottom()
		{
			onValueChanged.RemoveListener(OnValueChangedListener);
			((VerticalRecyclingSystem)_recyclingSystem).ScrollToEnd();
			verticalNormalizedPosition = 0f;
			StopMovement();
			onValueChanged.AddListener(OnValueChangedListener);
		}
	}
}
