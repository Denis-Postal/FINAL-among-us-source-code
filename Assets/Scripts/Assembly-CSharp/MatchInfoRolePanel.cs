using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class MatchInfoRolePanel : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro roleName;

	[SerializeField]
	private TextMeshPro roleDescription;

	[SerializeField]
	private TextMeshPro roleCount;

	[SerializeField]
	private SpriteRenderer roleIcon;

	private readonly string STENCIL_NAME= "_Stencil";

	public void SetPanel(RoleBehaviour role, int numPerGame, int chancePerGame)
	{
		roleName.text = role.NiceName;
		roleDescription.text = role.BlurbMed;
		roleIcon.sprite = role.RoleIconColor;
		roleCount.text = $"{numPerGame.ToString()} at {chancePerGame}%";
		roleIcon.material.SetInt(PlayerMaterial.MaskLayer, 50);
		roleName.fontMaterial.SetFloat(STENCIL_NAME, 50f);
		roleDescription.fontMaterial.SetFloat(STENCIL_NAME, 50f);
		roleCount.fontMaterial.SetFloat(STENCIL_NAME, 50f);
	}

	public MatchInfoRolePanel()
	{ }
}
