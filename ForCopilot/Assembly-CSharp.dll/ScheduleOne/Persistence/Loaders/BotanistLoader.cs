using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003BC RID: 956
	public class BotanistLoader : EmployeeLoader
	{
		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x0005F558 File Offset: 0x0005D758
		public override string NPCType
		{
			get
			{
				return typeof(BotanistData).Name;
			}
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0005F574 File Offset: 0x0005D774
		public override void Load(DynamicSaveData saveData)
		{
			BotanistLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new BotanistLoader.<>c__DisplayClass3_0();
			Employee employee = base.CreateAndLoadEmployee(saveData);
			if (employee == null)
			{
				return;
			}
			CS$<>8__locals1.botanist = (employee as Botanist);
			if (CS$<>8__locals1.botanist == null)
			{
				Console.LogWarning("Failed to cast employee to botanist", null);
				return;
			}
			if (saveData.TryGetData<BotanistConfigurationData>("Configuration", out CS$<>8__locals1.configData) && CS$<>8__locals1.configData != null)
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
			}
			if (DynamicLoader.TryExtractBaseData<BotanistData>(saveData, out CS$<>8__locals1.data))
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|1));
			}
		}
	}
}
