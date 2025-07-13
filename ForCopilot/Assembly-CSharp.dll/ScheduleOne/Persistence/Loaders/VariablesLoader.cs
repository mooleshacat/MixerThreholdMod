using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003B8 RID: 952
	public class VariablesLoader : Loader
	{
		// Token: 0x06001547 RID: 5447 RVA: 0x0005F288 File Offset: 0x0005D488
		public override void Load(string mainPath)
		{
			Console.Log("Loading variables at: " + mainPath, null);
			string text;
			if (base.TryLoadFile(mainPath, out text, true))
			{
				VariableCollectionData variableCollectionData = JsonUtility.FromJson<VariableCollectionData>(text);
				if (variableCollectionData != null)
				{
					foreach (VariableData variableData in variableCollectionData.Variables)
					{
						if (variableData != null)
						{
							NetworkSingleton<VariableDatabase>.Instance.LoadVariable(variableData);
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
				Console.Log("Loading legacy variables", null);
				string[] files = Directory.GetFiles(mainPath);
				for (int j = 0; j < files.Length; j++)
				{
					string text2;
					if (base.TryLoadFile(files[j], out text2, false))
					{
						VariableData variableData2 = null;
						try
						{
							variableData2 = JsonUtility.FromJson<VariableData>(text2);
						}
						catch (Exception ex)
						{
							Debug.LogError("Error loading quest data: " + ex.Message);
						}
						if (variableData2 != null)
						{
							NetworkSingleton<VariableDatabase>.Instance.LoadVariable(variableData2);
						}
					}
				}
			}
		}
	}
}
