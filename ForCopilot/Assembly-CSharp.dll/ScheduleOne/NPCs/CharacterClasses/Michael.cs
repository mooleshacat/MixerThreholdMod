using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000504 RID: 1284
	public class Michael : NPC
	{
		// Token: 0x06001C37 RID: 7223 RVA: 0x00076B4F File Offset: 0x00074D4F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MichaelAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MichaelAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x00076B68 File Offset: 0x00074D68
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MichaelAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MichaelAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x00076B81 File Offset: 0x00074D81
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x00076B8F File Offset: 0x00074D8F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001768 RID: 5992
		private bool dll_Excuted;

		// Token: 0x04001769 RID: 5993
		private bool dll_Excuted;
	}
}
