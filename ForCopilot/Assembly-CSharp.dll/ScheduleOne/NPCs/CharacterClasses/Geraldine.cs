using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004E5 RID: 1253
	public class Geraldine : NPC
	{
		// Token: 0x06001B8A RID: 7050 RVA: 0x00075C78 File Offset: 0x00073E78
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GeraldineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GeraldineAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x00075C91 File Offset: 0x00073E91
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GeraldineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GeraldineAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x00075CAA File Offset: 0x00073EAA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x00075CB8 File Offset: 0x00073EB8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400171D RID: 5917
		private bool dll_Excuted;

		// Token: 0x0400171E RID: 5918
		private bool dll_Excuted;
	}
}
