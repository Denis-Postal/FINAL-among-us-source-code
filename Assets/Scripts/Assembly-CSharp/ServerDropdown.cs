using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServerDropdown : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer background;

	[SerializeField]
	private ObjectPoolBehavior ButtonPool;

	[SerializeField]
	private PassiveButton firstOption;

	[SerializeField]
	private float initialYPos = 0.85f;

	[SerializeField]
	private float y_posButton = -0.5f;

	private const float INITIAL_HEIGHT = 1.2f;

	private const float YOFFSET = -0.3f;

	private const float HEIGHT_INCREASE = 0.6f;

	private const float YOFFSET_BUTTON = -0.55f;

	private Action<string> serverSetAction;

	private Action closeAction;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	private UiElement defaultButtonSelected;

	private List<UiElement> controllerSelectable;

	private void Awake()
	{
		defaultButtonSelected = null;
		controllerSelectable = new List<UiElement>();
	}

	private void OnEnable()
	{
		FillServerOptions();
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, defaultButtonSelected, controllerSelectable);
	}

	public void Initialize(Action<string> setCallback, Action closeCallback)
	{
		serverSetAction = setCallback;
		closeAction = closeCallback;
	}

	private void FillServerOptions()
	{
		int num = 0;
		foreach (IRegionInfo item in DestroyableSingleton<ServerManager>.Instance.AvailableRegions.OrderBy((IRegionInfo s) => (uint)ServerManager.DefaultRegions.IndexOf((IRegionInfo d) => d.Name.Equals(s.Name))))
		{
			if (DestroyableSingleton<ServerManager>.Instance.CurrentRegion.Equals(item))
			{
				defaultButtonSelected = firstOption;
				firstOption.ChangeButtonText(DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(item.TranslateName, item.Name));
				continue;
			}
			IRegionInfo region = item;
			ServerListButton serverListButton = ButtonPool.Get<ServerListButton>();
			serverListButton.transform.localPosition = new Vector3(0f, y_posButton + -0.55f * (float)num, -1f);
			serverListButton.transform.localScale = Vector3.one;
			serverListButton.Text.text = DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(item.TranslateName, item.Name);
			serverListButton.Text.ForceMeshUpdate();
			serverListButton.Button.OnClick.RemoveAllListeners();
			serverListButton.Button.OnClick.AddListener(() =>
			{
				ChooseOption(region);
			});
			controllerSelectable.Add(serverListButton.Button);
			background.transform.localPosition = new Vector3(0f, initialYPos + -0.3f * (float)num, 0f);
			background.size = new Vector2(background.size.x, 1.2f + 0.6f * (float)num);
			num++;
		}
	}

	private void ChooseOption(IRegionInfo region)
	{
		DestroyableSingleton<ServerManager>.Instance.SetRegion(region);
		serverSetAction?.Invoke(DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(region.TranslateName, region.Name));
		Close();
	}

	public void OnDisable()
	{
		ButtonPool.ReclaimAll();
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public void Close()
	{
		closeAction?.Invoke();
		gameObject.SetActive(value: false);
	}
}
