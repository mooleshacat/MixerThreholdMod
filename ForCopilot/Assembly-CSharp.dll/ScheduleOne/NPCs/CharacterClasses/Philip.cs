using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200050E RID: 1294
	public class Philip : NPC
	{
		// Token: 0x06001C72 RID: 7282 RVA: 0x00077046 File Offset: 0x00075246
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PhilipAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PhilipAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x0007705F File Offset: 0x0007525F
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PhilipAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PhilipAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x00077078 File Offset: 0x00075278
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x00077086 File Offset: 0x00075286
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001782 RID: 6018
		private bool dll_Excuted;

		// Token: 0x04001783 RID: 6019
		private bool dll_Excuted;
	}
}
