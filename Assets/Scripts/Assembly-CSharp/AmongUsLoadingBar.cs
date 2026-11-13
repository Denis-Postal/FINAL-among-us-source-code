using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmongUsLoadingBar : MonoBehaviour
{
	[SerializeField]
	private Image barFill;

	[SerializeField]
	private TextMeshProUGUI percentText;

	[SerializeField]
	private TextMeshProUGUI loadingText;

	[SerializeField]
	private Transform crewmate;

	private void OnEnable()
	{
		barFill.fillAmount = 0f;
		percentText.text = string.Empty;
	}

	public unsafe void SetLoadingPercent(float percent, StringNames loadText = StringNames.None)
	{
		float fraction = percent / 100f;
		loadingText.text = loadText == StringNames.None ? string.Empty :
			DestroyableSingleton<TranslationController>.Instance.GetString(loadText, new object[0]);
		barFill.fillAmount = fraction;
		percentText.text = ((int)percent).ToString() + "%";
		Vector3 position = crewmate.position;
		position.x = Mathf.Lerp(UIExtension.MinX(barFill.rectTransform), UIExtension.MaxX(barFill.rectTransform), fraction);
		crewmate.position = position;
	}
}
