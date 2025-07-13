using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000B99 RID: 2969
	public class Cart : MonoBehaviour
	{
		// Token: 0x06004EC7 RID: 20167 RVA: 0x0014CCD1 File Offset: 0x0014AED1
		protected virtual void Start()
		{
			this.UpdateViewCartText();
			this.BuyButton.onClick.AddListener(new UnityAction(this.Buy));
		}

		// Token: 0x06004EC8 RID: 20168 RVA: 0x0014CCF5 File Offset: 0x0014AEF5
		protected virtual void Update()
		{
			if (this.Shop.IsOpen)
			{
				this.UpdateEntries();
				this.UpdateLoadVehicleToggle();
				this.UpdateTotal();
				this.UpdateProblem();
			}
		}

		// Token: 0x06004EC9 RID: 20169 RVA: 0x0014CD1C File Offset: 0x0014AF1C
		public void AddItem(ShopListing listing, int quantity)
		{
			if (!this.cartDictionary.ContainsKey(listing))
			{
				this.cartDictionary.Add(listing, 0);
			}
			Console.Log(string.Concat(new string[]
			{
				"Adding ",
				quantity.ToString(),
				" ",
				listing.Item.Name,
				" to cart"
			}), null);
			Dictionary<ShopListing, int> dictionary = this.cartDictionary;
			dictionary[listing] += quantity;
			listing.SetQuantityInCart(this.cartDictionary[listing]);
			this.UpdateViewCartText();
			this.UpdateEntries();
		}

		// Token: 0x06004ECA RID: 20170 RVA: 0x0014CDBC File Offset: 0x0014AFBC
		public void RemoveItem(ShopListing listing, int quantity)
		{
			Dictionary<ShopListing, int> dictionary = this.cartDictionary;
			dictionary[listing] -= quantity;
			if (this.cartDictionary[listing] <= 0)
			{
				this.cartDictionary.Remove(listing);
			}
			listing.SetQuantityInCart(this.cartDictionary.ContainsKey(listing) ? this.cartDictionary[listing] : 0);
			this.Shop.RemoveItemSound.Play();
			this.UpdateProblem();
			this.UpdateViewCartText();
			this.UpdateEntries();
			this.UpdateTotal();
		}

		// Token: 0x06004ECB RID: 20171 RVA: 0x0014CE4C File Offset: 0x0014B04C
		public void ClearCart()
		{
			this.cartDictionary.Clear();
			foreach (KeyValuePair<ShopListing, int> keyValuePair in this.cartDictionary)
			{
				keyValuePair.Key.SetQuantityInCart(0);
			}
			this.UpdateViewCartText();
			this.UpdateEntries();
			this.UpdateTotal();
		}

		// Token: 0x06004ECC RID: 20172 RVA: 0x0014CEC4 File Offset: 0x0014B0C4
		public int GetCartCount(ShopListing listing)
		{
			if (this.cartDictionary.ContainsKey(listing))
			{
				return this.cartDictionary[listing];
			}
			return 0;
		}

		// Token: 0x06004ECD RID: 20173 RVA: 0x0014CEE2 File Offset: 0x0014B0E2
		public void BopCartIcon()
		{
			if (this.cartIconBop != null)
			{
				base.StopCoroutine(this.cartIconBop);
			}
			this.cartIconBop = base.StartCoroutine(this.<BopCartIcon>g__Routine|21_0());
		}

		// Token: 0x06004ECE RID: 20174 RVA: 0x0014CF0C File Offset: 0x0014B10C
		public bool CanPlayerAffordCart()
		{
			float priceSum = this.GetPriceSum();
			switch (this.Shop.PaymentType)
			{
			case ShopInterface.EPaymentType.Cash:
				return NetworkSingleton<MoneyManager>.Instance.cashBalance >= priceSum;
			case ShopInterface.EPaymentType.Online:
				return NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= priceSum;
			case ShopInterface.EPaymentType.PreferCash:
				return NetworkSingleton<MoneyManager>.Instance.cashBalance >= priceSum || NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= priceSum;
			case ShopInterface.EPaymentType.PreferOnline:
				return NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= priceSum || NetworkSingleton<MoneyManager>.Instance.cashBalance >= priceSum;
			default:
				return false;
			}
		}

		// Token: 0x06004ECF RID: 20175 RVA: 0x0014CFA8 File Offset: 0x0014B1A8
		public void Buy()
		{
			string text;
			if (!this.CanCheckout(out text))
			{
				return;
			}
			foreach (KeyValuePair<ShopListing, int> keyValuePair in this.cartDictionary)
			{
				ShopListing key = keyValuePair.Key;
				int value = keyValuePair.Value;
				if (!key.IsUnlimitedStock)
				{
					key.RemoveStock(value);
				}
			}
			this.Shop.HandoverItems();
			switch (this.Shop.PaymentType)
			{
			case ShopInterface.EPaymentType.Cash:
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.GetPriceSum(), true, false);
				break;
			case ShopInterface.EPaymentType.Online:
				NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Purchase from " + this.Shop.ShopName, -this.GetPriceSum(), 1f, string.Empty);
				break;
			case ShopInterface.EPaymentType.PreferCash:
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= this.GetPriceSum())
				{
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.GetPriceSum(), true, false);
				}
				else
				{
					NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Purchase from " + this.Shop.ShopName, -this.GetPriceSum(), 1f, string.Empty);
				}
				break;
			case ShopInterface.EPaymentType.PreferOnline:
				if (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= this.GetPriceSum())
				{
					NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Purchase from " + this.Shop.ShopName, -this.GetPriceSum(), 1f, string.Empty);
				}
				else
				{
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.GetPriceSum(), true, false);
				}
				break;
			}
			this.ClearCart();
			this.Shop.CheckoutSound.Play();
			this.Shop.SetIsOpen(false);
			if (this.Shop.onOrderCompleted != null)
			{
				this.Shop.onOrderCompleted.Invoke();
			}
		}

		// Token: 0x06004ED0 RID: 20176 RVA: 0x0014D194 File Offset: 0x0014B394
		private void UpdateEntries()
		{
			List<ShopListing> list = this.cartDictionary.Keys.ToList<ShopListing>();
			for (int i = 0; i < list.Count; i++)
			{
				CartEntry cartEntry = this.GetEntry(list[i]);
				if (cartEntry == null)
				{
					cartEntry = UnityEngine.Object.Instantiate<CartEntry>(this.EntryPrefab, this.CartEntryContainer);
					cartEntry.Initialize(this, list[i], this.cartDictionary[list[i]]);
					this.cartEntries.Add(cartEntry);
				}
				if (cartEntry.Quantity != this.cartDictionary[list[i]])
				{
					cartEntry.SetQuantity(this.cartDictionary[list[i]]);
				}
			}
			for (int j = 0; j < this.cartEntries.Count; j++)
			{
				if (!this.cartDictionary.ContainsKey(this.cartEntries[j].Listing))
				{
					UnityEngine.Object.Destroy(this.cartEntries[j].gameObject);
					this.cartEntries.RemoveAt(j);
					j--;
				}
			}
		}

		// Token: 0x06004ED1 RID: 20177 RVA: 0x0014D2AC File Offset: 0x0014B4AC
		private void UpdateTotal()
		{
			this.TotalText.text = string.Concat(new string[]
			{
				"Total: <color=#",
				ColorUtility.ToHtmlStringRGBA(ListingUI.PriceLabelColor_Normal),
				">",
				MoneyManager.FormatAmount(this.GetPriceSum(), false, false),
				"</color>"
			});
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x0014D30C File Offset: 0x0014B50C
		private void UpdateProblem()
		{
			string text;
			bool flag = this.CanCheckout(out text);
			this.BuyButton.interactable = (flag && this.cartDictionary.Count > 0);
			if (flag)
			{
				this.ProblemText.enabled = false;
			}
			else
			{
				this.ProblemText.text = text;
				this.ProblemText.enabled = true;
			}
			string text2;
			if (this.GetWarning(out text2) && !this.ProblemText.enabled)
			{
				this.WarningText.text = text2;
				this.WarningText.enabled = true;
				return;
			}
			this.WarningText.enabled = false;
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x0014D3A8 File Offset: 0x0014B5A8
		private bool CanCheckout(out string reason)
		{
			if (!this.Shop.WillCartFit())
			{
				if (this.Shop.DeliveryBays.Length != 0)
				{
					reason = "Order too large";
				}
				else
				{
					reason = "Order won't fit in inventory";
				}
				return false;
			}
			if (!this.CanPlayerAffordCart())
			{
				if (this.Shop.PaymentType == ShopInterface.EPaymentType.Cash)
				{
					reason = "Insufficient cash. Visit an ATM to withdraw cash.";
				}
				else if (this.Shop.PaymentType == ShopInterface.EPaymentType.Online)
				{
					reason = "Insufficient online balance. Visit an ATM to deposit cash.";
				}
				else
				{
					reason = "Insufficient funds";
				}
				return false;
			}
			reason = string.Empty;
			return true;
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x0014D428 File Offset: 0x0014B628
		private bool GetWarning(out string warning)
		{
			warning = string.Empty;
			if (this.Shop.GetLoadingBayVehicle() != null && this.LoadVehicleToggle.isOn)
			{
				List<ItemSlot> itemSlots = this.Shop.GetLoadingBayVehicle().Storage.ItemSlots;
				if (!this.Shop.WillCartFit(itemSlots))
				{
					warning = "Vehicle won't fit everything. Some items will be placed on the pallets.";
					return true;
				}
			}
			else
			{
				List<ItemSlot> availableSlots = PlayerSingleton<PlayerInventory>.Instance.hotbarSlots.Cast<ItemSlot>().ToList<ItemSlot>();
				if (!this.Shop.WillCartFit(availableSlots))
				{
					warning = "Inventory won't fit everything. Some items will be placed on the pallets.";
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x0014D4B8 File Offset: 0x0014B6B8
		private void UpdateViewCartText()
		{
			int itemSum = this.GetItemSum();
			if (itemSum > 0)
			{
				this.ViewCartText.text = string.Concat(new string[]
				{
					"View Cart (",
					itemSum.ToString(),
					" item",
					(itemSum > 1) ? "s" : "",
					")"
				});
				return;
			}
			this.ViewCartText.text = "View Cart";
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x0014D52C File Offset: 0x0014B72C
		private void UpdateLoadVehicleToggle()
		{
			this.LoadVehicleToggle.gameObject.SetActive(this.Shop.GetLoadingBayVehicle() != null);
		}

		// Token: 0x06004ED7 RID: 20183 RVA: 0x0014D550 File Offset: 0x0014B750
		private int GetItemSum()
		{
			int num = 0;
			List<ShopListing> list = this.cartDictionary.Keys.ToList<ShopListing>();
			for (int i = 0; i < list.Count; i++)
			{
				num += this.cartDictionary[list[i]];
			}
			return num;
		}

		// Token: 0x06004ED8 RID: 20184 RVA: 0x0014D598 File Offset: 0x0014B798
		private float GetPriceSum()
		{
			float num = 0f;
			List<ShopListing> list = this.cartDictionary.Keys.ToList<ShopListing>();
			for (int i = 0; i < list.Count; i++)
			{
				num += (float)this.cartDictionary[list[i]] * list[i].Price;
			}
			return num;
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x0014D5F4 File Offset: 0x0014B7F4
		private CartEntry GetEntry(ShopListing listing)
		{
			return this.cartEntries.Find((CartEntry x) => x.Listing == listing);
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x0014D625 File Offset: 0x0014B825
		private bool IsMouseOverMenuArea()
		{
			return RectTransformUtility.RectangleContainsScreenPoint(this.CartArea.rectTransform, Input.mousePosition);
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x0014D644 File Offset: 0x0014B844
		public int GetTotalSlotRequirement()
		{
			ShopListing[] array = this.cartDictionary.Keys.ToArray<ShopListing>();
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = this.cartDictionary[array[i]];
				num += Mathf.CeilToInt((float)num2 / (float)array[i].Item.StackLimit);
			}
			return num;
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x0014D6B9 File Offset: 0x0014B8B9
		[CompilerGenerated]
		private IEnumerator <BopCartIcon>g__Routine|21_0()
		{
			Vector3 startScale = Vector3.one;
			Vector3 endScale = Vector3.one * 1.25f;
			float lerpTime = 0.09f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.CartIcon.transform.localScale = Vector3.Lerp(startScale, endScale, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.CartIcon.transform.localScale = Vector3.Lerp(endScale, startScale, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			this.CartIcon.transform.localScale = startScale;
			this.cartIconBop = null;
			yield break;
		}

		// Token: 0x04003AFC RID: 15100
		[Header("References")]
		public ShopInterface Shop;

		// Token: 0x04003AFD RID: 15101
		public Image CartIcon;

		// Token: 0x04003AFE RID: 15102
		public TextMeshProUGUI ViewCartText;

		// Token: 0x04003AFF RID: 15103
		public RectTransform CartEntryContainer;

		// Token: 0x04003B00 RID: 15104
		public TextMeshProUGUI ProblemText;

		// Token: 0x04003B01 RID: 15105
		public TextMeshProUGUI WarningText;

		// Token: 0x04003B02 RID: 15106
		public Button BuyButton;

		// Token: 0x04003B03 RID: 15107
		public RectTransform CartContainer;

		// Token: 0x04003B04 RID: 15108
		public Image CartArea;

		// Token: 0x04003B05 RID: 15109
		public TextMeshProUGUI TotalText;

		// Token: 0x04003B06 RID: 15110
		public Toggle LoadVehicleToggle;

		// Token: 0x04003B07 RID: 15111
		[Header("Prefabs")]
		public CartEntry EntryPrefab;

		// Token: 0x04003B08 RID: 15112
		public Dictionary<ShopListing, int> cartDictionary = new Dictionary<ShopListing, int>();

		// Token: 0x04003B09 RID: 15113
		private Coroutine cartIconBop;

		// Token: 0x04003B0A RID: 15114
		private List<CartEntry> cartEntries = new List<CartEntry>();
	}
}
