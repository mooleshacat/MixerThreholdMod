using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;

namespace ScheduleOne.Clothing
{
	// Token: 0x0200077F RID: 1919
	[Serializable]
	public class ClothingInstance : StorableItemInstance
	{
		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x060033D0 RID: 13264 RVA: 0x000D7CB4 File Offset: 0x000D5EB4
		public override string Name
		{
			get
			{
				return base.Name + ((this.Color != EClothingColor.White) ? (" (" + this.Color.GetLabel() + ")") : string.Empty);
			}
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x000D7CEA File Offset: 0x000D5EEA
		public ClothingInstance()
		{
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x000D7CF2 File Offset: 0x000D5EF2
		public ClothingInstance(ItemDefinition definition, int quantity, EClothingColor color) : base(definition, quantity)
		{
			this.Color = color;
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x000D7D04 File Offset: 0x000D5F04
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new ClothingInstance(base.Definition, quantity, this.Color);
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x000D7D30 File Offset: 0x000D5F30
		public override ItemData GetItemData()
		{
			return new ClothingData(this.ID, this.Quantity, this.Color);
		}

		// Token: 0x0400249A RID: 9370
		public EClothingColor Color;
	}
}
