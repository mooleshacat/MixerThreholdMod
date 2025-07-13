using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200043B RID: 1083
	public class NPCCollectionData : SaveData
	{
		// Token: 0x06001672 RID: 5746 RVA: 0x00063E81 File Offset: 0x00062081
		public NPCCollectionData(DynamicSaveData[] npcs)
		{
			this.NPCs = npcs;
		}

		// Token: 0x04001455 RID: 5205
		public DynamicSaveData[] NPCs;
	}
}
