using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003AD RID: 941
	public class PropertyLoader : Loader
	{
		// Token: 0x06001530 RID: 5424 RVA: 0x0005E1B0 File Offset: 0x0005C3B0
		public override void Load(string mainPath)
		{
			PropertyData propertyData = null;
			string text;
			if (base.TryLoadFile(mainPath, "Property", out text) || base.TryLoadFile(mainPath, "Business", out text))
			{
				try
				{
					propertyData = JsonUtility.FromJson<PropertyData>(text);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (propertyData != null)
				{
					Singleton<PropertyManager>.Instance.LoadProperty(propertyData);
				}
			}
			string text2 = Path.Combine(mainPath, "Objects");
			if (Directory.Exists(text2))
			{
				List<string> list = new List<string>();
				Dictionary<string, int> objectPriorities = new Dictionary<string, int>();
				BuildableItemLoader buildableItemLoader = new BuildableItemLoader();
				List<DirectoryInfo> directories = base.GetDirectories(text2);
				for (int i = 0; i < directories.Count; i++)
				{
					BuildableItemData buildableItemData = buildableItemLoader.GetBuildableItemData(directories[i].FullName);
					if (buildableItemData != null)
					{
						list.Add(directories[i].FullName);
						objectPriorities.Add(directories[i].FullName, buildableItemData.LoadOrder);
					}
				}
				list = (from x in list
				orderby objectPriorities[x]
				select x).ToList<string>();
				for (int j = 0; j < list.Count; j++)
				{
					new LoadRequest(list[j], buildableItemLoader);
				}
			}
			if (propertyData != null && propertyData.Employees != null)
			{
				foreach (DynamicSaveData dynamicSaveData in propertyData.Employees)
				{
					if (dynamicSaveData != null)
					{
						NPCLoader npcloader = Singleton<LoadManager>.Instance.GetNPCLoader(dynamicSaveData.DataType);
						if (npcloader == null)
						{
							Console.LogError("Failed to find loader for " + dynamicSaveData.DataType, null);
						}
						else
						{
							npcloader.Load(dynamicSaveData);
						}
					}
				}
				return;
			}
			string text3 = Path.Combine(mainPath, "Employees");
			if (Directory.Exists(text3))
			{
				List<DirectoryInfo> directories2 = base.GetDirectories(text3);
				for (int l = 0; l < directories2.Count; l++)
				{
					string text4;
					if (base.TryLoadFile(directories2[l].FullName, "NPC", out text4))
					{
						NPCData npcdata = null;
						try
						{
							npcdata = JsonUtility.FromJson<NPCData>(text4);
						}
						catch (Exception ex3)
						{
							string str3 = "Failed to load NPC data from ";
							string fullName = directories2[l].FullName;
							string str4 = "\n Exception: ";
							Exception ex4 = ex3;
							Console.LogWarning(str3 + fullName + str4 + ((ex4 != null) ? ex4.ToString() : null), null);
							goto IL_279;
						}
						LegacyNPCLoader legacyNPCLoader = Singleton<LoadManager>.Instance.GetLegacyNPCLoader(npcdata.DataType);
						if (legacyNPCLoader != null)
						{
							new LoadRequest(directories2[l].FullName, legacyNPCLoader);
						}
					}
					IL_279:;
				}
			}
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0005E468 File Offset: 0x0005C668
		public virtual void Load(PropertyData propertyData)
		{
			if (propertyData == null)
			{
				return;
			}
			Singleton<PropertyManager>.Instance.LoadProperty(propertyData);
			foreach (DynamicSaveData dynamicSaveData in propertyData.Objects)
			{
				if (dynamicSaveData != null)
				{
					BuildableItemLoader objectLoader = Singleton<LoadManager>.Instance.GetObjectLoader(dynamicSaveData.DataType);
					if (objectLoader == null)
					{
						Console.LogError("Failed to find loader for " + dynamicSaveData.DataType, null);
					}
					else
					{
						objectLoader.Load(dynamicSaveData);
					}
				}
			}
			foreach (DynamicSaveData dynamicSaveData2 in propertyData.Employees)
			{
				if (dynamicSaveData2 != null)
				{
					NPCLoader npcloader = Singleton<LoadManager>.Instance.GetNPCLoader(dynamicSaveData2.DataType);
					if (npcloader == null)
					{
						Console.LogError("Failed to find loader for " + dynamicSaveData2.DataType, null);
					}
					else
					{
						npcloader.Load(dynamicSaveData2);
					}
				}
			}
		}
	}
}
