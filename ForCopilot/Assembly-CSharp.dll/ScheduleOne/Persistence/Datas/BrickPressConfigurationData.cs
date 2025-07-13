using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000425 RID: 1061
	[Serializable]
	public class BrickPressConfigurationData : SaveData
	{
		// Token: 0x0600165B RID: 5723 RVA: 0x00063C61 File Offset: 0x00061E61
		public BrickPressConfigurationData(ObjectFieldData destination)
		{
			this.Destination = destination;
		}

		// Token: 0x04001429 RID: 5161
		public ObjectFieldData Destination;
	}
}
