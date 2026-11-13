using TMPro;
using UnityEngine;

public class HowToPlaySceneRoleTitle : MonoBehaviour
{
	[SerializeField]
	private TMP_Text text;

	[SerializeField]
	private StringNames teamString;

	[SerializeField]
	private StringNames roleName;

	[SerializeField]
	private Color roleColor;

	private void Awake()
	{
		text.text = DestroyableSingleton<TranslationController>.Instance.GetString(teamString, "<b>" + DestroyableSingleton<TranslationController>.Instance.GetString(roleName).Color(roleColor) + "</b>");
	}
}
