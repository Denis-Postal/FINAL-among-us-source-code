#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace AmongUsGameTools
{
	// Opt-in observation only: never modifies button state or materials.
	public static class HudIconCapture
	{
		const BindingFlags Fields = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		static readonly int[] Samples = { 1, 5, 30, 120, 600 };
		static int spawnFrame = -1, sample;
		public static void Tick(string output)
		{
			if (!EditorPrefs.GetBool("AmongUsGame.HudIcons", false)) return;
			var player = PlayerControl.LocalPlayer;
			var hud = UnityEngine.Object.FindFirstObjectByType<HudManager>();
			if (!player || !hud) return;
			if (spawnFrame < 0) spawnFrame = Time.frameCount;
			int frame = Time.frameCount - spawnFrame + 1;
			if (sample >= Samples.Length || frame < Samples[sample]) return;
			Dump(hud, "spawn+" + Samples[sample] + " actual=" + frame);
			string file = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(output), System.IO.Path.GetFileNameWithoutExtension(output) + "_hud_f" + Samples[sample] + ".png");
			ScreenCapture.CaptureScreenshot(file);
			Debug.Log("[HUDICONS] screenshot=" + file);
			sample++;
		}
		public static void Dump(HudManager hud, string phase)
		{
			var sb = new StringBuilder("[HUDICONS] BEGIN " + phase + " time=" + Time.time + " frame=" + Time.frameCount + "\n");
			foreach (var camera in UnityEngine.Object.FindObjectsByType<Camera>(FindObjectsSortMode.None))
				sb.AppendLine("CAMERA " + camera.name + " position=" + camera.transform.position + " near=" + camera.nearClipPlane + " far=" + camera.farClipPlane + " mask=" + camera.cullingMask);
			foreach (var field in typeof(HudManager).GetFields(Fields))
				if (typeof(ActionButton).IsAssignableFrom(field.FieldType))
				{
					var button = field.GetValue(hud) as ActionButton;
					if (!button) continue;
					sb.AppendLine("BUTTON " + field.Name + " name=" + button.name + " active=" + button.gameObject.activeInHierarchy + " component=" + button.enabled + " canInteract=" + button.canInteract + " cooling=" + button.IsOnCooldown);
					var graphic = typeof(ActionButton).GetField("graphic", Fields).GetValue(button) as SpriteRenderer;
					if (graphic) Renderer(sb, "GRAPHIC " + field.Name, graphic);
					else sb.AppendLine("GRAPHIC " + field.Name + " NULL");
					foreach (var text in button.GetComponentsInChildren<TMP_Text>(true))
						sb.AppendLine("TEXT " + field.Name + "/" + text.name + " active=" + text.gameObject.activeInHierarchy + " enabled=" + text.enabled + " value=" + text.text + " color=" + text.color.ToString("F4") + " material=" + text.fontSharedMaterial);
					foreach (var child in button.GetComponentsInChildren<SpriteRenderer>(true))
						if (child != graphic) Renderer(sb, "CHILD " + field.Name + "/" + child.name, child);
					foreach (var animator in button.GetComponentsInChildren<Animator>(true))
						sb.AppendLine("ANIMATOR " + field.Name + "/" + animator.name + " enabled=" + animator.enabled + " controller=" + animator.runtimeAnimatorController + " speed=" + animator.speed);
					foreach (var animation in button.GetComponentsInChildren<Animation>(true))
						sb.AppendLine("ANIMATION " + field.Name + "/" + animation.name + " enabled=" + animation.enabled + " playing=" + animation.isPlaying + " clip=" + animation.clip);
				}
			foreach (var glyph in hud.GetComponentsInChildren<ActionMapGlyphDisplay>(true))
				sb.AppendLine("GLYPH " + glyph.name + " active=" + glyph.gameObject.activeInHierarchy + " input=" + ActiveInputManager.currentControlType);
			sb.AppendLine("CHAT active=" + hud.Chat.gameObject.activeInHierarchy);
			sb.AppendLine("[HUDICONS] END");
			Debug.Log(sb.ToString());
		}
		static void Renderer(StringBuilder sb, string label, SpriteRenderer renderer)
		{
			var material = renderer.sharedMaterial;
			sb.AppendLine(label + " active=" + renderer.gameObject.activeInHierarchy + " enabled=" + renderer.enabled + " sprite=" + (renderer.sprite ? renderer.sprite.name : "NULL") + " spriteID=" + (renderer.sprite ? renderer.sprite.GetHashCode() : 0) + " color=" + renderer.color.ToString("F4") + " position=" + renderer.transform.position.ToString("F4") + " scale=" + renderer.transform.lossyScale.ToString("F4") + " bounds=" + renderer.bounds + " layer=" + renderer.gameObject.layer + " sortingLayer=" + renderer.sortingLayerID + " order=" + renderer.sortingOrder + " mask=" + renderer.maskInteraction + " propertyBlock=" + renderer.HasPropertyBlock());
			if (!material || !material.shader) { sb.AppendLine("MATERIAL " + label + " NULL"); return; }
			sb.AppendLine("MATERIAL " + label + " name=" + material.name + " id=" + material.GetHashCode() + " asset=" + AssetDatabase.GetAssetPath(material) + " shader=" + material.shader.name + " supported=" + material.shader.isSupported + " queue=" + material.renderQueue + " passes=" + material.passCount + " keywords=" + string.Join(",", material.shaderKeywords));
			for (int index = 0; index < material.shader.GetPropertyCount(); index++)
			{
				string name = material.shader.GetPropertyName(index);
				string value;
				switch (material.shader.GetPropertyType(index))
				{
					case ShaderPropertyType.Color: value = material.GetColor(name).ToString("F4"); break;
					case ShaderPropertyType.Vector: value = material.GetVector(name).ToString("F4"); break;
					case ShaderPropertyType.Texture: value = material.GetTexture(name) + " scale=" + material.GetTextureScale(name) + " offset=" + material.GetTextureOffset(name); break;
					default: value = material.GetFloat(name).ToString("R"); break;
				}
				sb.AppendLine("PROPERTY " + label + " " + name + "=" + value);
			}
			if (renderer.HasPropertyBlock())
			{
				var block = new MaterialPropertyBlock();
				renderer.GetPropertyBlock(block);
				sb.AppendLine("BLOCK " + label + " texture=" + block.GetTexture("_MainTex") + " hasColor=" + block.HasProperty("_Color") + " color=" + block.GetColor("_Color").ToString("F4") + " hasPercent=" + block.HasProperty("_Percent") + " percent=" + block.GetFloat("_Percent") + " desat=" + block.GetFloat("_Desat") + " uv=" + block.GetVector("_NormalizedUvs") + " hasST=" + block.HasProperty("_MainTex_ST") + " st=" + block.GetVector("_MainTex_ST") + " forceOff=" + renderer.forceRenderingOff + " visible=" + renderer.isVisible);
			}
		}
	}
}
#endif
