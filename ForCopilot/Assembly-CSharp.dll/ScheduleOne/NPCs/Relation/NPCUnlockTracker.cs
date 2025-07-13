using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Relation
{
	// Token: 0x020004CD RID: 1229
	public class NPCUnlockTracker : MonoBehaviour
	{
		// Token: 0x06001AF9 RID: 6905 RVA: 0x00074E50 File Offset: 0x00073050
		private void Awake()
		{
			if (this.Npc.RelationData.Unlocked)
			{
				this.Invoke(this.Npc.RelationData.UnlockType, false);
			}
			NPCRelationData relationData = this.Npc.RelationData;
			relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.Invoke));
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x00074EB2 File Offset: 0x000730B2
		private void Invoke(NPCRelationData.EUnlockType type, bool t)
		{
			if (this.onUnlocked != null)
			{
				this.onUnlocked.Invoke();
			}
		}

		// Token: 0x040016DC RID: 5852
		public NPC Npc;

		// Token: 0x040016DD RID: 5853
		public UnityEvent onUnlocked;
	}
}
