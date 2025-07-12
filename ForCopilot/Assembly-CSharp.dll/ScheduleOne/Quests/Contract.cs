using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using ScheduleOne.UI;
using ScheduleOne.UI.Handover;
using ScheduleOne.UI.Phone;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.Quests
{
	// Token: 0x020002E2 RID: 738
	public class Contract : Quest
	{
		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06001006 RID: 4102 RVA: 0x0004715A File Offset: 0x0004535A
		// (set) Token: 0x06001007 RID: 4103 RVA: 0x00047162 File Offset: 0x00045362
		public NetworkObject Customer { get; protected set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06001008 RID: 4104 RVA: 0x0004716B File Offset: 0x0004536B
		// (set) Token: 0x06001009 RID: 4105 RVA: 0x00047173 File Offset: 0x00045373
		public Dealer Dealer { get; protected set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x0600100A RID: 4106 RVA: 0x0004717C File Offset: 0x0004537C
		// (set) Token: 0x0600100B RID: 4107 RVA: 0x00047184 File Offset: 0x00045384
		public float Payment { get; protected set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x0600100C RID: 4108 RVA: 0x0004718D File Offset: 0x0004538D
		// (set) Token: 0x0600100D RID: 4109 RVA: 0x00047195 File Offset: 0x00045395
		public int PickupScheduleIndex { get; protected set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x0600100E RID: 4110 RVA: 0x0004719E File Offset: 0x0004539E
		// (set) Token: 0x0600100F RID: 4111 RVA: 0x000471A6 File Offset: 0x000453A6
		public GameDateTime AcceptTime { get; protected set; }

		// Token: 0x06001010 RID: 4112 RVA: 0x000471AF File Offset: 0x000453AF
		protected override void Start()
		{
			this.autoInitialize = false;
			base.Start();
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x000471C0 File Offset: 0x000453C0
		public virtual void InitializeContract(string title, string description, QuestEntryData[] entries, string guid, NetworkObject customer, float payment, ProductList products, string deliveryLocationGUID, QuestWindowConfig deliveryWindow, int pickupScheduleIndex, GameDateTime acceptTime)
		{
			this.SilentlyInitializeContract(this.title, this.Description, entries, guid, customer, payment, products, deliveryLocationGUID, deliveryWindow, pickupScheduleIndex, acceptTime);
			Debug.Log("Contract initialized");
			Contract.Contracts.Add(this);
			base.InitializeQuest(title, description, entries, guid);
			this.Customer.GetComponent<Customer>().AssignContract(this);
			if (this.DeliveryLocation != null && !this.DeliveryLocation.ScheduledContracts.Contains(this))
			{
				this.DeliveryLocation.ScheduledContracts.Add(this);
			}
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00047254 File Offset: 0x00045454
		public virtual void SilentlyInitializeContract(string title, string description, QuestEntryData[] entries, string guid, NetworkObject customer, float payment, ProductList products, string deliveryLocationGUID, QuestWindowConfig deliveryWindow, int pickupScheduleIndex, GameDateTime acceptTime)
		{
			this.Customer = customer;
			this.Payment = Mathf.Clamp(payment, 0f, float.MaxValue);
			this.ProductList = products;
			if (GUIDManager.IsGUIDValid(deliveryLocationGUID))
			{
				this.DeliveryLocation = GUIDManager.GetObject<DeliveryLocation>(new Guid(deliveryLocationGUID));
			}
			this.DeliveryWindow = deliveryWindow;
			this.PickupScheduleIndex = pickupScheduleIndex;
			this.AcceptTime = acceptTime;
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x000472BB File Offset: 0x000454BB
		protected override void MinPass()
		{
			base.MinPass();
			this.UpdateTiming();
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x000472C9 File Offset: 0x000454C9
		private void OnDestroy()
		{
			Contract.Contracts.Remove(this);
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x000472D8 File Offset: 0x000454D8
		private void UpdateTiming()
		{
			if (base.Expires && this.ExpiryVisibility != EExpiryVisibility.Never)
			{
				int minsUntilExpiry = base.GetMinsUntilExpiry();
				int num = Mathf.FloorToInt((float)minsUntilExpiry / 60f);
				int num2 = minsUntilExpiry - 360;
				int num3 = Mathf.FloorToInt((float)num2 / 60f);
				if (num2 > 0)
				{
					if (num3 > 0)
					{
						base.SetSubtitle("<color=#c0c0c0ff> (Begins in " + num3.ToString() + " hrs)</color>");
						return;
					}
					base.SetSubtitle("<color=#c0c0c0ff> (Begins in " + num2.ToString() + " min)</color>");
					return;
				}
				else if (minsUntilExpiry < 120)
				{
					if (num > 0)
					{
						base.SetSubtitle(string.Concat(new string[]
						{
							"<color=#",
							ColorUtility.ToHtmlStringRGBA(this.criticalTimeBackground.color),
							"> (Expires in ",
							num.ToString(),
							" hrs)</color>"
						}));
						return;
					}
					base.SetSubtitle(string.Concat(new string[]
					{
						"<color=#",
						ColorUtility.ToHtmlStringRGBA(this.criticalTimeBackground.color),
						"> (Expires in ",
						minsUntilExpiry.ToString(),
						" min)</color>"
					}));
					return;
				}
				else
				{
					if (num > 0)
					{
						base.SetSubtitle("<color=green> (Expires in " + num.ToString() + " hrs)</color>");
						return;
					}
					base.SetSubtitle("<color=green> (Expires in " + num.ToString() + " min)</color>");
				}
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00047440 File Offset: 0x00045640
		public override void End()
		{
			base.End();
			if (this.DeliveryLocation != null)
			{
				this.DeliveryLocation.ScheduledContracts.Remove(this);
			}
			Contract.Contracts.Remove(this);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00047474 File Offset: 0x00045674
		public override void Complete(bool network = true)
		{
			if (InstanceFinder.IsServer && !this.completedContractsIncremented)
			{
				this.completedContractsIncremented = true;
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Completed_Contracts_Count", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Completed_Contracts_Count") + 1f).ToString(), true);
			}
			base.Complete(network);
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x000474CB File Offset: 0x000456CB
		public void SetDealer(Dealer dealer)
		{
			this.Dealer = dealer;
			if (this.journalEntry != null)
			{
				this.journalEntry.gameObject.SetActive(this.ShouldShowJournalEntry());
			}
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x000474F8 File Offset: 0x000456F8
		public virtual void SubmitPayment(float bonusTotal)
		{
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(this.Payment + bonusTotal, true, false);
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0004750E File Offset: 0x0004570E
		protected override void SendExpiryReminder()
		{
			Singleton<NotificationsManager>.Instance.SendNotification("<color=#FFB43C>Deal Expiring Soon</color>", this.title, PlayerSingleton<JournalApp>.Instance.AppIcon, 5f, true);
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00047535 File Offset: 0x00045735
		protected override void SendExpiredNotification()
		{
			Singleton<NotificationsManager>.Instance.SendNotification("<color=#FF6455>Deal Expired</color>", this.title, PlayerSingleton<JournalApp>.Instance.AppIcon, 5f, true);
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0004755C File Offset: 0x0004575C
		protected override bool ShouldShowJournalEntry()
		{
			return !(this.Dealer != null) && base.ShouldShowJournalEntry();
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00047574 File Offset: 0x00045774
		protected override bool CanExpire()
		{
			return !(Singleton<HandoverScreen>.Instance.CurrentContract == this) && !this.Customer.GetComponent<NPC>().dialogueHandler.IsPlaying && base.CanExpire();
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x000475AC File Offset: 0x000457AC
		public bool DoesProductListMatchSpecified(List<ItemInstance> items, bool enforceQuality)
		{
			using (List<ProductList.Entry>.Enumerator enumerator = this.ProductList.entries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ProductList.Entry entry = enumerator.Current;
					List<ItemInstance> list = (from x in items
					where x.ID == entry.ProductID
					select x).ToList<ItemInstance>();
					List<ProductItemInstance> list2 = new List<ProductItemInstance>();
					for (int i = 0; i < list.Count; i++)
					{
						list2.Add(list[i] as ProductItemInstance);
					}
					List<ProductItemInstance> list3 = new List<ProductItemInstance>();
					for (int j = 0; j < items.Count; j++)
					{
						ProductItemInstance productItemInstance = items[j] as ProductItemInstance;
						if (productItemInstance.Quality >= entry.Quality)
						{
							list3.Add(productItemInstance);
						}
					}
					int num = 0;
					for (int k = 0; k < list2.Count; k++)
					{
						num += list2[k].Quantity * list2[k].Amount;
					}
					int num2 = 0;
					for (int l = 0; l < list3.Count; l++)
					{
						num2 += list3[l].Quantity * list2[l].Amount;
					}
					if (enforceQuality)
					{
						if (num2 < entry.Quantity)
						{
							return false;
						}
					}
					else if (num < entry.Quantity)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0004774C File Offset: 0x0004594C
		public float GetProductListMatch(List<ItemInstance> items, out int matchedProductCount)
		{
			float num = 0f;
			int totalQuantity = this.ProductList.GetTotalQuantity();
			matchedProductCount = 0;
			List<ItemInstance> list = new List<ItemInstance>();
			for (int i = 0; i < items.Count; i++)
			{
				list.Add(items[i].GetCopy(-1));
			}
			foreach (ProductList.Entry entry in this.ProductList.entries)
			{
				int num2 = entry.Quantity;
				ProductDefinition other = Registry.GetItem(entry.ProductID) as ProductDefinition;
				Dictionary<ProductItemInstance, float> matchRatings = new Dictionary<ProductItemInstance, float>();
				foreach (ItemInstance itemInstance in list)
				{
					if (itemInstance.Quantity != 0)
					{
						ProductItemInstance productItemInstance = itemInstance as ProductItemInstance;
						if (productItemInstance != null && !(productItemInstance.AppliedPackaging == null))
						{
							matchRatings.Add(productItemInstance, productItemInstance.GetSimilarity(other, entry.Quality));
						}
					}
				}
				List<ProductItemInstance> list2 = matchRatings.Keys.ToList<ProductItemInstance>();
				list2.Sort((ProductItemInstance x, ProductItemInstance y) => matchRatings[y].CompareTo(matchRatings[x]));
				for (int j = 0; j < list2.Count; j++)
				{
					int amount = list2[j].Amount;
					int quantity = list2[j].Quantity;
					int num3 = Mathf.Min(Mathf.CeilToInt((float)num2 / (float)amount), list2[j].Quantity);
					num2 -= num3 * amount;
					num += matchRatings[list2[j]] * (float)num3 * (float)amount;
					if (matchRatings[list2[j]] > 0f)
					{
						matchedProductCount += num3 * amount;
					}
					list2[j].ChangeQuantity(-num3);
				}
			}
			return num / (float)totalQuantity;
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00047990 File Offset: 0x00045B90
		public override SaveData GetSaveData()
		{
			List<QuestEntryData> list = new List<QuestEntryData>();
			for (int i = 0; i < this.Entries.Count; i++)
			{
				list.Add(this.Entries[i].GetSaveData());
			}
			return new ContractData(base.GUID.ToString(), base.QuestState, base.IsTracked, this.title, this.Description, base.Expires, new GameDateTimeData(base.Expiry), list.ToArray(), this.Customer.GetComponent<NPC>().GUID.ToString(), this.Payment, this.ProductList, this.DeliveryLocation.GUID.ToString(), this.DeliveryWindow, this.PickupScheduleIndex, new GameDateTimeData(this.AcceptTime));
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x00047A72 File Offset: 0x00045C72
		public new bool ShouldSave()
		{
			return !(base.gameObject == null) && base.QuestState == EQuestState.Active;
		}

		// Token: 0x04001082 RID: 4226
		public const int DefaultExpiryTime = 2880;

		// Token: 0x04001083 RID: 4227
		public static List<Contract> Contracts = new List<Contract>();

		// Token: 0x04001087 RID: 4231
		[Header("Contract Settings")]
		public ProductList ProductList;

		// Token: 0x04001088 RID: 4232
		public DeliveryLocation DeliveryLocation;

		// Token: 0x04001089 RID: 4233
		public QuestWindowConfig DeliveryWindow;

		// Token: 0x0400108C RID: 4236
		private bool completedContractsIncremented;

		// Token: 0x020002E3 RID: 739
		public class BonusPayment
		{
			// Token: 0x06001024 RID: 4132 RVA: 0x00047AA1 File Offset: 0x00045CA1
			public BonusPayment(string title, float amount)
			{
				this.Title = title;
				this.Amount = Mathf.Clamp(amount, 0f, float.MaxValue);
			}

			// Token: 0x0400108D RID: 4237
			public string Title;

			// Token: 0x0400108E RID: 4238
			public float Amount;
		}
	}
}
