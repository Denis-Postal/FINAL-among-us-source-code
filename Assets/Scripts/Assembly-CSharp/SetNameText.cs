using AmongUs.Data;
using AmongUs.Data.Player;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class SetNameText : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro nameText;

	private void Start()
	{
		nameText.text = DataManager.Player.Customization.Name;
	}
}
