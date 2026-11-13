using UnityEngine;

public class AccountSignInList : MonoBehaviour
{
	public AccountButton ButtonPrefab;

	public Scroller ButtonParent;

	public float ButtonStart = 0.5f;

	public float ButtonHeight = 0.5f;

	private AccountButton[] AllButtons;

	public AccountsMenu parent;

	public ControllerNavMenu controllerNavParent;

	public bool createAccount;

	public void Start()
	{
		Collider2D component = ButtonParent.GetComponent<Collider2D>();
		string[] array = new string[4] { "Steam", "Epic", "Apple", "Google" };
		Vector3 localPosition = new Vector3(0f, ButtonStart, -0.5f);
		AllButtons = new AccountButton[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			AccountButton button = Object.Instantiate(ButtonPrefab, ButtonParent.Inner);
			AllButtons[i] = button;
			button.Text.text = array[i];
			if (!createAccount)
			{
				button.Button.OnClick.AddListener(() =>
				{
					LogInWith(button);
				});
			}
			button.Button.ClickMask = component;
			button.transform.localPosition = localPosition;
			localPosition.y -= ButtonHeight;
			controllerNavParent.ControllerSelectable.Add(button.Button);
		}
		if (array.Length != 0)
		{
			controllerNavParent.DefaultButtonSelected = AllButtons[0].Button;
		}
		ButtonParent.SetBoundsMax((float)array.Length * ButtonHeight - 2f * ButtonStart - 0.1f, 0f);
	}

	public void LogInWith(AccountButton selected)
	{
	}
}
