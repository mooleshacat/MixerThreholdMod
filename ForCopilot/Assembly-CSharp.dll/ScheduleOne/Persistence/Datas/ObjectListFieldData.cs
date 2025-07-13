using System;
using System.Collections.Generic;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000431 RID: 1073
	[Serializable]
	public class ObjectListFieldData
	{
		// Token: 0x06001667 RID: 5735 RVA: 0x00063D38 File Offset: 0x00061F38
		public ObjectListFieldData(List<string> objectGUIDs)
		{
			this.ObjectGUIDs = objectGUIDs;
		}

		// Token: 0x0400143A RID: 5178
		public List<string> ObjectGUIDs;
	}
}
