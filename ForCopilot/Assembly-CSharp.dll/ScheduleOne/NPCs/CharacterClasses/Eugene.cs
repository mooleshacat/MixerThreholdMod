using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004DF RID: 1247
	public class Eugene : NPC
	{
		// Token: 0x06001B64 RID: 7012 RVA: 0x000758B1 File Offset: 0x00073AB1
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.EugeneAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.EugeneAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x000758CA File Offset: 0x00073ACA
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.EugeneAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.EugeneAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x000758E3 File Offset: 0x00073AE3
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x000758F1 File Offset: 0x00073AF1
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001709 RID: 5897
		private bool dll_Excuted;

		// Token: 0x0400170A RID: 5898
		private bool dll_Excuted;
	}
}
