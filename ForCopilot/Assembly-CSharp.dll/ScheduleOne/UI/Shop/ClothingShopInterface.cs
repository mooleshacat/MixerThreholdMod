using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Clothing;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000B9F RID: 2975
	public class ClothingShopInterface : ShopInterface
	{
		// Token: 0x06004F00 RID: 20224 RVA: 0x0014DB0E File Offset: 0x0014BD0E
		protected override void Start()
		{
			base.Start();
			this.ColorPicker.onColorPicked.AddListener(new UnityAction<EClothingColor>(this.ColorPicked));
		}

		// Token: 0x06004F01 RID: 20225 RVA: 0x0014DB34 File Offset: 0x0014BD34
		public override void ListingClicked(ListingUI listingUI)
		{
			if (!listingUI.Listing.Item.IsPurchasable)
			{
				return;
			}
			if ((listingUI.Listing.Item as ClothingDefinition).Colorable)
			{
				this._selectedListing = listingUI.Listing;
				this.ColorPicker.Open(listingUI.Listing.Item);
				return;
			}
			base.ListingClicked(listingUI);
		}

		// Token: 0x06004F02 RID: 20226 RVA: 0x0014DB95 File Offset: 0x0014BD95
		protected override void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (this.ColorPicker != null && this.ColorPicker.IsOpen)
			{
				action.Used = true;
				this.ColorPicker.Close();
			}
			base.Exit(action);
		}

		// Token: 0x06004F03 RID: 20227 RVA: 0x0014DBD4 File Offset: 0x0014BDD4
		private void ColorPicked(EClothingColor color)
		{
			if (this._selectedListing == null)
			{
				return;
			}
			ClothingShopListing clothingShopListing = new ClothingShopListing();
			clothingShopListing.Item = this._selectedListing.Item;
			clothingShopListing.Color = color;
			this.Cart.AddItem(clothingShopListing, 1);
			this.AddItemSound.Play();
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x0014DC20 File Offset: 0x0014BE20
		public override bool HandoverItems()
		{
			List<ItemSlot> availableSlots = base.GetAvailableSlots();
			List<ShopListing> list = this.Cart.cartDictionary.Keys.ToList<ShopListing>();
			bool result = true;
			for (int i = 0; i < list.Count; i++)
			{
				NetworkSingleton<VariableDatabase>.Instance.NotifyItemAcquired(list[i].Item.ID, this.Cart.cartDictionary[list[i]]);
				int num = this.Cart.cartDictionary[list[i]];
				ClothingInstance clothingInstance = list[i].Item.GetDefaultInstance(1) as ClothingInstance;
				clothingInstance.Color = EClothingColor.White;
				if (list[i] is ClothingShopListing)
				{
					Console.Log("Color: " + (list[i] as ClothingShopListing).Color.ToString(), null);
					clothingInstance.Color = (list[i] as ClothingShopListing).Color;
				}
				int num2 = 0;
				while (num2 < availableSlots.Count && num > 0)
				{
					int capacityForItem = availableSlots[num2].GetCapacityForItem(clothingInstance, false);
					if (capacityForItem != 0)
					{
						int num3 = Mathf.Min(capacityForItem, num);
						availableSlots[num2].AddItem(clothingInstance.GetCopy(num3), false);
						num -= num3;
					}
					num2++;
				}
				if (num > 0)
				{
					Debug.LogWarning("Failed to handover all items in cart: " + clothingInstance.Name);
					result = false;
				}
			}
			return result;
		}

		// Token: 0x04003B1F RID: 15135
		public ShopColorPicker ColorPicker;

		// Token: 0x04003B20 RID: 15136
		private ShopListing _selectedListing;
	}
}
