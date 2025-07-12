using System;
using System.Collections.Generic;
using System.IO;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003A6 RID: 934
	public class NPCsLoader : Loader
	{
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001519 RID: 5401 RVA: 0x0005D84C File Offset: 0x0005BA4C
		public virtual string NPCType
		{
			get
			{
				return typeof(NPCCollectionData).Name;
			}
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x0005D860 File Offset: 0x0005BA60
		public override void Load(string mainPath)
		{
			NPCLoader npcloader = new NPCLoader();
			bool flag = false;
			string text;
			if (base.TryLoadFile(mainPath, out text, true))
			{
				NPCCollectionData npccollectionData = null;
				try
				{
					npccollectionData = JsonUtility.FromJson<NPCCollectionData>(text);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
					return;
				}
				if (npccollectionData != null)
				{
					flag = true;
					foreach (DynamicSaveData dynamicSaveData in npccollectionData.NPCs)
					{
						if (dynamicSaveData != null)
						{
							npcloader.Load(dynamicSaveData);
						}
					}
				}
			}
			if (!flag)
			{
				Console.Log("Loading legacy NPC stuff", null);
				List<DirectoryInfo> directories = base.GetDirectories(mainPath);
				LegacyNPCLoader loader = new LegacyNPCLoader();
				for (int j = 0; j < directories.Count; j++)
				{
					new LoadRequest(directories[j].FullName, loader);
				}
			}
		}
	}
}
