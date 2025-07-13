using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200050D RID: 1293
	public class Peter : NPC
	{
		// Token: 0x06001C6D RID: 7277 RVA: 0x00076FF2 File Offset: 0x000751F2
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PeterAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PeterAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x0007700B File Offset: 0x0007520B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PeterAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PeterAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x00077024 File Offset: 0x00075224
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x00077032 File Offset: 0x00075232
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001780 RID: 6016
		private bool dll_Excuted;

		// Token: 0x04001781 RID: 6017
		private bool dll_Excuted;
	}
}
