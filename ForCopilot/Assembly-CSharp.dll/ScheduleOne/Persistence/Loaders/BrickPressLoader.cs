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
	// Token: 0x020003DA RID: 986
	public class BrickPressLoader : GridItemLoader
	{
		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001598 RID: 5528 RVA: 0x00060B45 File Offset: 0x0005ED45
		public override string ItemType
		{
			get
			{
				return typeof(BrickPressData).Name;
			}
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x00060B60 File Offset: 0x0005ED60
		public override void Load(string mainPath)
		{
			BrickPressLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new BrickPressLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.brickPress = (gridItem as BrickPress);
			if (CS$<>8__locals1.brickPress == null)
			{
				Console.LogWarning("Failed to cast grid item to brick press", null);
				return;
			}
			BrickPressData data = base.GetData<BrickPressData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load brick press data", null);
				return;
			}
			data.Contents.LoadTo(CS$<>8__locals1.brickPress.ItemSlots);
			string text;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out text))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<BrickPressConfigurationData>(text);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x00060C34 File Offset: 0x0005EE34
		public override void Load(DynamicSaveData data)
		{
			BrickPressLoader.<>c__DisplayClass4_0 CS$<>8__locals1 = new BrickPressLoader.<>c__DisplayClass4_0();
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
			CS$<>8__locals1.brickPress = (gridItem as BrickPress);
			if (CS$<>8__locals1.brickPress == null)
			{
				Console.LogWarning("Failed to cast grid item to brick press", null);
				return;
			}
			BrickPressData brickPressData;
			if (data.TryExtractBaseData<BrickPressData>(out brickPressData))
			{
				brickPressData.Contents.LoadTo(CS$<>8__locals1.brickPress.ItemSlots);
			}
			if (data.TryGetData<BrickPressConfigurationData>("Configuration", out CS$<>8__locals1.configData))
			{
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
			}
		}
	}
}
