using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000988 RID: 2440
	public class ItemGiver : MonoBehaviour
	{
		// Token: 0x060041B6 RID: 16822 RVA: 0x00114FCB File Offset: 0x001131CB
		public void Give()
		{
			PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.Item.GetDefaultInstance(this.Quantity));
		}

		// Token: 0x04002EE1 RID: 12001
		public ItemDefinition Item;

		// Token: 0x04002EE2 RID: 12002
		public int Quantity;
	}
}
