using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004D6 RID: 1238
	public class Beth : NPC
	{
		// Token: 0x06001B33 RID: 6963 RVA: 0x000754E3 File Offset: 0x000736E3
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.BethAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.BethAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x000754FC File Offset: 0x000736FC
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.BethAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.BethAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x00075515 File Offset: 0x00073715
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x00075523 File Offset: 0x00073723
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016F4 RID: 5876
		private bool dll_Excuted;

		// Token: 0x040016F5 RID: 5877
		private bool dll_Excuted;
	}
}
