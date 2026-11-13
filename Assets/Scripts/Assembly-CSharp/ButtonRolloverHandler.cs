using System.Collections;
using TMPro;
using UnityEngine;

public class ButtonRolloverHandler : MonoBehaviour
{
	public SpriteRenderer Target;

	public SpriteRenderer Target2;

	public TextMeshPro TargetText;

	public MeshRenderer TargetMesh;

	public Color OverColor = Color.green;

	public Color OutColor = Color.white;

	public Color UnselectedColor = Color.white;

	public bool UseObjectsOutColor;

	public bool UseObjectSelected;

	public AudioClip HoverSound;

	public void Awake()
	{
		PassiveButton component = GetComponent<PassiveButton>();
		if (component != null)
		{
			component.OnMouseOver.AddListener(DoMouseOver);
			component.OnMouseOut.AddListener(DoMouseOut);
			if (UseObjectSelected)
			{
				component.OnClick.AddListener(DoMouseClick);
			}
			return;
		}
		ButtonBehavior component2 = GetComponent<ButtonBehavior>();
		if (component2 != null)
		{
			component2.OnMouseOver.AddListener(DoMouseOver);
			component2.OnMouseOut.AddListener(DoMouseOut);
			if (UseObjectSelected)
			{
				component2.OnClick.AddListener(DoMouseClick);
			}
			return;
		}
		SlideBar component3 = GetComponent<SlideBar>();
		if (component3 != null)
		{
			component3.OnMouseOver.AddListener(DoMouseOver);
			component3.OnMouseOut.AddListener(DoMouseOut);
		}
		else if (UseObjectsOutColor)
		{
			if (Target != null)
			{
				OutColor = Target.color;
			}
			if (TargetText != null)
			{
				OutColor = TargetText.color;
			}
			if (TargetMesh != null)
			{
				OutColor = TargetMesh.material.color;
			}
		}
	}

	public void DoMouseClick()
	{
		if (Target != null)
		{
			Target.color = OverColor;
		}
		if (TargetText != null)
		{
			TargetText.color = OverColor;
		}
		if (TargetMesh != null)
		{
			TargetMesh.material.SetColor("_Color", OverColor);
		}
	}

	public void DoMouseOver()
	{
		if (Target2 != null)
		{
			Target2.color = OverColor;
		}
		else if (Target != null)
		{
			Target.color = OverColor;
		}
		if (TargetText != null)
		{
			TargetText.color = OverColor;
		}
		if (TargetMesh != null)
		{
			TargetMesh.material.SetColor("_Color", OverColor);
		}
		if ((bool)HoverSound)
		{
			SoundManager.Instance.PlaySound(HoverSound, loop: false);
		}
	}

	public void DoMouseOut()
	{
		if (Target2 != null)
		{
			Target2.color = UnselectedColor;
		}
		if (Target != null)
		{
			Target.color = OutColor;
		}
		if (TargetText != null)
		{
			TargetText.color = OutColor;
		}
		if (TargetMesh != null)
		{
			TargetMesh.material.SetColor("_Color", OutColor);
		}
	}

	public void SetDisabledColors()
	{
		ChangeOutColor(Color.gray);
		OverColor = Color.gray;
		Target.color = Color.grey;
	}

	public void SetEnabledColors()
	{
		ChangeOutColor(Color.white);
		OverColor = Color.green;
		Target.color = Color.white;
	}

	public void ChangeOutColor(Color color)
	{
		OutColor = color;
		DoMouseOut();
	}

	public void WaitClickUnselect()
	{
		StartCoroutine(WaitChangeUnselect());
	}

	private IEnumerator WaitChangeUnselect()
	{
		yield return new WaitForEndOfFrame();
		DoMouseOut();
		DoMouseOver();
	}
}
