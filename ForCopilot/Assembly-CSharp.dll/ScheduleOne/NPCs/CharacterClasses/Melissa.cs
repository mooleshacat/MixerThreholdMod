using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000503 RID: 1283
	public class Melissa : NPC
	{
		// Token: 0x06001C32 RID: 7218 RVA: 0x00076AFB File Offset: 0x00074CFB
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MelissaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MelissaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x00076B14 File Offset: 0x00074D14
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MelissaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MelissaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x00076B2D File Offset: 0x00074D2D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x00076B3B File Offset: 0x00074D3B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001766 RID: 5990
		private bool dll_Excuted;

		// Token: 0x04001767 RID: 5991
		private bool dll_Excuted;
	}
}
