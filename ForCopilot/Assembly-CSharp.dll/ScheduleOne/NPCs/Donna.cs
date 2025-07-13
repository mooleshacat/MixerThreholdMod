using System;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000487 RID: 1159
	public class Donna : NPC
	{
		// Token: 0x060016E6 RID: 5862 RVA: 0x00065207 File Offset: 0x00063407
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.DonnaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.DonnaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00065220 File Offset: 0x00063420
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.DonnaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.DonnaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00065239 File Offset: 0x00063439
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00065247 File Offset: 0x00063447
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001514 RID: 5396
		private bool dll_Excuted;

		// Token: 0x04001515 RID: 5397
		private bool dll_Excuted;
	}
}
