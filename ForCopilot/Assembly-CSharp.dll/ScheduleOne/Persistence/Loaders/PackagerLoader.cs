using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003D8 RID: 984
	public class PackagerLoader : EmployeeLoader
	{
		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001592 RID: 5522 RVA: 0x00060631 File Offset: 0x0005E831
		public override string NPCType
		{
			get
			{
				return typeof(PackagerData).Name;
			}
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x000609F0 File Offset: 0x0005EBF0
		public override void Load(DynamicSaveData saveData)
		{
			PackagerLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new PackagerLoader.<>c__DisplayClass3_0();
			Employee employee = base.CreateAndLoadEmployee(saveData);
			if (employee == null)
			{
				return;
			}
			CS$<>8__locals1.packager = (employee as Packager);
			if (CS$<>8__locals1.packager == null)
			{
				Console.LogWarning("Failed to cast employee to packager", null);
				return;
			}
			if (saveData.TryGetData<PackagerConfigurationData>("Configuration", out CS$<>8__locals1.configData))
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
			}
			if (DynamicLoader.TryExtractBaseData<PackagerData>(saveData, out CS$<>8__locals1.data))
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|1));
			}
		}
	}
}
