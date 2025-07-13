using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000435 RID: 1077
	[Serializable]
	public class QualityFieldData
	{
		// Token: 0x0600166B RID: 5739 RVA: 0x00063DA0 File Offset: 0x00061FA0
		public QualityFieldData(EQuality value)
		{
			this.Value = value;
		}

		// Token: 0x04001444 RID: 5188
		public EQuality Value;
	}
}
