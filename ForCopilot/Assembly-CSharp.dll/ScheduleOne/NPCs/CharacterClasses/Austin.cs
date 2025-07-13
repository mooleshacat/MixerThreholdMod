using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004D5 RID: 1237
	public class Austin : NPC
	{
		// Token: 0x06001B2E RID: 6958 RVA: 0x0007548F File Offset: 0x0007368F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.AustinAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.AustinAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x000754A8 File Offset: 0x000736A8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.AustinAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.AustinAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x000754C1 File Offset: 0x000736C1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x000754CF File Offset: 0x000736CF
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016F2 RID: 5874
		private bool dll_Excuted;

		// Token: 0x040016F3 RID: 5875
		private bool dll_Excuted;
	}
}
