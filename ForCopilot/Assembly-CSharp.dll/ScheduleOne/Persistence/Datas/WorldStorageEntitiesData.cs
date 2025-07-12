using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000477 RID: 1143
	[Serializable]
	public class WorldStorageEntitiesData : SaveData
	{
		// Token: 0x060016B6 RID: 5814 RVA: 0x0006490B File Offset: 0x00062B0B
		public WorldStorageEntitiesData(WorldStorageEntityData[] entities)
		{
			this.Entities = entities;
		}

		// Token: 0x0400150B RID: 5387
		public WorldStorageEntityData[] Entities;
	}
}
