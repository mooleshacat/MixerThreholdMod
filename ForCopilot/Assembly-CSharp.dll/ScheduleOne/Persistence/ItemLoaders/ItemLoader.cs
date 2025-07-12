using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x0200047E RID: 1150
	public class ItemLoader
	{
		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060016C5 RID: 5829 RVA: 0x00064BE6 File Offset: 0x00062DE6
		public virtual string ItemType
		{
			get
			{
				return typeof(ItemData).Name;
			}
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x00064BF7 File Offset: 0x00062DF7
		public ItemLoader()
		{
			Singleton<LoadManager>.Instance.ItemLoaders.Add(this);
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00064C10 File Offset: 0x00062E10
		public virtual ItemInstance LoadItem(string itemString)
		{
			ItemData itemData = this.LoadData<ItemData>(itemString);
			if (itemData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (itemData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(itemData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + itemData.ID, null);
				return null;
			}
			return new StorableItemInstance(item, itemData.Quantity);
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x00064C88 File Offset: 0x00062E88
		protected T LoadData<T>(string itemString) where T : ItemData
		{
			T result = default(T);
			try
			{
				result = JsonUtility.FromJson<T>(itemString);
			}
			catch (Exception ex)
			{
				string[] array = new string[5];
				int num = 0;
				Type type = base.GetType();
				array[num] = ((type != null) ? type.ToString() : null);
				array[1] = " error parsing item data: ";
				array[2] = itemString;
				array[3] = "\n";
				int num2 = 4;
				Exception ex2 = ex;
				array[num2] = ((ex2 != null) ? ex2.ToString() : null);
				Console.LogError(string.Concat(array), null);
				return default(T);
			}
			return result;
		}
	}
}
