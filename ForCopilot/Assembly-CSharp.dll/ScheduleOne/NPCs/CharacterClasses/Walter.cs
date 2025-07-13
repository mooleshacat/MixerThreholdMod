using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000518 RID: 1304
	public class Walter : NPC
	{
		// Token: 0x06001CBD RID: 7357 RVA: 0x000779E4 File Offset: 0x00075BE4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.WalterAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.WalterAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x000779FD File Offset: 0x00075BFD
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.WalterAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.WalterAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x00077A16 File Offset: 0x00075C16
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x00077A24 File Offset: 0x00075C24
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040017A9 RID: 6057
		private bool dll_Excuted;

		// Token: 0x040017AA RID: 6058
		private bool dll_Excuted;
	}
}
