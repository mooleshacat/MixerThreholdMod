using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004FF RID: 1279
	public class Lucy : NPC
	{
		// Token: 0x06001C13 RID: 7187 RVA: 0x000766B5 File Offset: 0x000748B5
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LucyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LucyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x000766CE File Offset: 0x000748CE
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LucyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LucyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x000766E7 File Offset: 0x000748E7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x000766F5 File Offset: 0x000748F5
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001759 RID: 5977
		private bool dll_Excuted;

		// Token: 0x0400175A RID: 5978
		private bool dll_Excuted;
	}
}
