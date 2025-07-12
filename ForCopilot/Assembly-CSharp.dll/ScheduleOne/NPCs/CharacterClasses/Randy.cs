using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200050F RID: 1295
	public class Randy : NPC
	{
		// Token: 0x06001C77 RID: 7287 RVA: 0x0007709A File Offset: 0x0007529A
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.RandyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.RandyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x000770B3 File Offset: 0x000752B3
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.RandyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.RandyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000770CC File Offset: 0x000752CC
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000770DA File Offset: 0x000752DA
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001784 RID: 6020
		private bool dll_Excuted;

		// Token: 0x04001785 RID: 6021
		private bool dll_Excuted;
	}
}
