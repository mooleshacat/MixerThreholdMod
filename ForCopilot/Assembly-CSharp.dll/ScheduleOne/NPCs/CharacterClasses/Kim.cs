using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004F9 RID: 1273
	public class Kim : NPC
	{
		// Token: 0x06001BF2 RID: 7154 RVA: 0x0007646F File Offset: 0x0007466F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KimAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KimAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x00076488 File Offset: 0x00074688
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KimAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KimAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000764A1 File Offset: 0x000746A1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x000764AF File Offset: 0x000746AF
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400174A RID: 5962
		private bool dll_Excuted;

		// Token: 0x0400174B RID: 5963
		private bool dll_Excuted;
	}
}
