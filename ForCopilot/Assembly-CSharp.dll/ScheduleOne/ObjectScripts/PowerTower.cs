using System;
using ScheduleOne.ConstructableScripts;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C49 RID: 3145
	public class PowerTower : Constructable_GridBased
	{
		// Token: 0x06005881 RID: 22657 RVA: 0x00176572 File Offset: 0x00174772
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PowerTowerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PowerTowerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06005882 RID: 22658 RVA: 0x0017658B File Offset: 0x0017478B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.PowerTowerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.PowerTowerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06005883 RID: 22659 RVA: 0x001765A4 File Offset: 0x001747A4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005884 RID: 22660 RVA: 0x001765B2 File Offset: 0x001747B2
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040040E7 RID: 16615
		private bool dll_Excuted;

		// Token: 0x040040E8 RID: 16616
		private bool dll_Excuted;
	}
}
