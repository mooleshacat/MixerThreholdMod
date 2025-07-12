using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Trash;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003B7 RID: 951
	public class TrashLoader : Loader
	{
		// Token: 0x06001545 RID: 5445 RVA: 0x0005EE58 File Offset: 0x0005D058
		public override void Load(string mainPath)
		{
			TrashData trashData = null;
			string text;
			if (base.TryLoadFile(Path.Combine(mainPath, "Trash"), out text, true) || base.TryLoadFile(mainPath, out text, true))
			{
				try
				{
					trashData = JsonUtility.FromJson<TrashData>(text);
				}
				catch (Exception ex)
				{
					Debug.LogError("Error loading data: " + ex.Message);
				}
				if (trashData != null && trashData.Items != null)
				{
					foreach (TrashItemData trashItemData in trashData.Items)
					{
						TrashItem trashItem;
						if (trashItemData.DataType == "TrashBagData")
						{
							trashItem = NetworkSingleton<TrashManager>.Instance.CreateTrashBag(trashItemData.TrashID, trashItemData.Position, trashItemData.Rotation, trashItemData.Contents, Vector3.zero, trashItemData.GUID, true);
						}
						else
						{
							trashItem = NetworkSingleton<TrashManager>.Instance.CreateTrashItem(trashItemData.TrashID, trashItemData.Position, trashItemData.Rotation, Vector3.zero, trashItemData.GUID, true);
						}
						if (trashItem != null)
						{
							trashItem.HasChanged = false;
						}
					}
				}
			}
			else
			{
				string path = Path.Combine(mainPath, "Items");
				if (Directory.Exists(mainPath) && Directory.Exists(path))
				{
					string[] files = Directory.GetFiles(path);
					for (int j = 0; j < files.Length; j++)
					{
						string text2;
						if (base.TryLoadFile(files[j], out text2, false))
						{
							TrashItemData trashItemData2 = null;
							try
							{
								trashItemData2 = JsonUtility.FromJson<TrashItemData>(text2);
							}
							catch (Exception ex2)
							{
								Debug.LogError("Error loading data: " + ex2.Message);
							}
							if (trashItemData2 != null)
							{
								TrashItem trashItem2 = null;
								if (trashItemData2.DataType == "TrashBagData")
								{
									TrashBagData trashBagData = null;
									try
									{
										trashBagData = JsonUtility.FromJson<TrashBagData>(text2);
									}
									catch (Exception ex3)
									{
										Debug.LogError("Error loading data: " + ex3.Message);
									}
									if (trashBagData != null)
									{
										trashItem2 = NetworkSingleton<TrashManager>.Instance.CreateTrashBag(trashBagData.TrashID, trashBagData.Position, trashBagData.Rotation, trashBagData.Contents, Vector3.zero, trashBagData.GUID, true);
									}
								}
								else
								{
									trashItem2 = NetworkSingleton<TrashManager>.Instance.CreateTrashItem(trashItemData2.TrashID, trashItemData2.Position, trashItemData2.Rotation, Vector3.zero, trashItemData2.GUID, true);
								}
								if (trashItem2 != null)
								{
									trashItem2.HasChanged = false;
								}
							}
						}
					}
				}
			}
			if (trashData != null && trashData.Generators != null)
			{
				foreach (TrashGeneratorData trashGeneratorData in trashData.Generators)
				{
					if (trashGeneratorData != null)
					{
						TrashGenerator @object = GUIDManager.GetObject<TrashGenerator>(new Guid(trashGeneratorData.GUID));
						if (@object != null)
						{
							for (int k = 0; k < trashGeneratorData.GeneratedItems.Length; k++)
							{
								TrashItem object2 = GUIDManager.GetObject<TrashItem>(new Guid(trashGeneratorData.GeneratedItems[k]));
								if (object2 != null)
								{
									@object.AddGeneratedTrash(object2);
								}
							}
							@object.HasChanged = false;
						}
					}
				}
				return;
			}
			Console.Log("Loading legacy trash generators at: " + mainPath, null);
			string path2 = Path.Combine(mainPath, "Generators");
			if (Directory.Exists(mainPath) && Directory.Exists(path2))
			{
				string[] files2 = Directory.GetFiles(path2);
				for (int l = 0; l < files2.Length; l++)
				{
					string text3;
					if (base.TryLoadFile(files2[l], out text3, false))
					{
						TrashGeneratorData trashGeneratorData2 = null;
						try
						{
							trashGeneratorData2 = JsonUtility.FromJson<TrashGeneratorData>(text3);
						}
						catch (Exception ex4)
						{
							Debug.LogError("Error loading data: " + ex4.Message);
						}
						if (trashGeneratorData2 != null)
						{
							TrashGenerator object3 = GUIDManager.GetObject<TrashGenerator>(new Guid(trashGeneratorData2.GUID));
							if (object3 != null)
							{
								for (int m = 0; m < trashGeneratorData2.GeneratedItems.Length; m++)
								{
									TrashItem object4 = GUIDManager.GetObject<TrashItem>(new Guid(trashGeneratorData2.GeneratedItems[m]));
									if (object4 != null)
									{
										object3.AddGeneratedTrash(object4);
									}
								}
								object3.HasChanged = false;
							}
						}
					}
				}
			}
		}
	}
}
