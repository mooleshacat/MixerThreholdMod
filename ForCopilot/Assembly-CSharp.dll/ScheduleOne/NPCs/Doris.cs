using System;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000488 RID: 1160
	public class Doris : NPC
	{
		// Token: 0x060016EB RID: 5867 RVA: 0x0006525B File Offset: 0x0006345B
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.DorisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.DorisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x00065274 File Offset: 0x00063474
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.DorisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.DorisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x0006528D File Offset: 0x0006348D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x0006529B File Offset: 0x0006349B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001516 RID: 5398
		private bool dll_Excuted;

		// Token: 0x04001517 RID: 5399
		private bool dll_Excuted;
	}
}
