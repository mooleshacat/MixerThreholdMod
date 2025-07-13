using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004F5 RID: 1269
	public class Karen : NPC
	{
		// Token: 0x06001BDE RID: 7134 RVA: 0x0007631F File Offset: 0x0007451F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KarenAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KarenAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x00076338 File Offset: 0x00074538
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KarenAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KarenAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x00076351 File Offset: 0x00074551
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x0007635F File Offset: 0x0007455F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001742 RID: 5954
		private bool dll_Excuted;

		// Token: 0x04001743 RID: 5955
		private bool dll_Excuted;
	}
}
