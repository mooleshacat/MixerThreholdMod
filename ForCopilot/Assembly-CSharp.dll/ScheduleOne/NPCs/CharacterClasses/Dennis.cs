using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004DD RID: 1245
	public class Dennis : NPC
	{
		// Token: 0x06001B5A RID: 7002 RVA: 0x00075809 File Offset: 0x00073A09
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.DennisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.DennisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x00075822 File Offset: 0x00073A22
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.DennisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.DennisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x0007583B File Offset: 0x00073A3B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x00075849 File Offset: 0x00073A49
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001705 RID: 5893
		private bool dll_Excuted;

		// Token: 0x04001706 RID: 5894
		private bool dll_Excuted;
	}
}
