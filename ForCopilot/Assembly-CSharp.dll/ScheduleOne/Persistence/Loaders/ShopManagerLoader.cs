using System;
using System.IO;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Shop;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003B3 RID: 947
	public class ShopManagerLoader : Loader
	{
		// Token: 0x0600153D RID: 5437 RVA: 0x0005EB90 File Offset: 0x0005CD90
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, out text, true))
			{
				ShopManagerData shopManagerData = null;
				try
				{
					shopManagerData = JsonUtility.FromJson<ShopManagerData>(text);
				}
				catch (Exception ex)
				{
					Debug.LogError("Error loading data: " + ex.Message);
				}
				if (shopManagerData != null)
				{
					ShopData[] shops = shopManagerData.Shops;
					for (int i = 0; i < shops.Length; i++)
					{
						ShopData shopData = shops[i];
						if (shopData != null)
						{
							ShopInterface shopInterface = ShopInterface.AllShops.Find((ShopInterface x) => x.ShopCode == shopData.ShopCode);
							if (shopInterface == null)
							{
								Debug.LogError("Failed to load shop data: Shop not found: " + shopData.ShopCode);
								return;
							}
							shopInterface.Load(shopData);
						}
					}
					return;
				}
			}
			else
			{
				if (!Directory.Exists(mainPath))
				{
					return;
				}
				Console.Log("Loading legacy shops at: " + mainPath, null);
				ShopLoader loader = new ShopLoader();
				string[] files = Directory.GetFiles(mainPath);
				for (int j = 0; j < files.Length; j++)
				{
					Console.Log("Loading shop file: " + files[j], null);
					new LoadRequest(files[j], loader);
				}
			}
		}
	}
}
