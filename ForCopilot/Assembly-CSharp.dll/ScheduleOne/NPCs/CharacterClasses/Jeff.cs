using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004EE RID: 1262
	public class Jeff : NPC
	{
		// Token: 0x06001BB9 RID: 7097 RVA: 0x00075FE7 File Offset: 0x000741E7
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JeffAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JeffAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x00076000 File Offset: 0x00074200
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JeffAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JeffAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x00076019 File Offset: 0x00074219
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x00076027 File Offset: 0x00074227
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001731 RID: 5937
		private bool dll_Excuted;

		// Token: 0x04001732 RID: 5938
		private bool dll_Excuted;
	}
}
