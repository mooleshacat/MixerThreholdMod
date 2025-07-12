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
	// Token: 0x020003ED RID: 1005
	public class MixingStationLoader : GridItemLoader
	{
		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060015D4 RID: 5588 RVA: 0x00061E9D File Offset: 0x0006009D
		public override string ItemType
		{
			get
			{
				return typeof(MixingStationData).Name;
			}
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x00061EB0 File Offset: 0x000600B0
		public override void Load(string mainPath)
		{
			MixingStationLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new MixingStationLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.station = (gridItem as MixingStation);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to mixing station", null);
				return;
			}
			MixingStationData data = base.GetData<MixingStationData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load mixing station data", null);
				return;
			}
			data.ProductContents.LoadTo(CS$<>8__locals1.station.ProductSlot, 0);
			data.MixerContents.LoadTo(CS$<>8__locals1.station.MixerSlot, 0);
			data.OutputContents.LoadTo(CS$<>8__locals1.station.OutputSlot, 0);
			if (data.CurrentMixOperation != null)
			{
				CS$<>8__locals1.station.SetMixOperation(null, data.CurrentMixOperation, data.CurrentMixTime);
				if (data.CurrentMixTime >= CS$<>8__locals1.station.GetMixTimeForCurrentOperation())
				{
					CS$<>8__locals1.station.MixingDone_Networked();
				}
			}
			string text;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out text))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<MixingStationConfigurationData>(text);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x00061FF4 File Offset: 0x000601F4
		public override void Load(DynamicSaveData data)
		{
			MixingStationLoader.<>c__DisplayClass4_0 CS$<>8__locals1 = new MixingStationLoader.<>c__DisplayClass4_0();
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
			CS$<>8__locals1.station = (gridItem as MixingStation);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to mixing station", null);
				return;
			}
			MixingStationData mixingStationData;
			if (data.TryExtractBaseData<MixingStationData>(out mixingStationData))
			{
				mixingStationData.ProductContents.LoadTo(CS$<>8__locals1.station.ProductSlot, 0);
				mixingStationData.MixerContents.LoadTo(CS$<>8__locals1.station.MixerSlot, 0);
				mixingStationData.OutputContents.LoadTo(CS$<>8__locals1.station.OutputSlot, 0);
				if (mixingStationData.CurrentMixOperation != null)
				{
					CS$<>8__locals1.station.SetMixOperation(null, mixingStationData.CurrentMixOperation, mixingStationData.CurrentMixTime);
					if (mixingStationData.CurrentMixTime >= CS$<>8__locals1.station.GetMixTimeForCurrentOperation())
					{
						CS$<>8__locals1.station.MixingDone_Networked();
					}
				}
			}
			if (data.TryGetData<MixingStationConfigurationData>("Configuration", out CS$<>8__locals1.configData))
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
			}
		}
	}
}
