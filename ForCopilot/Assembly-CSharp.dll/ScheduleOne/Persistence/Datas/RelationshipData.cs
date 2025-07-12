using System;
using ScheduleOne.NPCs.Relation;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200041D RID: 1053
	[Serializable]
	public class RelationshipData : SaveData
	{
		// Token: 0x06001652 RID: 5714 RVA: 0x00063BBB File Offset: 0x00061DBB
		public RelationshipData(float relationDelta, bool unlocked, NPCRelationData.EUnlockType unlockType)
		{
			this.RelationDelta = relationDelta;
			this.Unlocked = unlocked;
			this.UnlockType = unlockType;
		}

		// Token: 0x0400141A RID: 5146
		public float RelationDelta;

		// Token: 0x0400141B RID: 5147
		public bool Unlocked;

		// Token: 0x0400141C RID: 5148
		public NPCRelationData.EUnlockType UnlockType;
	}
}
