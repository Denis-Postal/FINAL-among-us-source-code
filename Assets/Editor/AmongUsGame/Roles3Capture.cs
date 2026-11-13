#if UNITY_EDITOR
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmongUsGameTools
{
	// roles3 agent capture actions (roles, kill, pet, name colours, audio). Hooked from MenuCapture.RunAction.
	public static class Roles3Capture
	{
		const BindingFlags F = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
		static string P(object o) { return o == null ? "null" : o.ToString(); }

		public static bool Run(string action)
		{
			if(!action.StartsWith("r3")) return false;
			string arg = action.Contains("=") ? action.Substring(action.IndexOf('=') + 1) : "";
			string name = action.Contains("=") ? action.Substring(0, action.IndexOf('=')) : action;
			try
			{
				switch(name)
				{
					case "r3hud": Hud(); break;
					case "r3names": Names(); break;
					case "r3audio": Audio(); break;
					case "r3state": State(); break;
					case "r3ambient": Ambient(); break;
					case "r3ventdbg":
					{
						var lp = PlayerControl.LocalPlayer; var v = ShipStatus.Instance.AllVents[0];
						float d = v.CanUse(lp.Data, out var cu, out var cou);
						Debug.Log("[R3] VENTDBG dist=" + d + " canUse=" + cu + " couldUse=" + cou + " roleCanVent=" + lp.Data.Role.CanVent + " roleCanUse=" + lp.Data.Role.CanUse(v) + " usables=" + GameManager.Instance.LogicUsables.CanUse(v, lp) + " mustClean=" + lp.MustCleanVent(v.Id) + " canMove=" + lp.CanMove + " usableDist=" + v.UsableDistance + " col=" + (v.GetComponent<Collider2D>() ? v.GetComponent<Collider2D>().enabled.ToString() : "none") + " layer=" + v.gameObject.layer + " inRangeCount=" + lp.GetType().GetField("itemsInRange", F).GetValue(lp));
						break;
					}
					case "r3bodydbg":
					{
						var lp = PlayerControl.LocalPlayer; var tp = lp.GetTruePosition();
						Debug.Log("[R3] BODYDBG canMove=" + lp.CanMove + " canReport=" + GameManager.Instance.CanReportBodies() + " maxRep=" + lp.MaxReportDistance + " dead=" + lp.Data.IsDead + " usablesMask=" + Constants.Usables);
						foreach(var b in UnityEngine.Object.FindObjectsByType<DeadBody>(FindObjectsInactive.Include, FindObjectsSortMode.None))
						{
							var c = b.GetComponent<Collider2D>();
							Debug.Log("[R3] BODYDBG body enabled=" + b.enabled + " reported=" + b.Reported + " tag=" + b.tag + " layer=" + b.gameObject.layer + " col=" + (c ? c.GetType().Name + " en=" + c.enabled + " trig=" + c.isTrigger : "null") + " tp=" + b.TruePosition + " dist=" + Vector2.Distance(tp, b.TruePosition) + " between=" + PhysicsHelpers.AnythingBetween(tp, b.TruePosition, Constants.ShipAndObjectsMask, false));
						}
						var buf = new Collider2D[20]; int n = Physics2D.OverlapCircleNonAlloc(tp, lp.MaxReportDistance, buf, Constants.Usables);
						for(int i = 0; i < n; i++) Debug.Log("[R3] BODYDBG overlap " + buf[i].name + " tag=" + buf[i].tag);
						break;
					}
					case "r3hang": { var end = DateTime.Now.AddSeconds(60); while(DateTime.Now < end) { } break; }
					case "r3dummyrole": { var d = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p.isDummy); if(d) d.RpcSetRole((AmongUs.GameOptions.RoleTypes)Enum.Parse(typeof(AmongUs.GameOptions.RoleTypes), arg, true)); break; }
					case "r3tpvent": { var v = ShipStatus.Instance.AllVents[int.Parse(arg == "" ? "0" : arg)]; PlayerControl.LocalPlayer.NetTransform.RpcSnapTo((Vector2)v.transform.position + new Vector2(0, 0f)); Debug.Log("[R3] tpvent " + v.name + " " + v.transform.position); break; }
					case "r3shiftpick":
					{
						var g = UnityEngine.Object.FindFirstObjectByType<ShapeshifterMinigame>();
						Debug.Log("[R3] SHIFTPICK game=" + (g != null));
						if(g) { var list = (System.Collections.Generic.List<ShapeshifterPanel>)typeof(ShapeshifterMinigame).GetField("potentialVictims", F).GetValue(g); Debug.Log("[R3] SHIFTPICK panels=" + list.Count); if(list.Count > 0) list[0].ShapeShift(); }
						break;
					}
					case "r3dummy":
					{
						var a = arg.Split(';');
						LocalFreeplayDebugCommands.CreateCharacter(a[0], a.Length > 1 ? int.Parse(a[1]) : 3, "");
						break;
					}
					case "r3dummytp":
					{
						// place the first living dummy at offset (dx;dy) from the local player
						var a = arg.Split(';');
						var d = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p.isDummy && !p.Data.IsDead);
						if(d) d.transform.position = PlayerControl.LocalPlayer.transform.position + new Vector3(float.Parse(a[0], System.Globalization.CultureInfo.InvariantCulture), float.Parse(a[1], System.Globalization.CultureInfo.InvariantCulture), 0);
						break;
					}
					case "r3click": Click(arg); break;
					case "r3pet":
					{
						var pd = HatManager.Instance.allPets.FirstOrDefault(p => p.ProductId != "pet_EmptyPet" && p.ProductId != "");
						Debug.Log("[R3] pet id=" + (pd ? pd.ProductId : "none") + " count=" + HatManager.Instance.allPets.Length);
						if(pd) PlayerControl.LocalPlayer.RpcSetPet(arg != "" ? arg : pd.ProductId);
						break;
					}
					case "r3petstate": PetState(); break;
					case "r3petwatch": HudManager.Instance.StartCoroutine(PetWatch()); break;
					case "r3shot": ScreenCapture.CaptureScreenshot("C:/spacemafia-game/scratch/shots/r3_" + arg + ".png"); break;
					default: return false;
				}
			}
			catch(Exception ex) { Debug.Log("[R3] ERROR " + action + " " + ex); Debug.LogException(ex); }
			return true;
		}

		static void Hud()
		{
			var h = HudManager.Instance;
			var role = PlayerControl.LocalPlayer.Data.Role;
			Debug.Log("[R3] HUD role=" + role.GetType().Name + " type=" + role.Role + " impostor=" + role.IsImpostor);
			foreach(var b in h.GetComponentsInChildren<ActionButton>(true))
			{
				var graphic = (SpriteRenderer)typeof(ActionButton).GetField("graphic", F).GetValue(b);
				var label = (TMPro.TextMeshPro)typeof(ActionButton).GetField("buttonLabelText", F).GetValue(b);
				var cd = (TMPro.TextMeshPro)typeof(ActionButton).GetField("cooldownTimerText", F).GetValue(b);
				Debug.Log("[R3] BTN " + b.GetType().Name + " " + b.name + " active=" + b.isActiveAndEnabled + " go=" + b.gameObject.activeInHierarchy + " sprite=" + (graphic && graphic.sprite ? graphic.sprite.name : "null") + " label='" + (label ? label.text : "null") + "' cdText='" + (cd ? cd.text : "null") + "' cdActive=" + (cd ? cd.gameObject.activeSelf.ToString() : "-") + " cooling=" + b.IsOnCooldown + " canInteract=" + b.canInteract);
			}
		}

		static void Names()
		{
			foreach(var p in PlayerControl.AllPlayerControls.ToArray())
			{
				var t = p.cosmetics != null ? (TMPro.TextMeshPro)typeof(CosmeticsLayer).GetField("nameText", F).GetValue(p.cosmetics) : null;
				Debug.Log("[R3] NAME " + p.name + " pid=" + p.PlayerId + " role=" + (p.Data != null && p.Data.Role ? p.Data.Role.Role.ToString() : "?") + " imp=" + (p.Data != null && p.Data.Role && p.Data.Role.IsImpostor) + " text='" + (t ? t.text : "") + "' color=" + (t ? t.color.ToString() : "null") + " dead=" + (p.Data != null && p.Data.IsDead) + " local=" + (p == PlayerControl.LocalPlayer));
			}
		}

		static void Audio()
		{
			var lp = PlayerControl.LocalPlayer ? (Vector2)PlayerControl.LocalPlayer.transform.position : Vector2.zero;
			Debug.Log("[R3] AUDIO local=" + lp);
			var sm = SoundManager.Instance;
			var spl = (System.Collections.IList)typeof(SoundManager).GetField("soundPlayers", F).GetValue(sm);
			foreach(var sp in spl)
			{
				var dyn = sp as DynamicSound;
				Debug.Log("[R3] SOUNDPLAYER " + ((ISoundPlayer)sp).Name + " type=" + sp.GetType().Name + (dyn != null ? " func=" + (dyn.volumeFunc != null ? dyn.volumeFunc.Target + "::" + dyn.volumeFunc.Method.Name : "NULL") : "") + " playing=" + ((ISoundPlayer)sp).Player.isPlaying + " vol=" + ((ISoundPlayer)sp).Player.volume.ToString("F3") + " clip=" + (((ISoundPlayer)sp).Player.clip ? ((ISoundPlayer)sp).Player.clip.name : "null"));
			}
			foreach(var a in UnityEngine.Object.FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if(!a.isPlaying && !a.playOnAwake) continue;
				Debug.Log("[R3] AUDIO " + (a.isPlaying ? "PLAYING " : "idle ") + GetPath(a.transform) + " clip=" + (a.clip ? a.clip.name : "null") + " loop=" + a.loop + " vol=" + a.volume.ToString("F2") + " blend=" + a.spatialBlend + " min=" + a.minDistance + " max=" + a.maxDistance + " rolloff=" + a.rolloffMode + " mute=" + a.mute + " pos=" + (Vector2)a.transform.position + " dist=" + Vector2.Distance(lp, a.transform.position).ToString("F1") + " group=" + (a.outputAudioMixerGroup ? a.outputAudioMixerGroup.name : "-") + " active=" + a.gameObject.activeInHierarchy);
			}
		}
		static void Ambient()
		{
			var lp = (Vector2)PlayerControl.LocalPlayer.GetTruePosition();
			foreach(var a in UnityEngine.Object.FindObjectsByType<AmbientSoundPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				bool inside = false; string areas = "";
				if(a.HitAreas != null) foreach(var c in a.HitAreas) { if(!c) continue; inside |= c.OverlapPoint(lp); areas += c.bounds.center.ToString("F1") + "/" + c.bounds.size.ToString("F1") + " "; }
				Debug.Log("[R3] AMBIENT " + GetPath(a.transform) + " clip=" + (a.AmbientSound ? a.AmbientSound.name : "null") + " max=" + a.MaxVolume + " inside=" + inside + " areas=" + areas + " active=" + a.isActiveAndEnabled);
			}
			foreach(var a in UnityEngine.Object.FindObjectsByType<RaycastAmbientSoundPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None)) Debug.Log("[R3] RAYAMBIENT " + GetPath(a.transform));
			foreach(var a in UnityEngine.Object.FindObjectsByType<TagAmbientSoundPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None)) Debug.Log("[R3] TAGAMBIENT " + GetPath(a.transform));
		}
		static string GetPath(Transform t) { string s = t.name; while(t.parent) { t = t.parent; s = t.name + "/" + s; if(s.Length > 120) break; } return s; }

		static void State()
		{
			var lp = PlayerControl.LocalPlayer;
			Debug.Log("[R3] STATE pos=" + (Vector2)lp.transform.position + " role=" + lp.Data.Role.Role + " killTimer=" + typeof(PlayerControl).GetField("killTimer", F).GetValue(lp) + " dead=" + lp.Data.IsDead + " players=" + PlayerControl.AllPlayerControls.Count + " bodies=" + UnityEngine.Object.FindObjectsByType<DeadBody>(FindObjectsSortMode.None).Length + " petting=" + lp.petting + " canMove=" + lp.CanMove);
			foreach(var p in PlayerControl.AllPlayerControls.ToArray()) Debug.Log("[R3] PLAYER " + p.name + " dead=" + p.Data.IsDead + " role=" + p.Data.Role.Role + " pos=" + (Vector2)p.transform.position + " shown=" + p.Visible + " dummy=" + p.isDummy);
			var t = lp.GetType().GetField("closest", F);
			Debug.Log("[R3] TARGET closest=" + (t != null ? P(t.GetValue(lp)) : "?") + " killTarget=" + P(HudManager.Instance.KillButton.Target));
		}

		static void Click(string which)
		{
			var h = HudManager.Instance;
			ActionButton b = null;
			switch(which)
			{
				case "Kill": b = h.KillButton; break;
				case "Ability": b = h.AbilityButton; break;
				case "Ability2": b = h.SecondaryAbilityButton; break;
				case "Pet": b = h.PetButton; break;
				case "Use": b = h.UseButton; break;
				case "Report": b = h.ReportButton; break;
				case "Vent": b = h.ImpostorVentButton; break;
				case "Sabotage": b = h.SabotageButton; break;
			}
			if(!b) { Debug.Log("[R3] click: no button " + which); return; }
			Debug.Log("[R3] CLICK " + which + " active=" + b.isActiveAndEnabled + " canInteract=" + b.canInteract);
			b.DoClick();
		}

		static void PetState()
		{
			var lp = PlayerControl.LocalPlayer; var pet = lp.cosmetics.currentPet;
			if(!pet) { Debug.Log("[R3] PETSTATE no pet equipped hasPet=" + lp.cosmetics.HasPetEquipped()); return; }
			var anim = (PowerTools.SpriteAnim)typeof(PetBehaviour).GetField("animator", F).GetValue(pet);
			var spr = pet.GetComponentInChildren<SpriteRenderer>();
			Debug.Log("[R3] PETSTATE pet=" + pet.name + " petting=" + lp.petting + " anim=" + (anim && anim.m_currAnim ? anim.m_currAnim.name : "null") + " sprite=" + (spr && spr.sprite ? spr.sprite.name : "null") + " beingPet=" + typeof(PetBehaviour).GetField("beingPet", F).GetValue(pet) + " hand=" + lp.cosmetics.PettingHand.gameObject.activeSelf + " handSprite=" + lp.cosmetics.PettingHand.HandSprite.sprite);
		}
		static IEnumerator PetWatch()
		{
			for(int i = 0; i < 24; i++) { PetState(); yield return new WaitForSeconds(0.25f); }
		}
	}
}
#endif
