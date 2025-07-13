using System;
using FishNet.Serializing.Helping;
using ScheduleOne.Equipping;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000989 RID: 2441
	[Serializable]
	public abstract class ItemInstance
	{
		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x060041B8 RID: 16824 RVA: 0x00114FE8 File Offset: 0x001131E8
		[CodegenExclude]
		public ItemDefinition Definition
		{
			get
			{
				if (this.definition == null)
				{
					this.definition = Registry.GetItem(this.ID);
					if (this.definition == null)
					{
						Console.LogError("Failed to find definition with ID: " + this.ID, null);
					}
				}
				return this.definition;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x060041B9 RID: 16825 RVA: 0x0011503E File Offset: 0x0011323E
		[CodegenExclude]
		public virtual string Name
		{
			get
			{
				return this.Definition.Name;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x060041BA RID: 16826 RVA: 0x0011504B File Offset: 0x0011324B
		[CodegenExclude]
		public virtual string Description
		{
			get
			{
				return this.Definition.Description;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x060041BB RID: 16827 RVA: 0x00115058 File Offset: 0x00113258
		[CodegenExclude]
		public virtual Sprite Icon
		{
			get
			{
				return this.Definition.Icon;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x060041BC RID: 16828 RVA: 0x00115065 File Offset: 0x00113265
		[CodegenExclude]
		public virtual EItemCategory Category
		{
			get
			{
				return this.Definition.Category;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x060041BD RID: 16829 RVA: 0x00115072 File Offset: 0x00113272
		[CodegenExclude]
		public virtual int StackLimit
		{
			get
			{
				return this.Definition.StackLimit;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x060041BE RID: 16830 RVA: 0x0011507F File Offset: 0x0011327F
		[CodegenExclude]
		public virtual Color LabelDisplayColor
		{
			get
			{
				return this.Definition.LabelDisplayColor;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x060041BF RID: 16831 RVA: 0x0011508C File Offset: 0x0011328C
		[CodegenExclude]
		public virtual Equippable Equippable
		{
			get
			{
				return this.Definition.Equippable;
			}
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x00115099 File Offset: 0x00113299
		public ItemInstance()
		{
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x001150B3 File Offset: 0x001132B3
		public ItemInstance(ItemDefinition definition, int quantity)
		{
			this.definition = definition;
			this.Quantity = quantity;
			this.ID = definition.ID;
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x001150E7 File Offset: 0x001132E7
		public virtual bool CanStackWith(ItemInstance other, bool checkQuantities = true)
		{
			return other != null && !(other.ID != this.ID) && (!checkQuantities || this.Quantity + other.Quantity <= this.StackLimit);
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x00114F94 File Offset: 0x00113194
		public virtual ItemInstance GetCopy(int overrideQuantity = -1)
		{
			Console.LogError("This should be overridden in the definition class!", null);
			return null;
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x0011511E File Offset: 0x0011331E
		public virtual bool IsValidInstance()
		{
			return this.ID != string.Empty && this.Definition != null && this.Quantity > 0;
		}

		// Token: 0x060041C5 RID: 16837 RVA: 0x0011514B File Offset: 0x0011334B
		protected void InvokeDataChange()
		{
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x060041C6 RID: 16838 RVA: 0x00115160 File Offset: 0x00113360
		public void SetQuantity(int quantity)
		{
			if (quantity < 0)
			{
				Debug.LogError("SetQuantity called and passed quantity less than zero.");
				return;
			}
			if (quantity > this.StackLimit && quantity > this.Quantity)
			{
				Debug.LogError("SetQuantity called and passed quantity larger than stack limit.");
				return;
			}
			this.Quantity = quantity;
			this.InvokeDataChange();
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x0011519C File Offset: 0x0011339C
		public void ChangeQuantity(int change)
		{
			int num = this.Quantity + change;
			if (num < 0)
			{
				Debug.LogError("ChangeQuantity called and passed quantity less than zero.");
				return;
			}
			if (num > this.StackLimit)
			{
				Debug.LogError("ChangeQuantity called and passed quantity larger than stack limit.");
				return;
			}
			this.Quantity = num;
			this.InvokeDataChange();
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x001151E2 File Offset: 0x001133E2
		public virtual ItemData GetItemData()
		{
			return new ItemData(this.ID, this.Quantity);
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x001151F5 File Offset: 0x001133F5
		public virtual float GetMonetaryValue()
		{
			return 0f;
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x001151FC File Offset: 0x001133FC
		public void RequestClearSlot()
		{
			if (this.requestClearSlot != null)
			{
				this.requestClearSlot();
			}
		}

		// Token: 0x04002EE3 RID: 12003
		[CodegenExclude]
		protected ItemDefinition definition;

		// Token: 0x04002EE4 RID: 12004
		public string ID = string.Empty;

		// Token: 0x04002EE5 RID: 12005
		public int Quantity = 1;

		// Token: 0x04002EE6 RID: 12006
		[CodegenExclude]
		public Action onDataChanged;

		// Token: 0x04002EE7 RID: 12007
		[CodegenExclude]
		public Action requestClearSlot;
	}
}
