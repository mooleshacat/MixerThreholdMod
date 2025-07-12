using System;
using System.Collections.Generic;
using System.IO;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200039A RID: 922
	public class BusinessesLoader : Loader
	{
		// Token: 0x060014F8 RID: 5368 RVA: 0x0005CF0C File Offset: 0x0005B10C
		public override void Load(string mainPath)
		{
			BusinessLoader businessLoader = new BusinessLoader();
			List<FileInfo> files = base.GetFiles(mainPath);
			bool flag = false;
			if (files.Count > 0)
			{
				flag = true;
				foreach (FileInfo fileInfo in files)
				{
					string fullName = fileInfo.FullName;
					string json;
					BusinessData propertyData;
					if (base.TryLoadFile(fullName, out json, false) && Loader.TryDeserialize<BusinessData>(json, out propertyData))
					{
						businessLoader.Load(propertyData);
					}
				}
			}
			if (!flag)
			{
				if (!Directory.Exists(mainPath))
				{
					return;
				}
				List<DirectoryInfo> directories = base.GetDirectories(mainPath);
				for (int i = 0; i < directories.Count; i++)
				{
					new LoadRequest(directories[i].FullName, businessLoader);
				}
			}
		}
	}
}
