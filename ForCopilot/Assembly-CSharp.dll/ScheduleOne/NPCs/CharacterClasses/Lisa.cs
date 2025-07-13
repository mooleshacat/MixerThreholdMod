using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004FD RID: 1277
	public class Lisa : NPC
	{
		// Token: 0x06001C09 RID: 7177 RVA: 0x0007660D File Offset: 0x0007480D
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LisaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LisaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x00076626 File Offset: 0x00074826
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LisaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LisaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x0007663F File Offset: 0x0007483F
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x0007664D File Offset: 0x0007484D
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001755 RID: 5973
		private bool dll_Excuted;

		// Token: 0x04001756 RID: 5974
		private bool dll_Excuted;
	}
}
