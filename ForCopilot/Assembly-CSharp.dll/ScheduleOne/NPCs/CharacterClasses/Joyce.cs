using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004F4 RID: 1268
	public class Joyce : NPC
	{
		// Token: 0x06001BD9 RID: 7129 RVA: 0x000762CB File Offset: 0x000744CB
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JoyceAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JoyceAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x000762E4 File Offset: 0x000744E4
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JoyceAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JoyceAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x000762FD File Offset: 0x000744FD
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x0007630B File Offset: 0x0007450B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001740 RID: 5952
		private bool dll_Excuted;

		// Token: 0x04001741 RID: 5953
		private bool dll_Excuted;
	}
}
