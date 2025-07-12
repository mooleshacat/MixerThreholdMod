using System;
using ScheduleOne.Money;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000BA2 RID: 2978
	public class ListingUI : MonoBehaviour
	{
		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06004F07 RID: 20231 RVA: 0x0014DDA8 File Offset: 0x0014BFA8
		// (set) Token: 0x06004F08 RID: 20232 RVA: 0x0014DDB0 File Offset: 0x0014BFB0
		public ShopListing Listing { get; protected set; }

		// Token: 0x06004F09 RID: 20233 RVA: 0x0014DDBC File Offset: 0x0014BFBC
		public virtual void Initialize(ShopListing listing)
		{
			this.Listing = listing;
			this.Icon.sprite = listing.Item.Icon;
			this.Icon.color = (listing.UseIconTint ? listing.IconTint : Color.white);
			this.NameLabel.text = listing.Item.Name;
			this.UpdatePrice();
			this.UpdateStock();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = 0;
			entry.callback.AddListener(delegate(BaseEventData <p0>)
			{
				this.HoverStart();
			});
			this.Trigger.triggers.Add(entry);
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			entry2.eventID = 1;
			entry2.callback.AddListener(delegate(BaseEventData <p0>)
			{
				this.HoverEnd();
			});
			this.Trigger.triggers.Add(entry2);
			listing.onStockChanged = (Action)Delegate.Combine(listing.onStockChanged, new Action(this.StockChanged));
			this.BuyButton.onClick.AddListener(new UnityAction(this.Clicked));
			this.DropdownButton.onClick.AddListener(new UnityAction(this.DropdownClicked));
			this.UpdateLockStatus();
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x0014DEF2 File Offset: 0x0014C0F2
		public virtual RectTransform GetIconCopy(RectTransform parent)
		{
			return UnityEngine.Object.Instantiate<GameObject>(this.Icon.gameObject, parent).GetComponent<RectTransform>();
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x0014DF0A File Offset: 0x0014C10A
		public void Update()
		{
			this.UpdateButtons();
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x0014DF12 File Offset: 0x0014C112
		private void Clicked()
		{
			if (this.onClicked != null)
			{
				this.onClicked();
			}
		}

		// Token: 0x06004F0D RID: 20237 RVA: 0x0014DF27 File Offset: 0x0014C127
		private void DropdownClicked()
		{
			if (this.onDropdownClicked != null)
			{
				this.onDropdownClicked();
			}
		}

		// Token: 0x06004F0E RID: 20238 RVA: 0x0014DF3C File Offset: 0x0014C13C
		private void HoverStart()
		{
			if (this.hoverStart != null)
			{
				this.hoverStart();
			}
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x0014DF51 File Offset: 0x0014C151
		private void HoverEnd()
		{
			if (this.hoverEnd != null)
			{
				this.hoverEnd();
			}
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x0014DF66 File Offset: 0x0014C166
		private void StockChanged()
		{
			this.UpdateButtons();
			this.UpdatePrice();
			this.UpdateStock();
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x0014DF7A File Offset: 0x0014C17A
		private void UpdatePrice()
		{
			this.PriceLabel.text = MoneyManager.FormatAmount(this.Listing.Price, false, false);
			this.PriceLabel.color = ListingUI.PriceLabelColor_Normal;
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x0014DFB0 File Offset: 0x0014C1B0
		private void UpdateStock()
		{
			if (this.StockLabel == null)
			{
				return;
			}
			if (this.Listing.IsUnlimitedStock)
			{
				this.StockLabel.enabled = false;
				return;
			}
			int currentStockMinusCart = this.Listing.CurrentStockMinusCart;
			this.StockLabel.text = currentStockMinusCart.ToString() + " / " + this.Listing.DefaultStock.ToString();
			if (currentStockMinusCart > 0)
			{
				this.StockLabel.color = this.StockLabelDefault;
			}
			else
			{
				this.StockLabel.text = "Out of stock";
				this.StockLabel.color = this.StockLabelNone;
			}
			if (currentStockMinusCart == 1 && this.Listing.RestockRate == ShopListing.ERestockRate.Never)
			{
				this.StockLabel.text = "1 of 1";
			}
			this.StockLabel.enabled = true;
		}

		// Token: 0x06004F13 RID: 20243 RVA: 0x0014E090 File Offset: 0x0014C290
		private void UpdateButtons()
		{
			bool interactable = this.CanAddToCart();
			this.BuyButton.interactable = interactable;
			this.DropdownButton.interactable = interactable;
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x0014E0BC File Offset: 0x0014C2BC
		public bool CanAddToCart()
		{
			return this.Listing.IsUnlimitedStock || this.Listing.CurrentStockMinusCart > 0;
		}

		// Token: 0x06004F15 RID: 20245 RVA: 0x0014E0DB File Offset: 0x0014C2DB
		public void UpdateLockStatus()
		{
			this.LockedContainer.gameObject.SetActive(!this.Listing.Item.IsPurchasable);
		}

		// Token: 0x04003B35 RID: 15157
		public static Color32 PriceLabelColor_Normal = new Color32(90, 185, 90, byte.MaxValue);

		// Token: 0x04003B36 RID: 15158
		public static Color32 PriceLabelColor_NoStock = new Color32(165, 70, 60, byte.MaxValue);

		// Token: 0x04003B38 RID: 15160
		[Header("Colors")]
		public Color32 StockLabelDefault = new Color32(40, 40, 40, byte.MaxValue);

		// Token: 0x04003B39 RID: 15161
		public Color32 StockLabelNone = new Color32(185, 55, 55, byte.MaxValue);

		// Token: 0x04003B3A RID: 15162
		[Header("References")]
		public Image Icon;

		// Token: 0x04003B3B RID: 15163
		public TextMeshProUGUI NameLabel;

		// Token: 0x04003B3C RID: 15164
		public TextMeshProUGUI PriceLabel;

		// Token: 0x04003B3D RID: 15165
		public TextMeshProUGUI StockLabel;

		// Token: 0x04003B3E RID: 15166
		public GameObject LockedContainer;

		// Token: 0x04003B3F RID: 15167
		public Button BuyButton;

		// Token: 0x04003B40 RID: 15168
		public Button DropdownButton;

		// Token: 0x04003B41 RID: 15169
		public EventTrigger Trigger;

		// Token: 0x04003B42 RID: 15170
		public RectTransform DetailPanelAnchor;

		// Token: 0x04003B43 RID: 15171
		public RectTransform DropdownAnchor;

		// Token: 0x04003B44 RID: 15172
		public RectTransform TopDropdownAnchor;

		// Token: 0x04003B45 RID: 15173
		public Action hoverStart;

		// Token: 0x04003B46 RID: 15174
		public Action hoverEnd;

		// Token: 0x04003B47 RID: 15175
		public Action onClicked;

		// Token: 0x04003B48 RID: 15176
		public Action onDropdownClicked;
	}
}
