using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200042E RID: 1070
	[Serializable]
	public class NPCFieldData
	{
		// Token: 0x06001664 RID: 5732 RVA: 0x00063D0B File Offset: 0x00061F0B
		public NPCFieldData(string npcGuid)
		{
			this.NPCGuid = npcGuid;
		}

		// Token: 0x04001437 RID: 5175
		public string NPCGuid;
	}
}
