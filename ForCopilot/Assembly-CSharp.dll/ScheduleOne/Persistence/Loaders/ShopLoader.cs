using System;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Shop;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003B1 RID: 945
	public class ShopLoader : Loader
	{
		// Token: 0x06001539 RID: 5433 RVA: 0x0005EA9C File Offset: 0x0005CC9C
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, out text, false))
			{
				Console.Log("Loading shop file a: " + mainPath, null);
				ShopData data = null;
				try
				{
					data = JsonUtility.FromJson<ShopData>(text);
				}
				catch (Exception ex)
				{
					Debug.LogError("Failed to load shop data: " + ex.Message);
				}
				if (data != null)
				{
					Console.Log("Found shop data", null);
					ShopInterface shopInterface = ShopInterface.AllShops.Find((ShopInterface x) => x.ShopCode == data.ShopCode);
					if (shopInterface == null)
					{
						Debug.LogError("Failed to load shop data: Shop not found: " + data.ShopCode);
						return;
					}
					shopInterface.Load(data);
					return;
				}
			}
			else
			{
				Console.Log("Failed to load shop file: " + mainPath, null);
			}
		}
	}
}
