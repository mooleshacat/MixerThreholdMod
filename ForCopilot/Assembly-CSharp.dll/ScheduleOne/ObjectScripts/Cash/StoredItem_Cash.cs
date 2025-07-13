using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.Cash
{
	// Token: 0x02000C5C RID: 3164
	public class StoredItem_Cash : StoredItem
	{
		// Token: 0x0600591D RID: 22813 RVA: 0x001788B0 File Offset: 0x00176AB0
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			this.cashInstance = (base.item as CashInstance);
			this.RefreshShownBills();
			CashInstance cashInstance = this.cashInstance;
			cashInstance.onDataChanged = (Action)Delegate.Combine(cashInstance.onDataChanged, new Action(this.RefreshShownBills));
		}

		// Token: 0x0600591E RID: 22814 RVA: 0x00178906 File Offset: 0x00176B06
		private void RefreshShownBills()
		{
			this.Visuals.ShowAmount(this.cashInstance.Balance);
		}

		// Token: 0x04004143 RID: 16707
		protected CashInstance cashInstance;

		// Token: 0x04004144 RID: 16708
		[Header("References")]
		public CashStackVisuals Visuals;
	}
}
