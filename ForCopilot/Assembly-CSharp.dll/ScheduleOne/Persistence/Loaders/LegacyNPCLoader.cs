using System;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003D0 RID: 976
	public class LegacyNPCLoader : Loader
	{
		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600157F RID: 5503 RVA: 0x000602D8 File Offset: 0x0005E4D8
		public virtual string NPCType
		{
			get
			{
				return typeof(NPCData).Name;
			}
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x000602E9 File Offset: 0x0005E4E9
		public LegacyNPCLoader()
		{
			Singleton<LoadManager>.Instance.LegacyNPCLoaders.Add(this);
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00060304 File Offset: 0x0005E504
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, "NPC", out text))
			{
				NPCData data = null;
				try
				{
					data = JsonUtility.FromJson<NPCData>(text);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				NPC npc = NPCManager.NPCRegistry.FirstOrDefault((NPC x) => x.ID == data.ID);
				if (npc == null)
				{
					Console.LogWarning("Failed to find NPC with ID: " + data.ID, null);
					return;
				}
				npc.Load(data, mainPath);
				string text2;
				if (base.TryLoadFile(mainPath, "Relationship", out text2))
				{
					RelationshipData relationshipData = null;
					try
					{
						relationshipData = JsonUtility.FromJson<RelationshipData>(text2);
					}
					catch (Exception ex3)
					{
						Type type2 = base.GetType();
						string str3 = (type2 != null) ? type2.ToString() : null;
						string str4 = " error reading relationship data: ";
						Exception ex4 = ex3;
						Console.LogError(str3 + str4 + ((ex4 != null) ? ex4.ToString() : null), null);
					}
					if (relationshipData != null)
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
				}
				this.TryLoadInventory(mainPath, npc);
				string text3;
				if (base.TryLoadFile(mainPath, "Health", out text3))
				{
					NPCHealthData npchealthData = null;
					try
					{
						npchealthData = JsonUtility.FromJson<NPCHealthData>(text3);
					}
					catch (Exception ex5)
					{
						Type type3 = base.GetType();
						string str5 = (type3 != null) ? type3.ToString() : null;
						string str6 = " error reading health data: ";
						Exception ex6 = ex5;
						Console.LogError(str5 + str6 + ((ex6 != null) ? ex6.ToString() : null), null);
					}
					if (npchealthData != null)
					{
						npc.Health.Load(npchealthData);
					}
				}
				string text4;
				if (base.TryLoadFile(mainPath, "MessageConversation", out text4))
				{
					MSGConversationData msgconversationData = null;
					try
					{
						msgconversationData = JsonUtility.FromJson<MSGConversationData>(text4);
					}
					catch (Exception ex7)
					{
						Type type4 = base.GetType();
						string str7 = (type4 != null) ? type4.ToString() : null;
						string str8 = " error reading message data: ";
						Exception ex8 = ex7;
						Console.LogError(str7 + str8 + ((ex8 != null) ? ex8.ToString() : null), null);
					}
					if (msgconversationData != null)
					{
						npc.MSGConversation.Load(msgconversationData);
					}
				}
				string text5;
				if (base.TryLoadFile(mainPath, "CustomerData", out text5))
				{
					ScheduleOne.Persistence.Datas.CustomerData customerData = null;
					try
					{
						customerData = JsonUtility.FromJson<ScheduleOne.Persistence.Datas.CustomerData>(text5);
					}
					catch (Exception ex9)
					{
						Type type5 = base.GetType();
						string str9 = (type5 != null) ? type5.ToString() : null;
						string str10 = " error reading customer data: ";
						Exception ex10 = ex9;
						Console.LogError(str9 + str10 + ((ex10 != null) ? ex10.ToString() : null), null);
					}
					if (customerData != null && npc.GetComponent<Customer>() != null)
					{
						npc.GetComponent<Customer>().Load(customerData);
					}
				}
			}
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x000605E0 File Offset: 0x0005E7E0
		protected void TryLoadInventory(string mainPath, NPC npc)
		{
			string json;
			if (base.TryLoadFile(mainPath, "Inventory", out json))
			{
				DeserializedItemSet deserializedItemSet;
				if (!ItemSet.TryDeserialize(json, out deserializedItemSet))
				{
					return;
				}
				deserializedItemSet.LoadTo(npc.Inventory.ItemSlots);
			}
		}
	}
}
