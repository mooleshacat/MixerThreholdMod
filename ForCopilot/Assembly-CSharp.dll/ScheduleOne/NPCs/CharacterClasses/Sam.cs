using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000511 RID: 1297
	public class Sam : NPC
	{
		// Token: 0x06001C85 RID: 7301 RVA: 0x00077255 File Offset: 0x00075455
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.SamAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.SamAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x0007726E File Offset: 0x0007546E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.SamAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.SamAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x00077287 File Offset: 0x00075487
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x00077295 File Offset: 0x00075495
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400178F RID: 6031
		private bool dll_Excuted;

		// Token: 0x04001790 RID: 6032
		private bool dll_Excuted;
	}
}
