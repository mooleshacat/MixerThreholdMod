using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200040A RID: 1034
	[Serializable]
	public class GenericSaveablesData : SaveData
	{
		// Token: 0x06001633 RID: 5683 RVA: 0x00063912 File Offset: 0x00061B12
		public GenericSaveablesData(GenericSaveData[] saveables)
		{
			this.Saveables = saveables;
		}

		// Token: 0x04001401 RID: 5121
		public GenericSaveData[] Saveables;
	}
}
