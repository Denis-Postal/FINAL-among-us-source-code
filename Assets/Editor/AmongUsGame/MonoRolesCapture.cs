#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using InnerNet;
using UnityEngine;

namespace AmongUsGameTools
{
    // Editor-only actions: call production role methods, then print observable state.
    // MenuCapture.RunAction can dispatch with: if (MonoRolesCapture.Run(action)) return;
    public static class MonoRolesCapture
    {
        const BindingFlags Fields = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        public static bool Run(string action)
        {
            if (!action.StartsWith("mr")) return false;
            var key = action.Split('=')[0];
            var arg = action.Contains("=") ? action.Substring(action.IndexOf('=') + 1) : "";
            try
            {
                switch (key)
                {
                    case "mrnotes":
                        ((DetectiveRole)PlayerControl.LocalPlayer.Data.Role).UseAbility();
                        break;
                    case "mrvictim":
                    {
                        var role = (DetectiveRole)PlayerControl.LocalPlayer.Data.Role;
                        var body = UnityEngine.Object.FindFirstObjectByType<DeadBody>();
                        if (!body) throw new InvalidOperationException("mrvictim needs a real killed dummy body");
                        var victim = GameData.Instance.GetPlayerById(body.ParentId);
                        typeof(DetectiveRole).GetMethod("AddVictimToNotes", Fields).Invoke(role, new object[] { victim });
                        Debug.Log("[MR] Added actual victim via production AddVictimToNotes id=" + victim.PlayerId + " pages=" + role.notesPageInfos.Count);
                        break;
                    }
                    case "mrinterrogate":
                    {
                        var role = (DetectiveRole)PlayerControl.LocalPlayer.Data.Role;
                        var suspect = PlayerControl.AllPlayerControls.ToArray().First(p => p.isDummy && !p.Data.IsDead);
                        suspect.transform.position = PlayerControl.LocalPlayer.transform.position + Vector3.right * 0.5f;
                        role.SetPlayerTarget(suspect);
                        role.UseSecondaryAbility();
                        Debug.Log("[MR] Interrogate production method suspect=" + suspect.PlayerId + " counts=" + string.Join(",", role.notesPageInfos.Select(p => p.suspects.Count)));
                        break;
                    }
                    case "mrpreposition": Notes().SetVictimPreposition(arg.Length == 0 ? "in" : arg); break;
                    case "mrlocation": Notes().SetVictimLocation(arg.Length == 0 ? "Cafeteria" : arg); break;
                    case "mrimpostor": Notes().SetImpostorPopup(true); break;
                    case "mrpage": Notes().ChangePage(int.Parse(arg)); break;
                    case "mrclose": Notes().Close(); break;
                    case "mrjudge":
                    {
                        var role = (JudgeRole)PlayerControl.LocalPlayer.Data.Role;
                        var target = PlayerControl.AllPlayerControls.ToArray().First(p => p.isDummy && !p.Data.IsDead);
                        Debug.Log("[MR] Judge blocked=" + role.IsBlockedByTasks() + " use=" + role.HasAnOverruleUse + " meeting=" + (bool)MeetingHud.Instance);
                        var accepted = role.TryOverrule((PlayerId)target.PlayerId);
                        Debug.Log("[MR] Judge TryOverrule=" + accepted + " target=" + role.OverruledPlayerId + " used=" + role.HasAlreadyOverruledThisMeeting + " nonce=" + role.OverruleNonce);
                        break;
                    }
                    case "mrstate": State(); break;
                    default: return false;
                }
            }
            catch (Exception e) { Debug.LogException(e); }
            return true;
        }
        static DetectiveNotesMinigame Notes() { return UnityEngine.Object.FindFirstObjectByType<DetectiveNotesMinigame>(); }
        static void State()
        {
            var local = PlayerControl.LocalPlayer;
            Debug.Log("[MR] Role=" + local.Data.Role.Role + " canMove=" + local.CanMove + " meeting=" + (bool)MeetingHud.Instance);
            var detective = local.Data.Role as DetectiveRole;
            if (detective)
                foreach (var page in detective.notesPageInfos)
                    Debug.Log("[MR] Page victim=" + page.victimPlayer.PlayerName + " suspects=" + page.suspects.Count + " location=" + page.locationName + " preposition=" + page.prepositionStringName);
            var notes = Notes();
            if (!notes) return;
            foreach (var text in notes.GetComponentsInChildren<TMPro.TMP_Text>())
                Debug.Log("[MR] Note text " + text.name + "=" + text.text);
            var tabs = (System.Collections.IList)typeof(DetectiveNotesMinigame).GetField("tabs", Fields).GetValue(notes);
            Debug.Log("[MR] Notes tabs=" + tabs.Count + " materials=" + notes.bodyColorMaterials.Count);
        }
    }
}
#endif
