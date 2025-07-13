using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000472 RID: 1138
	[Serializable]
	public class TrashItemData
	{
		// Token: 0x060016B0 RID: 5808 RVA: 0x00064826 File Offset: 0x00062A26
		public TrashItemData(string trashID, string guid, Vector3 position, Quaternion rotation)
		{
			this.DataType = "TrashItemData";
			this.TrashID = trashID;
			this.GUID = guid;
			this.Position = position;
			this.Rotation = rotation;
		}

		// Token: 0x040014FB RID: 5371
		public string DataType = string.Empty;

		// Token: 0x040014FC RID: 5372
		public string TrashID;

		// Token: 0x040014FD RID: 5373
		public string GUID;

		// Token: 0x040014FE RID: 5374
		public Vector3 Position;

		// Token: 0x040014FF RID: 5375
		public Quaternion Rotation;

		// Token: 0x04001500 RID: 5376
		public TrashContentData Contents;
	}
}
