using System.Collections;
using TMPro;
using UnityEngine;

public class PlanetSurveillanceMinigame : Minigame
{
	public Camera Camera;

	public GameObject Viewables;

	public MeshRenderer ViewPort;

	public TextMeshPro LocationName;

	public TextMeshPro SabText;

	private RenderTexture texture;

	public MeshRenderer FillQuad;

	public Material DefaultMaterial;

	public Material StaticMaterial;

	private bool isStatic;

	private SurvCamera[] survCameras;

	public Transform DotParent;

	private SpriteRenderer[] Dots;

	public Sprite DotEnabled;

	public Sprite DotDisabled;

	private int currentCamera;

	public AudioClip ChangeSound;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		DestroyableSingleton<HudManager>.Instance.PlayerCam.Locked = true;
		RenderTexture renderTexture = (texture = RenderTexture.GetTemporary(330, 230, 16, RenderTextureFormat.ARGB32));
		Camera.targetTexture = renderTexture;
		ViewPort.material.SetTexture("_MainTex", renderTexture);
		survCameras = ShipStatus.Instance.AllCameras;
		Dots = new SpriteRenderer[survCameras.Length];
		for (int i = 0; i < Dots.Length; i++)
		{
			GameObject gameObject = new GameObject("Dot" + i, typeof(SpriteRenderer));
			gameObject.layer = base.gameObject.layer;
			gameObject.transform.SetParent(DotParent);
			SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
			component.sprite = DotDisabled;
			Dots[i] = component;
		}
		DotAligner.Align(DotParent, 0.25f, even: true);
		NextCamera(0);
		if (!PlayerControl.LocalPlayer.Data.IsDead)
		{
			ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Security, 1);
		}
		else
		{
			DestroyableSingleton<HudManager>.Instance.Chat.SetVisible(visible: false);
		}
		SetupInput(disableCursor: true);
	}

	public void Update()
	{
		if (isStatic && !PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
		{
			isStatic = false;
			ViewPort.sharedMaterial = DefaultMaterial;
			ViewPort.material.SetTexture("_MainTex", texture);
			SabText.gameObject.SetActive(value: false);
		}
		else if (!isStatic && PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
		{
			isStatic = true;
			ViewPort.sharedMaterial = StaticMaterial;
			SabText.gameObject.SetActive(value: true);
		}
	}

	public void NextCamera(int direction)
	{
		if (direction != 0 && Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(ChangeSound, loop: false);
		}
		Dots[currentCamera].sprite = DotDisabled;
		currentCamera = (currentCamera + direction).Wrap(survCameras.Length);
		Dots[currentCamera].sprite = DotEnabled;
		SurvCamera survCamera = survCameras[currentCamera];
		Camera.transform.position = survCamera.transform.position + survCameras[currentCamera].Offset;
		LocationName.text = ((survCamera.NewName > StringNames.None) ? DestroyableSingleton<TranslationController>.Instance.GetString(survCamera.NewName) : survCamera.CamName);
		if (!PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
		{
			StartCoroutine(PulseStatic());
		}
	}

	private IEnumerator PulseStatic()
	{
		ViewPort.sharedMaterial = StaticMaterial;
		ViewPort.material.SetTexture("_MainTex", null);
		yield return Effects.Wait(0.2f);
		ViewPort.sharedMaterial = DefaultMaterial;
		ViewPort.material.SetTexture("_MainTex", texture);
		isStatic = false;
	}

	protected override IEnumerator CoAnimateOpen()
	{
		Viewables.SetActive(value: false);
		FillQuad.material.SetFloat("_Center", -5f);
		FillQuad.material.SetColor("_Color2", Color.clear);
		for (float timer = 0f; timer < 0.25f; timer += Time.deltaTime)
		{
			FillQuad.material.SetColor("_Color2", Color.Lerp(Color.clear, Color.black, timer / 0.25f));
			yield return null;
		}
		FillQuad.material.SetColor("_Color2", Color.black);
		Viewables.SetActive(value: true);
		for (float timer = 0f; timer < 0.1f; timer += Time.deltaTime)
		{
			FillQuad.material.SetFloat("_Center", Mathf.Lerp(-5f, 0f, timer / 0.1f));
			yield return null;
		}
		for (float timer = 0f; timer < 0.15f; timer += Time.deltaTime)
		{
			FillQuad.material.SetFloat("_Center", Mathf.Lerp(-3f, 0.4f, timer / 0.15f));
			yield return null;
		}
		FillQuad.material.SetFloat("_Center", 0.4f);
	}

	private IEnumerator CoAnimateClose()
	{
		for (float timer = 0f; timer < 0.1f; timer += Time.deltaTime)
		{
			FillQuad.material.SetFloat("_Center", Mathf.Lerp(0.4f, -5f, timer / 0.1f));
			yield return null;
		}
		Viewables.SetActive(value: false);
		for (float timer = 0f; timer < 0.3f; timer += Time.deltaTime)
		{
			FillQuad.material.SetColor("_Color2", Color.Lerp(Color.black, Color.clear, timer / 0.3f));
			yield return null;
		}
		FillQuad.material.SetColor("_Color2", Color.clear);
	}

	protected override IEnumerator CoDestroySelf()
	{
		DestroyableSingleton<HudManager>.Instance.PlayerCam.Locked = false;
		yield return CoAnimateClose();
		Object.Destroy(gameObject);
	}

	public override void Close()
	{
		if (PlayerControl.LocalPlayer.Data.IsDead)
		{
			DestroyableSingleton<HudManager>.Instance.Chat.SetVisible(visible: true);
		}
		ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Security, 2);
		base.Close();
	}

	public void OnDestroy()
	{
		texture.Release();
	}
}
