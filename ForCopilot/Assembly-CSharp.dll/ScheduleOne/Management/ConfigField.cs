using System;

namespace ScheduleOne.Management
{
	// Token: 0x020005AC RID: 1452
	public abstract class ConfigField
	{
		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x060023AF RID: 9135 RVA: 0x000936FB File Offset: 0x000918FB
		// (set) Token: 0x060023B0 RID: 9136 RVA: 0x00093703 File Offset: 0x00091903
		public EntityConfiguration ParentConfig { get; protected set; }

		// Token: 0x060023B1 RID: 9137 RVA: 0x0009370C File Offset: 0x0009190C
		public ConfigField(EntityConfiguration parentConfig)
		{
			this.ParentConfig = parentConfig;
			this.ParentConfig.Fields.Add(this);
		}

		// Token: 0x060023B2 RID: 9138
		public abstract bool IsValueDefault();
	}
}
