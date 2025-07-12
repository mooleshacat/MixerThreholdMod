using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000501 RID: 1281
	public class Mac : NPC
	{
		// Token: 0x06001C1D RID: 7197 RVA: 0x0007675D File Offset: 0x0007495D
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MacAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MacAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x00076776 File Offset: 0x00074976
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MacAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MacAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x0007678F File Offset: 0x0007498F
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x0007679D File Offset: 0x0007499D
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400175D RID: 5981
		private bool dll_Excuted;

		// Token: 0x0400175E RID: 5982
		private bool dll_Excuted;
	}
}
