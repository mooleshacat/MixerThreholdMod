using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200042D RID: 1069
	[Serializable]
	public class MixingStationConfigurationData : SaveData
	{
		// Token: 0x06001663 RID: 5731 RVA: 0x00063CF5 File Offset: 0x00061EF5
		public MixingStationConfigurationData(ObjectFieldData destination, NumberFieldData threshold)
		{
			this.Destination = destination;
			this.Threshold = threshold;
		}

		// Token: 0x04001435 RID: 5173
		public ObjectFieldData Destination;

		// Token: 0x04001436 RID: 5174
		public NumberFieldData Threshold;
	}
}
