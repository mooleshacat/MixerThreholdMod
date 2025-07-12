using System;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003D6 RID: 982
	public class NPCLoader : DynamicLoader
	{
		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x0600158D RID: 5517 RVA: 0x000602D8 File Offset: 0x0005E4D8
		public virtual string NPCType
		{
			get
			{
				return typeof(NPCData).Name;
			}
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x0006084B File Offset: 0x0005EA4B
		public NPCLoader()
		{
			Singleton<LoadManager>.Instance.NPCLoaders.Add(this);
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x00060864 File Offset: 0x0005EA64
		public override void Load(DynamicSaveData saveData)
		{
			base.Load(saveData);
			NPCData baseData = DynamicLoader.ExtractBaseData<NPCData>(saveData);
			if (baseData != null)
			{
				NPC npc = NPCManager.NPCRegistry.FirstOrDefault((NPC x) => x.ID == baseData.ID);
				if (npc == null)
				{
					Console.LogWarning("Failed to find NPC with ID: " + baseData.ID, null);
					return;
				}
				npc.Load(saveData, baseData);
				RelationshipData relationshipData;
				if (saveData.TryGetData<RelationshipData>("Relationship", out relationshipData))
				{
					if (!float.IsNaN(relationshipData.RelationDelta) && !float.IsInfinity(relationshipData.RelationDelta))
					{
						npc.RelationData.SetRelationship(relationshipData.RelationDelta);
					}
					if (relationshipData.Unlocked)
					{
						npc.RelationData.Unlock(relationshipData.UnlockType, false);
					}
				}
				NPCHealthData healthData;
				if (saveData.TryGetData<NPCHealthData>("Health", out healthData))
				{
					npc.Health.Load(healthData);
				}
				MSGConversationData data;
				if (saveData.TryGetData<MSGConversationData>("MessageConversation", out data))
				{
					npc.MSGConversation.Load(data);
				}
				ScheduleOne.Persistence.Datas.CustomerData data2;
				if (saveData.TryGetData<ScheduleOne.Persistence.Datas.CustomerData>("CustomerData", out data2) && npc.GetComponent<Customer>() != null)
				{
					npc.GetComponent<Customer>().Load(data2);
				}
				string json;
				if (saveData.TryGetData("Inventory", out json))
				{
					DeserializedItemSet deserializedItemSet;
					if (ItemSet.TryDeserialize(json, out deserializedItemSet))
					{
						deserializedItemSet.LoadTo(npc.Inventory.ItemSlots);
						return;
					}
					Console.LogWarning("Failed to deserialize inventory for NPC: " + npc.ID, null);
				}
			}
		}
	}
}
