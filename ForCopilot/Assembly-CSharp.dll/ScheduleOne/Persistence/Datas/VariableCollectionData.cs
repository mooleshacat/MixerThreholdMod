using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000473 RID: 1139
	[Serializable]
	public class VariableCollectionData : SaveData
	{
		// Token: 0x060016B1 RID: 5809 RVA: 0x00064861 File Offset: 0x00062A61
		public VariableCollectionData(VariableData[] variables)
		{
			this.Variables = variables;
		}

		// Token: 0x04001501 RID: 5377
		public VariableData[] Variables;
	}
}
