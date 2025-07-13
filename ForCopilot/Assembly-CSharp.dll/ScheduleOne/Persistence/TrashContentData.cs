using System;
using System.Collections.Generic;
using ScheduleOne.Trash;

namespace ScheduleOne.Persistence
{
	// Token: 0x02000393 RID: 915
	[Serializable]
	public class TrashContentData
	{
		// Token: 0x060014BD RID: 5309 RVA: 0x0005BE23 File Offset: 0x0005A023
		public TrashContentData()
		{
			this.TrashIDs = new string[0];
			this.TrashQuantities = new int[0];
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0005BE43 File Offset: 0x0005A043
		public TrashContentData(string[] trashIDs, int[] trashQuantities)
		{
			this.TrashIDs = trashIDs;
			this.TrashQuantities = trashQuantities;
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0005BE5C File Offset: 0x0005A05C
		public TrashContentData(List<TrashItem> trashItems)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			foreach (TrashItem trashItem in trashItems)
			{
				if (!dictionary.ContainsKey(trashItem.ID))
				{
					dictionary.Add(trashItem.ID, 0);
				}
				Dictionary<string, int> dictionary2 = dictionary;
				string id = trashItem.ID;
				int num = dictionary2[id];
				dictionary2[id] = num + 1;
			}
			this.TrashIDs = new string[dictionary.Count];
			this.TrashQuantities = new int[dictionary.Count];
			int num2 = 0;
			foreach (KeyValuePair<string, int> keyValuePair in dictionary)
			{
				this.TrashIDs[num2] = keyValuePair.Key;
				this.TrashQuantities[num2] = keyValuePair.Value;
				num2++;
			}
		}

		// Token: 0x04001362 RID: 4962
		public string[] TrashIDs;

		// Token: 0x04001363 RID: 4963
		public int[] TrashQuantities;
	}
}
