using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000468 RID: 1128
	[Serializable]
	public class SaveData
	{
		// Token: 0x060016A1 RID: 5793 RVA: 0x00064664 File Offset: 0x00062864
		public SaveData()
		{
			this.DataType = base.GetType().Name;
			this.DataVersion = this.GetDataVersion();
			this.GameVersion = Application.version;
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00014B5A File Offset: 0x00012D5A
		protected virtual int GetDataVersion()
		{
			return 0;
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x000646B5 File Offset: 0x000628B5
		public virtual string GetJson(bool prettyPrint = true)
		{
			if (this.DataType == string.Empty)
			{
				Type type = base.GetType();
				Console.LogError(((type != null) ? type.ToString() : null) + " GetJson() called but has no data type set!", null);
			}
			return JsonUtility.ToJson(this, prettyPrint);
		}

		// Token: 0x040014E4 RID: 5348
		public string DataType = string.Empty;

		// Token: 0x040014E5 RID: 5349
		public int DataVersion;

		// Token: 0x040014E6 RID: 5350
		public string GameVersion = string.Empty;
	}
}
