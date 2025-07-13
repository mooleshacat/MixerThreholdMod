using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003C0 RID: 960
	public class CleanerLoader : EmployeeLoader
	{
		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x0005F82B File Offset: 0x0005DA2B
		public override string NPCType
		{
			get
			{
				return typeof(CleanerData).Name;
			}
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x0005F83C File Offset: 0x0005DA3C
		public override void Load(DynamicSaveData saveData)
		{
			CleanerLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new CleanerLoader.<>c__DisplayClass3_0();
			Employee employee = base.CreateAndLoadEmployee(saveData);
			if (employee == null)
			{
				return;
			}
			CS$<>8__locals1.cleaner = (employee as Cleaner);
			if (CS$<>8__locals1.cleaner == null)
			{
				Console.LogWarning("Failed to cast employee to Cleaner", null);
				return;
			}
			if (saveData.TryGetData<CleanerConfigurationData>("Configuration", out CS$<>8__locals1.configData) && CS$<>8__locals1.configData != null)
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
			}
			if (DynamicLoader.TryExtractBaseData<CleanerData>(saveData, out CS$<>8__locals1.data))
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|1));
			}
		}
	}
}
