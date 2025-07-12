using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000BC3 RID: 3011
	public class ItemSlotFilterButton : MonoBehaviour
	{
		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06004FEE RID: 20462 RVA: 0x001512C3 File Offset: 0x0014F4C3
		// (set) Token: 0x06004FEF RID: 20463 RVA: 0x001512CB File Offset: 0x0014F4CB
		public ItemSlot AssignedSlot { get; protected set; }

		// Token: 0x06004FF0 RID: 20464 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Awake()
		{
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x001512D4 File Offset: 0x0014F4D4
		public void AssignSlot(ItemSlot slot)
		{
			if (this.AssignedSlot != null)
			{
				this.UnassignSlot();
			}
			this.AssignedSlot = slot;
			ItemSlot assignedSlot = this.AssignedSlot;
			assignedSlot.onFilterChange = (Action)Delegate.Combine(assignedSlot.onFilterChange, new Action(this.RefreshAppearance));
			this.RefreshAppearance();
			base.gameObject.SetActive(true);
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x00151330 File Offset: 0x0014F530
		public void UnassignSlot()
		{
			if (this.AssignedSlot == null)
			{
				return;
			}
			ItemSlot assignedSlot = this.AssignedSlot;
			assignedSlot.onFilterChange = (Action)Delegate.Remove(assignedSlot.onFilterChange, new Action(this.RefreshAppearance));
			this.AssignedSlot = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x00151380 File Offset: 0x0014F580
		public void Clicked()
		{
			if (this.AssignedSlot == null)
			{
				return;
			}
			if (!this.AssignedSlot.CanPlayerSetFilter)
			{
				return;
			}
			if (this.AssignedSlot.IsLocked)
			{
				return;
			}
			if (Singleton<ItemUIManager>.Instance.FilterConfigPanel.IsOpen && Singleton<ItemUIManager>.Instance.FilterConfigPanel.OpenSlot == this.AssignedSlot)
			{
				Singleton<ItemUIManager>.Instance.FilterConfigPanel.Close();
				return;
			}
			Singleton<ItemUIManager>.Instance.FilterConfigPanel.Open(this.ItemSlotUI);
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x00151400 File Offset: 0x0014F600
		private void RefreshAppearance()
		{
			switch (this.AssignedSlot.PlayerFilter.Type)
			{
			case SlotFilter.EType.None:
				this.IconImage.color = new Color32(115, 115, 115, 125);
				this.SpotImage.enabled = false;
				break;
			case SlotFilter.EType.Whitelist:
				this.IconImage.color = Color.white;
				this.SpotImage.enabled = false;
				break;
			case SlotFilter.EType.Blacklist:
				this.IconImage.color = Color.white;
				this.SpotImage.enabled = true;
				break;
			}
			for (int i = 0; i < this.FilterItemImages.Length; i++)
			{
				if (this.AssignedSlot.PlayerFilter.ItemIDs.Count > i)
				{
					this.FilterItemImages[i].sprite = Registry.GetItem(this.AssignedSlot.PlayerFilter.ItemIDs[i]).Icon;
					this.FilterItemImages[i].gameObject.SetActive(true);
				}
				else
				{
					this.FilterItemImages[i].gameObject.SetActive(false);
				}
			}
			if (this.AssignedSlot.PlayerFilter.ItemIDs.Count > this.FilterItemImages.Length)
			{
				this.FilterMoreItemsLabel.gameObject.SetActive(true);
				this.FilterMoreItemsLabel.text = "+" + (this.AssignedSlot.PlayerFilter.ItemIDs.Count - this.FilterItemImages.Length).ToString();
				return;
			}
			this.FilterMoreItemsLabel.gameObject.SetActive(false);
		}

		// Token: 0x04003BF1 RID: 15345
		public ItemSlotUI ItemSlotUI;

		// Token: 0x04003BF2 RID: 15346
		public Button Button;

		// Token: 0x04003BF3 RID: 15347
		public Image IconImage;

		// Token: 0x04003BF4 RID: 15348
		public Image SpotImage;

		// Token: 0x04003BF5 RID: 15349
		public Image[] FilterItemImages;

		// Token: 0x04003BF6 RID: 15350
		public TextMeshProUGUI FilterMoreItemsLabel;
	}
}
