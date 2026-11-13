using Object = UnityEngine.Object;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class PopupDialog : MonoBehaviour
{
	public TextMeshPro workingText;

	public float secondsBetweenDots= 0.7f;

	public int maxDots= 3;

	public string currentProgressText= "";

	private float textUpdateTimer;

	private static PopupDialog instance;

	public static void Display()
	{
		if (!instance)
		{
			instance = Object.Instantiate(Resources.Load<GameObject>("WaitForConnectionDialog")).GetComponent<PopupDialog>();
		}
	}

	public static void Dispose()
	{
		if ((bool)instance)
		{
			Object.Destroy(instance);
		}
	}

	private void Start()
	{
		ControllerManager.Instance.enabled = false;
	}

	private void OnDestroy()
	{
		ControllerManager.Instance.enabled = true;
	}

	public void Update()
	{
		textUpdateTimer -= Time.unscaledDeltaTime;
		if (textUpdateTimer <= 0f)
		{
			textUpdateTimer += secondsBetweenDots;
			if (currentProgressText.Length == maxDots)
			{
				currentProgressText = "";
			}
			else
			{
				currentProgressText += ".";
			}
			workingText.text = currentProgressText;
		}
	}

	public PopupDialog()
	{ }
}
