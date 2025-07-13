using System;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000489 RID: 1161
	public class Jerry : NPC
	{
		// Token: 0x060016F0 RID: 5872 RVA: 0x000652AF File Offset: 0x000634AF
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.JerryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.JerryAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x000652C8 File Offset: 0x000634C8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.JerryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.JerryAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x000652E1 File Offset: 0x000634E1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x000652EF File Offset: 0x000634EF
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001518 RID: 5400
		private bool dll_Excuted;

		// Token: 0x04001519 RID: 5401
		private bool dll_Excuted;
	}
}
