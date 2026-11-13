using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using UnityEngine;

public class AdvancedRoleViewPanel : MonoBehaviour
{
	[SerializeField]
	private CategoryHeaderRoleVariant header;

	[SerializeField]
	private SpriteRenderer divider;

	[SerializeField]
	private ViewSettingsInfoPanel infoPanelOrigin;

	[SerializeField]
	private float xPosStart;

	[SerializeField]
	private float yPosStart;

	private const float START_BODY_HEIGHT = 1.08f;

	private const float BODY_SPACING = 0.8f;

	public unsafe float SetUp(RoleBehaviour role, float spacingY, int maskLayer)
	{
		header.SetHeader(role.StringName, maskLayer, role.TeamType == RoleTeamTypes.Crewmate, role.RoleIconSolid);
		divider.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		float num = yPosStart;
		float num2 = 1.08f;
		for (int i = 0; i < role.AllGameSettings.Count; i++)
		{
			BaseGameSetting baseGameSetting = role.AllGameSettings[i];
			ViewSettingsInfoPanel viewSettingsInfoPanel = Object.Instantiate(infoPanelOrigin);
			viewSettingsInfoPanel.transform.SetParent(transform);
			viewSettingsInfoPanel.transform.localScale = Vector3.one;
			viewSettingsInfoPanel.transform.localPosition = new Vector3(xPosStart, num, -2f);
			float value = GameOptionsManager.Instance.CurrentGameOptions.GetValue(baseGameSetting);
			if (baseGameSetting.Type == OptionTypes.Checkbox)
			{
				viewSettingsInfoPanel.SetInfoCheckbox(baseGameSetting.Title, maskLayer, value > 0f);
			}
			else
			{
				viewSettingsInfoPanel.SetInfo(baseGameSetting.Title, baseGameSetting.GetValueString(value), maskLayer);
			}
			num -= spacingY;
			if (i > 0)
			{
				num2 += 0.8f;
			}
		}
		return num2;
	}
}
