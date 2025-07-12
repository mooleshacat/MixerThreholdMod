using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004E3 RID: 1251
	public class Genghis : NPC
	{
		// Token: 0x06001B80 RID: 7040 RVA: 0x00075BD0 File Offset: 0x00073DD0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GenghisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GenghisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x00075BE9 File Offset: 0x00073DE9
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GenghisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GenghisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x00075C02 File Offset: 0x00073E02
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x00075C10 File Offset: 0x00073E10
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001719 RID: 5913
		private bool dll_Excuted;

		// Token: 0x0400171A RID: 5914
		private bool dll_Excuted;
	}
}
