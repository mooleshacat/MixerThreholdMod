using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.ItemLoaders;
using UnityEngine;

namespace ScheduleOne.Persistence
{
	// Token: 0x0200037C RID: 892
	public static class ItemDeserializer
	{
		// Token: 0x0600143B RID: 5179 RVA: 0x000597F0 File Offset: 0x000579F0
		public static ItemInstance LoadItem(string itemString)
		{
			ItemData itemData = null;
			try
			{
				itemData = JsonUtility.FromJson<ItemData>(itemString);
			}
			catch (Exception ex)
			{
				string str = "Failed to deserialize ItemData from JSON: ";
				string str2 = "\nException: ";
				Exception ex2 = ex;
				Console.LogError(str + itemString + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				return null;
			}
			if (itemData == null)
			{
				Console.LogWarning("Failed to deserialize ItemData from JSON: " + itemString, null);
				return null;
			}
			ItemLoader itemLoader = Singleton<LoadManager>.Instance.GetItemLoader(itemData.DataType);
			if (itemLoader == null)
			{
				Console.LogError("Failed to find item loader for " + itemData.DataType, null);
				return null;
			}
			return itemLoader.LoadItem(itemString);
		}
	}
}
