using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004AA RID: 1194
	public class ConversationLocation : MonoBehaviour
	{
		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001924 RID: 6436 RVA: 0x0006EDE0 File Offset: 0x0006CFE0
		public bool NPCsReady
		{
			get
			{
				return (from npcReady in this.npcReady
				where npcReady.Value
				select npcReady).Count<KeyValuePair<NPC, bool>>() >= 2;
			}
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0006EE18 File Offset: 0x0006D018
		public void Awake()
		{
			if (this.StandPoints.Length < this.NPCs.Count)
			{
				Console.LogError("ConversationLocation has less StandPoints than NPCs", null);
			}
			foreach (NPC key in this.NPCs)
			{
				this.npcReady.Add(key, false);
			}
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0006EE94 File Offset: 0x0006D094
		public Transform GetStandPoint(NPC npc)
		{
			if (!this.NPCs.Contains(npc))
			{
				Console.LogWarning("NPC is not part of this conversation", null);
				return this.StandPoints[0];
			}
			return this.StandPoints[this.NPCs.IndexOf(npc)];
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0006EECB File Offset: 0x0006D0CB
		public void SetNPCReady(NPC npc, bool ready)
		{
			if (!this.NPCs.Contains(npc))
			{
				Console.LogWarning("NPC is not part of this conversation", null);
				return;
			}
			this.npcReady[npc] = ready;
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0006EEF4 File Offset: 0x0006D0F4
		public NPC GetOtherNPC(NPC npc)
		{
			if (!this.NPCs.Contains(npc))
			{
				Console.LogWarning("NPC is not part of this conversation", null);
				return null;
			}
			return (from otherNPC in this.NPCs
			where otherNPC != npc
			select otherNPC).FirstOrDefault<NPC>();
		}

		// Token: 0x04001625 RID: 5669
		public Transform[] StandPoints;

		// Token: 0x04001626 RID: 5670
		[HideInInspector]
		public List<NPC> NPCs = new List<NPC>();

		// Token: 0x04001627 RID: 5671
		private Dictionary<NPC, bool> npcReady = new Dictionary<NPC, bool>();
	}
}
