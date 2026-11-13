using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

public abstract class LightSourceRenderer : IDisposable
{
	protected readonly LightSource lightSource;

	public abstract LightSourceRendererType Type { get; }

	public LightSourceRenderer(LightSource lightSource)
	{
		this.lightSource = lightSource;
	}

	public abstract void Initialize();

	public abstract void Render(Vector2 origin);

	public abstract void Dispose();

	public static LightSourceRenderer Create(LightSourceRendererType type, LightSource lightSource)
	
	{
		switch (type)
		{
		default:
			Logger.GlobalInstance.Error(string.Format("[{0}] {1} {2} has not been configured. Aborting. (This should not be happening.)", "LightSourceRenderer", "LightSourceRendererType", type));
			return null;
		case LightSourceRendererType.Auto:
		{
			LightSourceRendererType platformType = GetPlatformType();
			Logger.GlobalInstance.Info(string.Format("[{0}] {1} {2} returned {3}.", "LightSourceRenderer", "LightSourceRendererType", type, platformType));
			return Create(platformType, lightSource);
		}
		case LightSourceRendererType.Raycast:
			return new LightSourceRaycastRenderer(lightSource);
		case LightSourceRendererType.GPU:
			return new LightSourceGpuRenderer(lightSource);
		}
	}

	public static LightSourceRendererType GetPlatformType()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return LightSourceRendererType.Raycast;
		}
		return LightSourceRendererType.GPU;
	}
}
