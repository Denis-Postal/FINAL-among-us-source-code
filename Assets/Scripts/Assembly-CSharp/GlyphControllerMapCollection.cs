using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

public class GlyphControllerMapCollection : ScriptableObject
{
	[Serializable]
	public class GlyphControllerMap
	{
		public string controllerName;

		public string glyphCollectionPath;
	}

	public List<GlyphControllerMap> nameToGlyphCollectionList= new List<GlyphControllerMap>();

	public Dictionary<string, string> nameToGlyphCollectionDict;

	public static GlyphControllerMapCollection _instance;

	public static GlyphControllerMapCollection Instance
	{
		get
		{
			if (!_instance)
			{
				_instance = Resources.Load<GlyphControllerMapCollection>("ControllerGlyphMapAsset");
				_instance.Initialize();
			}
			return _instance;
		}
	}

	public unsafe void Initialize()
	{
		nameToGlyphCollectionDict = new Dictionary<string, string>();
		foreach (GlyphControllerMap nameToGlyphCollection in nameToGlyphCollectionList)
		{
			nameToGlyphCollectionDict.Add(nameToGlyphCollection.controllerName, nameToGlyphCollection.glyphCollectionPath);
		}
	}

	public GlyphCollection TryGetGlyphCollectionForController(string controllerName)
	{
		if (nameToGlyphCollectionDict.TryGetValue(controllerName, out var value))
		{
			GlyphCollection glyphCollection = Resources.Load<GlyphCollection>(value);
			if ((bool)glyphCollection)
			{
				glyphCollection.Initialize();
				return glyphCollection;
			}
		}
		return null;
	}

	public GlyphControllerMapCollection()
	{ }
}
