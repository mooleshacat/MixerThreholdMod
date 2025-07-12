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
	// Token: 0x020003DE RID: 990
	public class CauldronLoader : GridItemLoader
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x00060EF4 File Offset: 0x0005F0F4
		public override string ItemType
		{
			get
			{
				return typeof(CauldronData).Name;
			}
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x00060F08 File Offset: 0x0005F108
		public override void Load(string mainPath)
		{
			CauldronLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new CauldronLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.station = (gridItem as Cauldron);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to Cauldron", null);
				return;
			}
			CauldronData data = base.GetData<CauldronData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load cauldron data", null);
				return;
			}
			data.Ingredients.LoadTo(CS$<>8__locals1.station.IngredientSlots);
			data.Liquid.LoadTo(CS$<>8__locals1.station.LiquidSlot, 0);
			data.Output.LoadTo(CS$<>8__locals1.station.OutputSlot, 0);
			if (data.RemainingCookTime > 0)
			{
				CS$<>8__locals1.station.StartCookOperation(null, data.RemainingCookTime, data.InputQuality);
			}
			string text;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out text))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<CauldronConfigurationData>(text);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x0006102C File Offset: 0x0005F22C
		public override void Load(DynamicSaveData data)
		{
			CauldronLoader.<>c__DisplayClass4_0 CS$<>8__locals1 = new CauldronLoader.<>c__DisplayClass4_0();
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
			CS$<>8__locals1.station = (gridItem as Cauldron);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to Cauldron", null);
				return;
			}
			CauldronData cauldronData;
			if (data.TryExtractBaseData<CauldronData>(out cauldronData))
			{
				if (cauldronData == null)
				{
					Console.LogWarning("Failed to load cauldron data", null);
					return;
				}
				cauldronData.Ingredients.LoadTo(CS$<>8__locals1.station.IngredientSlots);
				cauldronData.Liquid.LoadTo(CS$<>8__locals1.station.LiquidSlot, 0);
				cauldronData.Output.LoadTo(CS$<>8__locals1.station.OutputSlot, 0);
				if (cauldronData.RemainingCookTime > 0)
				{
					CS$<>8__locals1.station.StartCookOperation(null, cauldronData.RemainingCookTime, cauldronData.InputQuality);
				}
			}
			if (data.TryGetData<CauldronConfigurationData>("Configuration", out CS$<>8__locals1.configData))
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
			}
		}
	}
}
