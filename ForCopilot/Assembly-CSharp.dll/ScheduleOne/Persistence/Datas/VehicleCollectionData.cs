using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000475 RID: 1141
	[Serializable]
	public class VehicleCollectionData : SaveData
	{
		// Token: 0x060016B4 RID: 5812 RVA: 0x000648A4 File Offset: 0x00062AA4
		public VehicleCollectionData(VehicleData[] vehicles)
		{
			this.Vehicles = vehicles;
		}

		// Token: 0x04001504 RID: 5380
		public VehicleData[] Vehicles;
	}
}
