using System;
using ScheduleOne.Money;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000B9C RID: 2972
	public class CartEntry : MonoBehaviour
	{
		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06004EE6 RID: 20198 RVA: 0x0014D85B File Offset: 0x0014BA5B
		// (set) Token: 0x06004EE7 RID: 20199 RVA: 0x0014D863 File Offset: 0x0014BA63
		public int Quantity { get; protected set; }

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06004EE8 RID: 20200 RVA: 0x0014D86C File Offset: 0x0014BA6C
		// (set) Token: 0x06004EE9 RID: 20201 RVA: 0x0014D874 File Offset: 0x0014BA74
		public Cart Cart { get; protected set; }

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06004EEA RID: 20202 RVA: 0x0014D87D File Offset: 0x0014BA7D
		// (set) Token: 0x06004EEB RID: 20203 RVA: 0x0014D885 File Offset: 0x0014BA85
		public ShopListing Listing { get; protected set; }

		// Token: 0x06004EEC RID: 20204 RVA: 0x0014D890 File Offset: 0x0014BA90
		public void Initialize(Cart cart, ShopListing listing, int quantity)
		{
			this.Cart = cart;
			this.Listing = listing;
			this.Quantity = quantity;
			this.IncrementButton.onClick.AddListener(delegate()
			{
				this.ChangeAmount(1);
			});
			this.DecrementButton.onClick.AddListener(delegate()
			{
				this.ChangeAmount(-1);
			});
			this.RemoveButton.onClick.AddListener(delegate()
			{
				this.ChangeAmount(-999);
			});
			this.UpdateTitle();
			this.UpdatePrice();
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x0014D912 File Offset: 0x0014BB12
		public void SetQuantity(int quantity)
		{
			this.Quantity = quantity;
			this.UpdateTitle();
			this.UpdatePrice();
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x0014D928 File Offset: 0x0014BB28
		protected virtual void UpdateTitle()
		{
			this.NameLabel.text = this.Quantity.ToString() + "x " + this.Listing.Item.Name;
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x0014D968 File Offset: 0x0014BB68
		private void UpdatePrice()
		{
			this.PriceLabel.text = MoneyManager.FormatAmount((float)this.Quantity * this.Listing.Price, false, false);
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x0014D98F File Offset: 0x0014BB8F
		private void ChangeAmount(int change)
		{
			if (change > 0)
			{
				this.Cart.AddItem(this.Listing, change);
				return;
			}
			if (change < 0)
			{
				this.Cart.RemoveItem(this.Listing, -change);
			}
		}

		// Token: 0x04003B13 RID: 15123
		[Header("References")]
		public TextMeshProUGUI NameLabel;

		// Token: 0x04003B14 RID: 15124
		public TextMeshProUGUI PriceLabel;

		// Token: 0x04003B15 RID: 15125
		public Button IncrementButton;

		// Token: 0x04003B16 RID: 15126
		public Button DecrementButton;

		// Token: 0x04003B17 RID: 15127
		public Button RemoveButton;
	}
}
