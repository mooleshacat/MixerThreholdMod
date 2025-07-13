using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004F0 RID: 1264
	public class Jennifer : NPC
	{
		// Token: 0x06001BC3 RID: 7107 RVA: 0x0007608F File Offset: 0x0007428F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JenniferAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JenniferAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x000760A8 File Offset: 0x000742A8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JenniferAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JenniferAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x000760C1 File Offset: 0x000742C1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x000760CF File Offset: 0x000742CF
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001735 RID: 5941
		private bool dll_Excuted;

		// Token: 0x04001736 RID: 5942
		private bool dll_Excuted;
	}
}
