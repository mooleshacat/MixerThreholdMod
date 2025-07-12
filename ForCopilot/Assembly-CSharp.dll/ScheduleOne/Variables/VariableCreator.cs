using System;

namespace ScheduleOne.Variables
{
	// Token: 0x020002A0 RID: 672
	[Serializable]
	public class VariableCreator
	{
		// Token: 0x04000E5C RID: 3676
		public string Name;

		// Token: 0x04000E5D RID: 3677
		public VariableDatabase.EVariableType Type;

		// Token: 0x04000E5E RID: 3678
		public string InitialValue = string.Empty;

		// Token: 0x04000E5F RID: 3679
		public bool Persistent = true;

		// Token: 0x04000E60 RID: 3680
		public EVariableMode Mode;
	}
}
