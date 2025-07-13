using System;
using FishNet.Serializing.Helping;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Storage
{
	// Token: 0x020008E4 RID: 2276
	[Serializable]
	public class StorableItemInstance : ItemInstance
	{
		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06003D6D RID: 15725 RVA: 0x00102E90 File Offset: 0x00101090
		[CodegenExclude]
		public virtual StoredItem StoredItem
		{
			get
			{
				if (base.Definition != null && base.Definition is StorableItemDefinition)
				{
					return (base.Definition as StorableItemDefinition).StoredItem;
				}
				string str = "StorableItemInstance has invalid definition: ";
				ItemDefinition definition = base.Definition;
				Console.LogError(str + ((definition != null) ? definition.ToString() : null), null);
				return null;
			}
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x00102EEC File Offset: 0x001010EC
		public StorableItemInstance()
		{
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x00102EF4 File Offset: 0x001010F4
		public StorableItemInstance(ItemDefinition definition, int quantity) : base(definition, quantity)
		{
			if (definition as StorableItemDefinition == null)
			{
				Console.LogError("StoredItemInstance initialized with invalid definition!", null);
				return;
			}
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x00102F18 File Offset: 0x00101118
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new StorableItemInstance(base.Definition, quantity);
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x00102F3E File Offset: 0x0010113E
		public override float GetMonetaryValue()
		{
			return (base.Definition as StorableItemDefinition).BasePurchasePrice;
		}
	}
}
