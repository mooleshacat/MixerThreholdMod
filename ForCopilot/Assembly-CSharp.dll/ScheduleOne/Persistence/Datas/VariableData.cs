using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000474 RID: 1140
	[Serializable]
	public class VariableData : SaveData
	{
		// Token: 0x060016B2 RID: 5810 RVA: 0x00064870 File Offset: 0x00062A70
		public VariableData(string name, string value)
		{
			this.Name = name;
			this.Value = value;
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x00064886 File Offset: 0x00062A86
		public VariableData()
		{
			this.Name = "";
			this.Value = "";
		}

		// Token: 0x04001502 RID: 5378
		public string Name;

		// Token: 0x04001503 RID: 5379
		public string Value;
	}
}
