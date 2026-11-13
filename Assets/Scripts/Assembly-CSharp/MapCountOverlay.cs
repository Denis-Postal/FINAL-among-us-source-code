using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class MapCountOverlay : MonoBehaviour
{
	public AlphaPulse BackgroundColor;

	public TextMeshPro SabotageText;

	public CounterArea[] CountAreas;

	private readonly Collider2D[] buffer = new Collider2D[45];

	private ContactFilter2D filter;

	private float timer;

	private bool isSab;

	public bool showLivePlayerPosition;

	public bool includeDeadBodies;

	public void SetOptions(bool showLivePlayerPosition, bool includeDeadBodies)
	{
		this.showLivePlayerPosition = showLivePlayerPosition;
		this.includeDeadBodies = includeDeadBodies;
	}

	private void Awake()
	{
		filter.useLayerMask = true;
		filter.layerMask = Constants.LivingPlayersOnlyMask;
		filter.useTriggers = true;
	}

	private unsafe void OnEnable()
	{ /* spliced-from-2019 */
		BackgroundColor.SetColor(PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer) ? Palette.DisabledGrey : Color.green);
		timer = 1f;
	}

	private void OnDisable()
	{
		for (int i = 0; i < CountAreas.Length; i++)
		{
			CounterArea counterArea = CountAreas[i];
			if (!counterArea.DetectiveExclusiveLocation)
			{
				counterArea.UpdateCount(0);
			}
		}
	}

	private unsafe void Update()
	{
		timer += Time.deltaTime;
		if (timer < 0.1f)
		{
			return;
		}
		timer = 0f;
		if (!isSab && PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
		{
			isSab = true;
			BackgroundColor.SetColor(Palette.DisabledGrey);
			SabotageText.gameObject.SetActive(value: true);
			return;
		}
		if (isSab && !PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
		{
			isSab = false;
			BackgroundColor.SetColor(Color.green);
			SabotageText.gameObject.SetActive(value: false);
		}
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = 0; i < CountAreas.Length; i++)
		{
			CounterArea counterArea = CountAreas[i];
			if (counterArea.DetectiveExclusiveLocation)
			{
				continue;
			}
			if (!PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
			{
				if (ShipStatus.Instance.FastRooms.TryGetValue(counterArea.RoomType, out var value) && (bool)value.roomArea)
				{
					int num = value.roomArea.Overlap(filter, buffer);
					int num2 = 0;
					for (int j = 0; j < num; j++)
					{
						Collider2D collider2D = buffer[j];
						if (collider2D.CompareTag("DeadBody") && includeDeadBodies)
						{
							DeadBody component = collider2D.GetComponent<DeadBody>();
							if (component != null && hashSet.Add(component.ParentId))
							{
								num2++;
							}
						}
						else if (!collider2D.isTrigger)
						{
							PlayerControl component2 = collider2D.GetComponent<PlayerControl>();
							if ((bool)component2 && component2.Data != null && !component2.Data.Disconnected && !component2.Data.IsDead && (showLivePlayerPosition || !component2.AmOwner) && hashSet.Add(component2.PlayerId))
							{
								num2++;
							}
						}
					}
					counterArea.UpdateCount(num2);
				}
				else
				{
					Debug.LogWarning("Couldn't find counter for:" + counterArea.RoomType);
				}
			}
			else
			{
				counterArea.UpdateCount(0);
			}
		}
	}
}
