using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x0200047A RID: 1146
	public class CashLoader : ItemLoader
	{
		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060016B9 RID: 5817 RVA: 0x00064953 File Offset: 0x00062B53
		public override string ItemType
		{
			get
			{
				return typeof(CashData).Name;
			}
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x0006496C File Offset: 0x00062B6C
		public override ItemInstance LoadItem(string itemString)
		{
			CashData cashData = base.LoadData<CashData>(itemString);
			if (cashData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (cashData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(cashData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + cashData.ID, null);
				return null;
			}
			CashInstance cashInstance = new CashInstance(item, cashData.Quantity);
			cashInstance.SetBalance(cashData.CashBalance, false);
			return cashInstance;
		}
	}
}
