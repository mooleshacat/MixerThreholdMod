using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.Misc;
using ScheduleOne.NPCs.CharacterClasses;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x02000306 RID: 774
	public class Quest_UnfavourableAgreements : Quest
	{
		// Token: 0x06001144 RID: 4420 RVA: 0x0004CAC8 File Offset: 0x0004ACC8
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onHourPass = (Action)Delegate.Combine(instance.onHourPass, new Action(this.HourPass));
			this.Thomas.onCartelContractReceived.AddListener(new UnityAction(this.HandoverCompleted));
			Singleton<SleepCanvas>.Instance.onSleepEndFade.AddListener(new UnityAction(this.CheckHandoverExpiry));
			this.UpdateName();
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x0004CB3E File Offset: 0x0004AD3E
		public override void Begin(bool network = true)
		{
			base.Begin(network);
			this.ResetTimer(false);
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x0004CB50 File Offset: 0x0004AD50
		private void HourPass()
		{
			float num = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Hours_Since_Cartel_Handover");
			float num2 = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Hours_Until_CartelContract_Due");
			if (this.Entries[0].State == EQuestState.Active)
			{
				num += 1f;
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Hours_Since_Cartel_Handover", num.ToString(), true);
				num2 -= 1f;
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Hours_Until_CartelContract_Due", num2.ToString(), true);
				this.UpdateName();
			}
			if (!this.handoverSetup && num >= 12f)
			{
				this.SetupHandover();
			}
			if (!this.Thomas.HandoverReminderSent && num2 <= 24f)
			{
				this.Thomas.SendHandoverReminder();
			}
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x0004CC0B File Offset: 0x0004AE0B
		private void SetupHandover()
		{
			this.handoverSetup = true;
			Debug.Log("Setting up handover");
			this.Gate.ActivateIntercom();
			this.Switch.SwitchOn();
			this.Thomas.SetHandoverEventActive(true);
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x0004CC40 File Offset: 0x0004AE40
		private void CheckHandoverExpiry()
		{
			if (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Hours_Until_CartelContract_Due") <= 0f)
			{
				Singleton<SleepCanvas>.Instance.QueueSleepMessage("You have failed to make the weekly delivery. Benzies family goons break in during the night, taking your stock and leaving you nearly dead.", 5f);
				this.RV.Ransack();
				this.ResetTimer(false);
				Player.Local.Health.SetHealth(65f);
			}
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x0004CCA0 File Offset: 0x0004AEA0
		private void UpdateName()
		{
			int num = Mathf.FloorToInt(NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Hours_Until_CartelContract_Due") / 24f);
			string str;
			if (num == -1)
			{
				str = string.Empty;
			}
			else if (num == 0)
			{
				str = "(due today)";
			}
			else if (num == 1)
			{
				str = "(" + num.ToString() + " day)";
			}
			else
			{
				str = "(" + num.ToString() + " days)";
			}
			this.Entries[0].SetEntryTitle(this.QuestEntryTitle + " " + str);
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0004CD35 File Offset: 0x0004AF35
		private void HandoverCompleted()
		{
			this.ResetTimer(true);
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0004CD40 File Offset: 0x0004AF40
		public void ResetTimer(bool allowBuildup)
		{
			float num = Mathf.Floor((float)TimeManager.GetMinSumFrom24HourTime(NetworkSingleton<TimeManager>.Instance.CurrentTime) / 60f);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Hours_Since_Cartel_Handover", num.ToString(), true);
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Hours_Until_CartelContract_Due");
			float num2 = 168f;
			if (allowBuildup)
			{
				num2 += value;
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Hours_Until_CartelContract_Due", num2.ToString(), true);
			this.UpdateName();
		}

		// Token: 0x04001132 RID: 4402
		public const float WEEKLY_DELIVERY_HOURS = 168f;

		// Token: 0x04001133 RID: 4403
		public const float REMINDER_THRESHOLD = 144f;

		// Token: 0x04001134 RID: 4404
		public Thomas Thomas;

		// Token: 0x04001135 RID: 4405
		public ManorGate Gate;

		// Token: 0x04001136 RID: 4406
		public ModularSwitch Switch;

		// Token: 0x04001137 RID: 4407
		public RV RV;

		// Token: 0x04001138 RID: 4408
		public string QuestEntryTitle;

		// Token: 0x04001139 RID: 4409
		private bool handoverSetup;
	}
}
