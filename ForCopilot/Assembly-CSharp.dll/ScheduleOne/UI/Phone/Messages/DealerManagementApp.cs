using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Economy;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.Product;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000B00 RID: 2816
	public class DealerManagementApp : App<DealerManagementApp>
	{
		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06004B91 RID: 19345 RVA: 0x0013D4A1 File Offset: 0x0013B6A1
		// (set) Token: 0x06004B92 RID: 19346 RVA: 0x0013D4A9 File Offset: 0x0013B6A9
		public Dealer SelectedDealer { get; private set; }

		// Token: 0x06004B93 RID: 19347 RVA: 0x0013D4B4 File Offset: 0x0013B6B4
		protected override void Awake()
		{
			base.Awake();
			foreach (Dealer dealer in Dealer.AllDealers)
			{
				if (dealer.IsRecruited)
				{
					this.AddDealer(dealer);
				}
			}
			Dealer.onDealerRecruited = (Action<Dealer>)Delegate.Combine(Dealer.onDealerRecruited, new Action<Dealer>(this.AddDealer));
			this.BackButton.onClick.AddListener(new UnityAction(this.BackPressed));
			this.NextButton.onClick.AddListener(new UnityAction(this.NextPressed));
			this.AssignCustomerButton.onClick.AddListener(new UnityAction(this.AssignCustomer));
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x0013D588 File Offset: 0x0013B788
		protected override void Start()
		{
			base.Start();
			this.CustomerSelector.onCustomerSelected.AddListener(new UnityAction<Customer>(this.AddCustomer));
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x0013D5AC File Offset: 0x0013B7AC
		protected override void OnDestroy()
		{
			Dealer.onDealerRecruited = (Action<Dealer>)Delegate.Remove(Dealer.onDealerRecruited, new Action<Dealer>(this.AddDealer));
			base.OnDestroy();
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x0013D5D4 File Offset: 0x0013B7D4
		public override void SetOpen(bool open)
		{
			if (this.SelectedDealer != null)
			{
				this.SetDisplayedDealer(this.SelectedDealer);
			}
			else if (this.dealers.Count > 0)
			{
				this.SetDisplayedDealer(this.dealers[0]);
			}
			else
			{
				this.NoDealersLabel.gameObject.SetActive(true);
				this.Content.gameObject.SetActive(false);
			}
			base.SetOpen(open);
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x0013D648 File Offset: 0x0013B848
		public void SetDisplayedDealer(Dealer dealer)
		{
			this.SelectedDealer = dealer;
			this.SelectorImage.sprite = dealer.MugshotSprite;
			this.SelectorTitle.text = dealer.fullName;
			this.CashLabel.text = MoneyManager.FormatAmount(dealer.Cash, false, false);
			this.CutLabel.text = Mathf.RoundToInt(dealer.Cut * 100f).ToString() + "%";
			this.HomeLabel.text = dealer.HomeName;
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			List<string> list = new List<string>();
			foreach (ItemSlot itemSlot in dealer.GetAllSlots())
			{
				if (itemSlot.Quantity != 0)
				{
					int num = itemSlot.Quantity;
					if (itemSlot.ItemInstance is ProductItemInstance)
					{
						num *= ((ProductItemInstance)itemSlot.ItemInstance).Amount;
					}
					if (list.Contains(itemSlot.ItemInstance.ID))
					{
						Dictionary<string, int> dictionary2 = dictionary;
						string id = itemSlot.ItemInstance.ID;
						dictionary2[id] += num;
					}
					else
					{
						list.Add(itemSlot.ItemInstance.ID);
						dictionary.Add(itemSlot.ItemInstance.ID, num);
					}
				}
			}
			for (int i = 0; i < this.InventoryEntries.Length; i++)
			{
				if (list.Count > i)
				{
					ItemDefinition item = Registry.GetItem(list[i]);
					this.InventoryEntries[i].Find("Image").GetComponent<Image>().sprite = item.Icon;
					this.InventoryEntries[i].Find("Title").GetComponent<Text>().text = dictionary[list[i]].ToString() + "x " + item.Name;
					this.InventoryEntries[i].gameObject.SetActive(true);
				}
				else
				{
					this.InventoryEntries[i].gameObject.SetActive(false);
				}
			}
			this.CustomerTitleLabel.text = string.Concat(new string[]
			{
				"Assigned Customers (",
				dealer.AssignedCustomers.Count.ToString(),
				"/",
				8.ToString(),
				")"
			});
			for (int j = 0; j < this.CustomerEntries.Length; j++)
			{
				if (dealer.AssignedCustomers.Count > j)
				{
					Customer customer = dealer.AssignedCustomers[j];
					this.CustomerEntries[j].Find("Mugshot").GetComponent<Image>().sprite = customer.NPC.MugshotSprite;
					this.CustomerEntries[j].Find("Name").GetComponent<Text>().text = customer.NPC.fullName;
					Button component = this.CustomerEntries[j].Find("Remove").GetComponent<Button>();
					component.onClick.RemoveAllListeners();
					component.onClick.AddListener(delegate()
					{
						this.RemoveCustomer(customer);
					});
					this.CustomerEntries[j].gameObject.SetActive(true);
				}
				else
				{
					this.CustomerEntries[j].gameObject.SetActive(false);
				}
			}
			this.BackButton.interactable = (this.dealers.IndexOf(dealer) > 0);
			this.NextButton.interactable = (this.dealers.IndexOf(dealer) < this.dealers.Count - 1);
			this.AssignCustomerButton.gameObject.SetActive(dealer.AssignedCustomers.Count < 8);
			this.NoDealersLabel.gameObject.SetActive(false);
			this.Content.gameObject.SetActive(true);
		}

		// Token: 0x06004B98 RID: 19352 RVA: 0x0013DA70 File Offset: 0x0013BC70
		private void AddDealer(Dealer dealer)
		{
			if (this.dealers.Contains(dealer))
			{
				return;
			}
			this.dealers.Add(dealer);
			this.dealers = (from d in this.dealers
			orderby d.FirstName
			select d).ToList<Dealer>();
		}

		// Token: 0x06004B99 RID: 19353 RVA: 0x0013DACD File Offset: 0x0013BCCD
		private void AddCustomer(Customer customer)
		{
			this.SelectedDealer.SendAddCustomer(customer.NPC.ID);
			if (customer.OfferedContractInfo != null)
			{
				Console.Log("Expiring...", null);
				customer.ExpireOffer();
			}
			this.SetDisplayedDealer(this.SelectedDealer);
		}

		// Token: 0x06004B9A RID: 19354 RVA: 0x0013DB0A File Offset: 0x0013BD0A
		private void RemoveCustomer(Customer customer)
		{
			this.SelectedDealer.SendRemoveCustomer(customer.NPC.ID);
			this.SetDisplayedDealer(this.SelectedDealer);
		}

		// Token: 0x06004B9B RID: 19355 RVA: 0x0013DB30 File Offset: 0x0013BD30
		private void BackPressed()
		{
			int num = this.dealers.IndexOf(this.SelectedDealer);
			if (num > 0)
			{
				this.SetDisplayedDealer(this.dealers[num - 1]);
			}
		}

		// Token: 0x06004B9C RID: 19356 RVA: 0x0013DB68 File Offset: 0x0013BD68
		private void NextPressed()
		{
			int num = this.dealers.IndexOf(this.SelectedDealer);
			if (num < this.dealers.Count - 1)
			{
				this.SetDisplayedDealer(this.dealers[num + 1]);
			}
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x0013DBAB File Offset: 0x0013BDAB
		public void AssignCustomer()
		{
			this.CustomerSelector.Open();
		}

		// Token: 0x040037E7 RID: 14311
		[Header("References")]
		public Text NoDealersLabel;

		// Token: 0x040037E8 RID: 14312
		public RectTransform Content;

		// Token: 0x040037E9 RID: 14313
		public CustomerSelector CustomerSelector;

		// Token: 0x040037EA RID: 14314
		[Header("Selector")]
		public Image SelectorImage;

		// Token: 0x040037EB RID: 14315
		public Text SelectorTitle;

		// Token: 0x040037EC RID: 14316
		public Button BackButton;

		// Token: 0x040037ED RID: 14317
		public Button NextButton;

		// Token: 0x040037EE RID: 14318
		[Header("Basic Info")]
		public Text CashLabel;

		// Token: 0x040037EF RID: 14319
		public Text CutLabel;

		// Token: 0x040037F0 RID: 14320
		public Text HomeLabel;

		// Token: 0x040037F1 RID: 14321
		[Header("Inventory")]
		public RectTransform[] InventoryEntries;

		// Token: 0x040037F2 RID: 14322
		[Header("Customers")]
		public Text CustomerTitleLabel;

		// Token: 0x040037F3 RID: 14323
		public RectTransform[] CustomerEntries;

		// Token: 0x040037F4 RID: 14324
		public Button AssignCustomerButton;

		// Token: 0x040037F5 RID: 14325
		private List<Dealer> dealers = new List<Dealer>();
	}
}
