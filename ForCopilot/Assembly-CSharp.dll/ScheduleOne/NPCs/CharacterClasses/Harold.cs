using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004E7 RID: 1255
	public class Harold : NPC
	{
		// Token: 0x06001B94 RID: 7060 RVA: 0x00075D20 File Offset: 0x00073F20
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.HaroldAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.HaroldAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x00075D39 File Offset: 0x00073F39
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.HaroldAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.HaroldAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x00075D52 File Offset: 0x00073F52
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x00075D60 File Offset: 0x00073F60
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001721 RID: 5921
		private bool dll_Excuted;

		// Token: 0x04001722 RID: 5922
		private bool dll_Excuted;
	}
}
