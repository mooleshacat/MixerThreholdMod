using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003A0 RID: 928
	public class GenericSaveablesLoader : Loader
	{
		// Token: 0x06001509 RID: 5385 RVA: 0x0005D458 File Offset: 0x0005B658
		public override void Load(string mainPath)
		{
			if (Directory.Exists(mainPath))
			{
				string[] files = Directory.GetFiles(mainPath);
				for (int i = 0; i < files.Length; i++)
				{
					string text;
					if (base.TryLoadFile(files[i], out text, false))
					{
						GenericSaveData genericSaveData = null;
						try
						{
							genericSaveData = JsonUtility.FromJson<GenericSaveData>(text);
						}
						catch (Exception ex)
						{
							Debug.LogError("Error loading generic save data: " + ex.Message);
						}
						if (genericSaveData != null)
						{
							Singleton<GenericSaveablesManager>.Instance.LoadSaveable(genericSaveData);
						}
					}
				}
			}
			string text2;
			if (base.TryLoadFile(mainPath, out text2, true))
			{
				GenericSaveablesData genericSaveablesData = JsonUtility.FromJson<GenericSaveablesData>(text2);
				if (genericSaveablesData != null)
				{
					foreach (GenericSaveData genericSaveData2 in genericSaveablesData.Saveables)
					{
						if (genericSaveData2 != null)
						{
							Singleton<GenericSaveablesManager>.Instance.LoadSaveable(genericSaveData2);
						}
					}
					return;
				}
			}
			else
			{
				Console.LogWarning("Failed to load generic saveables data from: " + mainPath, null);
			}
		}
	}
}
