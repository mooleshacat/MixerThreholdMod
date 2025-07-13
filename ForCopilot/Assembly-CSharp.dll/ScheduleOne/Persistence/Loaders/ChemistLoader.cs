using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003BE RID: 958
	public class ChemistLoader : EmployeeLoader
	{
		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x0005F6D1 File Offset: 0x0005D8D1
		public override string NPCType
		{
			get
			{
				return typeof(ChemistData).Name;
			}
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x0005F6E4 File Offset: 0x0005D8E4
		public override void Load(DynamicSaveData saveData)
		{
			ChemistLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new ChemistLoader.<>c__DisplayClass3_0();
			Employee employee = base.CreateAndLoadEmployee(saveData);
			if (employee == null)
			{
				return;
			}
			CS$<>8__locals1.chemist = (employee as Chemist);
			if (CS$<>8__locals1.chemist == null)
			{
				Console.LogWarning("Failed to cast employee to chemist", null);
				return;
			}
			if (saveData.TryGetData<ChemistConfigurationData>("Configuration", out CS$<>8__locals1.configData) && CS$<>8__locals1.configData != null)
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
			}
			if (DynamicLoader.TryExtractBaseData<ChemistData>(saveData, out CS$<>8__locals1.data))
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|1));
			}
		}
	}
}
