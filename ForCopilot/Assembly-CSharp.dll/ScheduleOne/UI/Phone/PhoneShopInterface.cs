using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.Messaging;
using ScheduleOne.Money;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000AD8 RID: 2776
	public class PhoneShopInterface : MonoBehaviour
	{
		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06004A7C RID: 19068 RVA: 0x0013900D File Offset: 0x0013720D
		// (set) Token: 0x06004A7D RID: 19069 RVA: 0x00139015 File Offset: 0x00137215
		public bool IsOpen { get; private set; }

		// Token: 0x06004A7E RID: 19070 RVA: 0x00139020 File Offset: 0x00137220
		private void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			this.ConfirmButton.onClick.AddListener(new UnityAction(this.ConfirmOrderPressed));
			this.ItemLimitContainer.gameObject.SetActive(true);
			this.Close();
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x00139074 File Offset: 0x00137274
		public void Open(string title, string subtitle, MSGConversation _conversation, List<PhoneShopInterface.Listing> listings, float _orderLimit, float debt, Action<List<PhoneShopInterface.CartEntry>, float> _orderConfirmedCallback)
		{
			this.IsOpen = true;
			this.TitleLabel.text = title;
			this.SubtitleLabel.text = subtitle;
			this.OrderLimitLabel.text = MoneyManager.FormatAmount(_orderLimit, false, false);
			this.DebtLabel.text = MoneyManager.FormatAmount(debt, false, false);
			this.orderLimit = _orderLimit;
			this.conversation = _conversation;
			MSGConversation msgconversation = this.conversation;
			msgconversation.onMessageRendered = (Action)Delegate.Combine(msgconversation.onMessageRendered, new Action(this.Close));
			this.orderConfirmedCallback = _orderConfirmedCallback;
			this._items.Clear();
			this._items.AddRange(listings);
			using (List<PhoneShopInterface.Listing>.Enumerator enumerator = listings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PhoneShopInterface.Listing entry = enumerator.Current;
					RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.EntryPrefab, this.EntryContainer);
					rectTransform.Find("Icon").GetComponent<Image>().sprite = entry.Item.Icon;
					rectTransform.Find("Name").GetComponent<Text>().text = entry.Item.Name;
					rectTransform.Find("Price").GetComponent<Text>().text = MoneyManager.FormatAmount(entry.Price, false, false);
					rectTransform.Find("Quantity").GetComponent<Text>().text = "0";
					StorableItemDefinition item = entry.Item;
					if (!item.RequiresLevelToPurchase || NetworkSingleton<LevelManager>.Instance.GetFullRank() >= item.RequiredRank)
					{
						rectTransform.Find("Quantity/Remove").GetComponent<Button>().onClick.AddListener(delegate()
						{
							this.ChangeListingQuantity(entry, -1);
						});
						rectTransform.Find("Quantity/Add").GetComponent<Button>().onClick.AddListener(delegate()
						{
							this.ChangeListingQuantity(entry, 1);
						});
						rectTransform.Find("Locked").gameObject.SetActive(false);
					}
					else
					{
						rectTransform.Find("Locked/Title").GetComponent<Text>().text = "Unlocks at " + item.RequiredRank.ToString();
						rectTransform.Find("Locked").gameObject.SetActive(true);
					}
					this._entries.Add(rectTransform);
				}
			}
			this.CartChanged();
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x00139314 File Offset: 0x00137514
		public void Close()
		{
			this.IsOpen = false;
			this._items.Clear();
			this._cart.Clear();
			if (this.conversation != null)
			{
				MSGConversation msgconversation = this.conversation;
				msgconversation.onMessageRendered = (Action)Delegate.Remove(msgconversation.onMessageRendered, new Action(this.Close));
			}
			foreach (RectTransform rectTransform in this._entries)
			{
				UnityEngine.Object.Destroy(rectTransform.gameObject);
			}
			this._entries.Clear();
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x06004A81 RID: 19073 RVA: 0x001393D4 File Offset: 0x001375D4
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

		// Token: 0x06004A82 RID: 19074 RVA: 0x001393F8 File Offset: 0x001375F8
		private void ChangeListingQuantity(PhoneShopInterface.Listing listing, int change)
		{
			PhoneShopInterface.CartEntry cartEntry = this._cart.Find((PhoneShopInterface.CartEntry e) => e.Listing.Item.ID == listing.Item.ID);
			if (cartEntry == null)
			{
				cartEntry = new PhoneShopInterface.CartEntry(listing, 0);
				this._cart.Add(cartEntry);
			}
			cartEntry.Quantity = Mathf.Clamp(cartEntry.Quantity + change, 0, 99);
			this._entries[this._items.IndexOf(listing)].Find("Quantity").GetComponent<Text>().text = cartEntry.Quantity.ToString();
			this.CartChanged();
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x0013949D File Offset: 0x0013769D
		private void CartChanged()
		{
			this.UpdateOrderTotal();
			this.ConfirmButton.interactable = this.CanConfirmOrder();
		}

		// Token: 0x06004A84 RID: 19076 RVA: 0x001394B8 File Offset: 0x001376B8
		private void ConfirmOrderPressed()
		{
			int num;
			this.orderConfirmedCallback(this._cart, this.GetOrderTotal(out num));
			this.Close();
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x001394E4 File Offset: 0x001376E4
		private bool CanConfirmOrder()
		{
			int num;
			float orderTotal = this.GetOrderTotal(out num);
			return orderTotal > 0f && orderTotal <= this.orderLimit && num <= 10;
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x00139518 File Offset: 0x00137718
		private void UpdateOrderTotal()
		{
			int num;
			float orderTotal = this.GetOrderTotal(out num);
			this.OrderTotalLabel.text = MoneyManager.FormatAmount(orderTotal, false, false);
			this.OrderTotalLabel.color = ((orderTotal <= this.orderLimit) ? this.ValidAmountColor : this.InvalidAmountColor);
			this.ItemLimitLabel.text = num.ToString() + "/" + 10.ToString();
			this.ItemLimitLabel.color = ((num <= 10) ? Color.black : this.InvalidAmountColor);
		}

		// Token: 0x06004A87 RID: 19079 RVA: 0x001395A8 File Offset: 0x001377A8
		private float GetOrderTotal(out int itemCount)
		{
			float num = 0f;
			itemCount = 0;
			foreach (PhoneShopInterface.CartEntry cartEntry in this._cart)
			{
				num += cartEntry.Listing.Price * (float)cartEntry.Quantity;
				itemCount += cartEntry.Quantity;
			}
			return num;
		}

		// Token: 0x040036D5 RID: 14037
		public RectTransform EntryPrefab;

		// Token: 0x040036D6 RID: 14038
		public Color ValidAmountColor;

		// Token: 0x040036D7 RID: 14039
		public Color InvalidAmountColor;

		// Token: 0x040036D8 RID: 14040
		[Header("References")]
		public GameObject Container;

		// Token: 0x040036D9 RID: 14041
		public Text TitleLabel;

		// Token: 0x040036DA RID: 14042
		public Text SubtitleLabel;

		// Token: 0x040036DB RID: 14043
		public RectTransform EntryContainer;

		// Token: 0x040036DC RID: 14044
		public Text OrderTotalLabel;

		// Token: 0x040036DD RID: 14045
		public Text OrderLimitLabel;

		// Token: 0x040036DE RID: 14046
		public Text DebtLabel;

		// Token: 0x040036DF RID: 14047
		public Button ConfirmButton;

		// Token: 0x040036E0 RID: 14048
		public GameObject ItemLimitContainer;

		// Token: 0x040036E1 RID: 14049
		public Text ItemLimitLabel;

		// Token: 0x040036E2 RID: 14050
		private List<RectTransform> _entries = new List<RectTransform>();

		// Token: 0x040036E3 RID: 14051
		private List<PhoneShopInterface.Listing> _items = new List<PhoneShopInterface.Listing>();

		// Token: 0x040036E4 RID: 14052
		private List<PhoneShopInterface.CartEntry> _cart = new List<PhoneShopInterface.CartEntry>();

		// Token: 0x040036E5 RID: 14053
		private float orderLimit;

		// Token: 0x040036E6 RID: 14054
		private Action<List<PhoneShopInterface.CartEntry>, float> orderConfirmedCallback;

		// Token: 0x040036E7 RID: 14055
		private MSGConversation conversation;

		// Token: 0x02000AD9 RID: 2777
		[Serializable]
		public class Listing
		{
			// Token: 0x17000A58 RID: 2648
			// (get) Token: 0x06004A89 RID: 19081 RVA: 0x00139649 File Offset: 0x00137849
			public float Price
			{
				get
				{
					return this.Item.BasePurchasePrice;
				}
			}

			// Token: 0x06004A8A RID: 19082 RVA: 0x00139656 File Offset: 0x00137856
			public Listing(StorableItemDefinition item)
			{
				this.Item = item;
			}

			// Token: 0x040036E8 RID: 14056
			public StorableItemDefinition Item;
		}

		// Token: 0x02000ADA RID: 2778
		[Serializable]
		public class CartEntry
		{
			// Token: 0x06004A8B RID: 19083 RVA: 0x00139665 File Offset: 0x00137865
			public CartEntry(PhoneShopInterface.Listing listing, int quantity)
			{
				this.Listing = listing;
				this.Quantity = quantity;
			}

			// Token: 0x040036E9 RID: 14057
			public PhoneShopInterface.Listing Listing;

			// Token: 0x040036EA RID: 14058
			public int Quantity;
		}
	}
}
