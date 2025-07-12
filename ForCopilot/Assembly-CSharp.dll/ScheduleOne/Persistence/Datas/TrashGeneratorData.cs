using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000471 RID: 1137
	[Serializable]
	public class TrashGeneratorData : SaveData
	{
		// Token: 0x060016AF RID: 5807 RVA: 0x00064810 File Offset: 0x00062A10
		public TrashGeneratorData(string guid, string[] generatedItems)
		{
			this.GUID = guid;
			this.GeneratedItems = generatedItems;
		}

		// Token: 0x040014F9 RID: 5369
		public string GUID;

		// Token: 0x040014FA RID: 5370
		public string[] GeneratedItems;
	}
}
