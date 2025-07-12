using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000469 RID: 1129
	[Serializable]
	public class SerializedSaveData
	{
		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060016A4 RID: 5796 RVA: 0x000646F2 File Offset: 0x000628F2
		public string Version
		{
			get
			{
				return Application.version;
			}
		}

		// Token: 0x040014E7 RID: 5351
		[NonSerialized]
		public static string _DataType;

		// Token: 0x040014E8 RID: 5352
		public string DataType = SerializedSaveData._DataType;

		// Token: 0x040014E9 RID: 5353
		[NonSerialized]
		public static int _DataVersion;

		// Token: 0x040014EA RID: 5354
		public int DataVersion = SerializedSaveData._DataVersion;
	}
}
