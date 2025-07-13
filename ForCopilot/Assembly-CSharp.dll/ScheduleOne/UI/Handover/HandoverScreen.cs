using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Quests;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Handover
{
	// Token: 0x02000B75 RID: 2933
	public class HandoverScreen : Singleton<HandoverScreen>
	{
		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06004DD2 RID: 19922 RVA: 0x0014879D File Offset: 0x0014699D
		// (set) Token: 0x06004DD3 RID: 19923 RVA: 0x001487A5 File Offset: 0x001469A5
		public Contract CurrentContract { get; protected set; }

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06004DD4 RID: 19924 RVA: 0x001487AE File Offset: 0x001469AE
		// (set) Token: 0x06004DD5 RID: 19925 RVA: 0x001487B6 File Offset: 0x001469B6
		public bool IsOpen { get; protected set; }

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06004DD6 RID: 19926 RVA: 0x001487BF File Offset: 0x001469BF
		// (set) Token: 0x06004DD7 RID: 19927 RVA: 0x001487C7 File Offset: 0x001469C7
		public bool TutorialOpen { get; private set; }

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06004DD8 RID: 19928 RVA: 0x001487D0 File Offset: 0x001469D0
		// (set) Token: 0x06004DD9 RID: 19929 RVA: 0x001487D8 File Offset: 0x001469D8
		public HandoverScreen.EMode Mode { get; protected set; }

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06004DDA RID: 19930 RVA: 0x001487E1 File Offset: 0x001469E1
		// (set) Token: 0x06004DDB RID: 19931 RVA: 0x001487E9 File Offset: 0x001469E9
		public Customer CurrentCustomer { get; private set; }

		// Token: 0x06004DDC RID: 19932 RVA: 0x001487F4 File Offset: 0x001469F4
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 8);
			this.VehicleSlotUIs = this.VehicleSlotContainer.GetComponentsInChildren<ItemSlotUI>();
			this.CustomerSlotUIs = this.CustomerSlotContainer.GetComponentsInChildren<ItemSlotUI>();
			this.DoneButton.onClick.AddListener(new UnityAction(this.DonePressed));
			for (int i = 0; i < this.CustomerSlots.Length; i++)
			{
				this.CustomerSlots[i] = new ItemSlot();
				this.CustomerSlotUIs[i].AssignSlot(this.CustomerSlots[i]);
				ItemSlot itemSlot = this.CustomerSlots[i];
				itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(this.CustomerItemsChanged));
			}
			this.VehicleSubtitle.text = "This is the vehicle you last drove.\nMust be within " + 20f.ToString() + " meters.";
			this.ClearCustomerSlots(false);
			this.PriceSelector.gameObject.SetActive(false);
			this.PriceSelector.onPriceChanged.AddListener(new UnityAction(this.UpdateSuccessChance));
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.IsOpen = false;
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x00148934 File Offset: 0x00146B34
		private void Update()
		{
			if (this.IsOpen && ((Player.Local.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None && Player.Local.CrimeData.TimeSinceSighted < 5f) || Player.Local.CrimeData.CurrentArrestProgress > 0.01f))
			{
				this.Close(HandoverScreen.EHandoverOutcome.Cancelled);
			}
		}

		// Token: 0x06004DDE RID: 19934 RVA: 0x0014898D File Offset: 0x00146B8D
		private void OpenTutorial()
		{
			this.CanvasGroup.alpha = 0f;
			this.TutorialOpen = true;
			this.TutorialContainer.gameObject.SetActive(true);
			this.TutorialAnimation.Play();
		}

		// Token: 0x06004DDF RID: 19935 RVA: 0x001489C3 File Offset: 0x00146BC3
		public void CloseTutorial()
		{
			this.CanvasGroup.alpha = 1f;
			this.TutorialOpen = false;
			this.TutorialContainer.gameObject.SetActive(false);
		}

		// Token: 0x06004DE0 RID: 19936 RVA: 0x001489F0 File Offset: 0x00146BF0
		public virtual void Open(Contract contract, Customer customer, HandoverScreen.EMode mode, Action<HandoverScreen.EHandoverOutcome, List<ItemInstance>, float> callback, Func<List<ItemInstance>, float, float> successChanceMethod)
		{
			if (mode == HandoverScreen.EMode.Contract && contract == null)
			{
				Console.LogWarning("Contract is null", null);
				return;
			}
			this.CurrentContract = contract;
			this.CurrentCustomer = customer;
			this.Mode = mode;
			if (this.Mode == HandoverScreen.EMode.Contract)
			{
				this.TitleLabel.text = "Complete Deal";
			}
			else if (this.Mode == HandoverScreen.EMode.Sample)
			{
				this.TitleLabel.text = "Give Free Sample";
			}
			else if (this.Mode == HandoverScreen.EMode.Offer)
			{
				this.TitleLabel.text = "Offer Deal";
			}
			this.DetailPanel.Open(customer);
			this.onHandoverComplete = callback;
			this.SuccessChanceMethod = successChanceMethod;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			Singleton<CompassManager>.Instance.SetVisible(false);
			List<ItemSlot> allInventorySlots = PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots();
			List<ItemSlot> secondarySlots = new List<ItemSlot>(this.CustomerSlots);
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("ItemAmountSelectionTutorialDone") && GameManager.IS_TUTORIAL)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("ItemAmountSelectionTutorialDone", true.ToString(), true);
				this.OpenTutorial();
			}
			else
			{
				Player.Local.VisualState.ApplyState("drugdeal", PlayerVisualState.EVisualState.DrugDealing, 0f);
			}
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			if (this.Mode == HandoverScreen.EMode.Contract)
			{
				this.DescriptionLabel.text = customer.NPC.FirstName + " is paying <color=#50E65A>" + MoneyManager.FormatAmount(contract.Payment, false, false) + "</color> for:";
				this.DescriptionLabel.enabled = true;
			}
			else
			{
				this.DescriptionLabel.enabled = false;
			}
			if (this.Mode == HandoverScreen.EMode.Sample)
			{
				EDrugType property = customer.GetOrderedDrugTypes()[0];
				string text = ColorUtility.ToHtmlStringRGB(property.GetColor());
				this.FavouriteDrugLabel.text = string.Concat(new string[]
				{
					customer.NPC.FirstName,
					"'s favourite drug: <color=#",
					text,
					">",
					property.ToString(),
					"</color>"
				});
				this.FavouriteDrugLabel.enabled = true;
				this.FavouritePropertiesLabel.text = customer.NPC.FirstName + "'s favourite effects:";
				for (int i = 0; i < this.PropertiesEntries.Length; i++)
				{
					if (customer.CustomerData.PreferredProperties.Count > i)
					{
						this.PropertiesEntries[i].text = "•  " + customer.CustomerData.PreferredProperties[i].Name;
						this.PropertiesEntries[i].color = customer.CustomerData.PreferredProperties[i].LabelColor;
						this.PropertiesEntries[i].enabled = true;
					}
					else
					{
						this.PropertiesEntries[i].enabled = false;
					}
				}
				this.FavouritePropertiesLabel.gameObject.SetActive(true);
			}
			else
			{
				this.FavouriteDrugLabel.enabled = false;
				this.FavouritePropertiesLabel.gameObject.SetActive(false);
			}
			for (int j = 0; j < this.ExpectationEntries.Length; j++)
			{
				if (contract != null && contract.ProductList.entries.Count > j)
				{
					this.ExpectationEntries[j].Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text = "<color=#FFC73D>" + contract.ProductList.entries[j].Quantity.ToString() + "x</color> " + Registry.GetItem(contract.ProductList.entries[j].ProductID).Name;
					this.ExpectationEntries[j].Find("Star").GetComponent<Image>().color = ItemQuality.GetColor(contract.ProductList.entries[j].Quality);
					this.ExpectationEntries[j].Find("Star").GetComponent<RectTransform>().anchoredPosition = new Vector2(-this.ExpectationEntries[j].Find("Title").gameObject.GetComponent<TextMeshProUGUI>().preferredWidth / 2f + 30f, 0f);
					this.ExpectationEntries[j].gameObject.SetActive(true);
				}
				else
				{
					this.ExpectationEntries[j].gameObject.SetActive(false);
				}
			}
			if (Player.Local.LastDrivenVehicle != null && Player.Local.LastDrivenVehicle.Storage != null && Vector3.Distance(Player.Local.LastDrivenVehicle.transform.position, Player.Local.transform.position) < 20f)
			{
				if (Player.Local.LastDrivenVehicle.Storage != null)
				{
					for (int k = 0; k < this.VehicleSlotUIs.Length; k++)
					{
						ItemSlot itemSlot = null;
						if (k < Player.Local.LastDrivenVehicle.Storage.ItemSlots.Count)
						{
							itemSlot = Player.Local.LastDrivenVehicle.Storage.ItemSlots[k];
						}
						if (itemSlot != null)
						{
							this.VehicleSlotUIs[k].AssignSlot(itemSlot);
							this.VehicleSlotUIs[k].gameObject.SetActive(true);
							allInventorySlots.Add(itemSlot);
						}
						else
						{
							this.VehicleSlotUIs[k].gameObject.SetActive(false);
						}
					}
				}
				this.NoVehicle.gameObject.SetActive(false);
				this.VehicleContainer.gameObject.SetActive(true);
			}
			else
			{
				this.NoVehicle.gameObject.SetActive(true);
				this.VehicleContainer.gameObject.SetActive(false);
			}
			if (this.Mode == HandoverScreen.EMode.Contract)
			{
				this.CustomerSubtitle.text = "Place the expected products here";
			}
			else if (this.Mode == HandoverScreen.EMode.Sample)
			{
				this.CustomerSubtitle.text = "Place a product here for " + customer.NPC.FirstName + " to try";
			}
			else if (this.Mode == HandoverScreen.EMode.Offer)
			{
				this.CustomerSubtitle.text = "Place product here";
			}
			if (mode == HandoverScreen.EMode.Offer)
			{
				this.PriceSelector.gameObject.SetActive(true);
				this.PriceSelector.SetPrice(1f);
			}
			else
			{
				this.PriceSelector.gameObject.SetActive(false);
			}
			this.RecordOriginalLocations();
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, true);
			Singleton<ItemUIManager>.Instance.EnableQuickMove(allInventorySlots, secondarySlots);
			this.CustomerItemsChanged();
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			this.IsOpen = true;
		}

		// Token: 0x06004DE1 RID: 19937 RVA: 0x001490E8 File Offset: 0x001472E8
		public virtual void Close(HandoverScreen.EHandoverOutcome outcome)
		{
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			List<ItemInstance> list = new List<ItemInstance>();
			if (outcome == HandoverScreen.EHandoverOutcome.Finalize)
			{
				for (int i = 0; i < this.CustomerSlots.Length; i++)
				{
					if (this.CustomerSlots[i].ItemInstance != null)
					{
						list.Add(this.CustomerSlots[i].ItemInstance);
					}
				}
			}
			Singleton<CompassManager>.Instance.SetVisible(true);
			this.CurrentContract = null;
			this.CurrentCustomer = null;
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			float arg = 0f;
			if (this.Mode == HandoverScreen.EMode.Offer)
			{
				this.PriceSelector.RefreshPrice();
				arg = this.PriceSelector.Price;
			}
			if (this.onHandoverComplete != null)
			{
				this.onHandoverComplete(outcome, list, arg);
			}
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			Player.Local.VisualState.RemoveState("drugdeal", 0f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			if (outcome == HandoverScreen.EHandoverOutcome.Cancelled)
			{
				this.ClearCustomerSlots(true);
			}
		}

		// Token: 0x06004DE2 RID: 19938 RVA: 0x00149221 File Offset: 0x00147421
		public void DonePressed()
		{
			this.Close(HandoverScreen.EHandoverOutcome.Finalize);
		}

		// Token: 0x06004DE3 RID: 19939 RVA: 0x0014922C File Offset: 0x0014742C
		private void RecordOriginalLocations()
		{
			foreach (HotbarSlot hotbarSlot in PlayerSingleton<PlayerInventory>.Instance.hotbarSlots)
			{
				if (hotbarSlot.ItemInstance != null)
				{
					if (this.OriginalItemLocations.ContainsKey(hotbarSlot.ItemInstance))
					{
						Console.LogWarning("Item already exists in original locations", null);
					}
					else
					{
						this.OriginalItemLocations.Add(hotbarSlot.ItemInstance, HandoverScreen.EItemSource.Player);
					}
				}
			}
		}

		// Token: 0x06004DE4 RID: 19940 RVA: 0x001492B8 File Offset: 0x001474B8
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.Used = true;
				if (this.TutorialOpen)
				{
					this.CloseTutorial();
					return;
				}
				this.Close(HandoverScreen.EHandoverOutcome.Cancelled);
			}
		}

		// Token: 0x06004DE5 RID: 19941 RVA: 0x001492F4 File Offset: 0x001474F4
		public void ClearCustomerSlots(bool returnToOriginals)
		{
			this.ignoreCustomerChangedEvents = true;
			foreach (ItemSlot itemSlot in this.CustomerSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					if (returnToOriginals)
					{
						PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(itemSlot.ItemInstance);
					}
					itemSlot.ClearStoredInstance(false);
				}
			}
			this.OriginalItemLocations.Clear();
			this.ignoreCustomerChangedEvents = false;
			this.CustomerItemsChanged();
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x0014935C File Offset: 0x0014755C
		private void CustomerItemsChanged()
		{
			if (this.ignoreCustomerChangedEvents)
			{
				return;
			}
			this.UpdateDoneButton();
			this.UpdateSuccessChance();
			if (this.Mode == HandoverScreen.EMode.Offer)
			{
				float customerItemsValue = this.GetCustomerItemsValue();
				this.PriceSelector.SetPrice(customerItemsValue);
				this.FairPriceLabel.text = "Fair price: " + MoneyManager.FormatAmount(customerItemsValue, false, false);
			}
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x001493B8 File Offset: 0x001475B8
		private void UpdateDoneButton()
		{
			string text;
			if (this.GetError(out text))
			{
				this.DoneButton.interactable = false;
				this.ErrorLabel.text = text;
				this.ErrorLabel.enabled = true;
			}
			else
			{
				this.DoneButton.interactable = true;
				this.ErrorLabel.enabled = false;
			}
			string text2;
			if (!this.ErrorLabel.enabled && this.GetWarning(out text2))
			{
				this.WarningLabel.text = text2;
				this.WarningLabel.enabled = true;
				return;
			}
			this.WarningLabel.enabled = false;
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x0014944C File Offset: 0x0014764C
		private void UpdateSuccessChance()
		{
			if (this.GetCustomerItems(false).Count == 0)
			{
				this.SuccessLabel.enabled = false;
				return;
			}
			float num;
			if (this.Mode == HandoverScreen.EMode.Sample)
			{
				Func<List<ItemInstance>, float, float> successChanceMethod = this.SuccessChanceMethod;
				num = ((successChanceMethod != null) ? successChanceMethod(this.GetCustomerItems(true), 0f) : 0f);
				this.SuccessLabel.text = Mathf.RoundToInt(num * 100f).ToString() + "% chance of success";
				this.SuccessLabel.color = this.SuccessColorMap.Evaluate(num);
				this.SuccessLabel.enabled = true;
				return;
			}
			if (this.Mode != HandoverScreen.EMode.Contract)
			{
				if (this.Mode == HandoverScreen.EMode.Offer)
				{
					float price = this.PriceSelector.Price;
					Func<List<ItemInstance>, float, float> successChanceMethod2 = this.SuccessChanceMethod;
					num = ((successChanceMethod2 != null) ? successChanceMethod2(this.GetCustomerItems(true), price) : 0f);
					this.SuccessLabel.text = Mathf.RoundToInt(num * 100f).ToString() + "% chance of success";
					this.SuccessLabel.color = this.SuccessColorMap.Evaluate(num);
					this.SuccessLabel.enabled = true;
				}
				return;
			}
			if (this.CurrentContract == null)
			{
				Console.LogWarning("Current contract is null", null);
				return;
			}
			int num2;
			num = Mathf.Clamp(this.CurrentContract.GetProductListMatch(this.GetCustomerItems(true), out num2), 0.01f, 1f);
			if (num < 1f)
			{
				this.SuccessLabel.text = Mathf.RoundToInt(num * 100f).ToString() + "% chance of customer accepting";
				this.SuccessLabel.color = this.SuccessColorMap.Evaluate(num);
				this.SuccessLabel.enabled = true;
				return;
			}
			this.SuccessLabel.enabled = false;
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x00149620 File Offset: 0x00147820
		private bool GetError(out string err)
		{
			err = string.Empty;
			if (this.Mode == HandoverScreen.EMode.Contract && this.CurrentContract != null)
			{
				if (this.GetCustomerItemsCount(false) == 0)
				{
					err = string.Empty;
					return true;
				}
				if (NetworkSingleton<GameManager>.Instance.IsTutorial && this.GetCustomerItemsCount(true) > this.CurrentContract.ProductList.GetTotalQuantity())
				{
					err = "You are providing more product than required.";
					return true;
				}
			}
			if ((this.Mode == HandoverScreen.EMode.Sample || this.Mode == HandoverScreen.EMode.Offer) && this.GetCustomerItemsCount(true) == 0)
			{
				bool flag = false;
				for (int i = 0; i < this.CustomerSlots.Length; i++)
				{
					if (this.CustomerSlots[i].ItemInstance != null && this.CustomerSlots[i].ItemInstance is ProductItemInstance && (this.CustomerSlots[i].ItemInstance as ProductItemInstance).AppliedPackaging == null)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					err = "Product must be packaged";
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x00149710 File Offset: 0x00147910
		private bool GetWarning(out string warning)
		{
			warning = string.Empty;
			if (this.Mode == HandoverScreen.EMode.Contract)
			{
				if (this.CurrentContract != null)
				{
					int num;
					if (this.CurrentContract.GetProductListMatch(this.GetCustomerItems(true), out num) < 1f)
					{
						warning = "Customer expectations not met";
						return true;
					}
					if (this.GetCustomerItemsCount(false) > this.CurrentContract.ProductList.GetTotalQuantity())
					{
						warning = "You are providing more items than required.";
						return true;
					}
				}
			}
			else if (this.Mode == HandoverScreen.EMode.Sample && this.GetCustomerItemsCount(false) > 1)
			{
				warning = "Only 1 sample product is required.";
				return true;
			}
			bool flag = false;
			for (int i = 0; i < this.CustomerSlots.Length; i++)
			{
				if (this.CustomerSlots[i].ItemInstance != null && this.CustomerSlots[i].ItemInstance is ProductItemInstance && (this.CustomerSlots[i].ItemInstance as ProductItemInstance).AppliedPackaging == null)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				warning = "Product must be packaged";
				return true;
			}
			return false;
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x00149804 File Offset: 0x00147A04
		private List<ItemInstance> GetCustomerItems(bool onlyPackagedProduct = true)
		{
			List<ItemInstance> list = new List<ItemInstance>();
			for (int i = 0; i < this.CustomerSlots.Length; i++)
			{
				if (this.CustomerSlots[i].ItemInstance != null)
				{
					if (onlyPackagedProduct)
					{
						ProductItemInstance productItemInstance = this.CustomerSlots[i].ItemInstance as ProductItemInstance;
						if (productItemInstance == null || productItemInstance.AppliedPackaging == null)
						{
							goto IL_53;
						}
					}
					list.Add(this.CustomerSlots[i].ItemInstance);
				}
				IL_53:;
			}
			return list;
		}

		// Token: 0x06004DEC RID: 19948 RVA: 0x00149874 File Offset: 0x00147A74
		private float GetCustomerItemsValue()
		{
			float num = 0f;
			foreach (ItemInstance itemInstance in this.GetCustomerItems(true))
			{
				if (itemInstance is ProductItemInstance)
				{
					ProductItemInstance productItemInstance = itemInstance as ProductItemInstance;
					num += (productItemInstance.Definition as ProductDefinition).MarketValue * (float)productItemInstance.Quantity * (float)productItemInstance.Amount;
				}
			}
			return num;
		}

		// Token: 0x06004DED RID: 19949 RVA: 0x001498FC File Offset: 0x00147AFC
		private int GetCustomerItemsCount(bool onlyPackagedProduct = true)
		{
			int num = 0;
			for (int i = 0; i < this.CustomerSlots.Length; i++)
			{
				if (this.CustomerSlots[i].ItemInstance != null)
				{
					ProductItemInstance productItemInstance = this.CustomerSlots[i].ItemInstance as ProductItemInstance;
					if (!onlyPackagedProduct || (productItemInstance != null && !(productItemInstance.AppliedPackaging == null)))
					{
						int num2 = 1;
						if (productItemInstance != null)
						{
							num2 = productItemInstance.Amount;
						}
						num += this.CustomerSlots[i].ItemInstance.Quantity * num2;
					}
				}
			}
			return num;
		}

		// Token: 0x04003A0E RID: 14862
		public const int CUSTOMER_SLOT_COUNT = 4;

		// Token: 0x04003A0F RID: 14863
		public const float VEHICLE_MAX_DIST = 20f;

		// Token: 0x04003A14 RID: 14868
		[Header("Settings")]
		public Gradient SuccessColorMap;

		// Token: 0x04003A15 RID: 14869
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003A16 RID: 14870
		public GameObject Container;

		// Token: 0x04003A17 RID: 14871
		public CanvasGroup CanvasGroup;

		// Token: 0x04003A18 RID: 14872
		public TextMeshProUGUI DescriptionLabel;

		// Token: 0x04003A19 RID: 14873
		public TextMeshProUGUI CustomerSubtitle;

		// Token: 0x04003A1A RID: 14874
		public TextMeshProUGUI FavouriteDrugLabel;

		// Token: 0x04003A1B RID: 14875
		public TextMeshProUGUI FavouritePropertiesLabel;

		// Token: 0x04003A1C RID: 14876
		public TextMeshProUGUI[] PropertiesEntries;

		// Token: 0x04003A1D RID: 14877
		public RectTransform[] ExpectationEntries;

		// Token: 0x04003A1E RID: 14878
		public GameObject NoVehicle;

		// Token: 0x04003A1F RID: 14879
		public RectTransform VehicleSlotContainer;

		// Token: 0x04003A20 RID: 14880
		public RectTransform CustomerSlotContainer;

		// Token: 0x04003A21 RID: 14881
		public TextMeshProUGUI VehicleSubtitle;

		// Token: 0x04003A22 RID: 14882
		public TextMeshProUGUI SuccessLabel;

		// Token: 0x04003A23 RID: 14883
		public TextMeshProUGUI ErrorLabel;

		// Token: 0x04003A24 RID: 14884
		public TextMeshProUGUI WarningLabel;

		// Token: 0x04003A25 RID: 14885
		public Button DoneButton;

		// Token: 0x04003A26 RID: 14886
		public RectTransform VehicleContainer;

		// Token: 0x04003A27 RID: 14887
		public TextMeshProUGUI TitleLabel;

		// Token: 0x04003A28 RID: 14888
		public HandoverScreenPriceSelector PriceSelector;

		// Token: 0x04003A29 RID: 14889
		public TextMeshProUGUI FairPriceLabel;

		// Token: 0x04003A2A RID: 14890
		public Animation TutorialAnimation;

		// Token: 0x04003A2B RID: 14891
		public RectTransform TutorialContainer;

		// Token: 0x04003A2C RID: 14892
		public HandoverScreenDetailPanel DetailPanel;

		// Token: 0x04003A2D RID: 14893
		public Action<HandoverScreen.EHandoverOutcome, List<ItemInstance>, float> onHandoverComplete;

		// Token: 0x04003A2E RID: 14894
		public Func<List<ItemInstance>, float, float> SuccessChanceMethod;

		// Token: 0x04003A2F RID: 14895
		private ItemSlotUI[] VehicleSlotUIs;

		// Token: 0x04003A30 RID: 14896
		private ItemSlotUI[] CustomerSlotUIs;

		// Token: 0x04003A31 RID: 14897
		private ItemSlot[] CustomerSlots = new ItemSlot[4];

		// Token: 0x04003A32 RID: 14898
		private Dictionary<ItemInstance, HandoverScreen.EItemSource> OriginalItemLocations = new Dictionary<ItemInstance, HandoverScreen.EItemSource>();

		// Token: 0x04003A33 RID: 14899
		private bool ignoreCustomerChangedEvents;

		// Token: 0x02000B76 RID: 2934
		public enum EMode
		{
			// Token: 0x04003A36 RID: 14902
			Contract,
			// Token: 0x04003A37 RID: 14903
			Sample,
			// Token: 0x04003A38 RID: 14904
			Offer
		}

		// Token: 0x02000B77 RID: 2935
		public enum EHandoverOutcome
		{
			// Token: 0x04003A3A RID: 14906
			Cancelled,
			// Token: 0x04003A3B RID: 14907
			Finalize
		}

		// Token: 0x02000B78 RID: 2936
		private enum EItemSource
		{
			// Token: 0x04003A3D RID: 14909
			Player,
			// Token: 0x04003A3E RID: 14910
			Vehicle
		}
	}
}
