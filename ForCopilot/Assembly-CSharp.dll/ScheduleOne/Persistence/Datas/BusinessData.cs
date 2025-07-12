using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000401 RID: 1025
	[Serializable]
	public class BusinessData : PropertyData
	{
		// Token: 0x0600161E RID: 5662 RVA: 0x000634C8 File Offset: 0x000616C8
		public BusinessData(string propertyCode, bool isOwned, bool[] switchStates, bool[] toggleableStates, DynamicSaveData[] employees, DynamicSaveData[] objects, LaunderOperationData[] launderingOperations) : base(propertyCode, isOwned, switchStates, toggleableStates, employees, objects)
		{
			this.LaunderingOperations = launderingOperations;
		}

		// Token: 0x040013DF RID: 5087
		public LaunderOperationData[] LaunderingOperations;
	}
}
