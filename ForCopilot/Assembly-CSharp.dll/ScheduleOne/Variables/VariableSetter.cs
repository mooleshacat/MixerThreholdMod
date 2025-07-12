using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Variables
{
	// Token: 0x020002A3 RID: 675
	[Serializable]
	public class VariableSetter
	{
		// Token: 0x06000E13 RID: 3603 RVA: 0x0003E554 File Offset: 0x0003C754
		public void Execute()
		{
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.VariableName, this.NewValue, true);
		}

		// Token: 0x04000E6F RID: 3695
		public string VariableName;

		// Token: 0x04000E70 RID: 3696
		public string NewValue;
	}
}
