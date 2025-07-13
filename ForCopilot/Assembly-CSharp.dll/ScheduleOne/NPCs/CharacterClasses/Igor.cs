using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004E9 RID: 1257
	public class Igor : NPC
	{
		// Token: 0x06001BA0 RID: 7072 RVA: 0x00075E43 File Offset: 0x00074043
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.IgorAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.IgorAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x00075E5C File Offset: 0x0007405C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.IgorAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.IgorAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x00075E75 File Offset: 0x00074075
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x00075E83 File Offset: 0x00074083
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001727 RID: 5927
		private bool dll_Excuted;

		// Token: 0x04001728 RID: 5928
		private bool dll_Excuted;
	}
}
