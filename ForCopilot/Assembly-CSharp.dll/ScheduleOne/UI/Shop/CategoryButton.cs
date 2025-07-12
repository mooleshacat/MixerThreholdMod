using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000B9E RID: 2974
	public class CategoryButton : MonoBehaviour
	{
		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06004EF7 RID: 20215 RVA: 0x0014DA47 File Offset: 0x0014BC47
		// (set) Token: 0x06004EF8 RID: 20216 RVA: 0x0014DA4F File Offset: 0x0014BC4F
		public bool isSelected { get; protected set; }

		// Token: 0x06004EF9 RID: 20217 RVA: 0x0014DA58 File Offset: 0x0014BC58
		private void Awake()
		{
			this.button = base.GetComponent<Button>();
			this.shop = base.GetComponentInParent<ShopInterface>();
			this.button.onClick.AddListener(new UnityAction(this.Clicked));
			this.Deselect();
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x0014DA94 File Offset: 0x0014BC94
		private void OnValidate()
		{
			base.gameObject.name = this.Category.ToString();
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x0014DAB2 File Offset: 0x0014BCB2
		private void Clicked()
		{
			if (this.isSelected)
			{
				this.Deselect();
				return;
			}
			this.Select();
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x0014DAC9 File Offset: 0x0014BCC9
		public void Deselect()
		{
			this.isSelected = false;
			this.RefreshUI();
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x0014DAD8 File Offset: 0x0014BCD8
		public void Select()
		{
			this.isSelected = true;
			this.RefreshUI();
			this.shop.CategorySelected(this.Category);
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x0014DAF8 File Offset: 0x0014BCF8
		private void RefreshUI()
		{
			this.button.interactable = !this.isSelected;
		}

		// Token: 0x04003B1C RID: 15132
		public EShopCategory Category;

		// Token: 0x04003B1D RID: 15133
		private Button button;

		// Token: 0x04003B1E RID: 15134
		private ShopInterface shop;
	}
}
