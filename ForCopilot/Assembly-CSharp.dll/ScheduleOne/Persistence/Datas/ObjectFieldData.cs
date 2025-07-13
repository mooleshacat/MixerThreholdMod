using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000430 RID: 1072
	[Serializable]
	public class ObjectFieldData
	{
		// Token: 0x06001666 RID: 5734 RVA: 0x00063D29 File Offset: 0x00061F29
		public ObjectFieldData(string objectGUID)
		{
			this.ObjectGUID = objectGUID;
		}

		// Token: 0x04001439 RID: 5177
		public string ObjectGUID;
	}
}
