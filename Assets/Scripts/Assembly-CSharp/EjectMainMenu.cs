using System.Collections;
using UnityEngine;

public class EjectMainMenu : MonoBehaviour
{
	[SerializeField]
	private Transform originCircle;

	[SerializeField]
	private GameObject ejectButtonAnimated;

	[SerializeField]
	private GameObject ejectButton;

	[SerializeField]
	private GameObject ejectMobileButton;

	[SerializeField]
	private ControllerButtonBehavior controllerL;

	[SerializeField]
	private ControllerButtonBehavior controllerR;

	[SerializeField]
	private GameObject pressState;

	[SerializeField]
	private AudioClip pressSound;

	public PlayerParticleInfo[] Sprites;

	public PlayerParticleInfo[] HorseSprites;

	public FloatRange velocity;

	public FloatRange scale;

	public ObjectPoolBehavior pool;

	public float StartRadius;

	private RandomFill<PlayerParticleInfo> fill;

	private Coroutine buttonCoroutine;

	private Coroutine cooldownCoroutine;

	private bool onCooldown;

	private void Start()
	{
		fill = new RandomFill<PlayerParticleInfo>();
		if (AprilFoolsMode.ShouldHorseModeMainMenu())
		{
			fill.Set(HorseSprites);
		}
		else
		{
			fill.Set(Sprites);
		}
		controllerL.OnPressedEvent += AnimatePress;
		controllerR.OnPressedEvent += AnimatePress;
	}

	private void OnEnable()
	{
		onCooldown = false;
		ejectButtonAnimated.SetActive(value: false);
		ejectMobileButton.SetActive(value: false);
		ejectButton.SetActive(value: false);
	}

	public void AnimatePress()
	{
		if (!onCooldown)
		{
			SoundManager.Instance.PlaySound(pressSound, loop: false);
			ejectButton.SetActive(value: false);
			ejectButtonAnimated.SetActive(value: true);
		}
	}

	public void FinishPressAnim()
	{
		ejectButtonAnimated.SetActive(value: false);
		EjectCrewmate();
	}

	public void StartEjectButton()
	{
		if (buttonCoroutine != null)
		{
			StopCoroutine(buttonCoroutine);
			buttonCoroutine = null;
		}
		buttonCoroutine = StartCoroutine(StartEjectButtonCo());
	}

	private IEnumerator StartEjectButtonCo()
	{
		yield return new WaitForSeconds(10f);
		ejectMobileButton.SetActive(value: false);
		ejectButton.SetActive(value: true);
	}

	private IEnumerator CoCooldown()
	{
		onCooldown = true;
		pressState.SetActive(value: true);
		ejectMobileButton.SetActive(value: false);
		ejectButton.SetActive(value: false);
		yield return new WaitForSeconds(5f);
		pressState.SetActive(value: false);
		ejectButton.SetActive(value: true);
		onCooldown = false;
	}

	private void OnDisable()
	{
		if (cooldownCoroutine != null)
		{
			StopCoroutine(cooldownCoroutine);
			cooldownCoroutine = null;
		}
	}

	public void EjectCrewmate()
	{
		if (!onCooldown)
		{
			if (cooldownCoroutine != null)
			{
				StopCoroutine(cooldownCoroutine);
			}
			cooldownCoroutine = StartCoroutine(CoCooldown());
			PlayerParticle playerParticle = pool.Get<PlayerParticle>();
			PlayerMaterial.SetColors(Random.Range(0, 17), playerParticle.myRend);
			PlacePlayer(playerParticle, initial: false);
		}
	}

	private void PlacePlayer(PlayerParticle part, bool initial)
	{
		Vector3 position = originCircle.position;
		Vector3 localPosition = new Vector2(position.x, position.y) + Random.insideUnitCircle;
		if (!initial)
		{
			localPosition.Normalize();
		}
		localPosition *= StartRadius;
		float num = scale.Next();
		part.transform.localScale = new Vector3(num, num, num);
		localPosition.z = (0f - num) * 0.001f;
		part.transform.localPosition = localPosition;
		PlayerParticleInfo playerParticleInfo = fill.Get();
		part.myRend.sprite = playerParticleInfo.image;
		part.myRend.flipX = BoolRange.Next();
		Vector2 self = -localPosition.normalized;
		self = self.Rotate(FloatRange.Next(-45f, 45f));
		part.velocity = self * velocity.Next();
		part.angularVelocity = playerParticleInfo.angularVel.Next();
		if (playerParticleInfo.alignToVel)
		{
			part.transform.localEulerAngles = new Vector3(0f, 0f, Vector2.up.AngleSigned(self));
		}
	}
}
