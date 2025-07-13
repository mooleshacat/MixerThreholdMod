using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200042C RID: 1068
	[Serializable]
	public class LabOvenConfigurationData : SaveData
	{
		// Token: 0x06001662 RID: 5730 RVA: 0x00063CE6 File Offset: 0x00061EE6
		public LabOvenConfigurationData(ObjectFieldData destination)
		{
			this.Destination = destination;
		}

		// Token: 0x04001434 RID: 5172
		public ObjectFieldData Destination;
	}
}
