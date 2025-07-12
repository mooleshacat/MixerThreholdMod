using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003F7 RID: 1015
	public class SoilPourerLoader : GridItemLoader
	{
		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060015F2 RID: 5618 RVA: 0x000629D9 File Offset: 0x00060BD9
		public override string ItemType
		{
			get
			{
				return typeof(SoilPourerData).Name;
			}
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x000629EC File Offset: 0x00060BEC
		public override void Load(string mainPath)
		{
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			SoilPourerData data = base.GetData<SoilPourerData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load toggleableitem data", null);
				return;
			}
			SoilPourer soilPourer = gridItem as SoilPourer;
			if (soilPourer != null)
			{
				soilPourer.SendSoil(data.SoilID);
			}
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x00062A4C File Offset: 0x00060C4C
		public override void Load(DynamicSaveData data)
		{
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
			SoilPourer soilPourer = gridItem as SoilPourer;
			if (soilPourer == null)
			{
				Console.LogWarning("Failed to cast grid item to SoilPourer", null);
				return;
			}
			SoilPourerData soilPourerData;
			if (data.TryExtractBaseData<SoilPourerData>(out soilPourerData))
			{
				soilPourer.SendSoil(soilPourerData.SoilID);
			}
		}
	}
}
