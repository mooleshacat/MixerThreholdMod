using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003F3 RID: 1011
	public class PotLoader : GridItemLoader
	{
		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x00062445 File Offset: 0x00060645
		public override string ItemType
		{
			get
			{
				return typeof(PotData).Name;
			}
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x00062458 File Offset: 0x00060658
		public override void Load(string mainPath)
		{
			PotLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new PotLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.pot = (gridItem as Pot);
			if (CS$<>8__locals1.pot == null)
			{
				Console.LogWarning("Failed to cast grid item to pot", null);
				return;
			}
			string text;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out text))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<PotConfigurationData>(text);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
			PotData data = base.GetData<PotData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load pot data", null);
				return;
			}
			if (!string.IsNullOrEmpty(data.SoilID))
			{
				CS$<>8__locals1.pot.SetSoilID(data.SoilID);
				CS$<>8__locals1.pot.AddSoil(data.SoilLevel);
				CS$<>8__locals1.pot.SetSoilUses(data.RemainingSoilUses);
			}
			CS$<>8__locals1.pot.ChangeWaterAmount(data.WaterLevel);
			for (int i = 0; i < data.AppliedAdditives.Length; i++)
			{
				CS$<>8__locals1.pot.ApplyAdditive(null, data.AppliedAdditives[i], false);
			}
			CS$<>8__locals1.pot.LoadPlant(data.PlantData);
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x000625A8 File Offset: 0x000607A8
		public override void Load(DynamicSaveData data)
		{
			PotLoader.<>c__DisplayClass4_0 CS$<>8__locals1 = new PotLoader.<>c__DisplayClass4_0();
			GridItem gridItem = null;
			GridItemData data2;
			if (data.TryExtractBaseData<GridItemData>(out data2))
			{
				gridItem = base.LoadAndCreate(data2);
			}
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.pot = (gridItem as Pot);
			if (CS$<>8__locals1.pot == null)
			{
				Console.LogWarning("Failed to cast grid item to pot", null);
				return;
			}
			PotData potData;
			if (data.TryExtractBaseData<PotData>(out potData))
			{
				if (!string.IsNullOrEmpty(potData.SoilID))
				{
					CS$<>8__locals1.pot.SetSoilID(potData.SoilID);
					CS$<>8__locals1.pot.AddSoil(potData.SoilLevel);
					CS$<>8__locals1.pot.SetSoilUses(potData.RemainingSoilUses);
				}
				CS$<>8__locals1.pot.ChangeWaterAmount(potData.WaterLevel);
				for (int i = 0; i < potData.AppliedAdditives.Length; i++)
				{
					CS$<>8__locals1.pot.ApplyAdditive(null, potData.AppliedAdditives[i], false);
				}
				CS$<>8__locals1.pot.LoadPlant(potData.PlantData);
			}
			if (data.TryGetData<PotConfigurationData>("Configuration", out CS$<>8__locals1.configData))
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
			}
		}
	}
}
