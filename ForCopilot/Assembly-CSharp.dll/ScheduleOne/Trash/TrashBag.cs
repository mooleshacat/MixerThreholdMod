using System;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Trash
{
	// Token: 0x02000866 RID: 2150
	public class TrashBag : TrashItem
	{
		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06003A94 RID: 14996 RVA: 0x000F8240 File Offset: 0x000F6440
		// (set) Token: 0x06003A95 RID: 14997 RVA: 0x000F8248 File Offset: 0x000F6448
		public TrashContent Content { get; private set; } = new TrashContent();

		// Token: 0x06003A96 RID: 14998 RVA: 0x000F8251 File Offset: 0x000F6451
		public void LoadContent(TrashContentData data)
		{
			this.Content.LoadFromData(data);
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x000F8260 File Offset: 0x000F6460
		public override TrashItemData GetData()
		{
			return new TrashBagData(this.ID, base.GUID.ToString(), base.transform.position, base.transform.rotation, this.Content.GetData());
		}
	}
}
