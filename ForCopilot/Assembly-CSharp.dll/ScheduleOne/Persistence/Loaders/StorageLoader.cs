using System;
using System.IO;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003B5 RID: 949
	public class StorageLoader : Loader
	{
		// Token: 0x06001541 RID: 5441 RVA: 0x0005ECE0 File Offset: 0x0005CEE0
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
						WorldStorageEntityData worldStorageEntityData = null;
						try
						{
							worldStorageEntityData = JsonUtility.FromJson<WorldStorageEntityData>(text);
						}
						catch (Exception ex)
						{
							Debug.LogError("Error loading data: " + ex.Message);
						}
						if (worldStorageEntityData != null)
						{
							WorldStorageEntity @object = GUIDManager.GetObject<WorldStorageEntity>(new Guid(worldStorageEntityData.GUID));
							if (@object != null)
							{
								@object.Load(worldStorageEntityData);
							}
						}
					}
				}
			}
			string text2;
			if (base.TryLoadFile(mainPath, out text2, true))
			{
				WorldStorageEntitiesData worldStorageEntitiesData = JsonUtility.FromJson<WorldStorageEntitiesData>(text2);
				if (worldStorageEntitiesData != null)
				{
					Console.Log("Found world storage entities: " + worldStorageEntitiesData.Entities.Length.ToString(), null);
					foreach (WorldStorageEntityData worldStorageEntityData2 in worldStorageEntitiesData.Entities)
					{
						if (worldStorageEntityData2 != null)
						{
							WorldStorageEntity object2 = GUIDManager.GetObject<WorldStorageEntity>(new Guid(worldStorageEntityData2.GUID));
							if (object2 != null)
							{
								object2.Load(worldStorageEntityData2);
							}
						}
					}
				}
			}
		}
	}
}
