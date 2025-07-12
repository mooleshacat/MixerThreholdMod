using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004E6 RID: 1254
	public class Greg : NPC
	{
		// Token: 0x06001B8F RID: 7055 RVA: 0x00075CCC File Offset: 0x00073ECC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GregAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GregAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x00075CE5 File Offset: 0x00073EE5
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GregAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GregAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x00075CFE File Offset: 0x00073EFE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x00075D0C File Offset: 0x00073F0C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400171F RID: 5919
		private bool dll_Excuted;

		// Token: 0x04001720 RID: 5920
		private bool dll_Excuted;
	}
}
