using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004DA RID: 1242
	public class Chris : NPC
	{
		// Token: 0x06001B47 RID: 6983 RVA: 0x0007563B File Offset: 0x0007383B
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ChrisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ChrisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x00075654 File Offset: 0x00073854
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ChrisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ChrisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x0007566D File Offset: 0x0007386D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x0007567B File Offset: 0x0007387B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016FC RID: 5884
		private bool dll_Excuted;

		// Token: 0x040016FD RID: 5885
		private bool dll_Excuted;
	}
}
