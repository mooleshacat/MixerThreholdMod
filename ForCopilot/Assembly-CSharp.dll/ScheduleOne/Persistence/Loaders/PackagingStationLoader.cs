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
	// Token: 0x020003F0 RID: 1008
	public class PackagingStationLoader : GridItemLoader
	{
		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060015DC RID: 5596 RVA: 0x000621FA File Offset: 0x000603FA
		public override string ItemType
		{
			get
			{
				return typeof(PackagingStationData).Name;
			}
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x0006220C File Offset: 0x0006040C
		public override void Load(string mainPath)
		{
			PackagingStationLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new PackagingStationLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.station = (gridItem as PackagingStation);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to pot", null);
				return;
			}
			PackagingStationData data = base.GetData<PackagingStationData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load packaging station data data", null);
				return;
			}
			data.Contents.LoadTo(CS$<>8__locals1.station.ItemSlots);
			CS$<>8__locals1.station.UpdatePackagingVisuals();
			CS$<>8__locals1.station.UpdateProductVisuals();
			string text;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out text))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<PackagingStationConfigurationData>(text);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x000622F8 File Offset: 0x000604F8
		public override void Load(DynamicSaveData data)
		{
			PackagingStationLoader.<>c__DisplayClass4_0 CS$<>8__locals1 = new PackagingStationLoader.<>c__DisplayClass4_0();
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
			CS$<>8__locals1.station = (gridItem as PackagingStation);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to pot", null);
				return;
			}
			PackagingStationData packagingStationData;
			if (data.TryExtractBaseData<PackagingStationData>(out packagingStationData))
			{
				packagingStationData.Contents.LoadTo(CS$<>8__locals1.station.ItemSlots);
			}
			if (data.TryGetData<PotConfigurationData>("Configuration", out CS$<>8__locals1.configData))
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
			}
		}
	}
}
