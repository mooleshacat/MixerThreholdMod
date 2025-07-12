using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004B7 RID: 1207
	public class NPCSignal : NPCAction
	{
		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001A04 RID: 6660 RVA: 0x0007213D File Offset: 0x0007033D
		public new string ActionName
		{
			get
			{
				return "Signal";
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x00072144 File Offset: 0x00070344
		// (set) Token: 0x06001A06 RID: 6662 RVA: 0x0007214C File Offset: 0x0007034C
		public bool StartedThisCycle { get; protected set; }

		// Token: 0x06001A07 RID: 6663 RVA: 0x00072155 File Offset: 0x00070355
		public override string GetName()
		{
			return this.ActionName;
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x0007215D File Offset: 0x0007035D
		public override void ActiveUpdate()
		{
			base.ActiveUpdate();
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x00072165 File Offset: 0x00070365
		public override string GetTimeDescription()
		{
			return TimeManager.Get12HourTime((float)this.StartTime, true);
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00072174 File Offset: 0x00070374
		public override int GetEndTime()
		{
			return TimeManager.AddMinutesTo24HourTime(this.StartTime, this.MaxDuration);
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x00072187 File Offset: 0x00070387
		public override void Started()
		{
			base.Started();
			this.StartedThisCycle = true;
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x00072196 File Offset: 0x00070396
		public override void LateStarted()
		{
			base.LateStarted();
			this.StartedThisCycle = true;
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x000721A5 File Offset: 0x000703A5
		public override bool ShouldStart()
		{
			return !this.StartedThisCycle && base.ShouldStart();
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x000721B7 File Offset: 0x000703B7
		public override void Interrupt()
		{
			this.StartedThisCycle = false;
			base.Interrupt();
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x000721C6 File Offset: 0x000703C6
		public override void MinPassed()
		{
			base.MinPassed();
			if (this.StartedThisCycle && !NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.GetEndTime()))
			{
				this.StartedThisCycle = false;
			}
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x00072205 File Offset: 0x00070405
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignalAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignalAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0007221E File Offset: 0x0007041E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignalAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignalAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x00072237 File Offset: 0x00070437
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x00072245 File Offset: 0x00070445
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001676 RID: 5750
		public int MaxDuration = 60;

		// Token: 0x04001678 RID: 5752
		private bool dll_Excuted;

		// Token: 0x04001679 RID: 5753
		private bool dll_Excuted;
	}
}
