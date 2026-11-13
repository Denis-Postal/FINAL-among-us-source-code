using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Rendering;

public class LightSourceGpuRenderer : LightSourceRenderer
{
	public delegate void AddEdgeDelegate(Vector3 a, Vector3 b);

	private const float TWO_PI = MathF.PI * 2f;

	private Mesh occluderMesh;

	public List<Vector3> occVerts= new List<Vector3>();

	public List<Vector3> occNorms= new List<Vector3>();

	public List<Vector2> occUVs= new List<Vector2>();

	public List<int> occTris= new List<int>();

	public Collider2D[] hits= new Collider2D[100];

	private Material shadowCasterMaterial;

	private CommandBuffer cb;

	private bool noGeomSupport;

	private RenderTexture shadowTexture;

	private Vector2 shadowOrigin;

	private AddEdgeDelegate AddEdge;

	public override LightSourceRendererType Type => LightSourceRendererType.GPU;

	public LightSourceGpuRenderer(LightSource lightSource)  : base(lightSource)
	{
	}

	public unsafe override void Initialize()
	{
		if (cb == null)
		{
			cb = new CommandBuffer();
			cb.name = "Draw occlusion";
		}
		GraphicsDeviceType graphicsDeviceType = SystemInfo.graphicsDeviceType;
		if (graphicsDeviceType == GraphicsDeviceType.OpenGLES2 || graphicsDeviceType == GraphicsDeviceType.Metal)
		{
			noGeomSupport = true;
			AddEdge = AddEdgeNoGeom;
			Logger.GlobalInstance.Info("Graphics API has no geometry shader support, switching to CPU workaround");
		}
		else
		{
			noGeomSupport = false;
			AddEdge = AddEdgeDefault;
			Logger.GlobalInstance.Info("Graphics API has geometry shader support");
		}
		if (!noGeomSupport && !SystemInfo.supportsGeometryShaders)
		{
			noGeomSupport = true;
			AddEdge = AddEdgeNoGeom;
			Logger.GlobalInstance.Info("Graphics hardware has no geometry shader support, switching to CPU workaround");
		}
		lightSource.LightChildMeshFilter.sharedMesh = lightSource.LightChildMesh;
		lightSource.LightCutawayMaterial.EnableKeyword("GPU_SHADOW");
		occluderMesh = new Mesh();
		occluderMesh.MarkDynamic();
		occluderMesh.name = "Occluder Mesh";
		GenerateShadowmap();
	}

	public override void Render(Vector2 origin)
	{
		GPUShadows(origin);
	}

	public override void Dispose()
	{
		if ((bool)shadowTexture)
		{
			shadowTexture.Release();
			shadowTexture = null;
		}
	}

	private void GenerateShadowmap()
	{
		if (SystemInfo.SupportsRenderTextureFormat(lightSource.GpuPreferredRTFormat))
		{
			shadowTexture = new RenderTexture(3, lightSource.GpuShadowmapResolution, 16, lightSource.GpuPreferredRTFormat);
		}
		else
		{
			shadowTexture = new RenderTexture(3, lightSource.GpuShadowmapResolution, 16, RenderTextureFormat.R8);
		}
		shadowTexture.wrapModeU = TextureWrapMode.Clamp;
		shadowTexture.wrapModeV = TextureWrapMode.Repeat;
		shadowTexture.filterMode = FilterMode.Bilinear;
		shadowTexture.Create();
		shadowCasterMaterial = new Material(lightSource.GpuShadowCasterMaterial);
	}

	private unsafe void GPUShadows(Vector2 origin)
	{
		ClearEdges();
		shadowOrigin = origin;
		lightSource.LightChild.transform.localScale = new Vector3(lightSource.ViewDistance * 3f, lightSource.ViewDistance * 3f, 1f);
		Camera main = Camera.main;
		Vector2 vector = (Vector2)main.transform.position - origin;
		float b = new Vector2(main.orthographicSize * main.aspect, main.orthographicSize).magnitude + vector.magnitude;
		float num = Mathf.Min(lightSource.ViewDistance, b);
		foreach (NoShadowBehaviour value3 in LightSource.NoShadows.Values)
		{
			value3.CheckHit(num, origin);
		}
		int num2 = Physics2D.OverlapCircleNonAlloc(origin, num, hits, Constants.ShadowMask);
		for (int i = 0; i < num2; i++)
		{
			Collider2D collider2D = hits[i];
			if (collider2D.isTrigger || (LightSource.NoShadows.TryGetValue(collider2D.gameObject, out var value) && value.hitOverride == collider2D) || (LightSource.OneWayShadows.TryGetValue(collider2D.gameObject, out var value2) && value2.IsIgnored(lightSource)))
			{
				continue;
			}
			EdgeCollider2D edgeCollider2D = collider2D as EdgeCollider2D;
			if ((bool)edgeCollider2D)
			{
				Vector2[] points = edgeCollider2D.points;
				for (int j = 0; j < points.Length - 1; j++)
				{
					Vector3 a = edgeCollider2D.transform.TransformPoint(points[j]);
					Vector3 b2 = edgeCollider2D.transform.TransformPoint(points[j + 1]);
					AddEdge(a, b2);
				}
				continue;
			}
			PolygonCollider2D polygonCollider2D = collider2D as PolygonCollider2D;
			if ((bool)polygonCollider2D)
			{
				Vector2[] points2 = polygonCollider2D.points;
				for (int k = 0; k < points2.Length; k++)
				{
					int num3 = k + 1;
					if (num3 == points2.Length)
					{
						num3 = 0;
					}
					Vector3 a2 = polygonCollider2D.transform.TransformPoint(points2[k]);
					Vector3 b3 = polygonCollider2D.transform.TransformPoint(points2[num3]);
					AddEdge(a2, b3);
				}
				continue;
			}
			BoxCollider2D boxCollider2D = collider2D as BoxCollider2D;
			if ((bool)boxCollider2D)
			{
				Vector2 vector2 = boxCollider2D.size / 2f;
				Vector2 vector3 = boxCollider2D.transform.TransformPoint(boxCollider2D.offset - vector2);
				Vector2 vector4 = boxCollider2D.transform.TransformPoint(boxCollider2D.offset + vector2);
				Vector3 a3 = vector3;
				Vector3 b4 = vector3;
				b4.y = vector4.y;
				AddEdge(a3, b4);
				a3.y = vector4.y;
				b4.x = vector4.x;
				AddEdge(a3, b4);
				a3 = vector4;
				b4.y = vector3.y;
				AddEdge(a3, b4);
				a3.y = vector3.y;
				b4 = vector3;
				AddEdge(a3, b4);
			}
		}
		UpdateOccMesh();
		DrawOcclusion(num, origin);
	}

	private void ClearEdges()
	{
		occluderMesh.Clear();
		occVerts.Clear();
		occNorms.Clear();
		if (noGeomSupport)
		{
			occUVs.Clear();
		}
		occTris.Clear();
	}

	private void UpdateOccMesh()
	{
		occluderMesh.SetVertices(occVerts);
		occluderMesh.SetNormals(occNorms);
		if (noGeomSupport)
		{
			occluderMesh.SetUVs(0, occUVs);
		}
		occluderMesh.SetTriangles(occTris, 0, occTris.Count, 0);
	}

	private void DrawOcclusion(float effectiveRadius, Vector2 origin)
	{
		if ((bool)shadowTexture && (bool)shadowCasterMaterial)
		{
			float num = shadowTexture.width;
			shadowCasterMaterial.SetFloat("_DepthCompressionValue", effectiveRadius);
			cb.Clear();
			cb.SetRenderTarget(shadowTexture);
			cb.ClearRenderTarget(clearDepth: true, clearColor: true, new Color(1f, 1f, 1f, 1f));
			cb.SetGlobalTexture("_ShmapTexture", shadowTexture);
			cb.SetGlobalFloat("_Radius", lightSource.ViewDistance);
			cb.SetGlobalFloat("_Column", 0f);
			cb.SetGlobalVector("_LightPosition", new Vector3(origin.x, origin.y, lightSource.transform.position.z));
			cb.SetGlobalVector("_TexelSize", new Vector4(1f / num, 1f / num, num, num));
			cb.SetGlobalFloat("_DepthCompressionValue", effectiveRadius);
			cb.DrawMesh(occluderMesh, Matrix4x4.identity, shadowCasterMaterial);
			Graphics.ExecuteCommandBuffer(cb);
		}
	}

	private unsafe void DebugDrawRadius(Vector2 center, float radius)
	{
		int num = Mathf.Max(20, (int)(20f * radius));
		Vector2 vector = center + Vector2.right * radius;
		for (int i = 1; i <= num; i++)
		{
			float f = (float)i / (float)num * MathF.PI * 2f;
			Vector2 vector2 = center + new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * radius;
			Debug.DrawLine(vector, vector2, Color.yellow);
			vector = vector2;
		}
	}

	private void AddEdgeNoGeom(Vector3 a, Vector3 b)
	{
		Vector2 vector = (Vector2)a - shadowOrigin;
		Vector2 vector2 = (Vector2)b - shadowOrigin;
		float num = Mathf.Atan2(vector.y, vector.x);
		float num2 = Mathf.Atan2(vector2.y, vector2.x);
		float num3 = num + ((num < 0f) ? (MathF.PI * 2f) : 0f);
		num2 += ((num2 < 0f) ? (MathF.PI * 2f) : 0f);
		if (Mathf.Abs(num3 - num2) > MathF.PI)
		{
			AddEdgeInternal(a, b, new Vector2(0f, 0f));
			AddEdgeInternal(a, b, new Vector2(0f, MathF.PI * 2f));
		}
		else
		{
			AddEdgeInternal(a, b, new Vector2(1f, 0f));
		}
	}

	private unsafe void AddEdgeInternal(Vector3 a, Vector3 b, Vector2 uv)
	{
		a.z = 0f;
		b.z = 0f;
		Vector3 vector = b - a;
		vector = new Vector3(vector.y, 0f - vector.x, 0f);
		Vector3 item = a + Vector3.forward * 0.5f;
		Vector3 item2 = a - Vector3.forward * 0.5f;
		Vector3 item3 = b + Vector3.forward * 0.5f;
		Vector3 item4 = b - Vector3.forward * 0.5f;
		int count;
		int num = (count = occVerts.Count);
		int item5 = num + 1;
		int item6 = num + 2;
		int item7 = num + 3;
		occVerts.Add(item);
		occVerts.Add(item2);
		occVerts.Add(item3);
		occVerts.Add(item4);
		occNorms.Add(vector);
		occNorms.Add(vector);
		occNorms.Add(vector);
		occNorms.Add(vector);
		occUVs.Add(uv);
		occUVs.Add(uv);
		occUVs.Add(uv);
		occUVs.Add(uv);
		occTris.Add(count);
		occTris.Add(item5);
		occTris.Add(item6);
		occTris.Add(item5);
		occTris.Add(item7);
		occTris.Add(item6);
	}

	private unsafe void AddEdgeDefault(Vector3 a, Vector3 b)
	{
		a.z = 0f;
		b.z = 0f;
		Vector3 vector = b - a;
		vector = new Vector3(vector.y, 0f - vector.x, 0f);
		Vector3 item = a + Vector3.forward * 0.5f;
		Vector3 item2 = a - Vector3.forward * 0.5f;
		Vector3 item3 = b + Vector3.forward * 0.5f;
		Vector3 item4 = b - Vector3.forward * 0.5f;
		int count;
		int num = (count = occVerts.Count);
		int item5 = num + 1;
		int item6 = num + 2;
		int item7 = num + 3;
		occVerts.Add(item);
		occVerts.Add(item2);
		occVerts.Add(item3);
		occVerts.Add(item4);
		occNorms.Add(vector);
		occNorms.Add(vector);
		occNorms.Add(vector);
		occNorms.Add(vector);
		occTris.Add(count);
		occTris.Add(item5);
		occTris.Add(item6);
		occTris.Add(item5);
		occTris.Add(item7);
		occTris.Add(item6);
	}
}
