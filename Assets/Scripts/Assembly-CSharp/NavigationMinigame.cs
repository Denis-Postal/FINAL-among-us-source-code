using System.Collections;
using Rewired;
using UnityEngine;

public class NavigationMinigame : Minigame
{
	public MeshRenderer TwoAxisImage;

	public SpriteRenderer CrossHairImage;

	public Collider2D hitbox;

	private Controller myController = new Controller();

	private Vector2 crossHair;

	private Vector2 half = new Vector2(0.5f, 0.5f);

	private Vector2 initialPos;

	private TouchpadBehavior touchpad;

	private bool prevHadInput;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		crossHair = Random.insideUnitCircle.normalized / 2f * 0.6f;
		Vector3 localPosition = new Vector3(crossHair.x * TwoAxisImage.bounds.size.x, crossHair.y * TwoAxisImage.bounds.size.y, -2f);
		CrossHairImage.transform.localPosition = localPosition;
		TwoAxisImage.material.SetVector("_CrossHair", crossHair + half);
		touchpad = GetComponent<TouchpadBehavior>();
		SetupInput(disableCursor: true);
	}

	public void FixedUpdate()
	{
		if ((bool)MyNormTask && MyNormTask.IsComplete)
		{
			return;
		}
		myController.Update();
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			Player player = ReInput.players.GetPlayer(0);
			Vector2 zero = Vector2.zero;
			if (player.GetButton(11))
			{
				if (!prevHadInput)
				{
					inputHandler.disableVirtualCursor = false;
					VirtualCursor.instance.SetWorldPosition(CrossHairImage.transform.position);
				}
				zero = VirtualCursor.currentPosition;
				Vector2 a = zero - (Vector2)(TwoAxisImage.transform.position - TwoAxisImage.bounds.size / 2f);
				crossHair = a.Div(TwoAxisImage.bounds.size);
				if ((crossHair - half).magnitude < 0.45f)
				{
					Vector3 localPosition = zero - (Vector2)transform.position;
					localPosition.z = -2f;
					CrossHairImage.transform.localPosition = localPosition;
					TwoAxisImage.material.SetVector("_CrossHair", crossHair);
				}
				prevHadInput = true;
				return;
			}
			if (touchpad.IsTouching())
			{
				if (touchpad.IsFirstTouch())
				{
					initialPos = TwoAxisImage.material.GetVector("_CrossHair");
				}
				crossHair = touchpad.GetTouchVector() + initialPos;
				if ((crossHair - half).magnitude < 0.45f)
				{
					CrossHairImage.transform.localPosition = (crossHair - half) * 5f;
					TwoAxisImage.material.SetVector("_CrossHair", crossHair);
				}
				prevHadInput = true;
				return;
			}
			if (prevHadInput)
			{
				inputHandler.disableVirtualCursor = true;
				if ((crossHair - half).magnitude < 0.05f)
				{
					StartCoroutine(CompleteGame());
					MyNormTask.NextStep();
				}
			}
			prevHadInput = false;
			return;
		}
		switch (myController.CheckDrag(hitbox))
		{
		case DragState.TouchStart:
		case DragState.Dragging:
		{
			Vector2 dragPosition = myController.DragPosition;
			Vector2 a2 = dragPosition - (Vector2)(TwoAxisImage.transform.position - TwoAxisImage.bounds.size / 2f);
			crossHair = a2.Div(TwoAxisImage.bounds.size);
			if ((crossHair - half).magnitude < 0.45f)
			{
				Vector3 localPosition2 = dragPosition - (Vector2)transform.position;
				localPosition2.z = -2f;
				CrossHairImage.transform.localPosition = localPosition2;
				TwoAxisImage.material.SetVector("_CrossHair", crossHair);
			}
			break;
		}
		case DragState.Released:
			if ((crossHair - half).magnitude < 0.05f)
			{
				StartCoroutine(CompleteGame());
				MyNormTask.NextStep();
			}
			break;
		case DragState.Holding:
			break;
		}
	}

	private IEnumerator CompleteGame()
	{
		WaitForSeconds wait = new WaitForSeconds(0.1f);
		Color green = new Color(0f, 0.8f, 0f, 1f);
		Color32 yellow = new Color32(byte.MaxValue, 202, 0, byte.MaxValue);
		CrossHairImage.transform.localPosition = new Vector3(0f, 0f, -2f);
		TwoAxisImage.material.SetVector("_CrossHair", half);
		CrossHairImage.color = yellow;
		TwoAxisImage.material.SetColor("_CrossColor", yellow);
		yield return wait;
		CrossHairImage.color = Color.white;
		TwoAxisImage.material.SetColor("_CrossColor", Color.white);
		yield return wait;
		CrossHairImage.color = yellow;
		TwoAxisImage.material.SetColor("_CrossColor", yellow);
		yield return wait;
		CrossHairImage.color = Color.white;
		TwoAxisImage.material.SetColor("_CrossColor", Color.white);
		yield return wait;
		CrossHairImage.color = green;
		TwoAxisImage.material.SetColor("_CrossColor", green);
		yield return CoStartClose();
	}
}
