using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004F7 RID: 1271
	public class Keith : NPC
	{
		// Token: 0x06001BE8 RID: 7144 RVA: 0x000763C7 File Offset: 0x000745C7
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KeithAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KeithAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x000763E0 File Offset: 0x000745E0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KeithAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KeithAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x000763F9 File Offset: 0x000745F9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x00076407 File Offset: 0x00074607
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001746 RID: 5958
		private bool dll_Excuted;

		// Token: 0x04001747 RID: 5959
		private bool dll_Excuted;
	}
}
