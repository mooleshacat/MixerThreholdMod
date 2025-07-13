using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000447 RID: 1095
	[Serializable]
	public class BuildableItemData : SaveData
	{
		// Token: 0x0600167E RID: 5758 RVA: 0x000640B0 File Offset: 0x000622B0
		public BuildableItemData(Guid guid, ItemInstance item, int loadOrder)
		{
			this.GUID = guid.ToString();
			this.ItemString = item.GetItemData().GetJson(true);
			this.LoadOrder = loadOrder;
		}

		// Token: 0x04001478 RID: 5240
		public string GUID;

		// Token: 0x04001479 RID: 5241
		public string ItemString;

		// Token: 0x0400147A RID: 5242
		public int LoadOrder;
	}
}
