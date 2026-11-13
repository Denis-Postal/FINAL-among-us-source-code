using System.Collections;
using Rewired;
using UnityEngine;

public class AcceptDivertPowerGame : Minigame
{
	private LineRenderer[] LeftWires;

	private LineRenderer[] RightWires;

	public GameObject RightWireParent;

	public GameObject LeftWireParent;

	public SpriteRenderer Switch;

	public AudioClip SwitchSound;

	private bool done;

	private bool prevHadInput;

	private float rotateAngle;

	private Vector2 prevStickDir;

	public void Start()
	{
		LeftWires = LeftWireParent.GetComponentsInChildren<LineRenderer>();
		RightWires = RightWireParent.GetComponentsInChildren<LineRenderer>();
		for (int i = 0; i < LeftWires.Length; i++)
		{
			LeftWires[i].material.SetColor("_Color", Color.yellow);
		}
		SetupInput(disableCursor: true);
	}

	public void DoSwitch()
	{
		if (!done)
		{
			done = true;
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(SwitchSound, loop: false);
			}
			StartCoroutine(CoDoSwitch());
		}
	}

	private IEnumerator CoDoSwitch()
	{
		yield return new WaitForLerp(0.25f, (float t) =>
		{
			Switch.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0f, 90f, t));
		});
		LeftWires[0].SetPosition(1, new Vector3(1.265f, 0f, 0f));
		for (int num = 0; num < RightWires.Length; num++)
		{
			RightWires[num].enabled = true;
			RightWires[num].material.SetColor("_Color", Color.yellow);
		}
		for (int num2 = 0; num2 < LeftWires.Length; num2++)
		{
			LeftWires[num2].material.SetColor("_Color", Color.yellow);
		}
		if ((bool)MyNormTask)
		{
			MyNormTask.NextStep();
		}
		StartCoroutine(CoStartClose());
	}

	public void Update()
	{
		if (!done)
		{
			Vector2 axis2DRaw = ReInput.players.GetPlayer(0).GetAxis2DRaw(13, 14);
			if (axis2DRaw.sqrMagnitude > 0.9f)
			{
				Vector2 normalized = axis2DRaw.normalized;
				if (prevHadInput)
				{
					float num = Vector2.SignedAngle(prevStickDir, normalized);
					rotateAngle += num;
					if (Mathf.Abs(rotateAngle) > 45f)
					{
						DoSwitch();
					}
				}
				prevStickDir = normalized;
				prevHadInput = true;
			}
			else
			{
				rotateAngle = 0f;
				prevHadInput = false;
			}
		}
		for (int i = 0; i < LeftWires.Length; i++)
		{
			Vector2 textureOffset = LeftWires[i].material.GetTextureOffset("_MainTex");
			textureOffset.x -= Time.deltaTime * 3f;
			LeftWires[i].material.SetTextureOffset("_MainTex", textureOffset);
		}
		for (int j = 0; j < RightWires.Length; j++)
		{
			Vector2 textureOffset2 = RightWires[j].material.GetTextureOffset("_MainTex");
			textureOffset2.x += Time.deltaTime * 3f;
			RightWires[j].material.SetTextureOffset("_MainTex", textureOffset2);
		}
	}
}
