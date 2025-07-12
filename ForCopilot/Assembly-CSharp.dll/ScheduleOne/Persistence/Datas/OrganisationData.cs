using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200045A RID: 1114
	[Serializable]
	public class OrganisationData : SaveData
	{
		// Token: 0x06001691 RID: 5777 RVA: 0x000643DB File Offset: 0x000625DB
		public OrganisationData(string name, float netWorth)
		{
			this.Name = name;
			this.NetWorth = netWorth;
		}

		// Token: 0x040014AD RID: 5293
		public string Name;

		// Token: 0x040014AE RID: 5294
		public float NetWorth;
	}
}
