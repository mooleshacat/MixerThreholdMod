using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000516 RID: 1302
	public class Trent : NPC
	{
		// Token: 0x06001CB2 RID: 7346 RVA: 0x000778A4 File Offset: 0x00075AA4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.TrentAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.TrentAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x000778BD File Offset: 0x00075ABD
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.TrentAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.TrentAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x000778D6 File Offset: 0x00075AD6
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x000778E4 File Offset: 0x00075AE4
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040017A3 RID: 6051
		private bool dll_Excuted;

		// Token: 0x040017A4 RID: 6052
		private bool dll_Excuted;
	}
}
