using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200050B RID: 1291
	public class Pearl : NPC
	{
		// Token: 0x06001C63 RID: 7267 RVA: 0x00076F4A File Offset: 0x0007514A
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PearlAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PearlAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x00076F63 File Offset: 0x00075163
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PearlAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PearlAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x00076F7C File Offset: 0x0007517C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x00076F8A File Offset: 0x0007518A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400177C RID: 6012
		private bool dll_Excuted;

		// Token: 0x0400177D RID: 6013
		private bool dll_Excuted;
	}
}
