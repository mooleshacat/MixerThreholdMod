using System;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000486 RID: 1158
	public class Chloe : NPC
	{
		// Token: 0x060016E1 RID: 5857 RVA: 0x000651B3 File Offset: 0x000633B3
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.ChloeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.ChloeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x000651CC File Offset: 0x000633CC
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.ChloeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.ChloeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x000651E5 File Offset: 0x000633E5
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x000651F3 File Offset: 0x000633F3
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001512 RID: 5394
		private bool dll_Excuted;

		// Token: 0x04001513 RID: 5395
		private bool dll_Excuted;
	}
}
