using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200050C RID: 1292
	public class Peggy : NPC
	{
		// Token: 0x06001C68 RID: 7272 RVA: 0x00076F9E File Offset: 0x0007519E
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PeggyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PeggyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x00076FB7 File Offset: 0x000751B7
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PeggyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PeggyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x00076FD0 File Offset: 0x000751D0
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x00076FDE File Offset: 0x000751DE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400177E RID: 6014
		private bool dll_Excuted;

		// Token: 0x0400177F RID: 6015
		private bool dll_Excuted;
	}
}
