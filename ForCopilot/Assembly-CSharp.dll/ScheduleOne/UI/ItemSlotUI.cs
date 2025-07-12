using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Items;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A3C RID: 2620
	public class ItemSlotUI : MonoBehaviour
	{
		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06004662 RID: 18018 RVA: 0x00127187 File Offset: 0x00125387
		// (set) Token: 0x06004663 RID: 18019 RVA: 0x0012718F File Offset: 0x0012538F
		public ItemSlot assignedSlot { get; protected set; }

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06004664 RID: 18020 RVA: 0x00127198 File Offset: 0x00125398
		// (set) Token: 0x06004665 RID: 18021 RVA: 0x001271A0 File Offset: 0x001253A0
		public ItemUI ItemUI { get; protected set; }

		// Token: 0x06004666 RID: 18022 RVA: 0x001271AC File Offset: 0x001253AC
		public virtual void AssignSlot(ItemSlot s)
		{
			if (s == null)
			{
				Console.LogWarning("AssignSlot passed null slot. Use ClearSlot() instead", null);
			}
			this.assignedSlot = s;
			ItemSlot assignedSlot = this.assignedSlot;
			assignedSlot.onItemInstanceChanged = (Action)Delegate.Combine(assignedSlot.onItemInstanceChanged, new Action(this.UpdateUI));
			ItemSlot assignedSlot2 = this.assignedSlot;
			assignedSlot2.onLocked = (Action)Delegate.Combine(assignedSlot2.onLocked, new Action(this.Lock));
			ItemSlot assignedSlot3 = this.assignedSlot;
			assignedSlot3.onUnlocked = (Action)Delegate.Combine(assignedSlot3.onUnlocked, new Action(this.Unlock));
			this.SetHighlighted(false);
			if (this.assignedSlot is HotbarSlot)
			{
				HotbarSlot hotbarSlot = this.assignedSlot as HotbarSlot;
				hotbarSlot.onEquipChanged = (HotbarSlot.EquipEvent)Delegate.Combine(hotbarSlot.onEquipChanged, new HotbarSlot.EquipEvent(this.SetHighlighted));
			}
			if (s.IsLocked)
			{
				this.SetLockVisible(true);
			}
			if (this.FilterButton != null && s.CanPlayerSetFilter)
			{
				this.FilterButton.AssignSlot(s);
				this.FilterButton.Button.interactable = !s.IsLocked;
			}
			this.UpdateUI();
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x001272D4 File Offset: 0x001254D4
		public virtual void ClearSlot()
		{
			if (this.assignedSlot != null)
			{
				ItemSlot assignedSlot = this.assignedSlot;
				assignedSlot.onItemInstanceChanged = (Action)Delegate.Remove(assignedSlot.onItemInstanceChanged, new Action(this.UpdateUI));
				ItemSlot assignedSlot2 = this.assignedSlot;
				assignedSlot2.onLocked = (Action)Delegate.Remove(assignedSlot2.onLocked, new Action(this.Lock));
				ItemSlot assignedSlot3 = this.assignedSlot;
				assignedSlot3.onUnlocked = (Action)Delegate.Remove(assignedSlot3.onUnlocked, new Action(this.Unlock));
				if (this.assignedSlot is HotbarSlot)
				{
					HotbarSlot hotbarSlot = this.assignedSlot as HotbarSlot;
					hotbarSlot.onEquipChanged = (HotbarSlot.EquipEvent)Delegate.Remove(hotbarSlot.onEquipChanged, new HotbarSlot.EquipEvent(this.SetHighlighted));
				}
				this.assignedSlot = null;
				this.SetLockVisible(false);
				if (this.FilterButton != null)
				{
					this.FilterButton.UnassignSlot();
				}
				this.UpdateUI();
			}
		}

		// Token: 0x06004668 RID: 18024 RVA: 0x001273C8 File Offset: 0x001255C8
		public void OnDestroy()
		{
			if (this.assignedSlot != null)
			{
				ItemSlot assignedSlot = this.assignedSlot;
				assignedSlot.onItemInstanceChanged = (Action)Delegate.Remove(assignedSlot.onItemInstanceChanged, new Action(this.UpdateUI));
			}
		}

		// Token: 0x06004669 RID: 18025 RVA: 0x001273FC File Offset: 0x001255FC
		public virtual void UpdateUI()
		{
			if (this.ItemUI != null)
			{
				this.ItemUI.Destroy();
				this.ItemUI = null;
			}
			if (this.assignedSlot != null && this.assignedSlot.ItemInstance != null)
			{
				ItemUI original = Singleton<ItemUIManager>.Instance.DefaultItemUIPrefab;
				if (this.assignedSlot.ItemInstance.Definition.CustomItemUI != null)
				{
					original = this.assignedSlot.ItemInstance.Definition.CustomItemUI;
				}
				this.ItemUI = UnityEngine.Object.Instantiate<ItemUI>(original, this.ItemContainer).GetComponent<ItemUI>();
				this.ItemUI.transform.SetAsLastSibling();
				this.ItemUI.Setup(this.assignedSlot.ItemInstance);
			}
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x001274BC File Offset: 0x001256BC
		public void SetHighlighted(bool h)
		{
			if (h)
			{
				this.Background.color = this.highlightColor;
				return;
			}
			this.Background.color = this.normalColor;
		}

		// Token: 0x0600466B RID: 18027 RVA: 0x001274EE File Offset: 0x001256EE
		public void SetNormalColor(Color color)
		{
			this.normalColor = color;
			this.SetHighlighted(false);
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x00127503 File Offset: 0x00125703
		public void SetHighlightColor(Color color)
		{
			this.highlightColor = color;
			this.SetHighlighted(false);
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x00127518 File Offset: 0x00125718
		private void Lock()
		{
			this.SetLockVisible(true);
			if (this.FilterButton != null)
			{
				this.FilterButton.Button.interactable = false;
			}
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x00127540 File Offset: 0x00125740
		private void Unlock()
		{
			this.SetLockVisible(false);
			if (this.FilterButton != null)
			{
				this.FilterButton.Button.interactable = true;
			}
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x00127568 File Offset: 0x00125768
		public void SetLockVisible(bool vis)
		{
			this.LockContainer.gameObject.SetActive(vis);
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x0012757B File Offset: 0x0012577B
		public RectTransform DuplicateIcon(Transform parent, int overriddenQuantity = -1)
		{
			if (this.ItemUI == null)
			{
				return null;
			}
			return this.ItemUI.DuplicateIcon(parent, overriddenQuantity);
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x0012759A File Offset: 0x0012579A
		public void SetVisible(bool shown)
		{
			if (this.ItemUI != null)
			{
				this.ItemUI.SetVisible(shown);
			}
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x001275B6 File Offset: 0x001257B6
		public void OverrideDisplayedQuantity(int quantity)
		{
			if (this.ItemUI == null)
			{
				return;
			}
			this.ItemUI.SetDisplayedQuantity(quantity);
		}

		// Token: 0x04003322 RID: 13090
		public Color32 normalColor = new Color32(140, 140, 140, 40);

		// Token: 0x04003323 RID: 13091
		public Color32 highlightColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 60);

		// Token: 0x04003325 RID: 13093
		[HideInInspector]
		public bool IsBeingDragged;

		// Token: 0x04003326 RID: 13094
		[Header("References")]
		public RectTransform Rect;

		// Token: 0x04003327 RID: 13095
		public Image Background;

		// Token: 0x04003328 RID: 13096
		public GameObject LockContainer;

		// Token: 0x04003329 RID: 13097
		public RectTransform ItemContainer;

		// Token: 0x0400332A RID: 13098
		public ItemSlotFilterButton FilterButton;
	}
}
