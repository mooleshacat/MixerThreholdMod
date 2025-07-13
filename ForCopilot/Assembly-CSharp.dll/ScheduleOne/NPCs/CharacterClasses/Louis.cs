using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004FE RID: 1278
	public class Louis : NPC
	{
		// Token: 0x06001C0E RID: 7182 RVA: 0x00076661 File Offset: 0x00074861
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LouisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LouisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x0007667A File Offset: 0x0007487A
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LouisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LouisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x00076693 File Offset: 0x00074893
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x000766A1 File Offset: 0x000748A1
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001757 RID: 5975
		private bool dll_Excuted;

		// Token: 0x04001758 RID: 5976
		private bool dll_Excuted;
	}
}
