using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence
{
	// Token: 0x0200037A RID: 890
	public interface IGenericSaveable
	{
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001420 RID: 5152
		Guid GUID { get; }

		// Token: 0x06001421 RID: 5153 RVA: 0x00059181 File Offset: 0x00057381
		void InitializeSaveable()
		{
			if (!Singleton<GenericSaveablesManager>.InstanceExists)
			{
				Console.LogError("GenericSaveablesManager does not exist in scene.", null);
				return;
			}
			Singleton<GenericSaveablesManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06001422 RID: 5154
		void Load(GenericSaveData data);

		// Token: 0x06001423 RID: 5155
		GenericSaveData GetSaveData();
	}
}
