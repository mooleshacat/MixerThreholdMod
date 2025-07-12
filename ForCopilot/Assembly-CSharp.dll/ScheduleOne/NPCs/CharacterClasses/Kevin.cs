using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004F8 RID: 1272
	public class Kevin : NPC
	{
		// Token: 0x06001BED RID: 7149 RVA: 0x0007641B File Offset: 0x0007461B
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KevinAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KevinAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x00076434 File Offset: 0x00074634
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KevinAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KevinAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x0007644D File Offset: 0x0007464D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x0007645B File Offset: 0x0007465B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001748 RID: 5960
		private bool dll_Excuted;

		// Token: 0x04001749 RID: 5961
		private bool dll_Excuted;
	}
}
