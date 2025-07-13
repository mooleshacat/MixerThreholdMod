using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004EA RID: 1258
	public class Jack : NPC
	{
		// Token: 0x06001BA5 RID: 7077 RVA: 0x00075E97 File Offset: 0x00074097
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JackAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JackAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x00075EB0 File Offset: 0x000740B0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JackAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JackAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x00075EC9 File Offset: 0x000740C9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x00075ED7 File Offset: 0x000740D7
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001729 RID: 5929
		private bool dll_Excuted;

		// Token: 0x0400172A RID: 5930
		private bool dll_Excuted;
	}
}
