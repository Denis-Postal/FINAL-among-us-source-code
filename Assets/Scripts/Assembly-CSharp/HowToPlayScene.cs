using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using AmongUs.GameOptions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayScene : MonoBehaviour
{
	public enum HowToPlayCategory
	{
		None = 0,
		GameMode_Classic = 100,
		GameMode_HideAndSeek = 101,
		Roles = 200,
		RolesSelection = 201
	}

	public static string SceneOnClose;

	[Header("UI")]
	[SerializeField]
	private GameObject startPage;

	[Space(10f)]
	[SerializeField]
	private GameObject arrows;

	[SerializeField]
	private SpriteRenderer leftButton;

	[SerializeField]
	private SpriteRenderer rightButton;

	[SerializeField]
	[Space(10f)]
	private UiElement closeButton;

	[SerializeField]
	[Space(10f)]
	private Transform dotParent;

	[SerializeField]
	private Transform dotTemplate;

	[SerializeField]
	[Header("Game Mode Scenes")]
	private AbstractHowToPlayScenePage[] classicScenes;

	[SerializeField]
	private AbstractHowToPlayScenePage[] hideAndSeekScenes;

	[SerializeField]
	[Space(10f)]
	private ConditionalHowToPlayScenePage pcMove;

	[Header("Misc Scenes")]
	[SerializeField]
	private GameObject roleSelectionScene;

	[SerializeField]
	private Transform roleButtonsParent;

	[SerializeField]
	private GameObject roleButtonPrefab;

	private HashSet<HowToPlayRoleButton> roleButtons = new HashSet<HowToPlayRoleButton>();

	[SerializeField]
	private PassiveButton roleRightArrowButton;

	[SerializeField]
	private PassiveButton roleLeftArrowButton;

	[SerializeField]
	private List<HowToPlayRolePage> rolesScenes;

	[SerializeField]
	[Header("Console Controller Navigation")]
	private UiElement defaultButtonSelected;

	private UiElement previouslySelectedRoleButton;

	[SerializeField]
	private List<UiElement> controllerSelectables;

	private int sceneIndex;

	private int rolePageIndex;

	private Transform[] dots;

	private HowToPlayCategory category;

	public void Start()
	{
		category = HowToPlayCategory.None;
		arrows.SetActive(value: false);
		startPage.SetActive(value: true);
		ControllerManager.Instance.NewScene(name, closeButton, defaultButtonSelected, controllerSelectables);
		SetupDots(0);
	}

	public void StartClassic()
	{
		sceneIndex = 0;
		category = HowToPlayCategory.GameMode_Classic;
		startPage.SetActive(value: false);
		arrows.SetActive(value: true);
		classicScenes[2] = pcMove;
		DisableAllScenes();
		SetupDots(11);
		ChangeScene(0);
		ControllerManager.Instance.SetDefaultSelection(defaultButtonSelected);
	}

	public void StartHideAndSeek()
	{
		sceneIndex = 0;
		category = HowToPlayCategory.GameMode_HideAndSeek;
		startPage.SetActive(value: false);
		arrows.SetActive(value: true);
		DisableAllScenes();
		SetupDots(5);
		ChangeScene(0);
		ControllerManager.Instance.SetDefaultSelection(defaultButtonSelected);
	}

	public void OpenRolesSelectionMenu()
	{
		sceneIndex = 0;
		category = HowToPlayCategory.RolesSelection;
		startPage.SetActive(value: false);
		if (roleButtonsParent.childCount == 0)
		{
			foreach (RoleBehaviour role in DestroyableSingleton<RoleManager>.Instance.AllRoles.OrderBy((RoleBehaviour roleBehaviour) => roleBehaviour.NiceName.ToString()))
			{
				if (!role.IsSimpleRole && role.Role != RoleTypes.CrewmateGhost && role.Role != RoleTypes.ImpostorGhost)
				{
					HowToPlayRoleButton component = Object.Instantiate(roleButtonPrefab, roleButtonsParent).GetComponent<HowToPlayRoleButton>();
					Sprite roleIcon = rolesScenes.First((HowToPlayRolePage r) => r.role == role.Role).roleIcon;
					component.SetRoleInfo(role, roleIcon);
					component.SetButtonAction(() =>
					{
						OpenRolePage(role.Role);
					});
					roleButtons.Add(component);
					controllerSelectables.Add(component.GetComponent<PassiveButton>());
				}
			}
			foreach (UiElement controllerSelectable in controllerSelectables)
			{
				controllerSelectable.ReceiveMouseOut();
			}
			ControllerManager.Instance.NewScene(name, closeButton, defaultButtonSelected, controllerSelectables);
		}
		DisableAllScenes();
		roleSelectionScene.SetActive(value: true);
		ControllerManager.Instance.SetDefaultSelection((previouslySelectedRoleButton != null) ? previouslySelectedRoleButton : defaultButtonSelected);
	}

	public void OpenRolesDescription()
	{
		OpenRolePage(RoleTypes.Crewmate);
	}

	public void OpenRolePage(RoleTypes roleType)
	{
		category = HowToPlayCategory.Roles;
		sceneIndex = rolesScenes.IndexOf(rolesScenes.First((HowToPlayRolePage r) => r.role == roleType));
		if (roleType != RoleTypes.Crewmate)
		{
			previouslySelectedRoleButton = roleButtons.First((HowToPlayRoleButton r) => r.GetRole().Role == roleType).GetComponent<PassiveButton>();
		}
		SetupDots(rolesScenes[sceneIndex].rolePages.Count);
		ChangeScene(0);
	}

	private void Reset()
	{
		DisableAllScenes();
		category = HowToPlayCategory.None;
		startPage.SetActive(value: true);
		arrows.SetActive(value: false);
		sceneIndex = 0;
		SetupDots(0);
	}

	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Close();
		}
	}

	private void DisableAllScenes()
	{
		classicScenes.ForEach((AbstractHowToPlayScenePage x) =>
		{
			x.gameObject.SetActive(value: false);
		});
		hideAndSeekScenes.ForEach((AbstractHowToPlayScenePage x) =>
		{
			x.gameObject.SetActive(value: false);
		});
		roleSelectionScene.SetActive(value: false);
		rolesScenes.ForEach((HowToPlayRolePage x) =>
		{
			x.rolePages.SetAllGameObjectsActive(isActive: false);
		});
	}

	public void NextScene()
	{
		ChangeScene(1);
	}

	public void PreviousScene()
	{
		ChangeScene(-1);
	}

	public void AdjustRolePage(int dir)
	{
		rolesScenes[sceneIndex].rolePages.SetAllGameObjectsActive(isActive: false);
		rolePageIndex += dir;
		rolePageIndex = Mathf.Clamp(rolePageIndex, 0, rolesScenes[sceneIndex].rolePages.Count - 1);
		rolesScenes[sceneIndex].rolePages[rolePageIndex].gameObject.SetActive(value: true);
		roleLeftArrowButton.gameObject.SetActive(rolePageIndex > 0);
		roleRightArrowButton.gameObject.SetActive(rolePageIndex < rolesScenes[sceneIndex].rolePages.Count - 1);
		SetActiveDot(rolePageIndex);
	}

	public void Close()
	{
		if (CanClose())
		{
			if (SceneOnClose != null)
			{
				ControllerManager.Instance.CloseOverlayMenu(name);
				string sceneOnClose = SceneOnClose;
				SceneOnClose = null;
				SceneManager.LoadScene(sceneOnClose);
			}
			else if (category == HowToPlayCategory.None)
			{
				ControllerManager.Instance.CloseOverlayMenu(name);
				SceneManager.LoadScene("MainMenu");
			}
			else if (category == HowToPlayCategory.Roles)
			{
				DisableAllScenes();
				OpenRolesSelectionMenu();
				SetupDots(0);
				roleLeftArrowButton.gameObject.SetActive(value: false);
				roleRightArrowButton.gameObject.SetActive(value: false);
			}
			else
			{
				Reset();
			}
		}
	}

	private void ChangeScene(int del)
	{
		switch (category)
		{
		default:
			Debug.LogWarning(string.Format("{0}.{1} has not been configured.", "HowToPlayCategory", category));
			break;
		case HowToPlayCategory.None:
			Debug.LogWarning("A HowToPlayCategory was not set. Aborting.");
			break;
		case HowToPlayCategory.GameMode_Classic:
			classicScenes[sceneIndex].gameObject.SetActive(value: false);
			sceneIndex = Mathf.Clamp(sceneIndex + del, 0, classicScenes.Length - 1);
			classicScenes[sceneIndex].gameObject.SetActive(value: true);
			rightButton.gameObject.SetActive(sceneIndex < classicScenes.Length - 1);
			break;
		case HowToPlayCategory.GameMode_HideAndSeek:
			hideAndSeekScenes[sceneIndex].gameObject.SetActive(value: false);
			sceneIndex = Mathf.Clamp(sceneIndex + del, 0, hideAndSeekScenes.Length - 1);
			hideAndSeekScenes[sceneIndex].gameObject.SetActive(value: true);
			rightButton.gameObject.SetActive(sceneIndex < hideAndSeekScenes.Length - 1);
			if (!DataManager.Player.Onboarding.ViewedHideAndSeekHowToPlay && sceneIndex == hideAndSeekScenes.Length - 1)
			{
				DataManager.Player.Onboarding.ViewedHideAndSeekHowToPlay = true;
			}
			break;
		case HowToPlayCategory.Roles:
			foreach (HowToPlayRolePage rolesScene in rolesScenes)
			{
				rolesScene.rolePages.SetAllGameObjectsActive(isActive: false);
			}
			rolePageIndex = 0;
			rolesScenes[sceneIndex].rolePages[rolePageIndex].gameObject.SetActive(value: true);
			arrows.SetActive(value: false);
			roleRightArrowButton.gameObject.SetActive(rolesScenes[sceneIndex].rolePages.Count > 1);
			roleLeftArrowButton.gameObject.SetActive(value: false);
			roleSelectionScene.SetActive(value: false);
			closeButton.gameObject.SetActive(CanClose());
			SetActiveDot(rolePageIndex);
			return;
		}
		closeButton.gameObject.SetActive(CanClose());
		SetActiveDot(sceneIndex);
		leftButton.gameObject.SetActive(sceneIndex > 0);
	}

	private bool CanClose()
	{
		return true;
	}

	private void SetupDots(int count)
	{
		if (dots != null)
		{
			Transform[] array = dots;
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy(array[i].gameObject);
			}
		}
		dots = new Transform[count];
		float num = ((count > 1) ? (count + 1) : 0);
		Vector3 a = new Vector3((0f - num) / 2f, 0f, 0f);
		Vector3 b = new Vector3(num / 2f, 0f, 0f);
		dotTemplate.gameObject.SetActive(value: true);
		for (int j = 0; j < count; j++)
		{
			float t = (float)j / ((float)count - 1f);
			dots[j] = Object.Instantiate(dotTemplate, dotParent);
			dots[j].localPosition = Vector3.Lerp(a, b, t);
			dots[j].name = $"Dot [{j:00}]";
		}
		dotTemplate.gameObject.SetActive(value: false);
		SetActiveDot(sceneIndex);
	}

	private void SetActiveDot(int index)
	{
		Transform[] array = dots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].localScale = Vector3.one;
		}
		if (index < dots.Length)
		{
			dots[index].localScale = new Vector3(1.5f, 1.5f, 1.5f);
		}
	}
}
