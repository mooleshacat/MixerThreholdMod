using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.Misc;
using ScheduleOne.NPCs.CharacterClasses;
using ScheduleOne.PlayerScripts;
using ScheduleOne.ScriptableObjects;
using ScheduleOne.UI;
using ScheduleOne.UI.Phone;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x02000303 RID: 771
	public class Quest_TheDeepEnd : Quest
	{
		// Token: 0x06001131 RID: 4401 RVA: 0x0004C758 File Offset: 0x0004A958
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onHourPass = (Action)Delegate.Combine(instance.onHourPass, new Action(this.HourPass));
			TimeManager.onSleepStart = (Action)Delegate.Combine(TimeManager.onSleepStart, new Action(this.BeforeSleep));
			Singleton<SleepCanvas>.Instance.onSleepEndFade.AddListener(new UnityAction(this.SleepFadeOut));
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x0004C7CC File Offset: 0x0004A9CC
		public override void Begin(bool network = true)
		{
			base.Begin(network);
			this.SetupFirstMeeting();
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x0004C7DC File Offset: 0x0004A9DC
		public void SetupFirstMeeting()
		{
			this.meetingSetup = true;
			this.Gate.ActivateIntercom();
			this.Switch.SwitchOn();
			this.Thomas.SetFirstMeetingEventActive(true);
			this.Thomas.dialogueHandler.onDialogueNodeDisplayed.AddListener(new UnityAction<string>(this.ThomasDialogueNodeDisplayed));
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x0004C834 File Offset: 0x0004AA34
		private void ThomasDialogueNodeDisplayed(string nodeLabel)
		{
			if (nodeLabel == "THOMAS_INTRO_DONE")
			{
				Debug.Log("Intro meeting done!");
				this.Gate.SetEnterable(false);
				this.Thomas.InitialMeetingComplete();
				this.Entries[0].SetState(EQuestState.Completed, true);
				this.Entries[1].SetState(EQuestState.Active, true);
				this.PostMeetingTrigger.Trigger();
				base.StartCoroutine(this.<ThomasDialogueNodeDisplayed>g__Wait|13_0());
			}
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x0004C8B0 File Offset: 0x0004AAB0
		private void HourPass()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (Quest.GetQuest("Sink or Swim").QuestState != EQuestState.Completed)
			{
				return;
			}
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Hours_Since_LoanSharks_Arrived");
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Hours_Since_LoanSharks_Arrived", (value + 1f).ToString(), true);
			if (this.Entries[0].State != EQuestState.Completed && value >= 36f && !this.Thomas.MeetingReminderSent)
			{
				this.Thomas.SendMeetingReminder();
				if (base.QuestState == EQuestState.Inactive)
				{
					this.Begin(true);
				}
			}
			if (this.Entries[0].State == EQuestState.Active && value >= 82f && !this.kidnapQueued)
			{
				this.kidnapQueued = true;
			}
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x0004C975 File Offset: 0x0004AB75
		private void BeforeSleep()
		{
			if (this.kidnapQueued)
			{
				Singleton<SleepCanvas>.Instance.QueueSleepMessage("In the middle of the night, the door is kicked in and you are dragged into a vehicle trunk...", 3f);
			}
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x0004C993 File Offset: 0x0004AB93
		private void SleepFadeOut()
		{
			if (this.kidnapQueued)
			{
				this.kidnapQueued = false;
				PlayerSingleton<PlayerMovement>.Instance.Teleport(this.MeetingTeleportPoint.position);
				Player.Local.transform.forward = this.MeetingTeleportPoint.forward;
			}
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0004C9D3 File Offset: 0x0004ABD3
		public override void SetQuestEntryState(int entryIndex, EQuestState state, bool network = true)
		{
			base.SetQuestEntryState(entryIndex, state, network);
			if (this.Entries[0].State == EQuestState.Active && !this.meetingSetup)
			{
				this.SetupFirstMeeting();
			}
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0004CA00 File Offset: 0x0004AC00
		[CompilerGenerated]
		private IEnumerator <ThomasDialogueNodeDisplayed>g__Wait|13_0()
		{
			yield return new WaitUntil(() => Player.Local.CurrentProperty == null);
			Singleton<CallInterface>.Instance.StartCall(this.PostMeetingCall, this.PostMeetingCall.CallerID, 0);
			yield break;
		}

		// Token: 0x04001123 RID: 4387
		public const float MEETING_REMINDER_TIME = 36f;

		// Token: 0x04001124 RID: 4388
		public const float KIDNAP_TIME = 82f;

		// Token: 0x04001125 RID: 4389
		private bool kidnapQueued;

		// Token: 0x04001126 RID: 4390
		private bool meetingSetup;

		// Token: 0x04001127 RID: 4391
		public Thomas Thomas;

		// Token: 0x04001128 RID: 4392
		public ManorGate Gate;

		// Token: 0x04001129 RID: 4393
		public ModularSwitch Switch;

		// Token: 0x0400112A RID: 4394
		public Transform MeetingTeleportPoint;

		// Token: 0x0400112B RID: 4395
		public PhoneCallData PostMeetingCall;

		// Token: 0x0400112C RID: 4396
		public SystemTriggerObject PostMeetingTrigger;
	}
}
