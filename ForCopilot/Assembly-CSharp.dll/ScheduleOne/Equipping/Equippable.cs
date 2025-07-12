using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000959 RID: 2393
	public class Equippable : MonoBehaviour
	{
		// Token: 0x0600408E RID: 16526 RVA: 0x00110F52 File Offset: 0x0010F152
		public virtual void Equip(ItemInstance item)
		{
			this.itemInstance = item;
			PlayerSingleton<PlayerInventory>.Instance.SetEquippable(this);
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x00110F66 File Offset: 0x0010F166
		public virtual void Unequip()
		{
			PlayerSingleton<PlayerInventory>.Instance.SetEquippable(null);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04002DFA RID: 11770
		protected ItemInstance itemInstance;

		// Token: 0x04002DFB RID: 11771
		public bool CanInteractWhenEquipped = true;

		// Token: 0x04002DFC RID: 11772
		public bool CanPickUpWhenEquipped = true;
	}
}
