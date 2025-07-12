using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004EF RID: 1263
	public class Jen : NPC
	{
		// Token: 0x06001BBE RID: 7102 RVA: 0x0007603B File Offset: 0x0007423B
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JenAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JenAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x00076054 File Offset: 0x00074254
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JenAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JenAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x0007606D File Offset: 0x0007426D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x0007607B File Offset: 0x0007427B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001733 RID: 5939
		private bool dll_Excuted;

		// Token: 0x04001734 RID: 5940
		private bool dll_Excuted;
	}
}
