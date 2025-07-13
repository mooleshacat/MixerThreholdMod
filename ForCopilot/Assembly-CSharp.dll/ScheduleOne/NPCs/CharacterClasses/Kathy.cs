using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004F6 RID: 1270
	public class Kathy : NPC
	{
		// Token: 0x06001BE3 RID: 7139 RVA: 0x00076373 File Offset: 0x00074573
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KathyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KathyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x0007638C File Offset: 0x0007458C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KathyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KathyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x000763A5 File Offset: 0x000745A5
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x000763B3 File Offset: 0x000745B3
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001744 RID: 5956
		private bool dll_Excuted;

		// Token: 0x04001745 RID: 5957
		private bool dll_Excuted;
	}
}
