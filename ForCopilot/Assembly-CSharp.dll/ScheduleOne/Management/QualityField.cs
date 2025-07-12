using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005B2 RID: 1458
	public class QualityField : ConfigField
	{
		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x00093E3E File Offset: 0x0009203E
		// (set) Token: 0x060023DF RID: 9183 RVA: 0x00093E46 File Offset: 0x00092046
		public EQuality Value { get; protected set; } = EQuality.Standard;

		// Token: 0x060023E0 RID: 9184 RVA: 0x00093E4F File Offset: 0x0009204F
		public QualityField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x00093E6A File Offset: 0x0009206A
		public void SetValue(EQuality value, bool network)
		{
			this.Value = value;
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onValueChanged != null)
			{
				this.onValueChanged.Invoke(this.Value);
			}
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x00093E9C File Offset: 0x0009209C
		public override bool IsValueDefault()
		{
			return this.Value == EQuality.Standard;
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x00093EA7 File Offset: 0x000920A7
		public QualityFieldData GetData()
		{
			return new QualityFieldData(this.Value);
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x00093EB4 File Offset: 0x000920B4
		public void Load(QualityFieldData data)
		{
			if (data != null)
			{
				this.SetValue(data.Value, true);
			}
		}

		// Token: 0x04001AC9 RID: 6857
		public UnityEvent<EQuality> onValueChanged = new UnityEvent<EQuality>();
	}
}
