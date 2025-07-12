using System;
using ScheduleOne.Money;
using ScheduleOne.UI.Shop;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Delivery
{
	// Token: 0x02000AFF RID: 2815
	public class ListingEntry : MonoBehaviour
	{
		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06004B83 RID: 19331 RVA: 0x0013D286 File Offset: 0x0013B486
		// (set) Token: 0x06004B84 RID: 19332 RVA: 0x0013D28E File Offset: 0x0013B48E
		public ShopListing MatchingListing { get; private set; }

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06004B85 RID: 19333 RVA: 0x0013D297 File Offset: 0x0013B497
		// (set) Token: 0x06004B86 RID: 19334 RVA: 0x0013D29F File Offset: 0x0013B49F
		public int SelectedQuantity { get; private set; }

		// Token: 0x06004B87 RID: 19335 RVA: 0x0013D2A8 File Offset: 0x0013B4A8
		public void Initialize(ShopListing match)
		{
			this.MatchingListing = match;
			this.Icon.sprite = this.MatchingListing.Item.Icon;
			this.ItemNameLabel.text = this.MatchingListing.Item.Name;
			this.ItemPriceLabel.text = MoneyManager.FormatAmount(this.MatchingListing.Price, false, false);
			this.QuantityInput.onSubmit.AddListener(new UnityAction<string>(this.OnQuantityInputSubmitted));
			this.QuantityInput.onEndEdit.AddListener(delegate(string value)
			{
				this.ValidateInput();
			});
			this.IncrementButton.onClick.AddListener(delegate()
			{
				this.ChangeQuantity(1);
			});
			this.DecrementButton.onClick.AddListener(delegate()
			{
				this.ChangeQuantity(-1);
			});
			this.QuantityInput.SetTextWithoutNotify(this.SelectedQuantity.ToString());
			this.RefreshLocked();
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x0013D39E File Offset: 0x0013B59E
		public void RefreshLocked()
		{
			if (this.MatchingListing.Item.IsPurchasable)
			{
				this.LockedContainer.gameObject.SetActive(false);
				return;
			}
			this.LockedContainer.gameObject.SetActive(true);
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x0013D3D8 File Offset: 0x0013B5D8
		public void SetQuantity(int quant, bool notify = true)
		{
			if (!this.MatchingListing.Item.IsPurchasable)
			{
				quant = 0;
			}
			this.SelectedQuantity = Mathf.Clamp(quant, 0, 999);
			this.QuantityInput.SetTextWithoutNotify(this.SelectedQuantity.ToString());
			if (notify && this.onQuantityChanged != null)
			{
				this.onQuantityChanged.Invoke();
			}
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x0013D43B File Offset: 0x0013B63B
		private void ChangeQuantity(int change)
		{
			this.SetQuantity(this.SelectedQuantity + change, true);
		}

		// Token: 0x06004B8B RID: 19339 RVA: 0x0013D44C File Offset: 0x0013B64C
		private void OnQuantityInputSubmitted(string value)
		{
			int quant;
			if (int.TryParse(value, out quant))
			{
				this.SetQuantity(quant, true);
				return;
			}
			this.SetQuantity(0, true);
		}

		// Token: 0x06004B8C RID: 19340 RVA: 0x0013D474 File Offset: 0x0013B674
		private void ValidateInput()
		{
			this.OnQuantityInputSubmitted(this.QuantityInput.text);
		}

		// Token: 0x040037DE RID: 14302
		[Header("References")]
		public Image Icon;

		// Token: 0x040037DF RID: 14303
		public Text ItemNameLabel;

		// Token: 0x040037E0 RID: 14304
		public Text ItemPriceLabel;

		// Token: 0x040037E1 RID: 14305
		public InputField QuantityInput;

		// Token: 0x040037E2 RID: 14306
		public Button IncrementButton;

		// Token: 0x040037E3 RID: 14307
		public Button DecrementButton;

		// Token: 0x040037E4 RID: 14308
		public RectTransform LockedContainer;

		// Token: 0x040037E5 RID: 14309
		public UnityEvent onQuantityChanged;
	}
}
