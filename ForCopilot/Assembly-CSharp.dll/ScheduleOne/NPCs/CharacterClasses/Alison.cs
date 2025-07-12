using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004D3 RID: 1235
	public class Alison : NPC
	{
		// Token: 0x06001B22 RID: 6946 RVA: 0x000753AE File Offset: 0x000735AE
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.AlisonAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.AlisonAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x000753C7 File Offset: 0x000735C7
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.AlisonAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.AlisonAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x000753E0 File Offset: 0x000735E0
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x000753EE File Offset: 0x000735EE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016EE RID: 5870
		private bool dll_Excuted;

		// Token: 0x040016EF RID: 5871
		private bool dll_Excuted;
	}
}
