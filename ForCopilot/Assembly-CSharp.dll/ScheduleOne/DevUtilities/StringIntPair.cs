using System;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000737 RID: 1847
	[Serializable]
	public class StringIntPair
	{
		// Token: 0x060031E8 RID: 12776 RVA: 0x000D03B8 File Offset: 0x000CE5B8
		public StringIntPair(string str, int i)
		{
			this.String = str;
			this.Int = i;
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x0000494F File Offset: 0x00002B4F
		public StringIntPair()
		{
		}

		// Token: 0x04002326 RID: 8998
		public string String;

		// Token: 0x04002327 RID: 8999
		public int Int;
	}
}
