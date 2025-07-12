using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003A3 RID: 931
	public class MetadataLoader : Loader
	{
		// Token: 0x06001514 RID: 5396 RVA: 0x0005D728 File Offset: 0x0005B928
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, out text, true))
			{
				MetaData metaData = JsonUtility.FromJson<MetaData>(text);
				if (metaData != null)
				{
					Singleton<MetadataManager>.Instance.Load(metaData);
				}
			}
		}
	}
}
