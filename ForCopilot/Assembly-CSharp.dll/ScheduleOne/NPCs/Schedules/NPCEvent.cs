using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004B6 RID: 1206
	public class NPCEvent : NPCAction
	{
		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x060019F7 RID: 6647 RVA: 0x00071FBE File Offset: 0x000701BE
		public new string ActionName
		{
			get
			{
				return "Event";
			}
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x00071FC5 File Offset: 0x000701C5
		[Button]
		public void ApplyDuration()
		{
			Debug.Log("Applying duration");
			this.EndTime = TimeManager.AddMinutesTo24HourTime(this.StartTime, this.Duration);
			base.GetComponentInParent<NPCScheduleManager>().InitializeActions();
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x00071FF4 File Offset: 0x000701F4
		[Button]
		public void ApplyEndTime()
		{
			if (this.EndTime > this.StartTime)
			{
				Debug.Log("Set duration");
				this.Duration = TimeManager.GetMinSumFrom24HourTime(this.EndTime) - TimeManager.GetMinSumFrom24HourTime(this.StartTime);
			}
			else
			{
				Debug.Log("Set duration");
				this.Duration = 1440 - TimeManager.GetMinSumFrom24HourTime(this.StartTime) + TimeManager.GetMinSumFrom24HourTime(this.EndTime);
			}
			base.GetComponentInParent<NPCScheduleManager>().InitializeActions();
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x00072070 File Offset: 0x00070270
		public override void ActiveMinPassed()
		{
			base.ActiveMinPassed();
			if (NetworkSingleton<TimeManager>.Instance.CurrentTime == this.GetEndTime())
			{
				this.End();
			}
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x00072090 File Offset: 0x00070290
		public override void PendingMinPassed()
		{
			base.PendingMinPassed();
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x00072098 File Offset: 0x00070298
		public override string GetName()
		{
			return this.ActionName;
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x000720A0 File Offset: 0x000702A0
		public override string GetTimeDescription()
		{
			return TimeManager.Get12HourTime((float)this.StartTime, true) + " - " + TimeManager.Get12HourTime((float)this.GetEndTime(), true);
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x000720C6 File Offset: 0x000702C6
		public override int GetEndTime()
		{
			return TimeManager.AddMinutesTo24HourTime(this.StartTime, this.Duration);
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x000720E9 File Offset: 0x000702E9
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEventAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEventAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x00072102 File Offset: 0x00070302
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEventAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEventAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x0007211B File Offset: 0x0007031B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x00072129 File Offset: 0x00070329
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001672 RID: 5746
		public int Duration = 60;

		// Token: 0x04001673 RID: 5747
		public int EndTime;

		// Token: 0x04001674 RID: 5748
		private bool dll_Excuted;

		// Token: 0x04001675 RID: 5749
		private bool dll_Excuted;
	}
}
