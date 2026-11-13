using System.Linq;
using TMPro;
using UnityEngine;

public class HauntMenuMinigame : Minigame
{
	private enum HauntFilters
	{
		None,
		Impostor,
		Crewmate,
		Ghost
	}

	private const float MinVelMultiplier = 0.75f;

	private const float MaxVelMultiplier = 4f;

	private const float MinDistance = 0.05f;

	private const float MinOffset = 0.2f;

	private const float MaxOffset = 0.5f;

	private const float Damping = 0.7f;

	private const float Easing = 0.2f;

	[SerializeField]
	private TextMeshPro NameText;

	[SerializeField]
	private TextMeshPro FilterText;

	[SerializeField]
	private TextMeshPro HauntingText;

	private HauntFilters filterMode;

	[SerializeField]
	private PassiveButton[] FilterButtons;

	[SerializeField]
	private PassiveButton[] Arrows;

	private PlayerControl HauntTarget;

	private Vector2 Offset;

	private void Start()
	{
		if (!GameManager.Instance.RevealTeams)
		{
			FilterButtons[0].gameObject.SetActive(value: false);
		}
		int num = 0;
		int num2 = FilterButtons.Count((PassiveButton s) => s.isActiveAndEnabled);
		float num3 = 1.2f * (float)num2 / 2f;
		for (int num4 = 0; num4 < FilterButtons.Length; num4++)
		{
			PassiveButton passiveButton = FilterButtons[num4];
			if (passiveButton.isActiveAndEnabled)
			{
				passiveButton.transform.SetLocalX(FloatRange.SpreadToEdges(0f - num3, num3, num, num2));
				num++;
			}
		}
	}

	public override void Begin(PlayerTask task)
	{
		DestroyableSingleton<HudManager>.Instance.ToggleMapButton(isActive: false);
		Offset = Random.insideUnitCircle;
		if (Offset.magnitude < 0.2f)
		{
			Offset = Offset.normalized * 0.2f;
		}
		else if (Offset.magnitude > 0.5f)
		{
			Offset = Offset.normalized * 0.5f;
		}
		base.Begin(task);
		Vector2 myPos = PlayerControl.LocalPlayer.transform.position;
		PlayerControl hauntTarget = (from pc in PlayerControl.AllPlayerControls
			where !pc.AmOwner
			orderby Vector2.Distance(pc.transform.position, myPos)
			select pc).First();
		SetHauntTarget(hauntTarget);
	}

	private void FixedUpdate()
	{
		if (amClosing != CloseState.None)
		{
			return;
		}
		Vector2 normalizedVelocity = Vector2.zero;
		if ((bool)HauntTarget)
		{
			Vector2 vector = HauntTarget.GetTruePosition() + Offset;
			Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
			Vector2 vector2 = PlayerControl.LocalPlayer.MyPhysics.GetVelocity() / PlayerControl.LocalPlayer.MyPhysics.TrueSpeed;
			Vector2 vector3 = vector - truePosition;
			float magnitude = vector3.magnitude;
			if (magnitude > 0.05f)
			{
				vector3 = vector3.normalized * Mathf.Clamp(magnitude, 0.75f, 4f);
				normalizedVelocity = vector2 * 0.8f + vector3 * 0.2f;
			}
			else
			{
				normalizedVelocity *= 0.7f;
			}
		}
		PlayerControl.LocalPlayer.MyPhysics.SetNormalizedVelocity(normalizedVelocity);
	}

	public void SetFilter(int filterInt)
	{
		HauntFilters hauntFilters = (HauntFilters)filterInt;
		if (hauntFilters == filterMode)
		{
			hauntFilters = HauntFilters.None;
			FilterButtons[(int)(filterMode - 1)].GetComponent<ButtonRolloverHandler>().WaitClickUnselect();
		}
		if (filterMode != HauntFilters.None)
		{
			FilterButtons[(int)(filterMode - 1)].GetComponent<ButtonRolloverHandler>().ChangeOutColor(Color.clear);
		}
		filterMode = hauntFilters;
		if (filterMode != HauntFilters.None)
		{
			FilterButtons[(int)(filterMode - 1)].GetComponent<ButtonRolloverHandler>().ChangeOutColor(Color.white);
		}
		ChangePick(0);
	}

	private void SetFilterText()
	{
		if (HauntTarget.Data.IsDead)
		{
			FilterText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Ghost);
		}
		else if (HauntTarget.Data.Role.IsImpostor)
		{
			FilterText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Impostor);
		}
		else
		{
			FilterText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Crewmate);
		}
	}

	public void ChangePick(int direction)
	{
		PlayerControl[] players = (from pc in PlayerControl.AllPlayerControls
			where !pc.AmOwner
			where MatchesFilter(pc)
			orderby pc.Data.GetPlayerName(PlayerOutfitType.Default)
			select pc).ToArray();
		Arrows.ForEach((PassiveButton a) =>
		{
			a.gameObject.SetActive(players.Length > 1);
		});
		if (players.Length == 0)
		{
			SetHauntTarget(null);
			return;
		}
		int num = players.IndexOf(HauntTarget);
		if (num == -1)
		{
			num = 0;
		}
		num = (num + direction).Wrap(players.Length);
		SetHauntTarget(players[num]);
	}

	private bool MatchesFilter(PlayerControl pc)
	{
		switch (filterMode)
		{
		default:
			return true;
		case HauntFilters.Impostor:
			if (pc.Data.Role.TeamType == RoleTeamTypes.Impostor)
			{
				return !pc.Data.IsDead;
			}
			return false;
		case HauntFilters.Crewmate:
			if (pc.Data.Role.TeamType == RoleTeamTypes.Crewmate || (pc.Data.Role.TeamType == RoleTeamTypes.Impostor && GameManager.Instance.LogicOptions is LogicOptionsNormal))
			{
				return !pc.Data.IsDead;
			}
			return false;
		case HauntFilters.Ghost:
			return pc.Data.IsDead;
		}
	}

	private void SetHauntTarget(PlayerControl target)
	{
		if (target == null)
		{
			HauntTarget = null;
			NameText.text = "";
			FilterText.text = "";
			HauntingText.enabled = false;
		}
		else
		{
			HauntTarget = target;
			HauntingText.enabled = true;
			NameText.text = target.Data?.GetPlayerName(PlayerOutfitType.Default);
			SetFilterText();
		}
	}

	public override void Close()
	{
		DestroyableSingleton<HudManager>.Instance.ToggleMapButton(isActive: true);
		DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
		base.Close();
	}
}
