using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000426 RID: 1062
	[Serializable]
	public class CauldronConfigurationData : SaveData
	{
		// Token: 0x0600165C RID: 5724 RVA: 0x00063C70 File Offset: 0x00061E70
		public CauldronConfigurationData(ObjectFieldData destination)
		{
			this.Destination = destination;
		}

		// Token: 0x0400142A RID: 5162
		public ObjectFieldData Destination;
	}
}
