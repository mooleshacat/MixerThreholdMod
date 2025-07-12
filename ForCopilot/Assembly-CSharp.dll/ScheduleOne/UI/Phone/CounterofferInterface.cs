using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Messaging;
using ScheduleOne.Money;
using ScheduleOne.Product;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000AD4 RID: 2772
	public class CounterofferInterface : MonoBehaviour
	{
		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06004A53 RID: 19027 RVA: 0x001384B7 File Offset: 0x001366B7
		// (set) Token: 0x06004A54 RID: 19028 RVA: 0x001384BF File Offset: 0x001366BF
		public bool IsOpen { get; private set; }

		// Token: 0x06004A55 RID: 19029 RVA: 0x001384C8 File Offset: 0x001366C8
		private void Awake()
		{
			CounterOfferProductSelector productSelector = this.ProductSelector;
			productSelector.onProductPreviewed = (Action<ProductDefinition>)Delegate.Combine(productSelector.onProductPreviewed, new Action<ProductDefinition>(this.DisplayProduct));
			CounterOfferProductSelector productSelector2 = this.ProductSelector;
			productSelector2.onProductSelected = (Action<ProductDefinition>)Delegate.Combine(productSelector2.onProductSelected, new Action<ProductDefinition>(this.SetProduct));
		}

		// Token: 0x06004A56 RID: 19030 RVA: 0x00138523 File Offset: 0x00136723
		private void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			this.Close();
		}

		// Token: 0x06004A57 RID: 19031 RVA: 0x00138540 File Offset: 0x00136740
		private void Update()
		{
			if (this.ProductSelector.IsOpen && GameInput.GetButtonUp(GameInput.ButtonCode.PrimaryClick) && this.mouseUp && !this.ProductSelector.IsMouseOverSelector())
			{
				this.ProductSelector.Close();
			}
			if (!GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
			{
				this.mouseUp = true;
			}
		}

		// Token: 0x06004A58 RID: 19032 RVA: 0x00138594 File Offset: 0x00136794
		public void Open(ProductDefinition product, int quantity, float price, MSGConversation _conversation, Action<ProductDefinition, int, float> _orderConfirmedCallback)
		{
			this.IsOpen = true;
			this.selectedProduct = product;
			this.quantity = Mathf.Clamp(quantity, 1, this.MaxQuantity);
			this.price = price;
			this.conversation = _conversation;
			MSGConversation msgconversation = this.conversation;
			msgconversation.onMessageRendered = (Action)Delegate.Combine(msgconversation.onMessageRendered, new Action(this.Close));
			this.orderConfirmedCallback = _orderConfirmedCallback;
			this.Container.gameObject.SetActive(true);
			this.SetProduct(product);
			this.PriceInput.text = price.ToString();
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x0013862C File Offset: 0x0013682C
		public void Close()
		{
			this.IsOpen = false;
			if (this.conversation != null)
			{
				MSGConversation msgconversation = this.conversation;
				msgconversation.onMessageRendered = (Action)Delegate.Remove(msgconversation.onMessageRendered, new Action(this.Close));
			}
			if (this.ProductSelector.IsOpen)
			{
				this.ProductSelector.Close();
			}
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x06004A5A RID: 19034 RVA: 0x00138698 File Offset: 0x00136898
		public void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			action.Used = true;
			this.Close();
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x001386BC File Offset: 0x001368BC
		public void Send()
		{
			float num;
			if (float.TryParse(this.PriceInput.text, out num))
			{
				this.price = num;
			}
			this.price = Mathf.Clamp(this.price, 1f, 9999f);
			this.PriceInput.SetTextWithoutNotify(this.price.ToString());
			if (this.orderConfirmedCallback != null)
			{
				this.orderConfirmedCallback(this.selectedProduct, this.quantity, this.price);
			}
			this.Close();
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x00138740 File Offset: 0x00136940
		private void UpdateFairPrice()
		{
			float amount = this.selectedProduct.MarketValue * (float)this.quantity;
			this.FairPriceLabel.text = "Fair price: " + MoneyManager.FormatAmount(amount, false, false);
		}

		// Token: 0x06004A5D RID: 19037 RVA: 0x0013877E File Offset: 0x0013697E
		private void SetProduct(ProductDefinition newProduct)
		{
			this.selectedProduct = newProduct;
			this.DisplayProduct(newProduct);
			this.UpdateFairPrice();
			this.ProductSelector.Close();
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x0013879F File Offset: 0x0013699F
		private void DisplayProduct(ProductDefinition tempProduct)
		{
			this.ProductIcon.sprite = tempProduct.Icon;
			this.UpdatePriceQuantityLabel(tempProduct.Name);
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x001387BE File Offset: 0x001369BE
		public void ChangeQuantity(int change)
		{
			this.quantity = Mathf.Clamp(this.quantity + change, 1, this.MaxQuantity);
			this.UpdatePriceQuantityLabel(this.selectedProduct.Name);
			this.UpdateFairPrice();
		}

		// Token: 0x06004A60 RID: 19040 RVA: 0x001387F4 File Offset: 0x001369F4
		private void UpdatePriceQuantityLabel(string productName)
		{
			this.ProductLabel.text = this.quantity.ToString() + "x " + productName;
			float value = -(this.ProductLabel.preferredWidth / 2f) + 20f;
			this.ProductLabelRect.anchoredPosition = new Vector2(Mathf.Clamp(value, -120f, float.MaxValue), this.ProductLabelRect.anchoredPosition.y);
		}

		// Token: 0x06004A61 RID: 19041 RVA: 0x0013886B File Offset: 0x00136A6B
		public void ChangePrice(float change)
		{
			this.price = Mathf.Clamp(this.price + change, 1f, 9999f);
			this.PriceInput.SetTextWithoutNotify(this.price.ToString());
		}

		// Token: 0x06004A62 RID: 19042 RVA: 0x001388A0 File Offset: 0x00136AA0
		public void PriceSubmitted(string value)
		{
			float num;
			if (float.TryParse(value, out num))
			{
				this.price = num;
			}
			else
			{
				this.price = 0f;
			}
			this.price = Mathf.Clamp(this.price, 1f, 9999f);
			this.PriceInput.SetTextWithoutNotify(this.price.ToString());
		}

		// Token: 0x06004A63 RID: 19043 RVA: 0x001388FC File Offset: 0x00136AFC
		public void OpenProductSelector()
		{
			if (!this.mouseUp)
			{
				return;
			}
			this.mouseUp = false;
			this.ProductSelector.Open();
		}

		// Token: 0x040036A8 RID: 13992
		public const int COUNTEROFFER_SUCCESS_XP = 5;

		// Token: 0x040036AA RID: 13994
		public const int MinQuantity = 1;

		// Token: 0x040036AB RID: 13995
		public int MaxQuantity = 50;

		// Token: 0x040036AC RID: 13996
		public const float MinPrice = 1f;

		// Token: 0x040036AD RID: 13997
		public const float MaxPrice = 9999f;

		// Token: 0x040036AE RID: 13998
		public float IconAlignment = 0.2f;

		// Token: 0x040036AF RID: 13999
		public GameObject ProductEntryPrefab;

		// Token: 0x040036B0 RID: 14000
		[Header("References")]
		public GameObject Container;

		// Token: 0x040036B1 RID: 14001
		public Text TitleLabel;

		// Token: 0x040036B2 RID: 14002
		public Button ConfirmButton;

		// Token: 0x040036B3 RID: 14003
		public Image ProductIcon;

		// Token: 0x040036B4 RID: 14004
		public Text ProductLabel;

		// Token: 0x040036B5 RID: 14005
		public RectTransform ProductLabelRect;

		// Token: 0x040036B6 RID: 14006
		public InputField PriceInput;

		// Token: 0x040036B7 RID: 14007
		public Text FairPriceLabel;

		// Token: 0x040036B8 RID: 14008
		public CounterOfferProductSelector ProductSelector;

		// Token: 0x040036B9 RID: 14009
		private Action<ProductDefinition, int, float> orderConfirmedCallback;

		// Token: 0x040036BA RID: 14010
		private ProductDefinition selectedProduct;

		// Token: 0x040036BB RID: 14011
		private int quantity;

		// Token: 0x040036BC RID: 14012
		private float price;

		// Token: 0x040036BD RID: 14013
		private Dictionary<ProductDefinition, RectTransform> productEntries = new Dictionary<ProductDefinition, RectTransform>();

		// Token: 0x040036BE RID: 14014
		private bool mouseUp;

		// Token: 0x040036BF RID: 14015
		private MSGConversation conversation;
	}
}
