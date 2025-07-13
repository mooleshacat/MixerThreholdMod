using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200098F RID: 2447
	public class ItemRemover : MonoBehaviour
	{
		// Token: 0x060041E8 RID: 16872 RVA: 0x00115774 File Offset: 0x00113974
		public void Remove()
		{
			PlayerSingleton<PlayerInventory>.Instance.RemoveAmountOfItem(this.Item.ID, (uint)this.Quantity);
		}

		// Token: 0x04002F05 RID: 12037
		public ItemDefinition Item;

		// Token: 0x04002F06 RID: 12038
		public int Quantity;
	}
}
