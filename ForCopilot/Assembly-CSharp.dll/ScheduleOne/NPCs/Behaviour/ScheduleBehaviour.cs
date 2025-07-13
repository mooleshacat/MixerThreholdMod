using System;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200056A RID: 1386
	public class ScheduleBehaviour : Behaviour
	{
		// Token: 0x06002140 RID: 8512 RVA: 0x000896B9 File Offset: 0x000878B9
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.ScheduleBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000896CD File Offset: 0x000878CD
		protected override void Begin()
		{
			base.Begin();
			this.schedule.EnableSchedule();
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x000896E0 File Offset: 0x000878E0
		protected override void Resume()
		{
			base.Resume();
			this.schedule.EnableSchedule();
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x000896F3 File Offset: 0x000878F3
		protected override void Pause()
		{
			base.Pause();
			this.schedule.DisableSchedule();
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x00089706 File Offset: 0x00087906
		protected override void End()
		{
			base.End();
			this.schedule.DisableSchedule();
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x00089719 File Offset: 0x00087919
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.ScheduleBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.ScheduleBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x00089732 File Offset: 0x00087932
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.ScheduleBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.ScheduleBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x0008974B File Offset: 0x0008794B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x00089759 File Offset: 0x00087959
		protected override void dll()
		{
			base.Awake();
		}

		// Token: 0x04001981 RID: 6529
		[Header("References")]
		public NPCScheduleManager schedule;

		// Token: 0x04001982 RID: 6530
		private bool dll_Excuted;

		// Token: 0x04001983 RID: 6531
		private bool dll_Excuted;
	}
}
