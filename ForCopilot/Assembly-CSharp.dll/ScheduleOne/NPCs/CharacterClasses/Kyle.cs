using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004FA RID: 1274
	public class Kyle : NPC
	{
		// Token: 0x06001BF7 RID: 7159 RVA: 0x000764C3 File Offset: 0x000746C3
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KyleAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KyleAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000764DC File Offset: 0x000746DC
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KyleAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KyleAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x000764F5 File Offset: 0x000746F5
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x00076503 File Offset: 0x00074703
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400174C RID: 5964
		private bool dll_Excuted;

		// Token: 0x0400174D RID: 5965
		private bool dll_Excuted;
	}
}
