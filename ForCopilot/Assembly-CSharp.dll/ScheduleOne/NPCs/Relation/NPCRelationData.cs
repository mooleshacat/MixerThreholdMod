using System;
using System.Collections.Generic;
using ScheduleOne.Economy;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.NPCs.Relation
{
	// Token: 0x020004C8 RID: 1224
	[Serializable]
	public class NPCRelationData
	{
		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001ADA RID: 6874 RVA: 0x000749E1 File Offset: 0x00072BE1
		// (set) Token: 0x06001ADB RID: 6875 RVA: 0x000749E9 File Offset: 0x00072BE9
		public float RelationDelta { get; protected set; } = 2f;

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001ADC RID: 6876 RVA: 0x000749F2 File Offset: 0x00072BF2
		public float NormalizedRelationDelta
		{
			get
			{
				return this.RelationDelta / 5f;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001ADD RID: 6877 RVA: 0x00074A00 File Offset: 0x00072C00
		// (set) Token: 0x06001ADE RID: 6878 RVA: 0x00074A08 File Offset: 0x00072C08
		public bool Unlocked { get; protected set; }

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001ADF RID: 6879 RVA: 0x00074A11 File Offset: 0x00072C11
		// (set) Token: 0x06001AE0 RID: 6880 RVA: 0x00074A19 File Offset: 0x00072C19
		public NPCRelationData.EUnlockType UnlockType { get; protected set; }

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001AE1 RID: 6881 RVA: 0x00074A22 File Offset: 0x00072C22
		// (set) Token: 0x06001AE2 RID: 6882 RVA: 0x00074A2A File Offset: 0x00072C2A
		public NPC NPC { get; protected set; }

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x00074A33 File Offset: 0x00072C33
		public List<NPC> Connections
		{
			get
			{
				return this.FullGameConnections;
			}
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x00074A3B File Offset: 0x00072C3B
		public void SetNPC(NPC npc)
		{
			this.NPC = npc;
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x00074A44 File Offset: 0x00072C44
		public void Init(NPC npc)
		{
			this.SetNPC(npc);
			for (int i = 0; i < this.Connections.Count; i++)
			{
				if (this.Connections[i] == null)
				{
					this.Connections.RemoveAt(i);
					i--;
				}
				else if (!this.Connections[i].RelationData.Connections.Contains(this.NPC))
				{
					this.Connections[i].RelationData.Connections.Add(this.NPC);
				}
			}
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x00074AD8 File Offset: 0x00072CD8
		public virtual void ChangeRelationship(float deltaChange, bool network = true)
		{
			float relationDelta = this.RelationDelta;
			this.RelationDelta = Mathf.Clamp(this.RelationDelta + deltaChange, 0f, 5f);
			if (this.RelationDelta - relationDelta != 0f && this.onRelationshipChange != null)
			{
				this.onRelationshipChange(this.RelationDelta - relationDelta);
			}
			if (network)
			{
				this.NPC.SendRelationship(this.RelationDelta);
			}
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x00074B48 File Offset: 0x00072D48
		public virtual void SetRelationship(float newDelta)
		{
			float relationDelta = this.RelationDelta;
			this.RelationDelta = Mathf.Clamp(newDelta, 0f, 5f);
			float relationDelta2 = this.RelationDelta;
			if (this.RelationDelta - relationDelta != 0f && this.onRelationshipChange != null)
			{
				this.onRelationshipChange(this.RelationDelta - relationDelta);
			}
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x00074BA9 File Offset: 0x00072DA9
		public virtual void Unlock(NPCRelationData.EUnlockType type, bool notify = true)
		{
			if (this.Unlocked)
			{
				return;
			}
			this.Unlocked = true;
			this.UnlockType = type;
			if (this.onUnlocked != null)
			{
				this.onUnlocked(type, notify);
			}
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x00074BD8 File Offset: 0x00072DD8
		public virtual void UnlockConnections()
		{
			for (int i = 0; i < this.Connections.Count; i++)
			{
				if (!this.Connections[i].RelationData.Unlocked)
				{
					this.Connections[i].RelationData.Unlock(NPCRelationData.EUnlockType.Recommendation, true);
				}
			}
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x00074C2B File Offset: 0x00072E2B
		public RelationshipData GetSaveData()
		{
			return new RelationshipData(this.RelationDelta, this.Unlocked, this.UnlockType);
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x00074C44 File Offset: 0x00072E44
		public float GetAverageMutualRelationship()
		{
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < this.Connections.Count; i++)
			{
				if (this.Connections[i].RelationData.Unlocked)
				{
					num2++;
					num += this.Connections[i].RelationData.RelationDelta;
				}
			}
			if (num2 == 0)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x00074CB1 File Offset: 0x00072EB1
		public bool IsKnown()
		{
			return this.Unlocked || this.IsMutuallyKnown();
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x00074CC4 File Offset: 0x00072EC4
		public bool IsMutuallyKnown()
		{
			for (int i = 0; i < this.Connections.Count; i++)
			{
				if (!(this.Connections[i] == null) && this.Connections[i].RelationData.Unlocked)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x00074D18 File Offset: 0x00072F18
		public List<NPC> GetLockedConnections(bool excludeCustomers = false)
		{
			return this.Connections.FindAll((NPC x) => !x.RelationData.Unlocked && (!excludeCustomers || x.GetComponent<Customer>() == null));
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x00074D4C File Offset: 0x00072F4C
		public List<NPC> GetLockedDealers(bool excludeRecommended)
		{
			return this.Connections.FindAll((NPC x) => !x.RelationData.Unlocked && x is Dealer && (!excludeRecommended || !(x as Dealer).HasBeenRecommended));
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x00074D7D File Offset: 0x00072F7D
		public List<NPC> GetLockedSuppliers()
		{
			return this.Connections.FindAll((NPC x) => !x.RelationData.Unlocked && x is Supplier);
		}

		// Token: 0x040016CA RID: 5834
		public const float MinDelta = 0f;

		// Token: 0x040016CB RID: 5835
		public const float MaxDelta = 5f;

		// Token: 0x040016CC RID: 5836
		public const float DEFAULT_RELATION_DELTA = 2f;

		// Token: 0x040016D1 RID: 5841
		[SerializeField]
		protected List<NPC> FullGameConnections = new List<NPC>();

		// Token: 0x040016D2 RID: 5842
		[SerializeField]
		protected List<NPC> DemoConnections = new List<NPC>();

		// Token: 0x040016D3 RID: 5843
		public Action<float> onRelationshipChange;

		// Token: 0x040016D4 RID: 5844
		public Action<NPCRelationData.EUnlockType, bool> onUnlocked;

		// Token: 0x020004C9 RID: 1225
		public enum EUnlockType
		{
			// Token: 0x040016D6 RID: 5846
			Recommendation,
			// Token: 0x040016D7 RID: 5847
			DirectApproach
		}
	}
}
