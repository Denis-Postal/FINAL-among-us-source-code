using Object = UnityEngine.Object;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class AirshipUploadGame : Minigame
{
	public SpriteRenderer Phone;

	public Collider2D Hotspot;

	public Collider2D Perfect;

	public Collider2D Good;

	public Collider2D Poor;

	public GameObject PerfectIcon;

	public GameObject GoodIcon;

	public GameObject PoorIcon;

	public GameObject NoneIcon;

	public HorizontalGauge gauge;

	public float moveSpeed = 1f;

	private const float MaxTimer = 20f;

	private float timer;

	public AudioClip nearSound;

	public float BeepPeriod = 0.5f;

	private float beepTimer;

	public Controller cont= new Controller();

	public SpriteRenderer promptGlyph;

	private Color glyphColor;

	private float glyphDisappearDelay= 5f;

	private bool phoneGrabbed;

	public unsafe void Start()
{
		Hotspot.transform.localPosition = Random.insideUnitCircle.normalized * 2.5f;
		PlayerControl.LocalPlayer.SetPlayerMaterialColors(Phone);
		SetupInput(disableCursor: false);
		glyphColor = promptGlyph.color;
	}

	public unsafe void Update()
{
		if (amClosing != CloseState.None)
		{
			return;
		}
		float num = 0f;
		if (Hotspot.IsTouching(Perfect))
		{
			if (!PerfectIcon.activeSelf)
			{
				DeactivateIcons();
				PerfectIcon.SetActive(value: true);
			}
			num = Time.deltaTime * 4f;
		}
		else if (Hotspot.IsTouching(Good))
		{
			if (!GoodIcon.activeSelf)
			{
				DeactivateIcons();
				GoodIcon.SetActive(value: true);
			}
			num = Time.deltaTime * 2f;
		}
		else if (Hotspot.IsTouching(Poor))
		{
			if (!PoorIcon.activeSelf)
			{
				DeactivateIcons();
				PoorIcon.SetActive(value: true);
			}
			num = Time.deltaTime;
		}
		else if (!NoneIcon.activeSelf)
		{
			DeactivateIcons();
			NoneIcon.SetActive(value: true);
		}
		cont.Update();
		if (glyphColor.a > 0f)
		{
			if (glyphDisappearDelay > 0f)
			{
				glyphDisappearDelay -= Time.deltaTime;
			}
			else
			{
				promptGlyph.color = glyphColor;
				glyphColor.a -= Time.deltaTime;
			}
		}
		else if (promptGlyph.gameObject.activeSelf)
		{
			promptGlyph.gameObject.SetActive(value: false);
		}
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			Phone.transform.position = VirtualCursor.currentPosition;
		}
		timer += num;
		if (Constants.ShouldPlaySfx())
		{
			beepTimer += (10f - Vector2.Distance(Hotspot.transform.localPosition, Phone.transform.localPosition)) * num / BeepPeriod;
			if (beepTimer >= 1f)
			{
				beepTimer = 0f;
				SoundManager.Instance.PlaySoundImmediate(nearSound, loop: false);
			}
		}
		if (phoneGrabbed)
		{
			float z = Phone.transform.position.z;
			Vector3 position = DestroyableSingleton<PassiveButtonManager>.Instance.controller.Touches[0].Position;
			position.z = z;
			Phone.transform.position = position;
		}
		gauge.Value = timer / 20f;
		if (timer >= 20f)
		{
			MyNormTask.NextStep();
			Close();
		}
	}

	public void ToggleGrab()
	{
		phoneGrabbed = !phoneGrabbed;
	}

	private void DeactivateIcons()
	{
		PerfectIcon.SetActive(value: false);
		GoodIcon.SetActive(value: false);
		PoorIcon.SetActive(value: false);
		NoneIcon.SetActive(value: false);
	}

	public AirshipUploadGame()
	{ }
}
