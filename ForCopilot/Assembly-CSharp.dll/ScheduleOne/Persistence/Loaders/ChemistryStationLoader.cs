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
	// Token: 0x020003E1 RID: 993
	public class ChemistryStationLoader : GridItemLoader
	{
		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060015AE RID: 5550 RVA: 0x000611D9 File Offset: 0x0005F3D9
		public override string ItemType
		{
			get
			{
				return typeof(ChemistryStationData).Name;
			}
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x000611EC File Offset: 0x0005F3EC
		public override void Load(string mainPath)
		{
			ChemistryStationLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new ChemistryStationLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.station = (gridItem as ChemistryStation);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to chemistry station", null);
				return;
			}
			ChemistryStationData data = base.GetData<ChemistryStationData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load chemistry station data", null);
				return;
			}
			data.InputContents.LoadTo(CS$<>8__locals1.station.ItemSlots);
			data.OutputContents.LoadTo(CS$<>8__locals1.station.OutputSlot, 0);
			if (data.CurrentRecipeID != string.Empty)
			{
				ChemistryCookOperation operation = new ChemistryCookOperation(data.CurrentRecipeID, data.ProductQuality, data.StartLiquidColor, data.LiquidLevel, data.CurrentTime);
				CS$<>8__locals1.station.SetCookOperation(null, operation);
			}
			string text;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out text))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<ChemistryStationConfigurationData>(text);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0006131C File Offset: 0x0005F51C
		public override void Load(DynamicSaveData data)
		{
			ChemistryStationLoader.<>c__DisplayClass4_0 CS$<>8__locals1 = new ChemistryStationLoader.<>c__DisplayClass4_0();
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
			CS$<>8__locals1.station = (gridItem as ChemistryStation);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to chemistry station", null);
				return;
			}
			ChemistryStationData chemistryStationData;
			if (data.TryExtractBaseData<ChemistryStationData>(out chemistryStationData))
			{
				if (chemistryStationData == null)
				{
					Console.LogWarning("Failed to load chemistry station data", null);
					return;
				}
				chemistryStationData.InputContents.LoadTo(CS$<>8__locals1.station.ItemSlots);
				chemistryStationData.OutputContents.LoadTo(CS$<>8__locals1.station.OutputSlot, 0);
				if (chemistryStationData.CurrentRecipeID != string.Empty)
				{
					ChemistryCookOperation operation = new ChemistryCookOperation(chemistryStationData.CurrentRecipeID, chemistryStationData.ProductQuality, chemistryStationData.StartLiquidColor, chemistryStationData.LiquidLevel, chemistryStationData.CurrentTime);
					CS$<>8__locals1.station.SetCookOperation(null, operation);
				}
			}
			if (data.TryGetData<ChemistryStationConfigurationData>("Configuration", out CS$<>8__locals1.configData))
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
			}
		}
	}
}
