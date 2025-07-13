using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Quests;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003AF RID: 943
	public class QuestsLoader : Loader
	{
		// Token: 0x06001535 RID: 5429 RVA: 0x0005E538 File Offset: 0x0005C738
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, out text, true))
			{
				QuestManagerData questManagerData = null;
				try
				{
					questManagerData = JsonUtility.FromJson<QuestManagerData>(text);
				}
				catch (Exception ex)
				{
					Debug.LogError("Error loading data: " + ex.Message);
				}
				if (questManagerData == null)
				{
					return;
				}
				if (questManagerData.Quests != null)
				{
					foreach (QuestData questData in questManagerData.Quests)
					{
						if (questData != null)
						{
							Quest @object = GUIDManager.GetObject<Quest>(new Guid(questData.GUID));
							if (@object == null)
							{
								Console.LogWarning("Failed to find quest with GUID: " + questData.GUID, null);
							}
							else
							{
								@object.Load(questData);
							}
						}
					}
				}
				if (questManagerData.DeaddropQuests != null)
				{
					foreach (DeaddropQuestData deaddropQuestData in questManagerData.DeaddropQuests)
					{
						if (deaddropQuestData != null)
						{
							DeadDrop object2 = GUIDManager.GetObject<DeadDrop>(new Guid(deaddropQuestData.DeaddropGUID));
							if (object2 == null)
							{
								Console.LogWarning("Failed to find deaddrop with GUID: " + deaddropQuestData.DeaddropGUID, null);
							}
							else
							{
								NetworkSingleton<QuestManager>.Instance.CreateDeaddropCollectionQuest(object2.GUID.ToString(), deaddropQuestData.GUID).Load(deaddropQuestData);
							}
						}
					}
				}
				if (questManagerData.Contracts != null)
				{
					foreach (ContractData contractData in questManagerData.Contracts)
					{
						if (contractData != null)
						{
							NPC object3 = GUIDManager.GetObject<NPC>(new Guid(contractData.CustomerGUID));
							if (object3 == null)
							{
								Console.LogWarning("Failed to find customer with GUID: " + contractData.CustomerGUID, null);
							}
							else
							{
								NetworkSingleton<QuestManager>.Instance.CreateContract_Local(contractData.Title, contractData.Description, contractData.Entries, contractData.GUID, contractData.IsTracked, object3.NetworkObject, contractData.Payment, contractData.ProductList, contractData.DeliveryLocationGUID, contractData.DeliveryWindow, contractData.Expires, new GameDateTime(contractData.ExpiryDate), contractData.PickupScheduleIndex, new GameDateTime(contractData.AcceptTime), null);
							}
						}
					}
					return;
				}
			}
			else
			{
				if (!Directory.Exists(mainPath))
				{
					return;
				}
				Console.Log("Loading legacy quests from: " + mainPath, null);
				string[] files = Directory.GetFiles(mainPath);
				for (int j = 0; j < files.Length; j++)
				{
					string text2;
					if (base.TryLoadFile(files[j], out text2, false))
					{
						QuestData questData2 = null;
						try
						{
							questData2 = JsonUtility.FromJson<QuestData>(text2);
						}
						catch (Exception ex2)
						{
							Debug.LogError("Error loading quest data: " + ex2.Message);
						}
						if (questData2 != null)
						{
							Quest quest;
							if (questData2.DataType == "DeaddropQuestData")
							{
								DeaddropQuestData deaddropQuestData2 = null;
								try
								{
									deaddropQuestData2 = JsonUtility.FromJson<DeaddropQuestData>(text2);
								}
								catch (Exception ex3)
								{
									Debug.LogError("Error loading quest data: " + ex3.Message);
								}
								if (deaddropQuestData2 == null)
								{
									goto IL_372;
								}
								DeadDrop object4 = GUIDManager.GetObject<DeadDrop>(new Guid(deaddropQuestData2.DeaddropGUID));
								if (object4 == null)
								{
									Console.LogWarning("Failed to find deaddrop with GUID: " + deaddropQuestData2.DeaddropGUID, null);
									goto IL_372;
								}
								quest = NetworkSingleton<QuestManager>.Instance.CreateDeaddropCollectionQuest(object4.GUID.ToString(), questData2.GUID);
							}
							else
							{
								quest = GUIDManager.GetObject<Quest>(new Guid(questData2.GUID));
							}
							if (quest == null)
							{
								Console.LogWarning("Failed to find quest with GUID: " + questData2.GUID, null);
							}
							else
							{
								quest.Load(questData2);
							}
						}
					}
					IL_372:;
				}
				string path = Path.Combine(mainPath, "Contracts");
				if (Directory.Exists(path))
				{
					string[] files2 = Directory.GetFiles(path);
					for (int k = 0; k < files2.Length; k++)
					{
						string text3;
						if (base.TryLoadFile(files2[k], out text3, false))
						{
							ContractData contractData2 = null;
							try
							{
								contractData2 = JsonUtility.FromJson<ContractData>(text3);
							}
							catch (Exception ex4)
							{
								Debug.LogError("Error loading contract data: " + ex4.Message);
							}
							if (contractData2 != null)
							{
								NPC object5 = GUIDManager.GetObject<NPC>(new Guid(contractData2.CustomerGUID));
								if (object5 == null)
								{
									Console.LogWarning("Failed to find customer with GUID: " + contractData2.CustomerGUID, null);
								}
								else
								{
									NetworkSingleton<QuestManager>.Instance.CreateContract_Local(contractData2.Title, contractData2.Description, contractData2.Entries, contractData2.GUID, contractData2.IsTracked, object5.NetworkObject, contractData2.Payment, contractData2.ProductList, contractData2.DeliveryLocationGUID, contractData2.DeliveryWindow, contractData2.Expires, new GameDateTime(contractData2.ExpiryDate), contractData2.PickupScheduleIndex, new GameDateTime(contractData2.AcceptTime), null);
								}
							}
						}
					}
				}
			}
		}
	}
}
