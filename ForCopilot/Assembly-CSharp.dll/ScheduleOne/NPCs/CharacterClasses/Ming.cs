using System;
using ScheduleOne.Property;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000506 RID: 1286
	public class Ming : NPC
	{
		// Token: 0x06001C42 RID: 7234 RVA: 0x0006558B File Offset: 0x0006378B
		public override string GetNameAddress()
		{
			return base.fullName;
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x00076C4A File Offset: 0x00074E4A
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MingAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MingAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x00076C63 File Offset: 0x00074E63
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MingAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MingAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x00076C7C File Offset: 0x00074E7C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x00076C8A File Offset: 0x00074E8A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400176C RID: 5996
		public Property Property;

		// Token: 0x0400176D RID: 5997
		private bool dll_Excuted;

		// Token: 0x0400176E RID: 5998
		private bool dll_Excuted;
	}
}
