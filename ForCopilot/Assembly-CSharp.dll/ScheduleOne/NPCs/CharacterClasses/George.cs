using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004E4 RID: 1252
	public class George : NPC
	{
		// Token: 0x06001B85 RID: 7045 RVA: 0x00075C24 File Offset: 0x00073E24
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GeorgeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GeorgeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x00075C3D File Offset: 0x00073E3D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GeorgeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GeorgeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x00075C56 File Offset: 0x00073E56
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x00075C64 File Offset: 0x00073E64
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400171B RID: 5915
		private bool dll_Excuted;

		// Token: 0x0400171C RID: 5916
		private bool dll_Excuted;
	}
}
