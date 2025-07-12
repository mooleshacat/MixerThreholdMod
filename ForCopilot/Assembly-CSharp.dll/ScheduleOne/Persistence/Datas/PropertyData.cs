using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000462 RID: 1122
	[Serializable]
	public class PropertyData : SaveData
	{
		// Token: 0x0600169A RID: 5786 RVA: 0x00064528 File Offset: 0x00062728
		public PropertyData(string propertyCode, bool isOwned, bool[] switchStates, bool[] toggleableStates, DynamicSaveData[] employees, DynamicSaveData[] objects)
		{
			this.PropertyCode = propertyCode;
			this.IsOwned = isOwned;
			this.SwitchStates = switchStates;
			this.ToggleableStates = toggleableStates;
			this.Employees = employees;
			this.Objects = objects;
		}

		// Token: 0x040014C9 RID: 5321
		public string PropertyCode;

		// Token: 0x040014CA RID: 5322
		public bool IsOwned;

		// Token: 0x040014CB RID: 5323
		public bool[] SwitchStates;

		// Token: 0x040014CC RID: 5324
		public bool[] ToggleableStates;

		// Token: 0x040014CD RID: 5325
		public DynamicSaveData[] Employees;

		// Token: 0x040014CE RID: 5326
		public DynamicSaveData[] Objects;
	}
}
