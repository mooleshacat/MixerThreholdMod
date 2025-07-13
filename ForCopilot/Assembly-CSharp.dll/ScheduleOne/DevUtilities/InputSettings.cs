using System;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000746 RID: 1862
	[Serializable]
	public class InputSettings
	{
		// Token: 0x04002363 RID: 9059
		public float MouseSensitivity;

		// Token: 0x04002364 RID: 9060
		public bool InvertMouse;

		// Token: 0x04002365 RID: 9061
		public InputSettings.EActionMode SprintMode;

		// Token: 0x04002366 RID: 9062
		public string BindingOverrides;

		// Token: 0x02000747 RID: 1863
		public enum EActionMode
		{
			// Token: 0x04002368 RID: 9064
			Press,
			// Token: 0x04002369 RID: 9065
			Hold
		}
	}
}
