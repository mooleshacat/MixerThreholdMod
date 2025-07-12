using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000500 RID: 1280
	public class Ludwig : NPC
	{
		// Token: 0x06001C18 RID: 7192 RVA: 0x00076709 File Offset: 0x00074909
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LudwigAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LudwigAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x00076722 File Offset: 0x00074922
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LudwigAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LudwigAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x0007673B File Offset: 0x0007493B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x00076749 File Offset: 0x00074949
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400175B RID: 5979
		private bool dll_Excuted;

		// Token: 0x0400175C RID: 5980
		private bool dll_Excuted;
	}
}
