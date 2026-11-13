using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

public class NoShadowBehaviour : MonoBehaviour
{
	public Renderer rend;

	public bool didHit;

	public Renderer shadowChild;

	public Collider2D hitOverride;

	private BoxCollider2D bc;

	private bool isBox;

	private bool verticalBox;

	private Vector2[] boxCheckPoints;

	private const int edgePoints = 4;

	private const int totalPointsPerEdge = 6;

	public void Start()
	{
		LightSource.NoShadows.Add(gameObject, this);
		SetMaskFunction(8);
		CalculateBoxEdgeCheckPoints();
	}

	private void CalculateBoxEdgeCheckPoints()
	{
		bc = GetComponent<BoxCollider2D>();
		if ((bool)bc)
		{
			isBox = true;
			verticalBox = false;
			boxCheckPoints = new Vector2[8];
			Vector2 vector = bc.size * 0.5f;
			Vector2 vector2 = new Vector2(vector.x, 0f);
			Vector2 vector3 = new Vector2(0f, vector.y);
			if (vector.y > vector.x)
			{
				verticalBox = true;
				vector3 = vector2;
				vector2 = new Vector2(0f, vector.y);
			}
			int num = 1;
			int num2 = 0;
			while (num < 5)
			{
				Vector2 vector4 = Vector2.Lerp(-vector2, vector2, (float)num / 5f);
				boxCheckPoints[num2] = vector4 + vector3 + bc.offset;
				boxCheckPoints[num2 + 4] = vector4 - vector3 + bc.offset;
				num++;
				num2++;
			}
		}
	}

	public void OnDestroy()
	{
		LightSource.NoShadows.Remove(gameObject);
	}

	private void SetMaskFunction(int func)
	{ /* spliced-from-2019 */
		rend.material.SetInt("_Mask", func);
		if ((bool)shadowChild)
		{
			shadowChild.material.SetInt("_Mask", func);
		}
	}

	internal unsafe void CheckHit(float lightRadius, Vector2 lightPosition)
	{
		Vector2 vector = transform.position;
		if (!(Vector2.Distance(vector, lightPosition) < lightRadius + 1f))
		{
			return;
		}
		if ((bool)hitOverride)
		{
			if (!PhysicsHelpers.AnythingBetween(lightPosition, vector, Constants.ShadowMask, useTriggers: false, hitOverride, transform))
			{
				didHit = true;
			}
		}
		else if (!PhysicsHelpers.AnythingBetween(lightPosition, vector, Constants.ShadowMask, useTriggers: false, null, transform))
		{
			didHit = true;
		}
		if (didHit || !isBox)
		{
			return;
		}
		int num = 0;
		int num2 = 4;
		if (verticalBox)
		{
			if (lightPosition.x < transform.position.x)
			{
				num += 4;
				num2 += 4;
			}
		}
		else if (lightPosition.y < transform.position.y)
		{
			num += 4;
			num2 += 4;
		}
		if ((bool)hitOverride)
		{
			for (int i = num; i < num2; i++)
			{
				if (!PhysicsHelpers.AnythingBetween(lightPosition, transform.TransformPoint(boxCheckPoints[i]), Constants.ShadowMask, useTriggers: false, hitOverride, transform))
				{
					didHit = true;
					break;
				}
			}
			return;
		}
		for (int j = num; j < num2; j++)
		{
			if (!PhysicsHelpers.AnythingBetween(lightPosition, transform.TransformPoint(boxCheckPoints[j]), Constants.ShadowMask, useTriggers: false, bc, transform))
			{
				didHit = true;
				break;
			}
		}
	}
}
