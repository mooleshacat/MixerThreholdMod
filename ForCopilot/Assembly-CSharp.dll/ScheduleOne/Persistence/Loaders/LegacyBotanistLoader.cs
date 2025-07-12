using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Persistence.Datas;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003C3 RID: 963
	public class LegacyBotanistLoader : LegacyEmployeeLoader
	{
		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001564 RID: 5476 RVA: 0x0005F558 File Offset: 0x0005D758
		public override string NPCType
		{
			get
			{
				return typeof(BotanistData).Name;
			}
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0005FB3C File Offset: 0x0005DD3C
		public override void Load(string mainPath)
		{
			LegacyBotanistLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new LegacyBotanistLoader.<>c__DisplayClass3_0();
			Employee employee = base.LoadAndCreateEmployee(mainPath);
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
			string text;
			if (base.TryLoadFile(mainPath, "Configuration", out text))
			{
				LegacyBotanistLoader.<>c__DisplayClass3_1 CS$<>8__locals2 = new LegacyBotanistLoader.<>c__DisplayClass3_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.configData = JsonUtility.FromJson<BotanistConfigurationData>(text);
				if (CS$<>8__locals2.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals2.<Load>g__LoadConfiguration|0));
				}
			}
			string text2;
			if (base.TryLoadFile(mainPath, "NPC", out text2))
			{
				LegacyBotanistLoader.<>c__DisplayClass3_2 CS$<>8__locals3 = new LegacyBotanistLoader.<>c__DisplayClass3_2();
				CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
				CS$<>8__locals3.data = null;
				try
				{
					CS$<>8__locals3.data = JsonUtility.FromJson<BotanistData>(text2);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (CS$<>8__locals3.data == null)
				{
					Console.LogWarning("Failed to load botanist data", null);
					return;
				}
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals3.<Load>g__LoadConfiguration|1));
			}
		}
	}
}
