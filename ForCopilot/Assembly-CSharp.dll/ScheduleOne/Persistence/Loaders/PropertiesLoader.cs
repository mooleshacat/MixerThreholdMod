using System;
using System.Collections.Generic;
using System.IO;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003AC RID: 940
	public class PropertiesLoader : Loader
	{
		// Token: 0x0600152E RID: 5422 RVA: 0x0005E0D4 File Offset: 0x0005C2D4
		public override void Load(string mainPath)
		{
			PropertyLoader propertyLoader = new PropertyLoader();
			List<FileInfo> files = base.GetFiles(mainPath);
			bool flag = false;
			if (files.Count > 0)
			{
				foreach (FileInfo fileInfo in files)
				{
					string fullName = fileInfo.FullName;
					Console.Log("Loading property file: " + fullName, null);
					string json;
					PropertyData propertyData;
					if (base.TryLoadFile(fullName, out json, false) && Loader.TryDeserialize<PropertyData>(json, out propertyData))
					{
						flag = true;
						propertyLoader.Load(propertyData);
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
					new LoadRequest(directories[i].FullName, propertyLoader);
				}
			}
		}
	}
}
