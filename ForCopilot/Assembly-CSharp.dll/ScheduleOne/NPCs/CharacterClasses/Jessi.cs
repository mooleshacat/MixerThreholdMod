using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004F3 RID: 1267
	public class Jessi : NPC
	{
		// Token: 0x06001BD4 RID: 7124 RVA: 0x00076277 File Offset: 0x00074477
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JessiAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JessiAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x00076290 File Offset: 0x00074490
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JessiAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JessiAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x000762A9 File Offset: 0x000744A9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x000762B7 File Offset: 0x000744B7
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400173E RID: 5950
		private bool dll_Excuted;

		// Token: 0x0400173F RID: 5951
		private bool dll_Excuted;
	}
}
