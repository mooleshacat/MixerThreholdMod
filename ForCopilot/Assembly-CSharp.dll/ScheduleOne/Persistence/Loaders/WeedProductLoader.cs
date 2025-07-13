using System;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003BB RID: 955
	public class WeedProductLoader : Loader
	{
		// Token: 0x0600154D RID: 5453 RVA: 0x0005F4D8 File Offset: 0x0005D6D8
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, out text, false))
			{
				WeedProductData weedProductData = null;
				try
				{
					weedProductData = JsonUtility.FromJson<WeedProductData>(text);
				}
				catch (Exception ex)
				{
					Debug.LogError("Error loading product data: " + ex.Message);
				}
				if (weedProductData == null)
				{
					return;
				}
				NetworkSingleton<ProductManager>.Instance.CreateWeed_Server(weedProductData.Name, weedProductData.ID, weedProductData.DrugType, weedProductData.Properties.ToList<string>(), weedProductData.AppearanceSettings);
			}
		}
	}
}
