using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using FishNet;
using ScheduleOne.Audio;
using ScheduleOne.Delivery;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000BA6 RID: 2982
	public class ShopInterface : MonoBehaviour, ISaveable
	{
		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06004F2E RID: 20270 RVA: 0x0014E49F File Offset: 0x0014C69F
		// (set) Token: 0x06004F2F RID: 20271 RVA: 0x0014E4A7 File Offset: 0x0014C6A7
		public bool IsOpen { get; protected set; }

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06004F30 RID: 20272 RVA: 0x0014E4B0 File Offset: 0x0014C6B0
		public string SaveFolderName
		{
			get
			{
				return SaveManager.MakeFileSafe(this.ShopCode);
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06004F31 RID: 20273 RVA: 0x0014E4B0 File Offset: 0x0014C6B0
		public string SaveFileName
		{
			get
			{
				return SaveManager.MakeFileSafe(this.ShopCode);
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06004F32 RID: 20274 RVA: 0x0014E4BD File Offset: 0x0014C6BD
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06004F33 RID: 20275 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06004F34 RID: 20276 RVA: 0x0014E4C5 File Offset: 0x0014C6C5
		// (set) Token: 0x06004F35 RID: 20277 RVA: 0x0014E4CD File Offset: 0x0014C6CD
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06004F36 RID: 20278 RVA: 0x0014E4D6 File Offset: 0x0014C6D6
		// (set) Token: 0x06004F37 RID: 20279 RVA: 0x0014E4DE File Offset: 0x0014C6DE
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06004F38 RID: 20280 RVA: 0x0014E4E7 File Offset: 0x0014C6E7
		// (set) Token: 0x06004F39 RID: 20281 RVA: 0x0014E4EF File Offset: 0x0014C6EF
		public bool HasChanged { get; set; } = true;

		// Token: 0x06004F3A RID: 20282 RVA: 0x0014E4F8 File Offset: 0x0014C6F8
		protected virtual void Awake()
		{
			foreach (ShopListing listing in this.Listings)
			{
				this.CreateListingUI(listing);
			}
			this.ListingScrollRect.verticalNormalizedPosition = 1f;
			this.Listings = (from x in this.Listings
			orderby x.Item.Name
			select x).ToList<ShopListing>();
			this.categoryButtons = base.GetComponentsInChildren<CategoryButton>().ToList<CategoryButton>();
			this.StoreNameLabel.text = this.ShopName;
			this.ListingContainer.anchoredPosition = Vector2.zero;
			this.AmountSelector.onSubmitted.AddListener(new UnityAction<int>(this.QuantitySelected));
			ShopInterface.AllShops.Add(this);
		}

		// Token: 0x06004F3B RID: 20283 RVA: 0x0014E5EC File Offset: 0x0014C7EC
		protected virtual void Start()
		{
			this.RefreshShownItems();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 7);
			this.RestockAllListings();
			foreach (ShopListing shopListing in this.Listings)
			{
				shopListing.Initialize(this);
				if (shopListing.Item.RequiresLevelToPurchase)
				{
					NetworkSingleton<LevelManager>.Instance.AddUnlockable(new Unlockable(shopListing.Item.RequiredRank, shopListing.Item.Name, shopListing.Item.Icon));
				}
			}
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			NetworkSingleton<TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.OnDayPass));
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.OnDayPass));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onWeekPass = (Action)Delegate.Remove(instance.onWeekPass, new Action(this.OnWeekPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onWeekPass = (Action)Delegate.Combine(instance2.onWeekPass, new Action(this.OnWeekPass));
			this.InitializeSaveable();
		}

		// Token: 0x06004F3C RID: 20284 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x0014E748 File Offset: 0x0014C948
		private void OnDestroy()
		{
			ShopInterface.AllShops.Remove(this);
		}

		// Token: 0x06004F3E RID: 20286 RVA: 0x0014E758 File Offset: 0x0014C958
		private void OnValidate()
		{
			this.StoreNameLabel.text = this.ShopName;
			for (int i = 0; i < this.Listings.Count; i++)
			{
				if (!(this.Listings[i].Item == null))
				{
					string text = "(";
					for (int j = 0; j < this.Listings[i].Item.ShopCategories.Count; j++)
					{
						text = text + this.Listings[i].Item.ShopCategories[j].Category.ToString() + ", ";
					}
					text += ")";
					this.Listings[i].name = string.Concat(new string[]
					{
						this.Listings[i].Item.Name,
						" ($",
						this.Listings[i].Price.ToString(),
						") ",
						text
					});
					if (this.Listings[i].Item.RequiresLevelToPurchase)
					{
						ShopListing shopListing = this.Listings[i];
						string name = shopListing.name;
						string str = " [Rank ";
						FullRank requiredRank = this.Listings[i].Item.RequiredRank;
						shopListing.name = name + str + requiredRank.ToString() + "]";
					}
				}
			}
		}

		// Token: 0x06004F3F RID: 20287 RVA: 0x0014E8E6 File Offset: 0x0014CAE6
		protected virtual void Update()
		{
			if (this.IsOpen && Input.GetMouseButtonUp(0))
			{
				if (this.dropdownMouseUp)
				{
					this.AmountSelector.Close();
					this.selectedListing = null;
					return;
				}
				this.dropdownMouseUp = true;
			}
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x0014E91C File Offset: 0x0014CB1C
		protected void OnDayPass()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			for (int i = 0; i < this.Listings.Count; i++)
			{
				if (!this.Listings[i].IsUnlimitedStock && this.Listings[i].RestockRate == ShopListing.ERestockRate.Daily)
				{
					this.Listings[i].Restock(true);
				}
			}
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x0014E980 File Offset: 0x0014CB80
		protected void OnWeekPass()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			for (int i = 0; i < this.Listings.Count; i++)
			{
				if (!this.Listings[i].IsUnlimitedStock && this.Listings[i].RestockRate == ShopListing.ERestockRate.Weekly)
				{
					this.Listings[i].Restock(true);
				}
			}
		}

		// Token: 0x06004F42 RID: 20290 RVA: 0x0014E9E4 File Offset: 0x0014CBE4
		[Button]
		public void Open()
		{
			this.SetIsOpen(true);
		}

		// Token: 0x06004F43 RID: 20291 RVA: 0x0014E9F0 File Offset: 0x0014CBF0
		public virtual void SetIsOpen(bool isOpen)
		{
			this.IsOpen = isOpen;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(this.ShopName);
			if (isOpen)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(this.ShopName);
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
				PlayerSingleton<PlayerMovement>.Instance.canMove = false;
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
				this.SelectCategory(EShopCategory.All);
				this.RefreshShownItems();
				this.ListingScrollRect.verticalNormalizedPosition = 1f;
				this.ListingScrollRect.content.anchoredPosition = Vector2.zero;
				this.RefreshUnlockStatus();
				Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
				if (this.ShowCurrencyHint)
				{
					this.ShowCurrencyHint = false;
					Singleton<HintDisplay>.Instance.ShowHint("Your <h1>online balance</h> is displayed in the top right corner.", 10f);
					base.Invoke("Hint", 10.5f);
				}
			}
			else
			{
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
				this.DetailPanel.Close();
				this.AmountSelector.Close();
			}
			this.Canvas.enabled = isOpen;
			this.Container.gameObject.SetActive(isOpen);
		}

		// Token: 0x06004F44 RID: 20292 RVA: 0x0014EB44 File Offset: 0x0014CD44
		private void Hint()
		{
			Singleton<HintDisplay>.Instance.ShowHint("Most legal shops will only accept <h1>card payments</h>, while most illegal shops only take cash. Visit an <h1>ATM</h> to deposit and withdraw cash.", 20f);
		}

		// Token: 0x06004F45 RID: 20293 RVA: 0x0014EB5A File Offset: 0x0014CD5A
		protected virtual void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (this.IsOpen)
			{
				action.Used = true;
				this.SetIsOpen(false);
			}
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x0014EB7C File Offset: 0x0014CD7C
		private void CreateListingUI(ShopListing listing)
		{
			ListingUI component = UnityEngine.Object.Instantiate<GameObject>(this.ListingUIPrefab.gameObject, this.ListingContainer).GetComponent<ListingUI>();
			component.Initialize(listing);
			ListingUI ui = component;
			ListingUI listingUI = component;
			listingUI.onClicked = (Action)Delegate.Combine(listingUI.onClicked, new Action(delegate()
			{
				this.ListingClicked(ui);
			}));
			ListingUI listingUI2 = component;
			listingUI2.onDropdownClicked = (Action)Delegate.Combine(listingUI2.onDropdownClicked, new Action(delegate()
			{
				this.DropdownClicked(ui);
			}));
			ListingUI listingUI3 = component;
			listingUI3.hoverStart = (Action)Delegate.Combine(listingUI3.hoverStart, new Action(delegate()
			{
				this.EntryHovered(ui);
			}));
			ListingUI listingUI4 = component;
			listingUI4.hoverEnd = (Action)Delegate.Combine(listingUI4.hoverEnd, new Action(this.EntryUnhovered));
			this.listingUI.Add(component);
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x0014EC54 File Offset: 0x0014CE54
		public void SelectCategory(EShopCategory category)
		{
			CategoryButton categoryButton = this.categoryButtons.Find((CategoryButton x) => x.Category == category);
			if (categoryButton == null)
			{
				Console.LogWarning("Category button not found: " + category.ToString(), null);
				return;
			}
			categoryButton.Select();
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x0014ECB8 File Offset: 0x0014CEB8
		public virtual void ListingClicked(ListingUI listingUI)
		{
			if (!listingUI.Listing.Item.IsPurchasable)
			{
				return;
			}
			if (!listingUI.CanAddToCart())
			{
				return;
			}
			int quantity = 1;
			if (this.AmountSelector.IsOpen)
			{
				quantity = this.AmountSelector.SelectedAmount;
			}
			this.Cart.AddItem(listingUI.Listing, quantity);
			this.AddItemSound.Play();
		}

		// Token: 0x06004F49 RID: 20297 RVA: 0x0014ED1C File Offset: 0x0014CF1C
		private void ShowCartAnimation(ListingUI listing)
		{
			ShopInterface.<>c__DisplayClass70_0 CS$<>8__locals1 = new ShopInterface.<>c__DisplayClass70_0();
			CS$<>8__locals1.listing = listing;
			CS$<>8__locals1.<>4__this = this;
			base.StartCoroutine(CS$<>8__locals1.<ShowCartAnimation>g__Routine|0());
		}

		// Token: 0x06004F4A RID: 20298 RVA: 0x0014ED4A File Offset: 0x0014CF4A
		public void CategorySelected(EShopCategory category)
		{
			if (category == this.categoryFilter)
			{
				return;
			}
			this.DeselectCurrentCategory();
			this.categoryFilter = category;
			this.RefreshShownItems();
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x0014ED69 File Offset: 0x0014CF69
		private void DeselectCurrentCategory()
		{
			this.categoryButtons.Find((CategoryButton x) => x.Category == this.categoryFilter).Deselect();
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x0014ED88 File Offset: 0x0014CF88
		private void RefreshShownItems()
		{
			for (int i = 0; i < this.listingUI.Count; i++)
			{
				if (this.searchTerm != string.Empty)
				{
					this.listingUI[i].gameObject.SetActive(this.listingUI[i].Listing.DoesListingMatchSearchTerm(this.searchTerm));
				}
				else
				{
					this.listingUI[i].gameObject.SetActive(this.listingUI[i].Listing.DoesListingMatchCategoryFilter(this.categoryFilter) && this.listingUI[i].Listing.ShouldShow());
				}
			}
			for (int j = 0; j < this.listingUI.Count; j++)
			{
				this.listingUI[j].transform.SetSiblingIndex(j);
			}
			List<ListingUI> list = this.listingUI.FindAll((ListingUI x) => !x.Listing.Item.IsPurchasable);
			list.Sort((ListingUI x, ListingUI y) => x.Listing.Item.RequiredRank.CompareTo(y.Listing.Item.RequiredRank));
			for (int k = 0; k < list.Count; k++)
			{
				list[k].transform.SetAsLastSibling();
			}
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x0014EEE4 File Offset: 0x0014D0E4
		private void RefreshUnlockStatus()
		{
			for (int i = 0; i < this.listingUI.Count; i++)
			{
				this.listingUI[i].UpdateLockStatus();
			}
		}

		// Token: 0x06004F4E RID: 20302 RVA: 0x0014EF18 File Offset: 0x0014D118
		private void RestockAllListings()
		{
			foreach (ShopListing shopListing in this.Listings)
			{
				shopListing.Restock(false);
			}
		}

		// Token: 0x06004F4F RID: 20303 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanCartFitItem(ShopListing listing)
		{
			return true;
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x0014EF6C File Offset: 0x0014D16C
		public bool WillCartFit()
		{
			List<ItemSlot> availableSlots = this.GetAvailableSlots();
			return this.WillCartFit(availableSlots);
		}

		// Token: 0x06004F51 RID: 20305 RVA: 0x0014EF88 File Offset: 0x0014D188
		public bool WillCartFit(List<ItemSlot> availableSlots)
		{
			List<ShopListing> list = this.Cart.cartDictionary.Keys.ToList<ShopListing>();
			List<ItemSlot> list2 = new List<ItemSlot>();
			for (int i = 0; i < list.Count; i++)
			{
				int num = this.Cart.cartDictionary[list[i]];
				ItemInstance defaultInstance = list[i].Item.GetDefaultInstance(1);
				int num2 = 0;
				while (num2 < availableSlots.Count && num > 0)
				{
					if (!list2.Contains(availableSlots[num2]))
					{
						int capacityForItem = availableSlots[num2].GetCapacityForItem(defaultInstance, false);
						if (capacityForItem > 0)
						{
							list2.Add(availableSlots[num2]);
							num -= Mathf.Min(num, capacityForItem);
						}
					}
					num2++;
				}
				if (num > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004F52 RID: 20306 RVA: 0x0014F054 File Offset: 0x0014D254
		public virtual bool HandoverItems()
		{
			List<ItemSlot> availableSlots = this.GetAvailableSlots();
			List<ShopListing> list = this.Cart.cartDictionary.Keys.ToList<ShopListing>();
			bool result = true;
			for (int i = 0; i < list.Count; i++)
			{
				NetworkSingleton<VariableDatabase>.Instance.NotifyItemAcquired(list[i].Item.ID, this.Cart.cartDictionary[list[i]]);
				int num = this.Cart.cartDictionary[list[i]];
				ItemInstance defaultInstance = list[i].Item.GetDefaultInstance(1);
				int num2 = 0;
				while (num2 < availableSlots.Count && num > 0)
				{
					int capacityForItem = availableSlots[num2].GetCapacityForItem(defaultInstance, false);
					if (capacityForItem != 0)
					{
						int num3 = Mathf.Min(capacityForItem, num);
						availableSlots[num2].AddItem(defaultInstance.GetCopy(num3), false);
						num -= num3;
					}
					num2++;
				}
				if (num > 0)
				{
					Debug.LogWarning("Failed to handover all items in cart: " + defaultInstance.Name);
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x0014F170 File Offset: 0x0014D370
		public List<ItemSlot> GetAvailableSlots()
		{
			List<ItemSlot> list = new List<ItemSlot>();
			LandVehicle loadingBayVehicle = this.GetLoadingBayVehicle();
			if (loadingBayVehicle != null && this.Cart.LoadVehicleToggle.isOn)
			{
				list.AddRange(loadingBayVehicle.Storage.ItemSlots);
			}
			else
			{
				list.AddRange(PlayerSingleton<PlayerInventory>.Instance.hotbarSlots);
			}
			for (int i = 0; i < this.DeliveryBays.Length; i++)
			{
				list.AddRange(this.DeliveryBays[i].ItemSlots);
			}
			return list;
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x0014F1F0 File Offset: 0x0014D3F0
		public LandVehicle GetLoadingBayVehicle()
		{
			if (this.LoadingBayDetector != null && this.LoadingBayDetector.closestVehicle != null && this.LoadingBayDetector.closestVehicle.IsPlayerOwned)
			{
				return this.LoadingBayDetector.closestVehicle;
			}
			return null;
		}

		// Token: 0x06004F55 RID: 20309 RVA: 0x0014F240 File Offset: 0x0014D440
		public void PlaceItemInDeliveryBay(ItemInstance item)
		{
			int num = item.Quantity;
			foreach (StorageEntity storageEntity in this.DeliveryBays)
			{
				int num2 = storageEntity.HowManyCanFit(item);
				if (num2 > 0)
				{
					ItemInstance copy = item.GetCopy(Mathf.Min(num, num2));
					storageEntity.InsertItem(copy, true);
					num -= copy.Quantity;
				}
				if (num <= 0)
				{
					break;
				}
			}
			if (num > 0)
			{
				Console.LogWarning("Could not fit all items in delivery bay!", null);
			}
		}

		// Token: 0x06004F56 RID: 20310 RVA: 0x0014F2B4 File Offset: 0x0014D4B4
		public void QuantitySelected(int amount)
		{
			if (this.selectedListing == null)
			{
				return;
			}
			if (!this.selectedListing.Listing.Item.IsPurchasable)
			{
				return;
			}
			int quantity = Mathf.Clamp(amount, 1, this.selectedListing.Listing.IsUnlimitedStock ? 100000000 : this.selectedListing.Listing.CurrentStockMinusCart);
			this.Cart.AddItem(this.selectedListing.Listing, quantity);
			this.AddItemSound.Play();
			this.AmountSelector.Close();
			this.selectedListing = null;
		}

		// Token: 0x06004F57 RID: 20311 RVA: 0x0014F350 File Offset: 0x0014D550
		public void OpenAmountSelector(ListingUI listing)
		{
			if (!listing.Listing.Item.IsPurchasable)
			{
				return;
			}
			if (!listing.CanAddToCart())
			{
				return;
			}
			this.selectedListing = listing;
			this.AmountSelector.transform.position = listing.TopDropdownAnchor.position;
			this.dropdownMouseUp = false;
			this.AmountSelector.Open();
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x0014F3AD File Offset: 0x0014D5AD
		private void DropdownClicked(ListingUI listing)
		{
			if (this.selectedListing == listing)
			{
				this.AmountSelector.Close();
				this.selectedListing = null;
				return;
			}
			this.OpenAmountSelector(listing);
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x0014F3D7 File Offset: 0x0014D5D7
		private void EntryHovered(ListingUI listing)
		{
			this.DetailPanel.Open(listing);
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x0014F3E5 File Offset: 0x0014D5E5
		private void EntryUnhovered()
		{
			this.DetailPanel.Close();
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x0014F3F4 File Offset: 0x0014D5F4
		public void Load(ShopData data)
		{
			Console.Log("Loading shop data: " + data.ShopCode, null);
			StringIntPair[] itemStockQuantities = data.ItemStockQuantities;
			for (int i = 0; i < itemStockQuantities.Length; i++)
			{
				StringIntPair stockQuantity = itemStockQuantities[i];
				ShopListing shopListing = this.Listings.Find((ShopListing x) => x.Item.ID == stockQuantity.String);
				if (shopListing == null)
				{
					Console.LogWarning("Failed to load shop data: Listing not found: " + stockQuantity.String, null);
				}
				else
				{
					shopListing.SetStock(stockQuantity.Int, true);
				}
			}
		}

		// Token: 0x06004F5C RID: 20316 RVA: 0x0014F488 File Offset: 0x0014D688
		public bool ShouldSave()
		{
			new List<StringIntPair>();
			foreach (ShopListing shopListing in this.Listings)
			{
				if (!shopListing.IsUnlimitedStock && shopListing.CurrentStock != shopListing.DefaultStock)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x0014F4F8 File Offset: 0x0014D6F8
		public ShopListing GetListing(string itemID)
		{
			return this.Listings.Find((ShopListing x) => x.Item.ID == itemID);
		}

		// Token: 0x06004F5E RID: 20318 RVA: 0x0014F52C File Offset: 0x0014D72C
		public virtual ShopData GetSaveData()
		{
			List<StringIntPair> list = new List<StringIntPair>();
			foreach (ShopListing shopListing in this.Listings)
			{
				if (!shopListing.IsUnlimitedStock && shopListing.CurrentStock != shopListing.DefaultStock)
				{
					list.Add(new StringIntPair(shopListing.Item.ID, shopListing.CurrentStock));
				}
			}
			return new ShopData(this.ShopCode, list.ToArray());
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x0014F5C4 File Offset: 0x0014D7C4
		public string GetSaveString()
		{
			return this.GetSaveData().GetJson(true);
		}

		// Token: 0x04003B55 RID: 15189
		public static List<ShopInterface> AllShops = new List<ShopInterface>();

		// Token: 0x04003B56 RID: 15190
		public const int MAX_ITEM_QUANTITY = 999;

		// Token: 0x04003B58 RID: 15192
		[Header("Settings")]
		public string ShopName = "Shop";

		// Token: 0x04003B59 RID: 15193
		public string ShopCode = "shop";

		// Token: 0x04003B5A RID: 15194
		public ShopInterface.EPaymentType PaymentType;

		// Token: 0x04003B5B RID: 15195
		public bool ShowCurrencyHint;

		// Token: 0x04003B5C RID: 15196
		[Header("Listings")]
		public List<ShopListing> Listings = new List<ShopListing>();

		// Token: 0x04003B5D RID: 15197
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003B5E RID: 15198
		public RectTransform Container;

		// Token: 0x04003B5F RID: 15199
		public RectTransform ListingContainer;

		// Token: 0x04003B60 RID: 15200
		public TextMeshProUGUI StoreNameLabel;

		// Token: 0x04003B61 RID: 15201
		public Cart Cart;

		// Token: 0x04003B62 RID: 15202
		public StorageEntity[] DeliveryBays;

		// Token: 0x04003B63 RID: 15203
		public VehicleDetector LoadingBayDetector;

		// Token: 0x04003B64 RID: 15204
		public ShopInterfaceDetailPanel DetailPanel;

		// Token: 0x04003B65 RID: 15205
		public ScrollRect ListingScrollRect;

		// Token: 0x04003B66 RID: 15206
		public ShopAmountSelector AmountSelector;

		// Token: 0x04003B67 RID: 15207
		public DeliveryVehicle DeliveryVehicle;

		// Token: 0x04003B68 RID: 15208
		[Header("Audio")]
		public AudioSourceController AddItemSound;

		// Token: 0x04003B69 RID: 15209
		public AudioSourceController RemoveItemSound;

		// Token: 0x04003B6A RID: 15210
		public AudioSourceController CheckoutSound;

		// Token: 0x04003B6B RID: 15211
		[Header("Prefabs")]
		public ListingUI ListingUIPrefab;

		// Token: 0x04003B6C RID: 15212
		public UnityEvent onOrderCompleted;

		// Token: 0x04003B6D RID: 15213
		[SerializeField]
		private List<CategoryButton> categoryButtons = new List<CategoryButton>();

		// Token: 0x04003B6E RID: 15214
		private EShopCategory categoryFilter;

		// Token: 0x04003B6F RID: 15215
		private string searchTerm = string.Empty;

		// Token: 0x04003B70 RID: 15216
		private List<ListingUI> listingUI = new List<ListingUI>();

		// Token: 0x04003B71 RID: 15217
		private ListingUI selectedListing;

		// Token: 0x04003B72 RID: 15218
		private bool dropdownMouseUp;

		// Token: 0x04003B73 RID: 15219
		private ShopLoader loader = new ShopLoader();

		// Token: 0x02000BA7 RID: 2983
		public enum EPaymentType
		{
			// Token: 0x04003B78 RID: 15224
			Cash,
			// Token: 0x04003B79 RID: 15225
			Online,
			// Token: 0x04003B7A RID: 15226
			PreferCash,
			// Token: 0x04003B7B RID: 15227
			PreferOnline
		}
	}
}
