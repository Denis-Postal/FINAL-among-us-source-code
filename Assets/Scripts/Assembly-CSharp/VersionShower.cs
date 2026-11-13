using TMPro;
using UnityEngine;

public class VersionShower : MonoBehaviour
{
	public TextMeshPro text;

	public void Start()
	{
		string text = "v" + DestroyableSingleton<ReferenceDataManager>.Instance.Refdata.userFacingVersion;
		text += "s";
		if (!DetectTamper.Detect())
		{
			text += "h";
		}
		if (!string.IsNullOrEmpty("(build num: 7489)"))
		{
			text += " (build num: 7489)";
		}
		this.text.text = text;
		Screen.sleepTimeout = -1;
		Debug.Log("Among Us Version " + text + " Pipeline Build Num: " + 7489);
	}
}
