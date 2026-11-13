using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

public class LightSourceRaycastRenderer : LightSourceRenderer
{
	public class VertInfo
	{
		public float Angle;

		public Vector3 Position;

		internal void Complete(float x, float y)
		
	{
			Position.x = x;
			Position.y = y;
			Angle = PseudoAngle(y, x);
		}

		internal void Complete(Vector2 point)
		
	{
			Position.x = point.x;
			Position.y = point.y;
			Angle = PseudoAngle(point.y, point.x);
		}
	}

	private class AngleComparer : IComparer<VertInfo>
	{
		public static readonly AngleComparer Instance= new AngleComparer();

		public int Compare(VertInfo x, VertInfo y)
		
	{
			if (!(x.Angle > y.Angle))
			{
				if (!(x.Angle < y.Angle))
				{
					return 0;
				}
				return -1;
			}
			return 1;
		}

		static AngleComparer()
		{ }
	}

	private class HitDepthComparer : IComparer<RaycastHit2D>
	{
		public static readonly HitDepthComparer Instance= new HitDepthComparer();

		public unsafe int Compare(RaycastHit2D x, RaycastHit2D y)
		
	{
			if (!(x.fraction > y.fraction))
			{
				return -1;
			}
			return 1;
		}

		static HitDepthComparer()
		{ }
	}

	private Camera camera;

	public List<VertInfo> verts= new List<VertInfo>(256);

	private int vertCount;

	private Vector3[] vec;

	private Vector2[] uvs;

	public int[] triangles= new int[1800];

	public RaycastHit2D[] buffer= new RaycastHit2D[50];

	private ContactFilter2D filter;

	public Collider2D[] hits= new Collider2D[100];

	private Vector2[] requiredDels;

	private Mesh myMesh;

	private Vector2 del;

	private Vector2 tan;

	private Vector2 side;

	public List<RaycastHit2D> lightHits= new List<RaycastHit2D>();

	public override LightSourceRendererType Type => LightSourceRendererType.Raycast;

	public LightSourceRaycastRenderer(LightSource lightSource) : base(lightSource)
	
	{
	}

	public override void Initialize()
	
	{
		myMesh = new Mesh();
		myMesh.MarkDynamic();
		myMesh.name = "ShadowMesh";
		filter.useTriggers = true;
		filter.layerMask = Constants.ShadowMask;
		filter.useLayerMask = true;
		lightSource.LightChildMeshFilter.sharedMesh = myMesh;
		lightSource.LightCutawayMaterial.DisableKeyword("GPU_SHADOW");
		requiredDels = new Vector2[lightSource.RaycastMinimumRayCount];
		for (int i = 0; i < requiredDels.Length; i++)
		{
			requiredDels[i] = Vector2.left.Rotate((float)i / (float)requiredDels.Length * 360f);
		}
	}

	public override void Render(Vector2 origin)
	{
		RaycastShadows(origin);
	}

	public override void Dispose()
	{
		lightSource.LightChildMeshFilter.sharedMesh = null;
		myMesh = null;
	}

	private float GetValidViewDistance()
	{
		if (camera == null)
		{
			camera = Camera.main;
		}
		return Mathf.Min(lightSource.ViewDistance, camera.orthographicSize * camera.aspect);
	}

	private unsafe void RaycastShadows(Vector2 origin)
	
	{
		vertCount = 0;
		lightSource.LightChild.transform.localScale = Vector3.one;
		int num = Physics2D.OverlapCircleNonAlloc(origin, GetValidViewDistance(), hits, Constants.ShadowMask);
		for (int i = 0; i < num; i++)
		{
			Collider2D collider2D = hits[i];
			if (collider2D.isTrigger)
			{
				continue;
			}
			EdgeCollider2D edgeCollider2D = collider2D as EdgeCollider2D;
			if ((bool)edgeCollider2D)
			{
				Vector2[] points = edgeCollider2D.points;
				for (int j = 0; j < points.Length; j++)
				{
					Vector2 vector = edgeCollider2D.transform.TransformPoint(points[j]);
					del.x = vector.x - origin.x;
					del.y = vector.y - origin.y;
					TestBothSides(origin);
				}
				continue;
			}
			PolygonCollider2D polygonCollider2D = collider2D as PolygonCollider2D;
			if ((bool)polygonCollider2D)
			{
				Vector2[] points2 = polygonCollider2D.points;
				for (int k = 0; k < points2.Length; k++)
				{
					Vector2 vector2 = polygonCollider2D.transform.TransformPoint(points2[k]);
					del.x = vector2.x - origin.x;
					del.y = vector2.y - origin.y;
					TestBothSides(origin);
				}
				continue;
			}
			BoxCollider2D boxCollider2D = collider2D as BoxCollider2D;
			if ((bool)boxCollider2D)
			{
				Vector2 vector3 = boxCollider2D.size / 2f;
				Vector2 vector4 = (Vector2)boxCollider2D.transform.TransformPoint(boxCollider2D.offset - vector3) - origin;
				Vector2 vector5 = (Vector2)boxCollider2D.transform.TransformPoint(boxCollider2D.offset + vector3) - origin;
				del.x = vector4.x;
				del.y = vector4.y;
				TestBothSides(origin);
				del.x = vector5.x;
				TestBothSides(origin);
				del.y = vector5.y;
				TestBothSides(origin);
				del.x = vector4.x;
				TestBothSides(origin);
			}
		}
		float num2 = GetValidViewDistance() * 1.05f;
		for (int l = 0; l < requiredDels.Length; l++)
		{
			Vector2 vector6 = num2 * requiredDels[l];
			CreateVert(origin, ref vector6);
		}
		verts.Sort(0, vertCount, AngleComparer.Instance);
		myMesh.Clear();
		if (vec == null || vec.Length < vertCount + 1)
		{
			vec = new Vector3[vertCount + 1];
			uvs = new Vector2[vec.Length];
		}
		vec[0] = Vector3.zero;
		uvs[0] = new Vector2(vec[0].x, vec[0].y);
		for (int m = 0; m < vertCount; m++)
		{
			int num3 = m + 1;
			vec[num3] = verts[m].Position;
			uvs[num3] = new Vector2(vec[num3].x, vec[num3].y);
		}
		int num4 = vertCount * 3;
		if (num4 > triangles.Length)
		{
			triangles = new int[num4];
			Debug.LogWarning("Resized triangles to: " + num4);
		}
		int num5 = 0;
		for (int n = 0; n < triangles.Length; n += 3)
		{
			if (n < num4)
			{
				triangles[n] = 0;
				triangles[n + 1] = num5 + 1;
				if (n == num4 - 3)
				{
					triangles[n + 2] = 1;
				}
				else
				{
					triangles[n + 2] = num5 + 2;
				}
				num5++;
			}
			else
			{
				triangles[n] = 0;
				triangles[n + 1] = 0;
				triangles[n + 2] = 0;
			}
		}
		myMesh.vertices = vec;
		myMesh.uv = uvs;
		myMesh.SetIndices(triangles, MeshTopology.Triangles, 0);
	}

	private unsafe void TestBothSides(Vector2 origin)
	
	{
		float num = length(del.x, del.x);
		tan.x = (0f - del.y) / num * lightSource.RaycastTolerance;
		tan.y = del.x / num * lightSource.RaycastTolerance;
		side.x = del.x + tan.x;
		side.y = del.y + tan.y;
		CreateVert(origin, ref side);
		side.x = del.x - tan.x;
		side.y = del.y - tan.y;
		CreateVert(origin, ref side);
	}

	private unsafe void CreateVert(Vector2 origin, ref Vector2 del)
	
	{
		float num = GetValidViewDistance() * 1.5f;
		int num2 = Physics2D.Raycast(origin, del, filter, buffer, num);
		if (num2 > 0)
		{
			lightHits.Clear();
			RaycastHit2D raycastHit2D = default;
			Collider2D collider2D = null;
			for (int i = 0; i < num2; i++)
			{
				RaycastHit2D raycastHit2D2 = buffer[i];
				Collider2D collider = raycastHit2D2.collider;
				if (!LightSource.OneWayShadows.TryGetValue(collider.gameObject, out var value) || !value.IsIgnored(lightSource))
				{
					lightHits.Add(raycastHit2D2);
					if (!collider.isTrigger)
					{
						raycastHit2D = raycastHit2D2;
						collider2D = collider;
						break;
					}
				}
			}
			for (int j = 0; j < lightHits.Count; j++)
			{
				RaycastHit2D raycastHit2D3 = lightHits[j];
				if (!(raycastHit2D3.distance > GetValidViewDistance()) && LightSource.NoShadows.TryGetValue(raycastHit2D3.collider.gameObject, out var value2))
				{
					value2.didHit = true;
				}
			}
			if ((bool)collider2D && !collider2D.isTrigger)
			{
				Vector2 point = raycastHit2D.point;
				GetEmptyVert().Complete(point.x - origin.x, point.y - origin.y);
				return;
			}
		}
		Vector2 normalized = del.normalized;
		GetEmptyVert().Complete(normalized.x * num, normalized.y * num);
	}

	private VertInfo GetEmptyVert()
	
	{
		if (vertCount < verts.Count)
		{
			return verts[vertCount++];
		}
		VertInfo vertInfo = new VertInfo();
		verts.Add(vertInfo);
		vertCount = verts.Count;
		return vertInfo;
	}

	private static float length(float x, float y)
	{
		return Mathf.Sqrt(x * x + y * y);
	}

	public static float PseudoAngle(float dx, float dy)
	
	{
		if (dx < 0f)
		{
			float num = 0f - dx;
			float num2 = ((dy > 0f) ? dy : (0f - dy));
			return 2f - dy / (num + num2);
		}
		float num3 = ((dy > 0f) ? dy : (0f - dy));
		return dy / (dx + num3);
	}
}
