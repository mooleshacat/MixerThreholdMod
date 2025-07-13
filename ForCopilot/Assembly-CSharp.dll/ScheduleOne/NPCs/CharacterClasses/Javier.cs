using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004ED RID: 1261
	public class Javier : NPC
	{
		// Token: 0x06001BB4 RID: 7092 RVA: 0x00075F93 File Offset: 0x00074193
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JavierAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JavierAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x00075FAC File Offset: 0x000741AC
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JavierAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JavierAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x00075FC5 File Offset: 0x000741C5
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x00075FD3 File Offset: 0x000741D3
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400172F RID: 5935
		private bool dll_Excuted;

		// Token: 0x04001730 RID: 5936
		private bool dll_Excuted;
	}
}
