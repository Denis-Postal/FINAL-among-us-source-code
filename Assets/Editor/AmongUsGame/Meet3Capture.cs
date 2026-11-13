#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsGameTools
{
	// meet3 agent capture actions (called from FreeplayWaveCapture.Run).
	public static class Meet3Capture
	{
		public static bool Run(string action)
		{
			if (action == "dbgopen") { DebugToolInstantiator.SetOverlayVisible(true); Debug.Log("[M3] dbg opened"); return true; }
			if (action.StartsWith("dbgtab=")) { Innersloth.DebugTool.DebugUIEvents.OnTabClicked(action.Substring(7)); return true; }
			if (action == "dbgclose") { DebugToolInstantiator.SetOverlayVisible(false); return true; }
			if (action.StartsWith("dbgrun="))
			{
				var dparts = action.Substring(7).Split('|'); string path = dparts[0].Replace("~", " ");
				var buttons = UnityEngine.Object.FindObjectsByType<Innersloth.DebugTool.DebugCommandButton>(FindObjectsInactive.Include, FindObjectsSortMode.None);
				Debug.Log("[M3] dbg buttons=" + buttons.Length + " : " + string.Join(", ", buttons.Select(b => b.Path).ToArray()));
				var btn = buttons.FirstOrDefault(b => b.Path == path);
				if (!btn) { Debug.Log("[M3] dbgrun: no button " + path); return true; }
				btn.GetComponent<Button>().onClick.Invoke();
				var page = UnityEngine.Object.FindFirstObjectByType<Innersloth.DebugTool.DebugCommandOverlayPage>(FindObjectsInactive.Include);
				if (dparts.Length > 1)
				{
					var rows = (System.Collections.Generic.List<Innersloth.DebugTool.AbstractDebugCommandArgumentRow>)typeof(Innersloth.DebugTool.DebugCommandOverlayPage).GetField("commandInputs", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(page);
					for (int ri = 0; ri < rows.Count && ri + 1 < dparts.Length; ri++)
					{
						var tf = rows[ri] as Innersloth.DebugTool.DebugCommandArgumentRow_TextField;
						if (tf != null) ((TMPro.TMP_InputField)typeof(Innersloth.DebugTool.DebugCommandArgumentRow_TextField).GetField("inputField", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tf)).text = dparts[ri + 1];
						Debug.Log("[M3] dbgrun arg" + ri + "=" + rows[ri].GetValue() + " rowtype=" + rows[ri].GetType().Name);
					}
				}
				var exec = (Button)typeof(Innersloth.DebugTool.DebugCommandOverlayPage).GetField("executeButton", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(page);
				Debug.Log("[M3] dbgrun invoking execute for " + path);
				exec.onClick.Invoke();
				return true;
			}
			if (action == "status")
			{
				var m = MeetingHud.Instance;
				Debug.Log("[M3] status players=" + (GameData.Instance ? GameData.Instance.PlayerCount : -1) + " meeting=" + (m ? m.state.ToString() + " states=" + (((Array)typeof(MeetingHud).GetField("playerStates", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(m)) is Array a ? a.Length : -1) : "none")
					+ " exile=" + (UnityEngine.Object.FindFirstObjectByType<ExileController>() != null) + " canMove=" + (PlayerControl.LocalPlayer ? PlayerControl.LocalPlayer.CanMove.ToString() : "?")
					+ " bodies=" + UnityEngine.Object.FindObjectsByType<DeadBody>(FindObjectsSortMode.None).Length);
				return true;
			}
			if (action.StartsWith("meetingvote="))
			{
				var m = MeetingHud.Instance;
				if (m) m.CmdCastVote(PlayerControl.LocalPlayer.PlayerId, (byte)int.Parse(action.Substring(12)));
				return true;
			}
			if (action == "meetingproceed") { if (MeetingHud.Instance) MeetingHud.Instance.HandleProceed(); return true; }
			if (action.StartsWith("infoscroll="))
			{
				var sc = MatchInfoGuide.Instance.GetComponentsInChildren<Scroller>(false).FirstOrDefault(x => x.name == "Scroller" && x.Inner != null && x.Inner.name == "SliderInner" && x.Inner.childCount > 0 && x.gameObject.activeInHierarchy);
				Debug.Log("[M3] infoscroll scroller=" + (sc ? sc.name : "none") + " y=" + (sc ? sc.Inner.localPosition.y.ToString() : "") + " bounds=" + (sc ? sc.ContentYBounds.min + ".." + sc.ContentYBounds.max : ""));
				if (sc) { sc.ScrollRelative(new Vector2(0, float.Parse(action.Substring(11)))); Debug.Log("[M3] infoscroll after y=" + sc.Inner.localPosition.y); }
				return true;
			}
			if (action == "infodump")
			{
				var g = MatchInfoGuide.Instance;
				var pool = (ObjectPoolBehavior)typeof(MatchInfoGuide).GetField("PlayerPool", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(g);
				Debug.Log("[M3] infodump pool=" + (pool ? pool.name + " active=" + pool.activeChildren.Count + " parent=" + pool.transform.parent.name + " ppos=" + pool.transform.position : "null"));
				var ma = (SpriteRenderer)typeof(MatchInfoGuide).GetField("matchInfoPlayersMaskArea", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(g);
				Debug.Log("[M3] maskarea " + (ma ? ma.name + " bounds=" + ma.bounds + " active=" + ma.gameObject.activeInHierarchy + " shader=" + ma.sharedMaterial.shader.name + " maskLayer=" + ma.material.GetInt(PlayerMaterial.MaskLayer) : "null"));
				var pfb = pool.Prefab as PlayerIdentifierButton; if (pfb) Debug.Log("[M3] prefab scale=" + pfb.transform.localScale + " preview=" + pfb.PlayerPreview.transform.localScale + " plocal=" + pfb.PlayerPreview.transform.localPosition);
				var f0 = pool.activeChildren.Count > 0 ? pool.activeChildren[0] as PlayerIdentifierButton : null; if (f0) Debug.Log("[M3] entry0 preview scale=" + f0.PlayerPreview.transform.localScale + " plocal=" + f0.PlayerPreview.transform.localPosition + " name=" + f0.NameText.transform.localPosition);
				var first = pool.activeChildren.Count > 0 ? pool.activeChildren[0] : null;
				if (first) foreach (var sr in first.GetComponentsInChildren<SpriteRenderer>(true)) Debug.Log("[M3] esr " + sr.name + " en=" + sr.enabled + " shader=" + (sr.sharedMaterial ? sr.sharedMaterial.shader.name : "null") + " bounds=" + sr.bounds.center + "/" + sr.bounds.extents + " maskLayer=" + (sr.sharedMaterial && sr.sharedMaterial.HasProperty(PlayerMaterial.MaskLayer) ? sr.material.GetInt(PlayerMaterial.MaskLayer).ToString() : "n/a") + " sprite=" + (sr.sprite ? sr.sprite.name : "null") + " col=" + sr.color + " order=" + sr.sortingOrder + " layer=" + sr.sortingLayerName + " mi=" + sr.maskInteraction + " rq=" + (sr.sharedMaterial ? sr.sharedMaterial.renderQueue : 0) + " stencil=" + (sr.sharedMaterial && sr.sharedMaterial.HasProperty("_Stencil") ? sr.material.GetFloat("_Stencil").ToString() : "n/a"));
				for (Transform t = pool.transform; t != null && t != g.transform.parent; t = t.parent) Debug.Log("[M3] chain " + t.name + " local=" + t.localPosition + " world=" + t.position + " scale=" + t.localScale + " comps=" + string.Join(",", t.GetComponents<Component>().Select(c => c ? c.GetType().Name : "null").ToArray()));
				foreach (var pb in pool.activeChildren) { var e = pb as PlayerIdentifierButton; Debug.Log("[M3] entry scale=" + pb.transform.localScale + " lscale=" + pb.transform.lossyScale + " " + pb.name + " activeSelf=" + pb.gameObject.activeSelf + " pos=" + pb.transform.position + " name='" + (e ? e.NameText.text : "") + "'"); }
				foreach (var sc in g.GetComponentsInChildren<Scroller>(true)) Debug.Log("[M3] scroller " + sc.name + " active=" + sc.gameObject.activeInHierarchy + " parent=" + sc.transform.parent.name + " y=" + (sc.Inner ? sc.Inner.localPosition.y.ToString() : "-") + " bounds=" + sc.ContentYBounds.min + ".." + sc.ContentYBounds.max + " hitbox=" + (sc.Hitbox ? sc.Hitbox.bounds.ToString() : "null"));
				return true;
			}
			if (action == "rpcaudit") { HudManager.Instance.StartCoroutine(RpcAudit()); return true; }
			if (action == "tasks")
			{
				var ss = ShipStatus.Instance;
				var me = PlayerControl.LocalPlayer;
				Debug.Log("[M3] tasks map=" + (ss ? ss.Type.ToString() : "none") + " myTasks=" + me.myTasks.Count + " gameData total=" + GameData.Instance.TotalTasks + " completed=" + GameData.Instance.CompletedTasks
					+ " common=" + (ss ? ss.CommonTasks.Length : -1) + " short=" + (ss ? ss.ShortTasks.Length : -1) + " long=" + (ss ? ss.LongTasks.Length : -1) + " consoles=" + UnityEngine.Object.FindObjectsByType<Console>(FindObjectsSortMode.None).Length);
				foreach (var t in me.myTasks)
				{
					var n = t as NormalPlayerTask;
					Debug.Log("[M3] task id=" + t.Id + " type=" + t.TaskType + " step=" + (n ? n.taskStep + "/" + n.MaxStep : "-") + " start=" + t.StartAt + (n ? " data=" + string.Join(",", n.Data) : ""));
				}
				var info = me.Data;
				Debug.Log("[M3] data tasks=" + (info.Tasks != null ? info.Tasks.Count : -1));
				return true;
			}
			if (action == "players")
			{
				foreach (var p in GameData.Instance.AllPlayers)
				{
					var o = p.DefaultOutfit;
					Debug.Log("[M3] player id=" + p.PlayerId + " client=" + p.ClientId + " name=" + p.PlayerName + " color=" + o.ColorId + " hat=" + o.HatId + " skin=" + o.SkinId + " pet=" + o.PetId + " visor=" + o.VisorId + " plate=" + o.NamePlateId + " fc=" + p.FriendCode + " puid=" + p.Puid + " level=" + p.PlayerLevel);
				}
				return true;
			}
			if (action == "killdummy")
			{
				var me = PlayerControl.LocalPlayer;
				var d = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p != me && p.Data != null && !p.Data.IsDead);
				if (!d) { Debug.Log("[M3] killdummy: no target"); return true; }
				me.transform.position = d.transform.position;
				me.CmdCheckMurder(d);
				Debug.Log("[M3] killdummy target=" + d.PlayerId + " isDead=" + d.Data.IsDead);
				return true;
			}
			if (action == "murderdummy")
			{
				var me = PlayerControl.LocalPlayer;
				var d = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p != me && p.Data != null && !p.Data.IsDead);
				if (!d) { Debug.Log("[M3] murderdummy: no target"); return true; }
				me.RpcMurderPlayer(d, true);
				Debug.Log("[M3] murderdummy target=" + d.PlayerId + " isDead=" + d.Data.IsDead);
				return true;
			}
			if (action == "sgopen")
			{
				var me = PlayerControl.LocalPlayer;
				var sg = me.Data.Role as SpiritGuideRole;
				if (sg == null) { Debug.Log("[M3] sgopen: role is " + me.Data.RoleType); return true; }
				var dummy = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p != me);
				var bf = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
				typeof(SpiritGuideRole).GetField("currentTarget", bf)?.SetValue(sg, dummy);
				typeof(SpiritGuideRole).GetMethod("OpenSpiritGuidePanel", bf).Invoke(sg, null);
				var sprites = (System.Collections.Generic.List<Sprite>)typeof(SpiritGuideRole).GetField("possibleSprites", bf).GetValue(sg);
				Debug.Log("[M3] sgopen possibleSprites=" + sprites.Count + " panelActive=" + ((GameObject)typeof(SpiritGuideRole).GetField("spiritGuidePanel", bf).GetValue(sg)).activeSelf + " buttons=" + sg.imageButtons.Count);
				foreach (var b in sg.imageButtons)
				{
					var img = b.spiritGuideImage;
					Debug.Log("[M3] sgbtn " + b.name + " sprite=" + (img.sprite ? img.sprite.name : "null") + " color=" + img.color + " enabled=" + img.enabled + " activeInHier=" + img.gameObject.activeInHierarchy + " scale=" + b.transform.lossyScale + " rect=" + img.rectTransform.rect + " canvas=" + (img.canvas ? img.canvas.name + "/" + img.canvas.renderMode : "none") + " mat=" + (img.material ? img.material.shader.name : "null"));
				}
				return true;
			}
			if (action.StartsWith("hier="))
			{
				string want = action.Substring(5);
				var root = UnityEngine.Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None).FirstOrDefault(t => t.name == want || t.name.StartsWith(want));
				if (!root) { Debug.Log("[HIER] not found " + want); return true; }
				void Dump(Transform t, int depth)
				{
					var rt = t as RectTransform;
					Debug.Log("[HIER] " + new string(' ', depth * 2) + t.name + " act=" + t.gameObject.activeSelf + " comps=" + string.Join(",", t.GetComponents<Component>().Where(c => c && !(c is Transform)).Select(c => c.GetType().Name).ToArray()) + (rt ? " anchors=" + rt.anchorMin + rt.anchorMax + " size=" + rt.rect.size + " pos=" + rt.anchoredPosition : " lpos=" + t.localPosition + " lscale=" + t.localScale));
					foreach (Transform c in t) Dump(c, depth + 1);
				}
				Dump(root, 0);
				return true;
			}
			if (action.StartsWith("uibtn="))
			{
				string want = action.Substring(6);
				var ub = UnityEngine.Object.FindObjectsByType<UnityEngine.UI.Button>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).FirstOrDefault(b => (b.transform.parent ? b.transform.parent.name + "/" : "") + b.name == want || b.name == want);
				Debug.Log("[M3] uibtn " + want + " -> " + (ub ? "click" : "NOT FOUND"));
				if (ub) ub.onClick.Invoke();
				return true;
			}
			if (action.StartsWith("bindkey="))
			{
				AmongUsControlMapper.TestKey = (KeyCode)Enum.Parse(typeof(KeyCode), action.Substring(8));
				return true;
			}
			if (action == "keylog")
			{
				foreach (var bnd in Rewired.KeyMap.Bindable) Debug.Log("[M3] keylog " + bnd.Value + " = " + string.Join(",", Rewired.KeyMap.Positive(bnd.Key)));
				return true;
			}
			if (action == "initgom")
			{
				if (GameOptionsManager.Instance == null) { new GameOptionsManager(new InnerNet.UnityLogger()); GameOptionsManager.Instance.Initialize(); }
				Debug.Log("[M3] initgom " + (GameOptionsManager.Instance != null));
				return true;
			}
			if (action == "fake")
			{
				AmongUsClient.Instance.StartCoroutine(FakePlayerFactory.CoCreate());
				return true;
			}
			if (action == "cubeimg")
			{
				foreach (var cd in DestroyableSingleton<CosmicubeManager>.Instance.GetAllCubeData())
				{
					var r = cd.StoreImageRef;
					var a = r != null ? r.Asset : null;
					Debug.Log("[M3] cubeimg " + cd.ProdId + " guid=" + (r != null ? r.AssetGUID : "null") + " sub=" + (r != null ? r.SubObjectName : "") + " asset=" + (a ? a.GetType().Name + ":" + a.name : "null") + " pod=" + (cd.podIconRef != null && cd.podIconRef.Asset ? cd.podIconRef.Asset.GetType().Name : "null"));
				}
				return true;
			}
			if (action == "hnsmusic")
			{
				foreach (var src in UnityEngine.Object.FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None))
					if (src.gameObject.name.Contains("HnS") || (src.clip && src.clip.name.ToLower().Contains("hns"))) Debug.Log("[M3] hnsmusic " + src.gameObject.name + " clip=" + (src.clip ? src.clip.name : "null") + " playing=" + src.isPlaying + " vol=" + src.volume + " mute=" + src.mute + " group=" + (src.outputAudioMixerGroup ? src.outputAudioMixerGroup.name : "none"));
				var ab = DestroyableSingleton<HudManager>.Instance.AbilityButton;
				Debug.Log("[M3] hnsmusic ability uses text=" + ((TMPro.TextMeshPro)typeof(ActionButton).GetField("usesRemainingText", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ab)).text);
				return true;
			}
			if (action == "emergency")
			{
				var eb = ShipStatus.Instance.EmergencyButton;
				Debug.Log("[M3] emergency sprite=" + (eb.Image.sprite ? eb.Image.sprite.name : "null") + " enabled=" + eb.enabled + " broken=" + (ShipStatus.Instance.BrokenEmergencyButton ? ShipStatus.Instance.BrokenEmergencyButton.name : "null") + " pos=" + eb.transform.position);
				return true;
			}
			if (action == "regionblock")
			{
				var sm = DestroyableSingleton<ServerManager>.Instance;
				Debug.Log("[M3] region before=" + sm.CurrentRegion.Name);
				typeof(ServerManager).GetMethod("SetRegion", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Invoke(sm, new object[] { ServerManager.DefaultRegions[0] });
				Debug.Log("[M3] region after trying North America=" + sm.CurrentRegion.Name);
				return true;
			}
			if (action == "physinfo")
			{
				var ph = PlayerControl.LocalPlayer.MyPhysics;
				Debug.Log("[M3] physinfo Speed=" + ph.Speed + " GhostSpeed=" + ph.GhostSpeed + " SpeedMod=" + ph.SpeedMod + " TrueSpeed=" + ph.TrueSpeed + " dead=" + PlayerControl.LocalPlayer.Data.IsDead);
				return true;
			}
			if (action == "votecheck")
			{
				var mh = MeetingHud.Instance; PlayerVoteArea[] states = mh ? (PlayerVoteArea[])typeof(MeetingHud).GetField("playerStates", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(mh) : null;
				if (!mh) { Debug.Log("[M3] votecheck: no meeting"); return true; }
				Debug.Log("[M3] votecheck state=" + mh.state + " discussionTimer=" + mh.discussionTimer + " me.dead=" + PlayerControl.LocalPlayer.Data.IsDead);
				foreach (var a in states)
				{
					var inf = GameData.Instance.GetPlayerById((byte)a.PlayerId);
					Debug.Log("[M3] votecheck area id=" + (byte)a.PlayerId + " amDead=" + a.AmDead + " dataDead=" + (inf != null && inf.IsDead) + " disc=" + (inf != null && inf.Disconnected) + " btnActive=" + (a.PlayerButton != null && a.PlayerButton.isActiveAndEnabled) + " overlay=" + (a.Overlay != null && a.Overlay.gameObject.activeSelf) + " xmark=" + (a.XMark != null && a.XMark.gameObject.activeSelf));
				}
				foreach (var a in states.Where(x => x.AmDead))
				{
					a.Select();
					var pbtn = a.PlayerButton as PassiveButton;
					if (pbtn != null)
					{
						Debug.Log("[M3] votecheck dead btn calls=" + string.Join(";", Enumerable.Range(0, pbtn.OnClick.GetPersistentEventCount()).Select(i => (pbtn.OnClick.GetPersistentTarget(i) ? pbtn.OnClick.GetPersistentTarget(i).GetType().Name : "null") + "." + pbtn.OnClick.GetPersistentMethodName(i))) + " handleUp=" + pbtn.HandleUp + " handleDown=" + pbtn.HandleDown);
						pbtn.OnClick.Invoke();
					}
					Debug.Log("[M3] votecheck select dead id=" + (byte)a.PlayerId + " -> buttonsShown=" + (a.Buttons != null && a.Buttons.activeSelf));
				}
				return true;
			}
			if (action == "bodyreport")
			{
				var body = UnityEngine.Object.FindFirstObjectByType<DeadBody>();
				if (!body) { Debug.Log("[M3] bodyreport: no body"); return true; }
				PlayerControl.LocalPlayer.CmdReportDeadBody(GameData.Instance.GetPlayerById(body.ParentId));
				return true;
			}
			return false;
		}
		static void Step(string name, Action a)
		{
			try { a(); Debug.Log("[M3RPC] OK " + name); } catch (Exception e) { Debug.Log("[M3RPC] FAIL " + name + " : " + e.GetType().Name + " " + e.Message + " @ " + (e.StackTrace ?? "").Split('\n')[0]); }
		}
		static System.Collections.IEnumerator RpcAudit()
		{
			var me = PlayerControl.LocalPlayer;
			var ship = ShipStatus.Instance;
			var dummy = PlayerControl.AllPlayerControls.ToArray().First(p => p != me && p.Data != null && !p.Data.IsDead);
			Debug.Log("[M3RPC] start me=" + me.PlayerId + " dummy=" + dummy.PlayerId + " doors=" + ship.AllDoors.Length + " vents=" + ship.AllVents.Length);
			Step("RpcSetName", () => me.RpcSetName("RpcTest"));
			yield return new WaitForSeconds(.3f); Debug.Log("[M3RPC] name now=" + me.Data.PlayerName + "/" + me.name);
			Step("RpcSetColor", () => me.RpcSetColor(3)); yield return new WaitForSeconds(.3f); Debug.Log("[M3RPC] color now=" + me.Data.DefaultOutfit.ColorId);
			Step("RpcSetHat", () => me.RpcSetHat("hat_whitetophat")); yield return new WaitForSeconds(.3f); Debug.Log("[M3RPC] hat now=" + me.Data.DefaultOutfit.HatId);
			Step("RpcSetLevel", () => me.RpcSetLevel(7)); yield return new WaitForSeconds(.3f); Debug.Log("[M3RPC] level now=" + me.Data.PlayerLevel);
			Step("RpcSendChat", () => { bool ok = me.RpcSendChat("hello world"); Debug.Log("[M3RPC] chat sent=" + ok); });
			yield return new WaitForSeconds(.5f);
			Step("RpcSetRole Impostor", () => me.RpcSetRole(AmongUs.GameOptions.RoleTypes.Impostor)); yield return new WaitForSeconds(.5f); Debug.Log("[M3RPC] role now=" + me.Data.RoleType + " isImp=" + me.Data.Role.IsImpostor);
			Step("RpcCloseDoorsOfType", () => ship.RpcCloseDoorsOfType(SystemTypes.Cafeteria)); yield return new WaitForSeconds(.5f);
			Debug.Log("[M3RPC] doors closed=" + ship.AllDoors.Count(d => !d.IsOpen) + "/" + ship.AllDoors.Length);
			Step("RpcUpdateSystem Sabotage Lights", () => ship.RpcUpdateSystem(SystemTypes.Sabotage, (byte)SystemTypes.Electrical)); yield return new WaitForSeconds(1f);
			Debug.Log("[M3RPC] sabotage anyActive=" + ((SabotageSystemType)ship.Systems[SystemTypes.Sabotage]).AnyActive);
			Step("RpcUpdateSystem Sabotage Comms", () => ship.RpcUpdateSystem(SystemTypes.Sabotage, (byte)SystemTypes.Comms)); yield return new WaitForSeconds(1f);
			var tk = me.myTasks.Count > 0 ? me.myTasks[0] : null;
			Step("RpcCompleteTask", () => { var before = GameData.Instance.CompletedTasks; me.RpcCompleteTask(tk.Id); Debug.Log("[M3RPC] complete task " + tk.Id + " " + tk.TaskType + " completed " + before + "->" + GameData.Instance.CompletedTasks + " isComplete=" + tk.IsComplete); });
			yield return new WaitForSeconds(.5f); Debug.Log("[M3RPC] tasks completed now=" + GameData.Instance.CompletedTasks + "/" + GameData.Instance.TotalTasks);
			var v0 = ship.AllVents[0];
			Step("RpcEnterVent", () => me.MyPhysics.RpcEnterVent(v0.Id)); yield return new WaitForSeconds(1f); Debug.Log("[M3RPC] inVent=" + me.inVent);
			Step("RpcExitVent", () => me.MyPhysics.RpcExitVent(v0.Id)); yield return new WaitForSeconds(1.2f); Debug.Log("[M3RPC] inVent=" + me.inVent);
			Step("RpcMurderPlayer", () => me.RpcMurderPlayer(dummy, true)); yield return new WaitForSeconds(2f);
			Debug.Log("[M3RPC] dummy dead=" + dummy.Data.IsDead + " bodies=" + UnityEngine.Object.FindObjectsByType<DeadBody>(FindObjectsSortMode.None).Length);
			Step("CmdReportDeadBody", () => me.CmdReportDeadBody(dummy.Data)); yield return new WaitForSeconds(6f);
			Debug.Log("[M3RPC] meeting=" + (MeetingHud.Instance ? MeetingHud.Instance.state.ToString() : "none"));
			Step("CmdCastVote", () => MeetingHud.Instance.CmdCastVote(me.PlayerId, 255)); yield return new WaitForSeconds(1f);
			Step("RpcSendChat(meeting)", () => me.RpcSendChat("meeting chat")); yield return new WaitForSeconds(.5f);
			Debug.Log("[M3RPC] done");
		}
	}
}
#endif
