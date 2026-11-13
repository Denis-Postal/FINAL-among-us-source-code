using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class MedScanMinigame : Minigame
{
	private enum PositionState
	{
		None,
		WalkingToPad,
		WalkingToOffset
	}

	private const int SomeKindOfPrimeNumber = 7;

	private static readonly string[] BloodTypes = new string[8] { "O-", "A-", "B-", "AB-", "O+", "A+", "B+", "AB+" };

	public TextMeshPro text;

	public TextMeshPro charStats;

	public HorizontalGauge gauge;

	private MedScanSystem medscan;

	public float ScanDuration = 10f;

	public float ScanTimer;

	private string completeString;

	public AudioClip ScanSound;

	public AudioClip TextSound;

	private Coroutine walking;

	private PositionState state;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		medscan = ShipStatus.Instance.Systems[SystemTypes.MedBay] as MedScanSystem;
		gauge.Value = 0f;
		transform.position = new Vector3(100f, 0f, 0f);
		NetworkedPlayerInfo data = PlayerControl.LocalPlayer.Data;
		int playerId = data.PlayerId;
		string colorName = Palette.GetColorName(data.DefaultOutfit.ColorId.Wrap(Palette.ColorNames.Length));
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MedID) + " ");
		string text = colorName.ToUpperInvariant();
		if (text.Length > 3)
		{
			text = text.Substring(0, 3);
		}
		stringBuilder.Append(text);
		stringBuilder.Append("P" + playerId);
		stringBuilder.Append(new string(' ', 8));
		stringBuilder.Append(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MedHT) + " 3' 6\"");
		stringBuilder.Append(new string(' ', 8));
		stringBuilder.Append(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MedWT) + " 92lb");
		stringBuilder.AppendLine();
		stringBuilder.Append(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MedC) + " ");
		stringBuilder.Append(colorName.PadRight(17));
		stringBuilder.Append(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MedBT) + " ");
		stringBuilder.Append(BloodTypes[playerId * 7 % BloodTypes.Length]);
		completeString = stringBuilder.ToString();
		charStats.text = string.Empty;
		ShipStatus.Instance.RpcUpdateSystem(SystemTypes.MedBay, (byte)(playerId | 0x80));
		walking = StartCoroutine(WalkToOffset());
		SetupInput(disableCursor: true);
	}

	private IEnumerator WalkToOffset()
	{
		float num = 24f;
		state = PositionState.WalkingToOffset;
		PlayerPhysics myPhysics = PlayerControl.LocalPlayer.MyPhysics;
		Vector2 worldPos = ShipStatus.Instance.MedScanner.Position;
		Vector2 vector = Vector2.left.Rotate((float)(int)PlayerControl.LocalPlayer.PlayerId * num);
		worldPos += vector / 2f;
		Camera.main.GetComponent<FollowerCamera>().Locked = false;
		yield return myPhysics.WalkPlayerTo(worldPos, 0.001f);
		yield return new WaitForSeconds(0.1f);
		Camera.main.GetComponent<FollowerCamera>().Locked = true;
		walking = null;
	}

	private IEnumerator WalkToPad()
	{
		state = PositionState.WalkingToPad;
		PlayerPhysics myPhysics = PlayerControl.LocalPlayer.MyPhysics;
		Vector2 worldPos = ShipStatus.Instance.MedScanner.Position;
		Camera.main.GetComponent<FollowerCamera>().Locked = false;
		yield return myPhysics.WalkPlayerTo(worldPos, 0.001f);
		yield return new WaitForSeconds(0.1f);
		Camera.main.GetComponent<FollowerCamera>().Locked = true;
		walking = null;
	}

	private void FixedUpdate()
	{
		if (MyNormTask.IsComplete)
		{
			return;
		}
		byte playerId = PlayerControl.LocalPlayer.PlayerId;
		if (medscan.CurrentUser != playerId)
		{
			if (medscan.CurrentUser == byte.MaxValue)
			{
				text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MedscanRequested);
				return;
			}
			NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(medscan.CurrentUser);
			text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MedscanWaitingFor, playerById.PlayerName);
		}
		else if (state != PositionState.WalkingToPad)
		{
			if (walking != null)
			{
				StopCoroutine(walking);
			}
			walking = StartCoroutine(WalkToPad());
		}
		else
		{
			if (walking != null)
			{
				return;
			}
			if (ScanTimer == 0f)
			{
				PlayerControl.LocalPlayer.RpcSetScanner(value: true);
				SoundManager.Instance.PlaySound(ScanSound, loop: false);
				VibrationManager.Vibrate(0.3f, 0.3f, 0f, VibrationManager.VibrationFalloff.None, ScanSound);
			}
			ScanTimer += Time.fixedDeltaTime;
			gauge.Value = ScanTimer / ScanDuration;
			int num = (int)(Mathf.Min(1f, ScanTimer / ScanDuration * 1.25f) * (float)completeString.Length);
			if (num > charStats.text.Length)
			{
				charStats.text = completeString.Substring(0, num);
				if (completeString[num - 1] != ' ')
				{
					SoundManager.Instance.PlaySoundImmediate(TextSound, loop: false, 0.7f, 0.3f);
				}
			}
			if (ScanTimer >= ScanDuration)
			{
				PlayerControl.LocalPlayer.RpcSetScanner(value: false);
				text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MedscanCompleted);
				MyNormTask.NextStep();
				ShipStatus.Instance.RpcUpdateSystem(SystemTypes.MedBay, (byte)(playerId | 0x40));
				StartCoroutine(CoStartClose());
			}
			else
			{
				text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MedscanCompleteIn, (int)(ScanDuration - ScanTimer));
			}
		}
	}

	public override void Close()
	{
		StopAllCoroutines();
		byte playerId = PlayerControl.LocalPlayer.PlayerId;
		SoundManager.Instance.StopSound(TextSound);
		SoundManager.Instance.StopSound(ScanSound);
		PlayerControl.LocalPlayer.RpcSetScanner(value: false);
		ShipStatus.Instance.RpcUpdateSystem(SystemTypes.MedBay, (byte)(playerId | 0x40));
		Camera.main.GetComponent<FollowerCamera>().Locked = false;
		VibrationManager.CancelVibration(ScanSound);
		base.Close();
	}
}
