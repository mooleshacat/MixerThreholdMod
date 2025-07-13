using System;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000485 RID: 1157
	public class Billy : NPC
	{
		// Token: 0x060016DC RID: 5852 RVA: 0x0006515F File Offset: 0x0006335F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.BillyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.BillyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x00065178 File Offset: 0x00063378
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.BillyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.BillyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x00065191 File Offset: 0x00063391
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x0006519F File Offset: 0x0006339F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001510 RID: 5392
		private bool dll_Excuted;

		// Token: 0x04001511 RID: 5393
		private bool dll_Excuted;
	}
}
