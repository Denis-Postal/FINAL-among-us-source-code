using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.AddressableAssets
{
	// guid -> asset map used by the offline Addressables shim (built by unity_editor/BuildRegistry.cs).
	// Must be in its own file named after the class so the ScriptableObject asset serializes with a valid m_Script.
	public class AssetGuidRegistry : ScriptableObject
	{
		public List<string> guids = new List<string>();
		public List<Object> objects = new List<Object>();
	}
}
