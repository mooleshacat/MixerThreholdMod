using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004DE RID: 1246
	public class Elizabeth : NPC
	{
		// Token: 0x06001B5F RID: 7007 RVA: 0x0007585D File Offset: 0x00073A5D
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ElizabethAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ElizabethAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x00075876 File Offset: 0x00073A76
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ElizabethAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ElizabethAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x0007588F File Offset: 0x00073A8F
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x0007589D File Offset: 0x00073A9D
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001707 RID: 5895
		private bool dll_Excuted;

		// Token: 0x04001708 RID: 5896
		private bool dll_Excuted;
	}
}
