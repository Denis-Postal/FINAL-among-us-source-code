using System.Collections;
using QRCoder;
using QRCoder.Unity;
using Rewired;
using Rewired.ControllerExtensions;
using TMPro;
using UnityEngine;

public class BoardPassGame : Minigame
{
	private static Color[] BgColors = new Color[5]
	{
		new Color32(101, 170, 119, byte.MaxValue),
		new Color32(85, 93, 182, byte.MaxValue),
		new Color32(198, 127, 174, byte.MaxValue),
		new Color32(161, 126, 100, byte.MaxValue),
		new Color32(149, 219, 209, byte.MaxValue)
	};

	public SpriteRenderer renderer;

	public SpriteRenderer pass;

	public Sprite passBack;

	public TextMeshPro NameText;

	public SpriteRenderer ImageBg;

	public SpriteRenderer Image;

	public Sprite[] Photos;

	public PassiveButton pullButton;

	public PassiveButton flipButton;

	public SpriteRenderer Scanner;

	public Sprite ScannerAccept;

	public Sprite ScannerScanning;

	public Sprite ScannerWaiting;

	public Collider2D Sensor;

	public Collider2D BarCode;

	public AudioClip slideinSound;

	public AudioClip flipSound;

	public AudioClip scanStartSound;

	public AudioClip scanSound;

	private Coroutine blinky;

	private Controller controller;

	private TouchpadBehavior touchpad;

	private bool prevHadInput;

	private float rotateAngle;

	private Vector2 prevStickDir;

	private bool enableControllerPassMovement;

	private bool grabbed;

	// Decorative QR-style texture (3 finder patterns + fixed pseudo-random modules); used when QRCoder is unavailable.
	private static Texture2D MakeFallbackQr()
	{
		const int modules = 29;
		const int size = modules + 2;
		Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, mipChain: false);
		texture2D.filterMode = FilterMode.Point;
		texture2D.wrapMode = TextureWrapMode.Clamp;
		System.Random random = new System.Random(1337);
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				int x = i - 1;
				int y = j - 1;
				bool dark = false;
				if (x >= 0 && y >= 0 && x < modules && y < modules)
				{
					int fx = (x < 8) ? x : ((x >= modules - 8) ? x - (modules - 8) + 1 : -1);
					int fy = (y < 8) ? y : ((y >= modules - 8) ? y - (modules - 8) + 1 : -1);
					bool leftOrRight = x < 8 || x >= modules - 8;
					bool bottomOrTop = y < 8 || y >= modules - 8;
					bool cornerFinder = leftOrRight && bottomOrTop && !(x >= modules - 8 && y >= modules - 8);
					if (cornerFinder)
					{
						int cx = (x < 8) ? x : x - (modules - 8);
						int cy = (y < 8) ? y : y - (modules - 8);
						if (cx == 7 || cy == 7 || cx < 0 || cy < 0)
						{
							dark = false;
						}
						else
						{
							bool ring = cx == 0 || cx == 6 || cy == 0 || cy == 6;
							bool core = cx >= 2 && cx <= 4 && cy >= 2 && cy <= 4;
							dark = ring || core;
						}
					}
					else
					{
						dark = random.Next(2) == 0;
					}
				}
				texture2D.SetPixel(i, j, dark ? Color.black : Color.white);
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	public void Start()
	{
		Texture2D graphic;
		try
		{
			graphic = new UnityQRCode(new QRCodeGenerator().CreateQrCode("Yo holmes, smell you later", QRCodeGenerator.ECCLevel.M)).GetGraphic(1);
		}
		catch (System.Exception)
		{
			// offline remake: the QRCoder.dll in the project is a signature-only stub (InvalidProgramException), which aborted Start and left `controller` null so the pass could never be grabbed
			graphic = MakeFallbackQr();
		}
		renderer.sprite = Sprite.Create(graphic, new Rect(0f, 0f, graphic.width, graphic.height), Vector2.one / 2f);
		Image.sprite = Photos[PlayerControl.LocalPlayer.PlayerId % Photos.Length];
		PlayerControl.LocalPlayer.SetPlayerMaterialColors(Image);
		NameText.text = PlayerControl.LocalPlayer.Data.PlayerName;
		touchpad = GetComponent<TouchpadBehavior>();
		SetupInput(disableCursor: true);
		controller = new Controller();
	}

	public void Update()
	{
		controller.Update();
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			Player player = ReInput.players.GetPlayer(0);
			player.controllers.Joysticks[0].GetExtension<IDualShock4Extension>();
			if (pullButton.gameObject.activeSelf)
			{
				if (player.GetAxisRaw(13) > 0.7f)
				{
					PullPass();
					pullButton.gameObject.SetActive(value: false);
				}
				else if (touchpad.IsTouching() && touchpad.GetTouchVector().x > 1f)
				{
					PullPass();
					pullButton.gameObject.SetActive(value: false);
				}
			}
			else if (flipButton.gameObject.activeSelf)
			{
				Vector2 axis2DRaw = player.GetAxis2DRaw(13, 14);
				if (axis2DRaw.sqrMagnitude > 0.9f)
				{
					Vector2 normalized = axis2DRaw.normalized;
					if (prevHadInput)
					{
						float num = Vector2.SignedAngle(prevStickDir, normalized);
						if (num > 0f)
						{
							rotateAngle += num;
						}
						if (rotateAngle > 45f)
						{
							FlipPass();
							flipButton.gameObject.SetActive(value: false);
						}
					}
					prevStickDir = normalized;
					prevHadInput = true;
				}
				else if (touchpad.IsTouching())
				{
					if (touchpad.GetTouchVector().x < -1f)
					{
						FlipPass();
						flipButton.gameObject.SetActive(value: false);
					}
				}
				else
				{
					prevHadInput = false;
				}
			}
			else if (enableControllerPassMovement)
			{
				Vector3 localPosition = (Vector3)VirtualCursor.currentPosition - transform.position;
				localPosition.z = -1f;
				pass.transform.localPosition = localPosition;
			}
		}
		else if (grabbed)
		{
			Vector3 localPosition2 = (Vector3)DestroyableSingleton<PassiveButtonManager>.Instance.controller.DragPosition - transform.position;
			localPosition2.z = -1f;
			pass.transform.localPosition = localPosition2;
		}
		if (flipButton.isActiveAndEnabled || pullButton.isActiveAndEnabled || MyNormTask.IsComplete)
		{
			return;
		}
		if (Sensor.IsTouching(BarCode))
		{
			if (blinky == null)
			{
				blinky = StartCoroutine(CoRunBlinky());
			}
		}
		else if (blinky != null)
		{
			StopCoroutine(blinky);
			blinky = null;
			Scanner.sprite = ScannerWaiting;
		}
	}

	private IEnumerator CoRunBlinky()
	{
		int i = 0;
		while (i < 3)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(scanStartSound, loop: false);
			}
			Scanner.sprite = ScannerAccept;
			yield return Effects.Wait(0.1f);
			Scanner.sprite = ScannerScanning;
			yield return Effects.Wait(0.2f);
			int num = i + 1;
			i = num;
		}
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(scanSound, loop: false);
		}
		blinky = null;
		Scanner.sprite = ScannerAccept;
		MyNormTask.NextStep();
		yield return CoStartClose();
	}

	public void PullPass()
	{
		StartCoroutine(CoPullPass());
	}

	private IEnumerator CoPullPass()
	{
		pullButton.gameObject.SetActive(value: false);
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(slideinSound, loop: false);
		}
		yield return Effects.Slide2D(pass.transform, new Vector2(-10f, 0f), new Vector2(-1.4f, 0f), 0.3f);
		flipButton.gameObject.SetActive(value: true);
	}

	public void Grab()
	{
		if (!flipButton.isActiveAndEnabled && !pullButton.isActiveAndEnabled)
		{
			grabbed = !grabbed;
		}
	}

	public void FlipPass()
	{
		StartCoroutine(CoFlipPass());
	}

	private IEnumerator CoFlipPass()
	{
		flipButton.gameObject.SetActive(value: false);
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(flipSound, loop: false);
		}
		yield return Effects.Lerp(0.2f, (float t) =>
		{
			pass.transform.localEulerAngles = new Vector3(0f, Mathf.Lerp(0f, 90f, t), 0f);
		});
		pass.sprite = passBack;
		renderer.gameObject.SetActive(value: false);
		ImageBg.gameObject.SetActive(value: false);
		NameText.gameObject.SetActive(value: false);
		yield return Effects.Lerp(0.2f, (float t) =>
		{
			pass.transform.localEulerAngles = new Vector3(0f, Mathf.Lerp(90f, 180f, t), 0f);
		});
		enableControllerPassMovement = true;
		inputHandler.disableVirtualCursor = false;
		VirtualCursor.instance.SetWorldPosition(pass.transform.position);
	}
}
