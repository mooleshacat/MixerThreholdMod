using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000470 RID: 1136
	[Serializable]
	public class TrashData : SaveData
	{
		// Token: 0x060016AE RID: 5806 RVA: 0x000647FA File Offset: 0x000629FA
		public TrashData(TrashItemData[] trash, TrashGeneratorData[] generators)
		{
			this.Items = trash;
			this.Generators = generators;
		}

		// Token: 0x040014F7 RID: 5367
		public TrashItemData[] Items;

		// Token: 0x040014F8 RID: 5368
		public TrashGeneratorData[] Generators;
	}
}
