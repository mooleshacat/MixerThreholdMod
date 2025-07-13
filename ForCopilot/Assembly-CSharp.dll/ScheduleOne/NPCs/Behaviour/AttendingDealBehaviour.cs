using System;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200051E RID: 1310
	public class AttendingDealBehaviour : Behaviour
	{
		// Token: 0x06001CE1 RID: 7393 RVA: 0x00077DCE File Offset: 0x00075FCE
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.AttendingDealBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.AttendingDealBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x00077DE7 File Offset: 0x00075FE7
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.AttendingDealBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.AttendingDealBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x00077E00 File Offset: 0x00076000
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x00077E0E File Offset: 0x0007600E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040017B6 RID: 6070
		private bool dll_Excuted;

		// Token: 0x040017B7 RID: 6071
		private bool dll_Excuted;
	}
}
