using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003DD RID: 989
	public class BuildableItemLoader : Loader
	{
		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x060015A0 RID: 5536 RVA: 0x00060D81 File Offset: 0x0005EF81
		public virtual string ItemType
		{
			get
			{
				return typeof(BuildableItemData).Name;
			}
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00060D92 File Offset: 0x0005EF92
		public BuildableItemLoader()
		{
			Singleton<LoadManager>.Instance.ObjectLoaders.Add(this);
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x00060DAC File Offset: 0x0005EFAC
		public override void Load(string mainPath)
		{
			BuildableItemData buildableItemData = this.GetBuildableItemData(mainPath);
			if (buildableItemData != null)
			{
				BuildableItemLoader objectLoader = Singleton<LoadManager>.Instance.GetObjectLoader(buildableItemData.DataType);
				if (objectLoader != null)
				{
					new LoadRequest(mainPath, objectLoader);
				}
			}
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x00060DE0 File Offset: 0x0005EFE0
		public virtual void Load(DynamicSaveData data)
		{
			if (data == null)
			{
				Type type = base.GetType();
				Console.LogError(((type != null) ? type.ToString() : null) + " error loading data: " + ((data != null) ? data.ToString() : null), null);
				return;
			}
			BuildableItemData buildableItemData;
			if (!data.TryExtractBaseData<BuildableItemData>(out buildableItemData))
			{
				Type type2 = base.GetType();
				Console.LogError(((type2 != null) ? type2.ToString() : null) + " error loading data: " + ((data != null) ? data.ToString() : null), null);
			}
			Singleton<LoadManager>.Instance.GetObjectLoader(data.DataType).Load(data);
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x00060E6F File Offset: 0x0005F06F
		public BuildableItemData GetBuildableItemData(string mainPath)
		{
			return this.GetData<BuildableItemData>(mainPath);
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00060E78 File Offset: 0x0005F078
		protected T GetData<T>(string mainPath) where T : BuildableItemData
		{
			string text;
			if (base.TryLoadFile(mainPath, "Data", out text))
			{
				T result = default(T);
				try
				{
					result = JsonUtility.FromJson<T>(text);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				return result;
			}
			return default(T);
		}
	}
}
