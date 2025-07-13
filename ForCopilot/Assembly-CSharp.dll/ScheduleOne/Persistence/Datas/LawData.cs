using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000422 RID: 1058
	public class LawData : SaveData
	{
		// Token: 0x06001657 RID: 5719 RVA: 0x00063C10 File Offset: 0x00061E10
		public LawData(float internalLawIntensity)
		{
			this.InternalLawIntensity = internalLawIntensity;
		}

		// Token: 0x04001421 RID: 5153
		public float InternalLawIntensity;
	}
}
