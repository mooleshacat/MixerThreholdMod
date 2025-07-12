using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004EB RID: 1259
	public class Jackie : NPC
	{
		// Token: 0x06001BAA RID: 7082 RVA: 0x00075EEB File Offset: 0x000740EB
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JackieAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JackieAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x00075F04 File Offset: 0x00074104
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JackieAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JackieAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x00075F1D File Offset: 0x0007411D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x00075F2B File Offset: 0x0007412B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400172B RID: 5931
		private bool dll_Excuted;

		// Token: 0x0400172C RID: 5932
		private bool dll_Excuted;
	}
}
