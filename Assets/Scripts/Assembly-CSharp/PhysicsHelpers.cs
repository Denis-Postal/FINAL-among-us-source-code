using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;

public static class PhysicsHelpers
{
	private static Collider2D[] colliderHits= new Collider2D[20];

	private static RaycastHit2D[] castHits= new RaycastHit2D[20];

	private static Vector2 temp= default;

	private static ContactFilter2D filter= new ContactFilter2D
	{
		useLayerMask = true
	};

	public static bool CircleContains(Vector2 source, float radius, int layerMask)
	{
		return Physics2D.OverlapCircleNonAlloc(source, radius, colliderHits, layerMask) > 0;
	}

	public unsafe static bool AnyEdgeTriggerBetween(Vector2 source, Vector2 target, int layerMask)
	{
		filter.layerMask = layerMask;
		filter.useTriggers = true;
		temp.x = target.x - source.x;
		temp.y = target.y - source.y;
		int num = Physics2D.Raycast(source, temp, filter, castHits, temp.magnitude);
		for (int i = 0; i < num; i++)
		{
			if (castHits[i].collider.isTrigger)
			{
				return true;
			}
		}
		return false;
	}

	public unsafe static bool AnythingBetween(Vector2 source, Vector2 target, int layerMask, bool useTriggers)
	{
		filter.layerMask = layerMask;
		filter.useTriggers = useTriggers;
		temp.x = target.x - source.x;
		temp.y = target.y - source.y;
		return Physics2D.Raycast(source, temp, filter, castHits, temp.magnitude) > 0;
	}

	public unsafe static bool AnythingBetween(Collider2D castObject, Vector2 source, Vector2 target, int layerMask, bool useTriggers)
	{
		filter.layerMask = layerMask;
		filter.useTriggers = useTriggers;
		temp.x = target.x - source.x;
		temp.y = target.y - source.y;
		return castObject.Cast(temp, filter, castHits, temp.magnitude) > 0;
	}

	public unsafe static bool AnyNonTriggersBetween(Vector2 source, Vector2 dirNorm, float mag, int layerMask)
	{ /* spliced-from-2019 */
		int num = Physics2D.RaycastNonAlloc(source, dirNorm, castHits, mag, layerMask);
		bool result = false;
		for (int i = 0; i < num; i++)
		{
			if (!castHits[i].collider.isTrigger)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public unsafe static bool AnythingBetween(Vector2 source, Vector2 target, int layerMask, bool useTriggers, Collider2D itemToIgnore, Transform objectToIgnore)
	{
		filter.layerMask = layerMask;
		filter.useTriggers = useTriggers;
		temp.x = target.x - source.x;
		temp.y = target.y - source.y;
		if (Physics2D.Raycast(source, temp, filter, castHits, temp.magnitude) > 0)
		{
			if (castHits[0].collider != itemToIgnore)
			{
				return castHits[0].collider.transform != objectToIgnore;
			}
			return false;
		}
		return false;
	}

	static PhysicsHelpers()
	{ }
}
