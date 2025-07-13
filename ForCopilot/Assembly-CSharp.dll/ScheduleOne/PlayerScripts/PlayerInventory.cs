using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.UI;
using ScheduleOne.UI.Items;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x0200062E RID: 1582
	public class PlayerInventory : PlayerSingleton<PlayerInventory>
	{
		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x060028B4 RID: 10420 RVA: 0x000A6FF7 File Offset: 0x000A51F7
		public int TOTAL_SLOT_COUNT
		{
			get
			{
				return 9 + (this.ManagementSlotEnabled ? 1 : 0);
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x060028B5 RID: 10421 RVA: 0x000A7008 File Offset: 0x000A5208
		// (set) Token: 0x060028B6 RID: 10422 RVA: 0x000A7010 File Offset: 0x000A5210
		public CashSlot cashSlot { get; private set; }

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x060028B7 RID: 10423 RVA: 0x000A7019 File Offset: 0x000A5219
		// (set) Token: 0x060028B8 RID: 10424 RVA: 0x000A7021 File Offset: 0x000A5221
		public CashInstance cashInstance { get; protected set; }

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x060028B9 RID: 10425 RVA: 0x000A702A File Offset: 0x000A522A
		// (set) Token: 0x060028BA RID: 10426 RVA: 0x000A7032 File Offset: 0x000A5232
		public int EquippedSlotIndex { get; protected set; } = -1;

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x060028BB RID: 10427 RVA: 0x000A703B File Offset: 0x000A523B
		// (set) Token: 0x060028BC RID: 10428 RVA: 0x000A7043 File Offset: 0x000A5243
		public bool HotbarEnabled { get; protected set; } = true;

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x060028BD RID: 10429 RVA: 0x000A704C File Offset: 0x000A524C
		// (set) Token: 0x060028BE RID: 10430 RVA: 0x000A7054 File Offset: 0x000A5254
		public bool EquippingEnabled { get; protected set; } = true;

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x060028BF RID: 10431 RVA: 0x000A705D File Offset: 0x000A525D
		// (set) Token: 0x060028C0 RID: 10432 RVA: 0x000A7065 File Offset: 0x000A5265
		public Equippable equippable { get; protected set; }

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x060028C1 RID: 10433 RVA: 0x000A706E File Offset: 0x000A526E
		public HotbarSlot equippedSlot
		{
			get
			{
				if (this.EquippedSlotIndex == -1)
				{
					return null;
				}
				return this.IndexAllSlots(this.EquippedSlotIndex);
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x060028C2 RID: 10434 RVA: 0x000A7087 File Offset: 0x000A5287
		public bool isAnythingEquipped
		{
			get
			{
				return this.equippedSlot != null && this.equippedSlot.ItemInstance != null;
			}
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x000A70A4 File Offset: 0x000A52A4
		public HotbarSlot IndexAllSlots(int index)
		{
			if (index < 0)
			{
				return null;
			}
			if (this.ManagementSlotEnabled)
			{
				if (index < this.hotbarSlots.Count)
				{
					return this.hotbarSlots[index];
				}
				if (index == 8)
				{
					return this.clipboardSlot;
				}
				if (index == 9)
				{
					return this.cashSlot;
				}
				return null;
			}
			else
			{
				if (index < this.hotbarSlots.Count)
				{
					return this.hotbarSlots[index];
				}
				if (index == 8)
				{
					return this.cashSlot;
				}
				return null;
			}
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x000A711A File Offset: 0x000A531A
		protected override void Awake()
		{
			base.Awake();
			this.SetupInventoryUI();
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x000A7128 File Offset: 0x000A5328
		private void SetupInventoryUI()
		{
			for (int i = 0; i < 8; i++)
			{
				HotbarSlot hotbarSlot = new HotbarSlot();
				this.hotbarSlots.Add(hotbarSlot);
				ItemSlotUI component = UnityEngine.Object.Instantiate<ItemSlotUI>(Singleton<ItemUIManager>.Instance.HotbarSlotUIPrefab, Singleton<HUD>.Instance.SlotContainer).GetComponent<ItemSlotUI>();
				component.AssignSlot(hotbarSlot);
				this.slotUIs.Add(component);
			}
			this.clipboardSlot = new ClipboardSlot();
			this.clipboardSlot.SetStoredItem(Registry.GetItem("managementclipboard").GetDefaultInstance(1), false);
			this.clipboardSlot.AddFilter(new ItemFilter_ID(new List<string>
			{
				"managementclipboard"
			}));
			this.clipboardSlot.SetIsRemovalLocked(true);
			this.clipboardSlot.SetIsAddLocked(true);
			Singleton<HUD>.Instance.managementSlotUI.AssignSlot(this.clipboardSlot);
			Singleton<HUD>.Instance.managementSlotContainer.gameObject.SetActive(false);
			this.slotUIs.Add(Singleton<HUD>.Instance.managementSlotUI);
			this.cashSlot = new CashSlot();
			this.cashSlot.SetStoredItem(Registry.GetItem("cash").GetDefaultInstance(1), false);
			this.cashInstance = (this.cashSlot.ItemInstance as CashInstance);
			this.cashSlot.AddFilter(new ItemFilter_Category(new List<EItemCategory>
			{
				EItemCategory.Cash
			}));
			Singleton<HUD>.Instance.cashSlotUI.GetComponent<CashSlotUI>().AssignSlot(this.cashSlot);
			this.slotUIs.Add(Singleton<HUD>.Instance.cashSlotUI.GetComponent<ItemSlotUI>());
			this.discardSlot = new ItemSlot();
			Singleton<HUD>.Instance.discardSlot.AssignSlot(this.discardSlot);
			this.RepositionUI();
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x000A72D8 File Offset: 0x000A54D8
		private void RepositionUI()
		{
			float num = 0f;
			float num2 = 20f;
			for (int i = 0; i < 8; i++)
			{
				ItemSlotUI itemSlotUI = this.slotUIs[i];
				itemSlotUI.Rect.Find("Background/Index").GetComponent<TextMeshProUGUI>().text = ((i + 1) % 10).ToString();
				itemSlotUI.Rect.anchoredPosition = new Vector2(num + itemSlotUI.Rect.sizeDelta.x / 2f + num2, 0f);
				num += itemSlotUI.Rect.sizeDelta.x + num2;
				if (i == 7)
				{
					itemSlotUI.Rect.Find("Seperator").gameObject.SetActive(true);
					itemSlotUI.Rect.Find("Seperator").GetComponent<RectTransform>().anchoredPosition = new Vector2(num2, 0f);
					num += num2;
				}
			}
			int num3 = 8;
			if (this.ManagementSlotEnabled)
			{
				Singleton<HUD>.Instance.managementSlotUI.transform.Find("Background/Index").GetComponent<Text>().text = ((num3 + 1) % 10).ToString();
				Singleton<HUD>.Instance.managementSlotContainer.anchoredPosition = new Vector2(num + Singleton<HUD>.Instance.managementSlotContainer.sizeDelta.x / 2f + num2, 0f);
				num += Singleton<HUD>.Instance.managementSlotContainer.sizeDelta.x + num2;
				num3++;
			}
			Singleton<HUD>.Instance.managementSlotContainer.gameObject.SetActive(this.ManagementSlotEnabled);
			Singleton<HUD>.Instance.cashSlotUI.Find("Background/Index").GetComponent<Text>().text = ((num3 + 1) % 10).ToString();
			Singleton<HUD>.Instance.cashSlotContainer.anchoredPosition = new Vector2(num + Singleton<HUD>.Instance.cashSlotContainer.sizeDelta.x / 2f + num2, 0f);
			num += Singleton<HUD>.Instance.cashSlotContainer.sizeDelta.x + num2;
			Singleton<HUD>.Instance.SlotContainer.anchoredPosition = new Vector2(-num / 2f, Singleton<HUD>.Instance.SlotContainer.anchoredPosition.y);
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x000A7528 File Offset: 0x000A5728
		protected override void Start()
		{
			base.Start();
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				HotbarSlot slot = this.hotbarSlots[i];
				Player.Local.SetInventoryItem(i, slot.ItemInstance);
				int index = i;
				HotbarSlot slot2 = slot;
				slot2.onItemDataChanged = (Action)Delegate.Combine(slot2.onItemDataChanged, new Action(delegate()
				{
					this.UpdateInventoryVariables();
					Player.Local.SetInventoryItem(index, slot.ItemInstance);
				}));
			}
			Player.Local.SetInventoryItem(8, this.cashSlot.ItemInstance);
			CashSlot cashSlot = this.cashSlot;
			cashSlot.onItemDataChanged = (Action)Delegate.Combine(cashSlot.onItemDataChanged, new Action(delegate()
			{
				this.UpdateInventoryVariables();
				Player.Local.SetInventoryItem(8, this.cashSlot.ItemInstance);
			}));
			if (this.giveStartupItems)
			{
				this.GiveStartupItems();
			}
			if (!GameManager.IS_TUTORIAL)
			{
				BoolVariable boolVariable = NetworkSingleton<VariableDatabase>.Instance.GetVariable("ClipboardAcquired") as BoolVariable;
				if (boolVariable.Value)
				{
					this.ClipboardAcquiredVarChange(true);
					return;
				}
				boolVariable.OnValueChanged.AddListener(new UnityAction<bool>(this.ClipboardAcquiredVarChange));
			}
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x000A7644 File Offset: 0x000A5844
		private void GiveStartupItems()
		{
			if (!Application.isEditor && !Debug.isDebugBuild)
			{
				return;
			}
			foreach (PlayerInventory.ItemAmount itemAmount in this.startupItems)
			{
				this.AddItemToInventory(itemAmount.Definition.GetDefaultInstance(itemAmount.Amount));
			}
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x000A76B8 File Offset: 0x000A58B8
		protected virtual void Update()
		{
			this.UpdateHotbarSelection();
			if (this.isAnythingEquipped && this.HotbarEnabled)
			{
				this.currentEquipTime += Time.deltaTime;
			}
			else
			{
				this.currentEquipTime = 0f;
			}
			if (this.isAnythingEquipped)
			{
				Singleton<HUD>.Instance.selectedItemLabel.text = this.equippedSlot.ItemInstance.Name;
				Singleton<HUD>.Instance.selectedItemLabel.color = this.equippedSlot.ItemInstance.LabelDisplayColor;
				if (this.currentEquipTime > 2f)
				{
					float num = Mathf.Clamp01((this.currentEquipTime - 2f) / 0.5f);
					Singleton<HUD>.Instance.selectedItemLabel.color = new Color(Singleton<HUD>.Instance.selectedItemLabel.color.r, Singleton<HUD>.Instance.selectedItemLabel.color.g, Singleton<HUD>.Instance.selectedItemLabel.color.b, 1f - num);
				}
				else
				{
					Singleton<HUD>.Instance.selectedItemLabel.color = new Color(Singleton<HUD>.Instance.selectedItemLabel.color.r, Singleton<HUD>.Instance.selectedItemLabel.color.g, Singleton<HUD>.Instance.selectedItemLabel.color.b, 1f);
				}
			}
			else
			{
				Singleton<HUD>.Instance.selectedItemLabel.text = string.Empty;
			}
			if (this.discardSlot.ItemInstance != null && !Singleton<HUD>.Instance.discardSlot.IsBeingDragged)
			{
				this.currentDiscardTime += Time.deltaTime;
				Singleton<HUD>.Instance.discardSlotFill.fillAmount = this.currentDiscardTime / 1.5f;
				if (this.currentDiscardTime >= 1.5f)
				{
					this.discardSlot.ClearStoredInstance(false);
					return;
				}
			}
			else
			{
				this.currentDiscardTime = 0f;
				Singleton<HUD>.Instance.discardSlotFill.fillAmount = 0f;
			}
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x000A78B0 File Offset: 0x000A5AB0
		private void UpdateHotbarSelection()
		{
			if (!this.HotbarEnabled)
			{
				return;
			}
			if (!this.EquippingEnabled)
			{
				return;
			}
			if (GameInput.IsTyping)
			{
				return;
			}
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				return;
			}
			int num = -1;
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				num = 0;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				num = 1;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				num = 2;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				num = 3;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				num = 4;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				num = 5;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				num = 6;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha8))
			{
				num = 7;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha9))
			{
				num = 8;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				num = 9;
			}
			if (num == -1)
			{
				float mouseScrollDelta = GameInput.MouseScrollDelta;
				if (mouseScrollDelta < 0f)
				{
					num = this.EquippedSlotIndex + 1;
					if (num >= this.TOTAL_SLOT_COUNT)
					{
						num = 0;
					}
				}
				else if (mouseScrollDelta > 0f)
				{
					num = this.EquippedSlotIndex - 1;
					if (num < 0)
					{
						num = this.TOTAL_SLOT_COUNT - 1;
					}
				}
			}
			if (num == -1 && GameInput.GetButtonDown(GameInput.ButtonCode.TertiaryClick))
			{
				if (this.EquippedSlotIndex != -1)
				{
					num = this.EquippedSlotIndex;
				}
				else if (this.PreviousEquippedSlotIndex != -1)
				{
					num = this.PreviousEquippedSlotIndex;
				}
			}
			if (num != -1)
			{
				if (num >= this.TOTAL_SLOT_COUNT)
				{
					return;
				}
				if (num != this.EquippedSlotIndex && this.EquippedSlotIndex != -1)
				{
					this.IndexAllSlots(this.EquippedSlotIndex).Unequip();
					this.currentEquipTime = 0f;
				}
				this.PreviousEquippedSlotIndex = this.EquippedSlotIndex;
				this.EquippedSlotIndex = -1;
				if (this.IndexAllSlots(num).IsEquipped)
				{
					this.IndexAllSlots(num).Unequip();
					return;
				}
				this.Equip(this.IndexAllSlots(num));
				this.EquippedSlotIndex = num;
				PlayerSingleton<ViewmodelSway>.Instance.RefreshViewmodel();
			}
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x000A7A66 File Offset: 0x000A5C66
		public void Equip(HotbarSlot slot)
		{
			slot.Equip();
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x000A7A6E File Offset: 0x000A5C6E
		public void SetInventoryEnabled(bool enabled)
		{
			this.HotbarEnabled = enabled;
			if (this.onInventoryStateChanged != null)
			{
				this.onInventoryStateChanged(enabled);
			}
			Singleton<HUD>.Instance.HotbarContainer.gameObject.SetActive(enabled);
			this.SetEquippingEnabled(enabled);
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x000A7AA8 File Offset: 0x000A5CA8
		public void SetEquippingEnabled(bool enabled)
		{
			if (this.EquippingEnabled == enabled)
			{
				return;
			}
			this.EquippingEnabled = enabled;
			this.equipContainer.gameObject.SetActive(enabled);
			if (enabled)
			{
				if (this.PriorEquippedSlotIndex != -1)
				{
					this.EquippedSlotIndex = this.PriorEquippedSlotIndex;
					this.Equip(this.IndexAllSlots(this.EquippedSlotIndex));
				}
			}
			else
			{
				this.PriorEquippedSlotIndex = this.EquippedSlotIndex;
				if (this.EquippedSlotIndex != -1)
				{
					this.IndexAllSlots(this.EquippedSlotIndex).Unequip();
					this.EquippedSlotIndex = -1;
				}
			}
			foreach (ItemSlotUI itemSlotUI in this.slotUIs)
			{
				itemSlotUI.Rect.Find("Background/Index").gameObject.SetActive(enabled);
			}
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x000A7B88 File Offset: 0x000A5D88
		private void ClipboardAcquiredVarChange(bool newVal)
		{
			this.SetManagementClipboardEnabled(newVal);
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x000A7B91 File Offset: 0x000A5D91
		public void SetManagementClipboardEnabled(bool enabled)
		{
			if (GameManager.IS_TUTORIAL)
			{
				enabled = false;
			}
			this.ManagementSlotEnabled = enabled;
			this.RepositionUI();
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x000A7BAC File Offset: 0x000A5DAC
		public void SetViewmodelVisible(bool visible)
		{
			PlayerSingleton<PlayerCamera>.Instance.Camera.cullingMask = (visible ? (PlayerSingleton<PlayerCamera>.Instance.Camera.cullingMask | 1 << LayerMask.NameToLayer("Viewmodel")) : (PlayerSingleton<PlayerCamera>.Instance.Camera.cullingMask & ~(1 << LayerMask.NameToLayer("Viewmodel"))));
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x000A7C0C File Offset: 0x000A5E0C
		public bool CanItemFitInInventory(ItemInstance item, int quantity = 1)
		{
			if (item == null)
			{
				Console.LogWarning("CanItemFitInInventory: item is null!", null);
				return false;
			}
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				if (this.hotbarSlots[i].ItemInstance == null)
				{
					quantity -= item.StackLimit;
				}
				else if (this.hotbarSlots[i].ItemInstance.CanStackWith(item, true))
				{
					quantity -= item.StackLimit - this.hotbarSlots[i].ItemInstance.Quantity;
				}
			}
			return quantity <= 0;
		}

		// Token: 0x060028D2 RID: 10450 RVA: 0x000A7CA0 File Offset: 0x000A5EA0
		public void AddItemToInventory(ItemInstance item)
		{
			if (item == null)
			{
				Console.LogError("AddItemToInventory: item is null!", null);
				return;
			}
			if (!item.IsValidInstance())
			{
				Console.LogError("AddItemToInventory: item is not valid!", null);
				return;
			}
			if (!this.CanItemFitInInventory(item, 1))
			{
				Console.LogWarning("AddItemToInventory: item won't fit!", null);
				return;
			}
			int num = item.Quantity;
			int num2 = 0;
			while (num2 < this.hotbarSlots.Count && num != 0)
			{
				if (this.hotbarSlots[num2].ItemInstance != null && this.hotbarSlots[num2].ItemInstance.CanStackWith(item, false))
				{
					int num3 = Mathf.Min(num, this.hotbarSlots[num2].ItemInstance.StackLimit - this.hotbarSlots[num2].Quantity);
					if (num3 > 0)
					{
						this.hotbarSlots[num2].ChangeQuantity(num3, false);
						num -= num3;
					}
				}
				num2++;
			}
			int num4 = 0;
			while (num4 < this.hotbarSlots.Count && num != 0)
			{
				if (this.hotbarSlots[num4].ItemInstance == null)
				{
					this.hotbarSlots[num4].SetStoredItem(item.GetCopy(num), false);
					num = 0;
				}
				num4++;
			}
			if (num > 0)
			{
				Console.LogWarning("Could not add full amount of '" + item.Name + "' to inventory!", null);
			}
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x000A7DEC File Offset: 0x000A5FEC
		public uint GetAmountOfItem(string ID)
		{
			uint num = 0U;
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				if (this.hotbarSlots[i].ItemInstance != null && this.hotbarSlots[i].ItemInstance.ID.ToLower() == ID.ToLower())
				{
					num += (uint)this.hotbarSlots[i].Quantity;
				}
			}
			return num;
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x000A7E64 File Offset: 0x000A6064
		public void RemoveAmountOfItem(string ID, uint amount = 1U)
		{
			uint num = amount;
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				if (this.hotbarSlots[i].ItemInstance != null && this.hotbarSlots[i].ItemInstance.ID.ToLower() == ID.ToLower())
				{
					uint num2 = num;
					if ((ulong)num2 > (ulong)((long)this.hotbarSlots[i].Quantity))
					{
						num2 = (uint)this.hotbarSlots[i].Quantity;
					}
					num -= num2;
					this.hotbarSlots[i].ChangeQuantity((int)(-(int)num2), false);
					if (num <= 0U)
					{
						break;
					}
				}
			}
			if (num > 0U)
			{
				Console.LogWarning("Could not fully remove " + amount.ToString() + " " + ID, null);
			}
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x000A7F34 File Offset: 0x000A6134
		public void ClearInventory()
		{
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				if (this.hotbarSlots[i].ItemInstance != null)
				{
					this.hotbarSlots[i].ClearStoredInstance(false);
				}
			}
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x000A7F7C File Offset: 0x000A617C
		public void RemoveProductFromInventory(EStealthLevel maxStealth)
		{
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				if (this.hotbarSlots[i].ItemInstance != null && this.hotbarSlots[i].ItemInstance is ProductItemInstance)
				{
					ProductItemInstance productItemInstance = this.hotbarSlots[i].ItemInstance as ProductItemInstance;
					EStealthLevel estealthLevel = EStealthLevel.None;
					if (productItemInstance.AppliedPackaging != null)
					{
						estealthLevel = productItemInstance.AppliedPackaging.StealthLevel;
					}
					if (estealthLevel <= maxStealth)
					{
						this.hotbarSlots[i].ClearStoredInstance(false);
					}
				}
			}
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x000A8018 File Offset: 0x000A6218
		public void RemoveRandomItemsFromInventory()
		{
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				if (this.hotbarSlots[i].ItemInstance != null && UnityEngine.Random.Range(0, 3) == 0)
				{
					int num = UnityEngine.Random.Range(1, this.hotbarSlots[i].ItemInstance.Quantity + 1);
					this.hotbarSlots[i].ChangeQuantity(-num, false);
				}
			}
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x000A808D File Offset: 0x000A628D
		public void SetEquippable(Equippable eq)
		{
			this.equippable = eq;
			if (this.equippable != null && this.onItemEquipped != null)
			{
				this.onItemEquipped.Invoke();
			}
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x000A80B8 File Offset: 0x000A62B8
		public void Reequip()
		{
			HotbarSlot equippedSlot = this.equippedSlot;
			if (equippedSlot != null)
			{
				equippedSlot.Unequip();
				this.currentEquipTime = 0f;
				this.Equip(equippedSlot);
			}
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x000A80E8 File Offset: 0x000A62E8
		public List<ItemSlot> GetAllInventorySlots()
		{
			List<ItemSlot> list = new List<ItemSlot>();
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				list.Add(this.hotbarSlots[i]);
			}
			list.Add(this.cashSlot);
			return list;
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000A8130 File Offset: 0x000A6330
		private void UpdateInventoryVariables()
		{
			if (!NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this.ItemVariables.Count; i++)
			{
				int num3 = 0;
				for (int j = 0; j < this.hotbarSlots.Count; j++)
				{
					if (this.hotbarSlots[j].ItemInstance != null && this.hotbarSlots[j].ItemInstance.ID.ToLower() == this.ItemVariables[i].Definition.ID.ToLower())
					{
						num3 += this.hotbarSlots[j].Quantity;
					}
					if (this.hotbarSlots[j].ItemInstance != null && NetworkSingleton<ProductManager>.Instance.ValidMixIngredients.Contains(this.hotbarSlots[j].ItemInstance.Definition))
					{
						num += this.hotbarSlots[j].Quantity;
					}
				}
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.ItemVariables[i].VariableName, num3.ToString(), false);
			}
			int num4 = 0;
			for (int k = 0; k < this.hotbarSlots.Count; k++)
			{
				if (this.hotbarSlots[k].ItemInstance != null && this.hotbarSlots[k].ItemInstance is ProductItemInstance)
				{
					if (this.hotbarSlots[k].ItemInstance is ProductItemInstance && (this.hotbarSlots[k].ItemInstance as ProductItemInstance).AppliedPackaging != null)
					{
						num4 += this.hotbarSlots[k].Quantity;
					}
					if (this.hotbarSlots[k].ItemInstance is WeedInstance)
					{
						num2 += this.hotbarSlots[k].Quantity;
					}
				}
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Inventory_Weed_Count", num2.ToString(), false);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Inventory_Packaged_Product", num4.ToString(), false);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Inventory_MixingIngredients", num.ToString(), false);
		}

		// Token: 0x04001D4B RID: 7499
		public const float LABEL_DISPLAY_TIME = 2f;

		// Token: 0x04001D4C RID: 7500
		public const float LABEL_FADE_TIME = 0.5f;

		// Token: 0x04001D4D RID: 7501
		public const float DISCARD_TIME = 1.5f;

		// Token: 0x04001D4E RID: 7502
		public const int INVENTORY_SLOT_COUNT = 8;

		// Token: 0x04001D4F RID: 7503
		[Header("Startup Items (Editor only)")]
		[SerializeField]
		private bool giveStartupItems;

		// Token: 0x04001D50 RID: 7504
		[SerializeField]
		private List<PlayerInventory.ItemAmount> startupItems = new List<PlayerInventory.ItemAmount>();

		// Token: 0x04001D51 RID: 7505
		[Header("References")]
		public Transform equipContainer;

		// Token: 0x04001D52 RID: 7506
		public List<HotbarSlot> hotbarSlots = new List<HotbarSlot>();

		// Token: 0x04001D55 RID: 7509
		private ClipboardSlot clipboardSlot;

		// Token: 0x04001D56 RID: 7510
		private List<ItemSlotUI> slotUIs = new List<ItemSlotUI>();

		// Token: 0x04001D57 RID: 7511
		private ItemSlot discardSlot;

		// Token: 0x04001D58 RID: 7512
		[Header("Item Variables")]
		public List<PlayerInventory.ItemVariable> ItemVariables = new List<PlayerInventory.ItemVariable>();

		// Token: 0x04001D5D RID: 7517
		public Action<bool> onInventoryStateChanged;

		// Token: 0x04001D5E RID: 7518
		private int PriorEquippedSlotIndex = -1;

		// Token: 0x04001D5F RID: 7519
		private int PreviousEquippedSlotIndex = -1;

		// Token: 0x04001D60 RID: 7520
		public UnityEvent onPreItemEquipped;

		// Token: 0x04001D61 RID: 7521
		public UnityEvent onItemEquipped;

		// Token: 0x04001D62 RID: 7522
		private bool ManagementSlotEnabled;

		// Token: 0x04001D63 RID: 7523
		public float currentEquipTime;

		// Token: 0x04001D64 RID: 7524
		protected float currentDiscardTime;

		// Token: 0x0200062F RID: 1583
		[Serializable]
		public class ItemVariable
		{
			// Token: 0x04001D65 RID: 7525
			public ItemDefinition Definition;

			// Token: 0x04001D66 RID: 7526
			public string VariableName;
		}

		// Token: 0x02000630 RID: 1584
		[Serializable]
		private class ItemAmount
		{
			// Token: 0x04001D67 RID: 7527
			public ItemDefinition Definition;

			// Token: 0x04001D68 RID: 7528
			public int Amount = 10;
		}
	}
}
