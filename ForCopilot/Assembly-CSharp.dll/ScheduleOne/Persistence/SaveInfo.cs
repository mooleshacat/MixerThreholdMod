using System;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence
{
	// Token: 0x02000394 RID: 916
	public class SaveInfo
	{
		// Token: 0x060014C0 RID: 5312 RVA: 0x0005BF68 File Offset: 0x0005A168
		public SaveInfo(string savePath, int saveSlotNumber, string organisationName, DateTime dateCreated, DateTime dateLastPlayed, float networth, string saveVersion, MetaData metaData)
		{
			this.SavePath = savePath;
			this.SaveSlotNumber = saveSlotNumber;
			this.OrganisationName = organisationName;
			this.DateCreated = dateCreated;
			this.DateLastPlayed = dateLastPlayed;
			this.Networth = networth;
			this.SaveVersion = saveVersion;
			this.MetaData = metaData;
		}

		// Token: 0x04001364 RID: 4964
		public string SavePath;

		// Token: 0x04001365 RID: 4965
		public int SaveSlotNumber;

		// Token: 0x04001366 RID: 4966
		public string OrganisationName;

		// Token: 0x04001367 RID: 4967
		public DateTime DateCreated;

		// Token: 0x04001368 RID: 4968
		public DateTime DateLastPlayed;

		// Token: 0x04001369 RID: 4969
		public float Networth;

		// Token: 0x0400136A RID: 4970
		public string SaveVersion;

		// Token: 0x0400136B RID: 4971
		public MetaData MetaData;
	}
}
