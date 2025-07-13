using System;

namespace ScheduleOne.NPCs
{
	// Token: 0x0200048A RID: 1162
	public class Meg : NPC
	{
		// Token: 0x060016F5 RID: 5877 RVA: 0x00065303 File Offset: 0x00063503
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.MegAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.MegAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x0006531C File Offset: 0x0006351C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.MegAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.MegAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00065335 File Offset: 0x00063535
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x00065343 File Offset: 0x00063543
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400151A RID: 5402
		private bool dll_Excuted;

		// Token: 0x0400151B RID: 5403
		private bool dll_Excuted;
	}
}
