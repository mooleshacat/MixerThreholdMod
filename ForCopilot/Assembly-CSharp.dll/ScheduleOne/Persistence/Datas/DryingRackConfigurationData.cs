using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200042A RID: 1066
	[Serializable]
	public class DryingRackConfigurationData : SaveData
	{
		// Token: 0x06001660 RID: 5728 RVA: 0x00063CC1 File Offset: 0x00061EC1
		public DryingRackConfigurationData(QualityFieldData targetquality, ObjectFieldData destination)
		{
			this.TargetQuality = targetquality;
			this.Destination = destination;
		}

		// Token: 0x04001431 RID: 5169
		public QualityFieldData TargetQuality;

		// Token: 0x04001432 RID: 5170
		public ObjectFieldData Destination;
	}
}
