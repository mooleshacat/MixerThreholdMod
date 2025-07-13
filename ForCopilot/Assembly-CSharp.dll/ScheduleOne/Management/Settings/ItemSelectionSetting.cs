using System;
using System.Collections.Generic;

namespace ScheduleOne.Management.Settings
{
	// Token: 0x020005CD RID: 1485
	[Serializable]
	public class ItemSelectionSetting
	{
		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06002488 RID: 9352 RVA: 0x00095702 File Offset: 0x00093902
		// (set) Token: 0x06002489 RID: 9353 RVA: 0x0009570A File Offset: 0x0009390A
		public List<string> SelectedItems { get; protected set; } = new List<string>();
	}
}
