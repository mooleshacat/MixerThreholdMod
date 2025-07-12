using System;

namespace ScheduleOne.ConstructableScripts
{
	// Token: 0x02000969 RID: 2409
	public class EmployeeTrailer : Constructable_GridBased
	{
		// Token: 0x060040FF RID: 16639 RVA: 0x00112D86 File Offset: 0x00110F86
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.EmployeeTrailerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.EmployeeTrailerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x00112D9F File Offset: 0x00110F9F
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ConstructableScripts.EmployeeTrailerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ConstructableScripts.EmployeeTrailerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x00112DB8 File Offset: 0x00110FB8
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x00112DC6 File Offset: 0x00110FC6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002E66 RID: 11878
		private bool dll_Excuted;

		// Token: 0x04002E67 RID: 11879
		private bool dll_Excuted;
	}
}
