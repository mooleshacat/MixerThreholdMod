using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000515 RID: 1301
	public class Tobias : NPC
	{
		// Token: 0x06001CAD RID: 7341 RVA: 0x00077850 File Offset: 0x00075A50
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.TobiasAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.TobiasAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x00077869 File Offset: 0x00075A69
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.TobiasAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.TobiasAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x00077882 File Offset: 0x00075A82
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x00077890 File Offset: 0x00075A90
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040017A1 RID: 6049
		private bool dll_Excuted;

		// Token: 0x040017A2 RID: 6050
		private bool dll_Excuted;
	}
}
