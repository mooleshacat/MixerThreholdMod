using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004E2 RID: 1250
	public class Frank : NPC
	{
		// Token: 0x06001B7B RID: 7035 RVA: 0x00075B7C File Offset: 0x00073D7C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.FrankAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.FrankAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x00075B95 File Offset: 0x00073D95
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.FrankAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.FrankAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x00075BAE File Offset: 0x00073DAE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x00075BBC File Offset: 0x00073DBC
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001717 RID: 5911
		private bool dll_Excuted;

		// Token: 0x04001718 RID: 5912
		private bool dll_Excuted;
	}
}
