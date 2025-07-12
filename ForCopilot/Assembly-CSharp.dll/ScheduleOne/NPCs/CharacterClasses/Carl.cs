using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004D8 RID: 1240
	public class Carl : NPC
	{
		// Token: 0x06001B3D RID: 6973 RVA: 0x00075593 File Offset: 0x00073793
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.CarlAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.CarlAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x000755AC File Offset: 0x000737AC
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.CarlAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.CarlAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x000755C5 File Offset: 0x000737C5
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x000755D3 File Offset: 0x000737D3
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016F8 RID: 5880
		private bool dll_Excuted;

		// Token: 0x040016F9 RID: 5881
		private bool dll_Excuted;
	}
}
