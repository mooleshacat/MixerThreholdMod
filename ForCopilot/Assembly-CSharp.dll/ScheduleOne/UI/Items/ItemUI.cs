using System;
using ScheduleOne.ItemFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000BC4 RID: 3012
	public class ItemUI : MonoBehaviour
	{
		// Token: 0x06004FF6 RID: 20470 RVA: 0x00151594 File Offset: 0x0014F794
		public virtual void Setup(ItemInstance item)
		{
			if (item == null)
			{
				Console.LogError("ItemUI.Setup called and passed null item", null);
			}
			this.itemInstance = item;
			ItemInstance itemInstance = this.itemInstance;
			itemInstance.onDataChanged = (Action)Delegate.Remove(itemInstance.onDataChanged, new Action(this.UpdateUI));
			ItemInstance itemInstance2 = this.itemInstance;
			itemInstance2.onDataChanged = (Action)Delegate.Combine(itemInstance2.onDataChanged, new Action(this.UpdateUI));
			this.UpdateUI();
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x0015160C File Offset: 0x0014F80C
		public virtual void Destroy()
		{
			this.Destroyed = true;
			ItemInstance itemInstance = this.itemInstance;
			itemInstance.onDataChanged = (Action)Delegate.Remove(itemInstance.onDataChanged, new Action(this.UpdateUI));
			this.itemInstance = null;
			UnityEngine.Object.Destroy(this.Rect.gameObject);
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x00151660 File Offset: 0x0014F860
		public virtual RectTransform DuplicateIcon(Transform parent, int overriddenQuantity = -1)
		{
			int displayedQuantity = this.DisplayedQuantity;
			if (overriddenQuantity != -1)
			{
				this.SetDisplayedQuantity(overriddenQuantity);
			}
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.IconImg.gameObject, parent).GetComponent<RectTransform>();
			component.localScale = Vector3.one;
			this.SetDisplayedQuantity(displayedQuantity);
			return component;
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x001516A7 File Offset: 0x0014F8A7
		public virtual void SetVisible(bool vis)
		{
			this.Rect.gameObject.SetActive(vis);
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x001516BA File Offset: 0x0014F8BA
		public virtual void UpdateUI()
		{
			if (this.Destroyed)
			{
				return;
			}
			this.IconImg.sprite = this.itemInstance.Icon;
			this.SetDisplayedQuantity(this.itemInstance.Quantity);
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x001516EC File Offset: 0x0014F8EC
		public virtual void SetDisplayedQuantity(int quantity)
		{
			this.DisplayedQuantity = quantity;
			if (quantity > 1)
			{
				this.QuantityLabel.text = quantity.ToString() + "x";
				return;
			}
			this.QuantityLabel.text = string.Empty;
		}

		// Token: 0x04003BF7 RID: 15351
		protected ItemInstance itemInstance;

		// Token: 0x04003BF8 RID: 15352
		[Header("References")]
		public RectTransform Rect;

		// Token: 0x04003BF9 RID: 15353
		public Image IconImg;

		// Token: 0x04003BFA RID: 15354
		public TextMeshProUGUI QuantityLabel;

		// Token: 0x04003BFB RID: 15355
		protected int DisplayedQuantity;

		// Token: 0x04003BFC RID: 15356
		protected bool Destroyed;
	}
}
