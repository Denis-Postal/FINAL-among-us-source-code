using System;
using System.Collections;
using UnityEngine;

public class InGamePlayerList : MonoBehaviour
{
	private enum ListState
	{
		Closed,
		Open,
		Opening,
		Closing
	}

	public Vector3 offsetFromEdge;

	private float windowWidth = 1f;

	private Vector3 openPosition;

	private Vector3 closedPosition;

	public float openTime = 0.5f;

	public float buttonHeight = 0.35f;

	public ObjectPoolBehavior buttonPool;

	public ControllerNavMenu controllerNavMenu;

	public SpriteRenderer backgroundSprite;

	public SpriteRenderer backgroundSpriteMask;

	public GameObject ChatScreen;

	public ControllerButtonBehavior OpenChatButton;

	private ListState currentState;

	private float openLevel;

	public static InGamePlayerList instance;

	public bool IsActive
	{
		get
		{
			if (currentState != ListState.Open)
			{
				return currentState == ListState.Opening;
			}
			return true;
		}
	}

	private void Awake()
	{
		instance = this;
		gameObject.SetActive(value: false);
		windowWidth = backgroundSprite.size.x;
		Camera main = Camera.main;
		_ = main.aspect;
		_ = main.orthographicSize;
		offsetFromEdge.x += windowWidth * 0.5f;
		SetBackgroundMaskLayer(0);
	}

	private void OnEnable()
	{
		openPosition = AspectPosition.ComputePosition(AspectPosition.EdgeAlignments.Left, offsetFromEdge);
		closedPosition = openPosition;
		closedPosition.x += (0f - windowWidth) * 1.05f;
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void RefreshMenu()
	{
		controllerNavMenu.ControllerSelectable.Clear();
		PopulateButtons();
		controllerNavMenu.DefaultButtonSelected = controllerNavMenu.ControllerSelectable[0];
	}

	public void ToggleActive()
	{
		if (!ChatScreen.activeInHierarchy && !(ShipStatus.Instance == null))
		{
			switch (currentState)
			{
			case ListState.Closed:
				currentState = ListState.Opening;
				gameObject.SetActive(value: true);
				RefreshMenu();
				StartCoroutine(TransitionCoroutine(1f));
				break;
			case ListState.Open:
				currentState = ListState.Closing;
				StartCoroutine(TransitionCoroutine(0f));
				break;
			case ListState.Opening:
				currentState = ListState.Closing;
				StopAllCoroutines();
				StartCoroutine(TransitionCoroutine(0f));
				break;
			case ListState.Closing:
				currentState = ListState.Opening;
				StopAllCoroutines();
				StartCoroutine(TransitionCoroutine(1f));
				break;
			}
		}
	}

	public void SetActive(bool active)
	{
		switch (currentState)
		{
		case ListState.Closed:
			if (active)
			{
				currentState = ListState.Opening;
				RefreshMenu();
				gameObject.SetActive(value: true);
				StartCoroutine(TransitionCoroutine(1f));
			}
			break;
		case ListState.Open:
			if (!active)
			{
				currentState = ListState.Closing;
				StartCoroutine(TransitionCoroutine(0f));
			}
			break;
		case ListState.Opening:
			if (!active)
			{
				currentState = ListState.Closing;
				StopAllCoroutines();
				StartCoroutine(TransitionCoroutine(0f));
			}
			break;
		case ListState.Closing:
			if (active)
			{
				currentState = ListState.Opening;
				StopAllCoroutines();
				StartCoroutine(TransitionCoroutine(1f));
			}
			break;
		}
	}

	private IEnumerator TransitionCoroutine(float targetValue)
	{
		switch (currentState)
		{
		case ListState.Opening:
			OpenChatButton.enabled = false;
			break;
		case ListState.Closing:
			OpenChatButton.enabled = true;
			break;
		}
		yield return null;
		float dir = Mathf.Sign(targetValue - openLevel);
		while (openLevel != targetValue)
		{
			openLevel += dir * Time.deltaTime / openTime;
			if ((dir > 0f && openLevel >= targetValue) || (dir < 0f && openLevel <= targetValue))
			{
				openLevel = targetValue;
			}
			float t = ((!(dir >= 0f)) ? Mathf.Sin(MathF.PI * (1f - 0.5f * openLevel)) : Mathf.Sin(MathF.PI / 2f * openLevel));
			transform.localPosition = Vector3.Lerp(closedPosition, openPosition, t);
			yield return null;
		}
		transform.localPosition = Vector3.Lerp(closedPosition, openPosition, targetValue);
		switch (currentState)
		{
		case ListState.Opening:
			currentState = ListState.Open;
			break;
		case ListState.Closing:
			currentState = ListState.Closed;
			gameObject.SetActive(value: false);
			break;
		}
	}

	private void SetBackgroundMaskLayer(int maskLayer)
	{
		backgroundSpriteMask.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		backgroundSpriteMask.material.SetInt("_MaskComp", 8);
	}

	private void PopulateButtons()
	{
		buttonPool.ReclaimAll();
		for (int i = 0; i < GameData.Instance.AllPlayers.Count; i++)
		{
			PlayerIdentifierButton component = buttonPool.Get<PoolableBehavior>().GetComponent<PlayerIdentifierButton>();
			Vector3 localPosition = new Vector3(0f, (0f - buttonHeight) * (float)i, 0f);
			component.transform.localPosition = localPosition;
			component.transform.localScale = Vector3.one;
			NetworkedPlayerInfo player = GameData.Instance.AllPlayers[i];
			component.Populate(player);
			UiElement componentInChildren = component.GetComponentInChildren<UiElement>(includeInactive: true);
			controllerNavMenu.ControllerSelectable.Add(componentInChildren);
		}
	}
}
