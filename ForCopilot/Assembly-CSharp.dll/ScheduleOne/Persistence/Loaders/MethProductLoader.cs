using System;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003A4 RID: 932
	public class MethProductLoader : Loader
	{
		// Token: 0x06001516 RID: 5398 RVA: 0x0005D758 File Offset: 0x0005B958
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, out text, false))
			{
				MethProductData methProductData = null;
				try
				{
					methProductData = JsonUtility.FromJson<MethProductData>(text);
				}
				catch (Exception ex)
				{
					Debug.LogError("Error loading product data: " + ex.Message);
				}
				if (methProductData == null)
				{
					return;
				}
				NetworkSingleton<ProductManager>.Instance.CreateMeth_Server(methProductData.Name, methProductData.ID, methProductData.DrugType, methProductData.Properties.ToList<string>(), methProductData.AppearanceSettings);
			}
		}
	}
}
