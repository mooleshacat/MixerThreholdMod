using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using TMPro;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000BB5 RID: 2997
	public class ItemUI_Cash : ItemUI
	{
		// Token: 0x06004F9D RID: 20381 RVA: 0x0014FD44 File Offset: 0x0014DF44
		public override void Setup(ItemInstance item)
		{
			this.cashInstance = (item as CashInstance);
			base.Setup(item);
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x0014FD59 File Offset: 0x0014DF59
		public override void UpdateUI()
		{
			base.UpdateUI();
			if (this.Destroyed)
			{
				return;
			}
			this.SetDisplayedBalance(this.cashInstance.Balance);
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x0014FD7B File Offset: 0x0014DF7B
		public void SetDisplayedBalance(float balance)
		{
			this.AmountLabel.text = MoneyManager.FormatAmount(balance, false, false);
		}

		// Token: 0x04003BAF RID: 15279
		protected CashInstance cashInstance;

		// Token: 0x04003BB0 RID: 15280
		public TextMeshProUGUI AmountLabel;
	}
}
