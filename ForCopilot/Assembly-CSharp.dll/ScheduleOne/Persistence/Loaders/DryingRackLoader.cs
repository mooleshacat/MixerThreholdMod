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
	// Token: 0x020003E4 RID: 996
	public class DryingRackLoader : GridItemLoader
	{
		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x00061522 File Offset: 0x0005F722
		public override string ItemType
		{
			get
			{
				return typeof(DryingRackData).Name;
			}
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x00061534 File Offset: 0x0005F734
		public override void Load(string mainPath)
		{
			DryingRackLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new DryingRackLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.station = (gridItem as DryingRack);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to DryingRack", null);
				return;
			}
			DryingRackData data = base.GetData<DryingRackData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load DryingRack data", null);
				return;
			}
			data.Input.LoadTo(CS$<>8__locals1.station.InputSlot, 0);
			data.Output.LoadTo(CS$<>8__locals1.station.OutputSlot, 0);
			for (int i = 0; i < data.DryingOperations.Length; i++)
			{
				if (data.DryingOperations[i] != null && data.DryingOperations[i].Quantity > 0 && !string.IsNullOrEmpty(data.DryingOperations[i].ItemID))
				{
					CS$<>8__locals1.station.DryingOperations.Add(data.DryingOperations[i]);
				}
			}
			CS$<>8__locals1.station.RefreshHangingVisuals();
			string text;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out text))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<DryingRackConfigurationData>(text);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x0006168C File Offset: 0x0005F88C
		public override void Load(DynamicSaveData data)
		{
			DryingRackLoader.<>c__DisplayClass4_0 CS$<>8__locals1 = new DryingRackLoader.<>c__DisplayClass4_0();
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
			CS$<>8__locals1.station = (gridItem as DryingRack);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to DryingRack", null);
				return;
			}
			DryingRackData dryingRackData;
			if (data.TryExtractBaseData<DryingRackData>(out dryingRackData))
			{
				dryingRackData.Input.LoadTo(CS$<>8__locals1.station.InputSlot, 0);
				dryingRackData.Output.LoadTo(CS$<>8__locals1.station.OutputSlot, 0);
				for (int i = 0; i < dryingRackData.DryingOperations.Length; i++)
				{
					if (dryingRackData.DryingOperations[i] != null && dryingRackData.DryingOperations[i].Quantity > 0 && !string.IsNullOrEmpty(dryingRackData.DryingOperations[i].ItemID))
					{
						CS$<>8__locals1.station.DryingOperations.Add(dryingRackData.DryingOperations[i]);
					}
				}
				CS$<>8__locals1.station.RefreshHangingVisuals();
			}
			if (data.TryGetData<DryingRackConfigurationData>("Configuration", out CS$<>8__locals1.configData))
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
			}
		}
	}
}
