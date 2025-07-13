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
	// Token: 0x020003EA RID: 1002
	public class LabOvenLoader : GridItemLoader
	{
		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x00061BA9 File Offset: 0x0005FDA9
		public override string ItemType
		{
			get
			{
				return typeof(LabOvenData).Name;
			}
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x00061BBC File Offset: 0x0005FDBC
		public override void Load(string mainPath)
		{
			LabOvenLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new LabOvenLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.station = (gridItem as LabOven);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to lab oven", null);
				return;
			}
			LabOvenData data = base.GetData<LabOvenData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load lab oven data", null);
				return;
			}
			data.InputContents.LoadTo(CS$<>8__locals1.station.ItemSlots);
			data.OutputContents.LoadTo(CS$<>8__locals1.station.OutputSlot, 0);
			if (data.CurrentIngredientID != string.Empty)
			{
				OvenCookOperation operation = new OvenCookOperation(data.CurrentIngredientID, data.CurrentIngredientQuality, data.CurrentIngredientQuantity, data.CurrentProductID, data.CurrentCookProgress);
				CS$<>8__locals1.station.SetCookOperation(null, operation, false);
			}
			string text;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out text))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<LabOvenConfigurationData>(text);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00061CF0 File Offset: 0x0005FEF0
		public override void Load(DynamicSaveData data)
		{
			LabOvenLoader.<>c__DisplayClass4_0 CS$<>8__locals1 = new LabOvenLoader.<>c__DisplayClass4_0();
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
			CS$<>8__locals1.station = (gridItem as LabOven);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to lab oven", null);
				return;
			}
			LabOvenData labOvenData;
			if (data.TryExtractBaseData<LabOvenData>(out labOvenData))
			{
				labOvenData.InputContents.LoadTo(CS$<>8__locals1.station.ItemSlots);
				labOvenData.OutputContents.LoadTo(CS$<>8__locals1.station.OutputSlot, 0);
				if (labOvenData.CurrentIngredientID != string.Empty)
				{
					OvenCookOperation operation = new OvenCookOperation(labOvenData.CurrentIngredientID, labOvenData.CurrentIngredientQuality, labOvenData.CurrentIngredientQuantity, labOvenData.CurrentProductID, labOvenData.CurrentCookProgress);
					CS$<>8__locals1.station.SetCookOperation(null, operation, false);
				}
			}
			if (data.TryGetData<LabOvenConfigurationData>("Configuration", out CS$<>8__locals1.configData))
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
			}
		}
	}
}
