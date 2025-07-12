using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x02000302 RID: 770
	public class Quest_SinkOrSwim : Quest
	{
		// Token: 0x06001127 RID: 4391 RVA: 0x0004C44B File Offset: 0x0004A64B
		protected override void Awake()
		{
			base.Awake();
			this.LoanSharkGraves.gameObject.SetActive(false);
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0004C464 File Offset: 0x0004A664
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onHourPass = (Action)Delegate.Combine(instance.onHourPass, new Action(this.HourPass));
			Singleton<SleepCanvas>.Instance.onSleepEndFade.AddListener(new UnityAction(this.SleepStart));
			Singleton<SleepCanvas>.Instance.onSleepEndFade.AddListener(new UnityAction(this.CheckArrival));
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.UpdateName));
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.UpdateName));
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0004C1FB File Offset: 0x0004A3FB
		protected override void MinPass()
		{
			base.MinPass();
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0004C509 File Offset: 0x0004A709
		private void HourPass()
		{
			if (this.Entries[0].State == EQuestState.Active)
			{
				this.UpdateName();
			}
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0004C528 File Offset: 0x0004A728
		private void SleepStart()
		{
			if (this.Entries[0].State == EQuestState.Active)
			{
				float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Days_Since_Tutorial_Completed");
				int num = 4 - (int)value;
				if (num == -1)
				{
					Singleton<SleepCanvas>.Instance.QueueSleepMessage("In the midst of night gunshots ring out nearby, but you are not the target. The rest of the night is quiet.", 5f);
					return;
				}
				if (num == 0)
				{
					Singleton<SleepCanvas>.Instance.QueueSleepMessage("The loan sharks are arriving tonight.", 4f);
					return;
				}
				if (num == 1)
				{
					Singleton<SleepCanvas>.Instance.QueueSleepMessage(num.ToString() + " day until the loan sharks arrive", 3f);
					return;
				}
				Singleton<SleepCanvas>.Instance.QueueSleepMessage(num.ToString() + " days until the loan sharks arrive", 3f);
			}
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0004C5D7 File Offset: 0x0004A7D7
		private void SpawnLoanSharkVehicle()
		{
			NetworkSingleton<VehicleManager>.Instance.SpawnLoanSharkVehicle(this.LoanSharkVehiclePosition.position, this.LoanSharkVehiclePosition.rotation);
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x0004C5FC File Offset: 0x0004A7FC
		private void CheckArrival()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("Loan_Sharks_Arrived"))
			{
				return;
			}
			if (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Days_Since_Tutorial_Completed") > 4f)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Loan_Sharks_Arrived", true.ToString(), true);
				this.SpawnLoanSharkVehicle();
				this.LoanSharkGraves.gameObject.SetActive(true);
				this.Entries[this.Entries.Count - 1].SetState(EQuestState.Completed, true);
			}
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x0004C688 File Offset: 0x0004A888
		public override void SetQuestState(EQuestState state, bool network = true)
		{
			base.SetQuestState(state, network);
			this.LoanSharkGraves.gameObject.SetActive(state == EQuestState.Completed);
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x0004C6A8 File Offset: 0x0004A8A8
		private void UpdateName()
		{
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Days_Since_Tutorial_Completed");
			int num = 4 - (int)value;
			string str;
			if (num == -1)
			{
				str = string.Empty;
			}
			else if (num == 0)
			{
				str = "(arriving tonight)";
			}
			else if (num == 1)
			{
				str = "(" + num.ToString() + " day remaining)";
			}
			else
			{
				str = "(" + num.ToString() + " days remaining)";
			}
			this.Entries[0].SetEntryTitle(this.QuestName + " " + str);
		}

		// Token: 0x0400111E RID: 4382
		public const int DAYS_TO_COMPLETE = 4;

		// Token: 0x0400111F RID: 4383
		public string QuestName = "Make at least $1,000 to pay off the sharks";

		// Token: 0x04001120 RID: 4384
		public int NelsonCallTime = 1215;

		// Token: 0x04001121 RID: 4385
		public Transform LoanSharkVehiclePosition;

		// Token: 0x04001122 RID: 4386
		public GameObject LoanSharkGraves;
	}
}
