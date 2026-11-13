using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using InnerNet;
using UnityEngine;

public class JudgeRole : CrewmateRole
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Func<PlayerTask, bool> _003C_003E9__26_0;

		public static Func<PlayerTask, bool> _003C_003E9__29_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003CIsBlockedByTasks_003Eb__26_0(PlayerTask task)
		{
			return task.IsComplete;
		}

		internal bool _003CTasksRemaining_003Eb__29_0(PlayerTask task)
		{
			return task.IsComplete;
		}
	}

	private sealed class _003CCoSetInitialTaskCount_003Ed__22 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		public JudgeRole _003C_003E4__this;

		private float _003Ctimeout_003E5__2;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CCoSetInitialTaskCount_003Ed__22(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			if (_003C_003E1__state == 0)
			{
				_003Ctimeout_003E5__2 = 10f;
			}
			else
			{
				if (_003C_003E1__state != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003Ctimeout_003E5__2 -= Time.deltaTime;
			}
			_003C_003E1__state = -1;
			if (_003C_003E4__this.Player.myTasks.Count == 0 && _003Ctimeout_003E5__2 > 0f)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this.initialTaskCount = _003C_003E4__this.Player.myTasks.Count;
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		void IEnumerator.Reset()
		{
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			NotSupportedException ex = new NotSupportedException();
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			throw ex;
		}
	}

	private const ushort UNINITIALIZED_OVERRULE_NONCE = 0;

	private const ushort MINIMUM_OVERRULE_NONCE = 1;

	private const ushort MAXIMUM_OVERRULE_NONCE = ushort.MaxValue;

	private float taskRequirementProportion;

	private float initialTaskCount;

	public PlayerId OverruledPlayerId { get; private set; } = PlayerId.Invalid;

	public ushort OverruleNonce { get; private set; }

	public bool HasAlreadyOverruledThisMeeting { get; private set; }

	public bool HasAnOverruleUse { get; private set; } = true;

	public override void Initialize(PlayerControl player)
	{
		base.Initialize(player);
		StartCoroutine("CoSetInitialTaskCount");
		taskRequirementProportion = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.JudgeTaskRequirementPercentage) / 100f;
	}

	private IEnumerator CoSetInitialTaskCount()
	{
		float timeout = 10f;
		while (Player.myTasks.Count == 0 || timeout < 0f)
		{
			timeout -= Time.deltaTime;
			yield return null;
		}
		initialTaskCount = Player.myTasks.Count;
	}

	public bool TryOverrule(PlayerId overruledPlayerId)
	{
		if (!Player.AmOwner)
		{
			return false;
		}
		if (HasAlreadyOverruledThisMeeting)
		{
			Debug.LogWarning("Attempt to overrule when already having overruled this meeting!");
			return false;
		}
		OverruledPlayerId = overruledPlayerId;
		HasAlreadyOverruledThisMeeting = true;
		if (OverruleNonce == 0)
		{
			OverruleNonce = (ushort)UnityEngine.Random.Range(1, 65535);
		}
		MeetingHud.Instance.SetVoteComplete(complete: true, clearButtons: true);
		MeetingHud.Instance.ClearJudgeUsesIndicator();
		MeetingHud.Instance.CmdQueueOverruleVotes(Player.PlayerId, overruledPlayerId, OverruleNonce);
		return true;
	}

	public void ClearOverrule()
	{
		PlayerId overruledPlayerId = OverruledPlayerId;
		if ((byte)overruledPlayerId != (byte)PlayerId.Invalid)
		{
			OverruledPlayerId = PlayerId.Invalid;
			HasAlreadyOverruledThisMeeting = false;
			MeetingHud.Instance.RemoveJudgeUsedLabel(overruledPlayerId);
		}
	}

	public void ConsumeOverruleVotesUsage()
	{
		HasAnOverruleUse = false;
	}

	public bool IsBlockedByTasks()
	{
		float num = initialTaskCount;
		return (float)Player.myTasks.Count((PlayerTask task) => task.IsComplete) / num < taskRequirementProportion;
	}

	public override void AppendTaskHint(StringBuilder taskStringBuilder)
	{
		int num = TasksRemaining();
		if (!HasAnOverruleUse)
		{
			RoleBehaviour.AppendTaskHint(this, taskStringBuilder);
			return;
		}
		if (num == 0)
		{
			RoleBehaviour.AppendTaskHint(this, taskStringBuilder.Append("<br>" + DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.JudgeAbilityUnlocked)));
			return;
		}
		RoleBehaviour.AppendTaskHint(this, taskStringBuilder.Append("<br>" + DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.JudgeAbilityProgress, TasksRemaining())));
	}

	public override void OnMeetingStart()
	{
		if (Player.AmOwner)
		{
			base.OnMeetingStart();
			HasAlreadyOverruledThisMeeting = false;
			OverruleNonce = 0;
			OverruledPlayerId = PlayerId.Invalid;
			MeetingHud.Instance.UpdateJudgeAbilityIndicator(HasAnOverruleUse, TasksRemaining());
		}
	}

	private int TasksRemaining()
	{
		float num = initialTaskCount;
		int num2 = Player.myTasks.Count((PlayerTask task) => task.IsComplete);
		return Mathf.Clamp(Mathf.CeilToInt(taskRequirementProportion * num) - num2, 0, 999);
	}

	public JudgeRole()
	{ }
}
