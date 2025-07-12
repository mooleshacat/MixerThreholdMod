using System;

namespace ScheduleOne.Storage
{
	// Token: 0x020008DC RID: 2268
	public class DisposalStorageEntity : StorageEntity
	{
		// Token: 0x06003D00 RID: 15616 RVA: 0x00100EE6 File Offset: 0x000FF0E6
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Storage.DisposalStorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Storage.DisposalStorageEntityAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x00100EFF File Offset: 0x000FF0FF
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Storage.DisposalStorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Storage.DisposalStorageEntityAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x00100F18 File Offset: 0x000FF118
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003D03 RID: 15619 RVA: 0x00100F26 File Offset: 0x000FF126
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002BDD RID: 11229
		private bool dll_Excuted;

		// Token: 0x04002BDE RID: 11230
		private bool dll_Excuted;
	}
}
