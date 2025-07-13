using System;
using System.Collections.Generic;
using ScheduleOne.Delivery;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.Property;
using ScheduleOne.UI.Shop;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Delivery
{
	// Token: 0x02000AFD RID: 2813
	public class DeliveryShop : MonoBehaviour
	{
		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06004B5F RID: 19295 RVA: 0x0013C5C7 File Offset: 0x0013A7C7
		// (set) Token: 0x06004B60 RID: 19296 RVA: 0x0013C5CF File Offset: 0x0013A7CF
		public ShopInterface MatchingShop { get; private set; }

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06004B61 RID: 19297 RVA: 0x0013C5D8 File Offset: 0x0013A7D8
		// (set) Token: 0x06004B62 RID: 19298 RVA: 0x0013C5E0 File Offset: 0x0013A7E0
		public bool IsExpanded { get; private set; }

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06004B63 RID: 19299 RVA: 0x0013C5E9 File Offset: 0x0013A7E9
		// (set) Token: 0x06004B64 RID: 19300 RVA: 0x0013C5F1 File Offset: 0x0013A7F1
		public bool IsAvailable { get; private set; }

		// Token: 0x06004B65 RID: 19301 RVA: 0x0013C5FC File Offset: 0x0013A7FC
		private void Start()
		{
			this.MatchingShop = ShopInterface.AllShops.Find((ShopInterface x) => x.ShopName == this.MatchingShopInterfaceName);
			if (this.MatchingShop == null)
			{
				Debug.LogError("Could not find shop interface with name " + this.MatchingShopInterfaceName);
				return;
			}
			foreach (ShopListing shopListing in this.MatchingShop.Listings)
			{
				if (shopListing.CanBeDelivered)
				{
					ListingEntry listingEntry = UnityEngine.Object.Instantiate<ListingEntry>(this.ListingEntryPrefab, this.ListingContainer);
					listingEntry.Initialize(shopListing);
					listingEntry.onQuantityChanged.AddListener(new UnityAction(this.RefreshCart));
					this.listingEntries.Add(listingEntry);
				}
			}
			this.DeliveryFeeLabel.text = MoneyManager.FormatAmount(this.DeliveryFee, false, false);
			int num = Mathf.CeilToInt((float)this.listingEntries.Count / 2f);
			this.ContentsContainer.sizeDelta = new Vector2(this.ContentsContainer.sizeDelta.x, 230f + (float)num * 60f);
			this.HeaderButton.onClick.AddListener(delegate()
			{
				this.SetIsExpanded(!this.IsExpanded);
			});
			this.OrderButton.onClick.AddListener(new UnityAction(this.OrderPressed));
			this.DestinationDropdown.onValueChanged.AddListener(new UnityAction<int>(this.DestinationDropdownSelected));
			this.LoadingDockDropdown.onValueChanged.AddListener(new UnityAction<int>(this.LoadingDockDropdownSelected));
			this.SetIsExpanded(false);
			if (this.AvailableByDefault)
			{
				this.SetIsAvailable();
			}
			else
			{
				base.gameObject.SetActive(false);
			}
			this.MatchingShop.DeliveryVehicle.Deactivate();
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x0013C7D4 File Offset: 0x0013A9D4
		private void FixedUpdate()
		{
			if (this.IsExpanded && PlayerSingleton<DeliveryApp>.Instance.isOpen)
			{
				this.RefreshOrderButton();
			}
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x0013C7F0 File Offset: 0x0013A9F0
		public void SetIsExpanded(bool expanded)
		{
			this.IsExpanded = expanded;
			this.ContentsContainer.gameObject.SetActive(this.IsExpanded);
			this.HeaderImage.sprite = (this.IsExpanded ? this.HeaderImage_Expanded : this.HeaderImage_Hidden);
			this.HeaderArrow.localRotation = (this.IsExpanded ? Quaternion.Euler(0f, 0f, 270f) : Quaternion.Euler(0f, 0f, 180f));
			PlayerSingleton<DeliveryApp>.Instance.RefreshContent(true);
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x0013C883 File Offset: 0x0013AA83
		public void SetIsAvailable()
		{
			this.IsAvailable = true;
			base.gameObject.SetActive(true);
			PlayerSingleton<DeliveryApp>.Instance.RefreshContent(true);
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x0013C8A4 File Offset: 0x0013AAA4
		public void OrderPressed()
		{
			string str;
			if (!this.CanOrder(out str))
			{
				Debug.LogWarning("Cannot order: " + str);
				return;
			}
			float orderTotal = this.GetOrderTotal();
			List<StringIntPair> list = new List<StringIntPair>();
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				if (listingEntry.SelectedQuantity > 0)
				{
					list.Add(new StringIntPair(listingEntry.MatchingListing.Item.ID, listingEntry.SelectedQuantity));
				}
			}
			int orderItemCount = this.GetOrderItemCount();
			int timeUntilArrival = Mathf.RoundToInt(Mathf.Lerp(60f, 360f, Mathf.Clamp01((float)orderItemCount / 160f)));
			DeliveryInstance delivery = new DeliveryInstance(GUIDManager.GenerateUniqueGUID().ToString(), this.MatchingShopInterfaceName, this.destinationProperty.PropertyCode, this.loadingDockIndex - 1, list.ToArray(), EDeliveryStatus.InTransit, timeUntilArrival);
			NetworkSingleton<DeliveryManager>.Instance.SendDelivery(delivery);
			NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Delivery from " + this.MatchingShop.ShopName, -orderTotal, 1f, string.Empty);
			PlayerSingleton<DeliveryApp>.Instance.PlayOrderSubmittedAnim();
			this.ResetCart();
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x0013C9F8 File Offset: 0x0013ABF8
		public void RefreshShop()
		{
			this.RefreshCart();
			this.RefreshOrderButton();
			this.RefreshDestinationUI();
			this.RefreshLoadingDockUI();
			this.RefreshEntryOrder();
			this.RefreshEntriesLocked();
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x0013CA20 File Offset: 0x0013AC20
		public void ResetCart()
		{
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				listingEntry.SetQuantity(0, false);
			}
			this.RefreshCart();
			this.RefreshOrderButton();
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x0013CA80 File Offset: 0x0013AC80
		private void RefreshCart()
		{
			this.ItemTotalLabel.text = MoneyManager.FormatAmount(this.GetCartCost(), false, false);
			this.OrderTotalLabel.text = MoneyManager.FormatAmount(this.GetOrderTotal(), false, false);
		}

		// Token: 0x06004B6D RID: 19309 RVA: 0x0013CAB4 File Offset: 0x0013ACB4
		private void RefreshOrderButton()
		{
			string text;
			if (this.CanOrder(out text))
			{
				this.OrderButton.interactable = true;
				this.OrderButtonNote.enabled = false;
				return;
			}
			this.OrderButton.interactable = false;
			this.OrderButtonNote.text = text;
			this.OrderButtonNote.enabled = true;
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x0013CB08 File Offset: 0x0013AD08
		public bool CanOrder(out string reason)
		{
			reason = string.Empty;
			if (this.HasActiveDelivery())
			{
				reason = "Delivery already in progress";
				return false;
			}
			float cartCost = this.GetCartCost();
			if (this.GetOrderTotal() > NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance)
			{
				reason = "Insufficient online balance";
				return false;
			}
			if (this.destinationProperty == null)
			{
				reason = "Select a destination";
				return false;
			}
			if (this.destinationProperty.LoadingDockCount == 0)
			{
				reason = "Selected destination has no loading docks";
				return false;
			}
			if (this.loadingDockIndex == 0)
			{
				reason = "Select a loading dock";
				return false;
			}
			if (!this.WillCartFitInVehicle())
			{
				reason = "Order is too large for delivery vehicle";
				return false;
			}
			return cartCost > 0f;
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x0013CBA6 File Offset: 0x0013ADA6
		public bool HasActiveDelivery()
		{
			return !(this.destinationProperty == null) && NetworkSingleton<DeliveryManager>.Instance.GetActiveShopDelivery(this) != null;
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x0013CBC8 File Offset: 0x0013ADC8
		public bool WillCartFitInVehicle()
		{
			int num = 0;
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				if (listingEntry.SelectedQuantity != 0)
				{
					int i = listingEntry.SelectedQuantity;
					int stackLimit = listingEntry.MatchingListing.Item.StackLimit;
					while (i > 0)
					{
						if (i > stackLimit)
						{
							i -= stackLimit;
						}
						else
						{
							i = 0;
						}
						num++;
					}
				}
			}
			return num <= 16;
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x0013CC58 File Offset: 0x0013AE58
		public void RefreshDestinationUI()
		{
			Property y = this.destinationProperty;
			this.destinationProperty = null;
			this.DestinationDropdown.ClearOptions();
			List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
			list.Add(new Dropdown.OptionData("-"));
			List<Property> potentialDestinations = this.GetPotentialDestinations();
			int num = 0;
			for (int i = 0; i < potentialDestinations.Count; i++)
			{
				list.Add(new Dropdown.OptionData(potentialDestinations[i].PropertyName));
				if (potentialDestinations[i] == y)
				{
					num = i + 1;
				}
			}
			this.DestinationDropdown.AddOptions(list);
			this.DestinationDropdown.SetValueWithoutNotify(num);
			this.DestinationDropdownSelected(num);
		}

		// Token: 0x06004B72 RID: 19314 RVA: 0x0013CD00 File Offset: 0x0013AF00
		private void DestinationDropdownSelected(int index)
		{
			if (index > 0 && index <= this.GetPotentialDestinations().Count)
			{
				this.destinationProperty = this.GetPotentialDestinations()[index - 1];
				if (this.loadingDockIndex == 0 && this.destinationProperty.LoadingDockCount > 0)
				{
					this.loadingDockIndex = 1;
				}
			}
			else
			{
				this.destinationProperty = null;
			}
			this.RefreshLoadingDockUI();
		}

		// Token: 0x06004B73 RID: 19315 RVA: 0x0013CD5F File Offset: 0x0013AF5F
		private List<Property> GetPotentialDestinations()
		{
			return new List<Property>(Property.OwnedProperties);
		}

		// Token: 0x06004B74 RID: 19316 RVA: 0x0013CD6C File Offset: 0x0013AF6C
		public void RefreshLoadingDockUI()
		{
			int value = this.loadingDockIndex;
			this.loadingDockIndex = 0;
			this.LoadingDockDropdown.ClearOptions();
			List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
			list.Add(new Dropdown.OptionData("-"));
			if (this.destinationProperty != null)
			{
				for (int i = 0; i < this.destinationProperty.LoadingDockCount; i++)
				{
					list.Add(new Dropdown.OptionData((i + 1).ToString()));
				}
			}
			this.LoadingDockDropdown.AddOptions(list);
			int num = Mathf.Clamp(value, 0, list.Count - 1);
			this.LoadingDockDropdown.SetValueWithoutNotify(num);
			this.LoadingDockDropdownSelected(num);
		}

		// Token: 0x06004B75 RID: 19317 RVA: 0x0013CE12 File Offset: 0x0013B012
		private void LoadingDockDropdownSelected(int index)
		{
			this.loadingDockIndex = index;
		}

		// Token: 0x06004B76 RID: 19318 RVA: 0x0013CE1C File Offset: 0x0013B01C
		private float GetCartCost()
		{
			float num = 0f;
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				num += (float)listingEntry.SelectedQuantity * listingEntry.MatchingListing.Price;
			}
			return num;
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x0013CE88 File Offset: 0x0013B088
		private float GetOrderTotal()
		{
			return this.GetCartCost() + this.DeliveryFee;
		}

		// Token: 0x06004B78 RID: 19320 RVA: 0x0013CE98 File Offset: 0x0013B098
		private int GetOrderItemCount()
		{
			int num = 0;
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				num += listingEntry.SelectedQuantity;
			}
			return num;
		}

		// Token: 0x06004B79 RID: 19321 RVA: 0x0013CEF0 File Offset: 0x0013B0F0
		private void RefreshEntryOrder()
		{
			List<ListingEntry> list = new List<ListingEntry>();
			List<ListingEntry> list2 = new List<ListingEntry>();
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				if (!listingEntry.MatchingListing.Item.IsPurchasable)
				{
					list2.Add(listingEntry);
				}
				else
				{
					list.Add(listingEntry);
				}
			}
			list.AddRange(list2);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].transform.SetSiblingIndex(i);
			}
		}

		// Token: 0x06004B7A RID: 19322 RVA: 0x0013CF9C File Offset: 0x0013B19C
		private void RefreshEntriesLocked()
		{
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				listingEntry.RefreshLocked();
			}
		}

		// Token: 0x040037B4 RID: 14260
		public const int DELIVERY_VEHICLE_SLOT_CAPACITY = 16;

		// Token: 0x040037B5 RID: 14261
		public const int DELIVERY_TIME_MIN = 60;

		// Token: 0x040037B6 RID: 14262
		public const int DELIVERY_TIME_MAX = 360;

		// Token: 0x040037B7 RID: 14263
		public const int DELIVERY_TIME_ITEM_COUNT_DIVISOR = 160;

		// Token: 0x040037BB RID: 14267
		[Header("References")]
		public Image HeaderImage;

		// Token: 0x040037BC RID: 14268
		public Button HeaderButton;

		// Token: 0x040037BD RID: 14269
		public RectTransform ContentsContainer;

		// Token: 0x040037BE RID: 14270
		public RectTransform ListingContainer;

		// Token: 0x040037BF RID: 14271
		public Text DeliveryFeeLabel;

		// Token: 0x040037C0 RID: 14272
		public Text ItemTotalLabel;

		// Token: 0x040037C1 RID: 14273
		public Text OrderTotalLabel;

		// Token: 0x040037C2 RID: 14274
		public Button OrderButton;

		// Token: 0x040037C3 RID: 14275
		public Text OrderButtonNote;

		// Token: 0x040037C4 RID: 14276
		public Dropdown DestinationDropdown;

		// Token: 0x040037C5 RID: 14277
		public Dropdown LoadingDockDropdown;

		// Token: 0x040037C6 RID: 14278
		[Header("Settings")]
		public string MatchingShopInterfaceName = "ShopInterface";

		// Token: 0x040037C7 RID: 14279
		public float DeliveryFee = 200f;

		// Token: 0x040037C8 RID: 14280
		public bool AvailableByDefault;

		// Token: 0x040037C9 RID: 14281
		public ListingEntry ListingEntryPrefab;

		// Token: 0x040037CA RID: 14282
		public Sprite HeaderImage_Hidden;

		// Token: 0x040037CB RID: 14283
		public Sprite HeaderImage_Expanded;

		// Token: 0x040037CC RID: 14284
		public RectTransform HeaderArrow;

		// Token: 0x040037CD RID: 14285
		private List<ListingEntry> listingEntries = new List<ListingEntry>();

		// Token: 0x040037CE RID: 14286
		private Property destinationProperty;

		// Token: 0x040037CF RID: 14287
		private int loadingDockIndex;
	}
}
