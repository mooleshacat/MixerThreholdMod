using System;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005AF RID: 1455
	public class NumberField : ConfigField
	{
		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x060023C2 RID: 9154 RVA: 0x0009391B File Offset: 0x00091B1B
		// (set) Token: 0x060023C3 RID: 9155 RVA: 0x00093923 File Offset: 0x00091B23
		public float Value { get; protected set; }

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x060023C4 RID: 9156 RVA: 0x0009392C File Offset: 0x00091B2C
		// (set) Token: 0x060023C5 RID: 9157 RVA: 0x00093934 File Offset: 0x00091B34
		public float MinValue { get; protected set; }

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060023C6 RID: 9158 RVA: 0x0009393D File Offset: 0x00091B3D
		// (set) Token: 0x060023C7 RID: 9159 RVA: 0x00093945 File Offset: 0x00091B45
		public float MaxValue { get; protected set; } = 100f;

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060023C8 RID: 9160 RVA: 0x0009394E File Offset: 0x00091B4E
		// (set) Token: 0x060023C9 RID: 9161 RVA: 0x00093956 File Offset: 0x00091B56
		public bool WholeNumbers { get; protected set; }

		// Token: 0x060023CA RID: 9162 RVA: 0x0009395F File Offset: 0x00091B5F
		public NumberField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x0009397E File Offset: 0x00091B7E
		public void SetValue(float value, bool network)
		{
			this.Value = value;
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onItemChanged != null)
			{
				this.onItemChanged.Invoke(this.Value);
			}
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x000939B0 File Offset: 0x00091BB0
		public void Configure(float minValue, float maxValue, bool wholeNumbers)
		{
			this.MinValue = minValue;
			this.MaxValue = maxValue;
			this.WholeNumbers = wholeNumbers;
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x000939C7 File Offset: 0x00091BC7
		public override bool IsValueDefault()
		{
			return this.Value == 0f;
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x000939D6 File Offset: 0x00091BD6
		public NumberFieldData GetData()
		{
			return new NumberFieldData(this.Value);
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x000939E3 File Offset: 0x00091BE3
		public void Load(NumberFieldData data)
		{
			if (data != null)
			{
				this.SetValue(data.Value, true);
			}
		}

		// Token: 0x04001ABD RID: 6845
		public UnityEvent<float> onItemChanged = new UnityEvent<float>();
	}
}
