using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000478 RID: 1144
	[Serializable]
	public class WorldStorageEntityData : SaveData
	{
		// Token: 0x060016B7 RID: 5815 RVA: 0x0006491A File Offset: 0x00062B1A
		public WorldStorageEntityData(Guid guid, ItemSet contents)
		{
			this.GUID = guid.ToString();
			this.Contents = contents;
		}

		// Token: 0x0400150C RID: 5388
		public string GUID;

		// Token: 0x0400150D RID: 5389
		public ItemSet Contents;
	}
}
