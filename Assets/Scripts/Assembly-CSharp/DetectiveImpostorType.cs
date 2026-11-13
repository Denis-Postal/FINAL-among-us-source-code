using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class DetectiveImpostorType : MonoBehaviour
{
	[SerializeField]
	public SpriteRenderer impostorSpriteR;

	[SerializeField]
	public TextMeshPro impostorText;

	public RoleBehaviour myRole;

	public DetectiveNotesMinigame miniGame;

	public void Initialize(DetectiveNotesMinigame minigame, RoleBehaviour role)
	{
		miniGame = minigame;
		myRole = role;
		impostorText.text = DestroyableSingleton<TranslationController>.Instance.GetString(role.StringName);
		impostorSpriteR.sprite = role.RoleIconWhite;
	}

	public void Click()
	{
		miniGame.SetImpostorType(DestroyableSingleton<TranslationController>.Instance.GetString(myRole.StringName));
	}
}
