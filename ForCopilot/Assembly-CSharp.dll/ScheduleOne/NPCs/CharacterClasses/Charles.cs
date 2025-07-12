using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004D9 RID: 1241
	public class Charles : NPC
	{
		// Token: 0x06001B42 RID: 6978 RVA: 0x000755E7 File Offset: 0x000737E7
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.CharlesAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.CharlesAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x00075600 File Offset: 0x00073800
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.CharlesAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.CharlesAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x00075619 File Offset: 0x00073819
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x00075627 File Offset: 0x00073827
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016FA RID: 5882
		private bool dll_Excuted;

		// Token: 0x040016FB RID: 5883
		private bool dll_Excuted;
	}
}
