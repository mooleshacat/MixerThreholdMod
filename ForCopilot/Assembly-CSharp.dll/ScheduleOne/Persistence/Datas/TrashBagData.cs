using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200046F RID: 1135
	public class TrashBagData : TrashItemData
	{
		// Token: 0x060016AD RID: 5805 RVA: 0x000647DA File Offset: 0x000629DA
		public TrashBagData(string trashID, string guid, Vector3 position, Quaternion rotation, TrashContentData contents) : base(trashID, guid, position, rotation)
		{
			this.DataType = "TrashBagData";
			this.Contents = contents;
		}
	}
}
