using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Settings;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CreateOptionsPicker : MonoBehaviour
{
	private sealed class _003C_003Ec__DisplayClass12_0
	{
		public int numPlayers;

		public CreateOptionsPicker _003C_003E4__this;

		internal void _003CAwake_003Eb__0()
		{
			_003C_003E4__this.SetMaxPlayersButtons(numPlayers);
		}
	}

	private const float MaxPlayerButtonWidth = 0.5f;

	public SpriteRenderer MaxPlayerButtonPrefab;

	private List<SpriteRenderer> MaxPlayerButtons= new List<SpriteRenderer>();

	public Transform MaxPlayersRoot;

	public MapPickerMenu MapMenu;

	public ImpostorsOptionButton[] ImpostorButtons;

	public TextMeshPro LanguageButton;

	public TextMeshPro GameModeText;

	public TextMeshPro ImpostorText;

	public SettingsMode mode;

	public CrewVisualizer CrewArea;

	public CreateGameOptions optionsMenu;

	public unsafe void Awake()
	{
		if (ImpostorButtons != null && ImpostorButtons.Length == 3 && ImpostorButtons[2] != null)
		{
			List<ImpostorsOptionButton> list = new List<ImpostorsOptionButton>(ImpostorButtons);
			Vector3 p1 = ImpostorButtons[1].transform.localPosition;
			Vector3 p2 = ImpostorButtons[2].transform.localPosition;
			Vector3 offset = p2 - p1;
			for (int b = 4; b <= 5; b++)
			{
				ImpostorsOptionButton newBtn = UnityEngine.Object.Instantiate(ImpostorButtons[2], ImpostorButtons[2].transform.parent);
				newBtn.transform.localPosition = p2 + offset * (b - 3);
				newBtn.name = b.ToString();
				int num = b;
				TextMeshPro tmp = newBtn.GetComponentInChildren<TextMeshPro>();
				if (tmp != null)
				{
					tmp.text = b.ToString();
				}
				PassiveButton pb = newBtn.GetComponent<PassiveButton>();
				if (pb != null)
				{
					pb.OnClick.RemoveAllListeners();
					pb.OnClick.AddListener(() => SetImpostorButtons(num));
					if (optionsMenu != null && optionsMenu.ControllerSelectable != null)
					{
						optionsMenu.ControllerSelectable.Add(pb);
					}
				}
				list.Add(newBtn);
			}
			ImpostorButtons = list.ToArray();
		}

		if (!MaxPlayersRoot)
		{
			return;
		}
		for (int i = 4; i <= 15; i++)
		{
			SpriteRenderer spriteRenderer = Object.Instantiate(MaxPlayerButtonPrefab, MaxPlayersRoot);
			spriteRenderer.transform.localPosition = new Vector3((float)(i - 4) * 0.5f, 0f, 0f);
			int numPlayers = i;
			spriteRenderer.name = numPlayers.ToString();
			PassiveButton component = spriteRenderer.GetComponent<PassiveButton>();
			component.OnClick.AddListener(() =>
			{
				SetMaxPlayersButtons(numPlayers);
			});
			spriteRenderer.GetComponentInChildren<TextMeshPro>().text = spriteRenderer.name;
			MaxPlayerButtons.Add(spriteRenderer);
			optionsMenu.ControllerSelectable.Add(component);
		}
	}

	public void Start()
	{
		Refresh();
	}

	public void Refresh()
	{
		IGameOptions targetOptions = GetTargetOptions();
		// Comment out the current implementation:
		// if (targetOptions.TryGetIntArray(Int32ArrayOptionNames.MaxImpostors, out var value))
		// {
		// 	if (value[targetOptions.MaxPlayers] < 1)
		// 	{
		// 		SetImpostorButtons(1);
		// 	}
		// 	else if (value[targetOptions.MaxPlayers] > 3)
		// 	{
		// 		SetImpostorButtons(3);
		// 	}
		// }
		if (targetOptions.TryGetIntArray(Int32ArrayOptionNames.MaxImpostors, out var value) && value != null)
		{
			int maxForPlayers = (targetOptions.MaxPlayers < value.Length) ? value[targetOptions.MaxPlayers] : 5;
			if (targetOptions.NumImpostors < 1)
			{
				SetImpostorButtons(1);
			}
			else if (targetOptions.NumImpostors > maxForPlayers)
			{
				SetImpostorButtons(maxForPlayers);
			}
		}
		UpdateImpostorsButtons(targetOptions.NumImpostors);
		UpdateMaxPlayersButtons(targetOptions);
		UpdateLanguageButton((uint)targetOptions.Keywords);
		MapMenu.UpdateMapButtons(targetOptions.MapId);
		GameModeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(GameModesHelpers.ModeToName[GameOptionsManager.Instance.CurrentGameOptions.GameMode]);
	}

	public IGameOptions GetTargetOptions()
	{
		if (mode == SettingsMode.Host)
		{
			return GameOptionsManager.Instance.GameHostOptions;
		}
		IGameOptions gameSearchOptions = GameOptionsManager.Instance.GameSearchOptions;
		if (gameSearchOptions.MapId == 0)
		{
			gameSearchOptions.ToggleMapFilter(0);
		}
		return gameSearchOptions;
	}

	private void SetTargetOptions(IGameOptions data)
	{
		if (mode == SettingsMode.Host)
		{
			GameOptionsManager.Instance.GameHostOptions = data;
		}
		else
		{
			GameOptionsManager.Instance.GameSearchOptions = data;
		}
	}

	public void SetMaxPlayersButtons(int maxPlayers)
	{
		IGameOptions targetOptions = GetTargetOptions();
		if (!targetOptions.TryGetIntArray(Int32ArrayOptionNames.MinPlayers, out var value))
		{
			Debug.LogError("CreateOptions picker could not obtain MinPlayers array from game options during SetMaxPlayersButtons");
		}
		else if (maxPlayers >= value[targetOptions.NumImpostors])
		{
			targetOptions.SetInt(Int32OptionNames.MaxPlayers, maxPlayers);
			SetTargetOptions(targetOptions);
			if (DestroyableSingleton<FindAGameManager>.InstanceExists)
			{
				DestroyableSingleton<FindAGameManager>.Instance.ResetTimer();
			}
			UpdateMaxPlayersButtons(targetOptions);
		}
	}

	private unsafe void UpdateMaxPlayersButtons(IGameOptions opts)
	{
		if ((bool)CrewArea)
		{
			CrewArea.SetCrewSize(opts.MaxPlayers, opts.NumImpostors);
		}
		if (!opts.TryGetIntArray(Int32ArrayOptionNames.MinPlayers, out var value))
		{
			Debug.LogError("CreateOptions picker could not obtain MinPlayers array from game options during UpdateMaxPlayersButtons");
			return;
		}
		for (int i = 0; i < MaxPlayerButtons.Count; i++)
		{
			SpriteRenderer spriteRenderer = MaxPlayerButtons[i];
			spriteRenderer.enabled = spriteRenderer.name == opts.MaxPlayers.ToString();
			spriteRenderer.GetComponentInChildren<TextMeshPro>().color = ((int.Parse(spriteRenderer.name) < value[opts.NumImpostors]) ? Palette.DisabledGrey : Color.white);
		}
	}

	public void SetImpostorButtons(int numImpostors)
	{
		IGameOptions targetOptions = GetTargetOptions();
		int[] intArray = targetOptions.GetIntArray(Int32ArrayOptionNames.MaxImpostors);
		if (intArray != null && targetOptions.MaxPlayers < intArray.Length && numImpostors <= intArray[targetOptions.MaxPlayers])
		{
			targetOptions.SetInt(Int32OptionNames.NumImpostors, numImpostors);
			SetTargetOptions(targetOptions);
			if (!targetOptions.TryGetIntArray(Int32ArrayOptionNames.MinPlayers, out var value))
			{
				Debug.LogError("CreateOptions picker could not obtain MinPlayers array from game options during SetImpostorButtons");
				return;
			}
			int b = (value != null && numImpostors < value.Length) ? value[numImpostors] : numImpostors;
			SetMaxPlayersButtons(Mathf.Max(targetOptions.MaxPlayers, b));
			UpdateImpostorsButtons(numImpostors);
		}
	}

	private unsafe void UpdateImpostorsButtons(int numImpostors)
	{
		IGameOptions targetOptions = GetTargetOptions();
		// Comment out the current implementation:
		// bool flag = targetOptions.GetIntArray(Int32ArrayOptionNames.MaxImpostors)[targetOptions.MaxPlayers] <= 1;
		// for (int i = 0; i < ImpostorButtons.Length; i++)
		// {
		// 	ImpostorsOptionButton impostorsOptionButton = ImpostorButtons[i];
		// 	if ((bool)impostorsOptionButton.SpriteRenderer)
		// 	{
		// 		impostorsOptionButton.SpriteRenderer.enabled = impostorsOptionButton.name == numImpostors.ToString();
		// 	}
		// 	if (targetOptions.GameMode == GameModes.HideNSeek)
		// 	{
		// 		impostorsOptionButton.SetOptionEnabled(!flag);
		// 	}
		// 	else
		// 	{
		// 		impostorsOptionButton.SetOptionEnabled(b: true);
		// 	}
		// }
		// ImpostorText.color = ((targetOptions.GameMode == GameModes.HideNSeek) ? Palette.DisabledGrey : Color.white);

		int[] maxImpostorArray = targetOptions.GetIntArray(Int32ArrayOptionNames.MaxImpostors);
		int maxAllowed = (maxImpostorArray != null && targetOptions.MaxPlayers < maxImpostorArray.Length) ? maxImpostorArray[targetOptions.MaxPlayers] : 5;
		for (int i = 0; i < ImpostorButtons.Length; i++)
		{
			ImpostorsOptionButton impostorsOptionButton = ImpostorButtons[i];
			if (impostorsOptionButton == null) continue;
			if ((bool)impostorsOptionButton.SpriteRenderer)
			{
				impostorsOptionButton.SpriteRenderer.enabled = impostorsOptionButton.name == numImpostors.ToString();
			}
			if (int.TryParse(impostorsOptionButton.name, out int buttonVal))
			{
				impostorsOptionButton.SetOptionEnabled(buttonVal <= maxAllowed);
			}
			else
			{
				impostorsOptionButton.SetOptionEnabled(true);
			}
		}
		ImpostorText.color = Color.white;
	}

	public void SetMap(int mapId)
	{
		IGameOptions targetOptions = GetTargetOptions();
		if (mode == SettingsMode.Host)
		{
			targetOptions.SetByte(ByteOptionNames.MapId, (byte)mapId);
		}
		else
		{
			targetOptions.ToggleMapFilter((byte)mapId);
		}
		SetTargetOptions(targetOptions);
		if (DestroyableSingleton<FindAGameManager>.InstanceExists)
		{
			DestroyableSingleton<FindAGameManager>.Instance.ResetTimer();
		}
		MapMenu.UpdateMapButtons(mapId);
	}

	public void SetLanguageFilter(uint keyword)
	{
		IGameOptions targetOptions = GetTargetOptions();
		targetOptions.SetUInt(UInt32OptionNames.Keywords, keyword);
		SetTargetOptions(targetOptions);
		if (DestroyableSingleton<FindAGameManager>.InstanceExists)
		{
			DestroyableSingleton<FindAGameManager>.Instance.ResetTimer();
		}
		UpdateLanguageButton(keyword);
	}

	private void UpdateLanguageButton(uint flag)
	{
		if (flag == 1)
		{
			LanguageButton.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.OtherLanguage);
		}
		else
		{
			LanguageButton.text = ChatLanguageSet.Instance.GetString(flag);
		}
	}

	public void SetGameMode(GameModes mode)
	{
		DataManager.Settings.Multiplayer.LastPlayedGameMode = mode;
		DataManager.Settings.Save();
		GameOptionsManager.Instance.SwitchGameMode(mode);
		GameModeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(GameModesHelpers.ModeToName[mode]);
		Refresh();
	}

	public CreateOptionsPicker()
	{ }
}
