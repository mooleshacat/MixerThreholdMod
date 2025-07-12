using System;
using ScheduleOne.Clothing;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200097B RID: 2427
	public class ItemFilter_ClothingSlot : ItemFilter
	{
		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x0600418F RID: 16783 RVA: 0x00114B36 File Offset: 0x00112D36
		// (set) Token: 0x06004190 RID: 16784 RVA: 0x00114B3E File Offset: 0x00112D3E
		public EClothingSlot SlotType { get; private set; }

		// Token: 0x06004191 RID: 16785 RVA: 0x00114B47 File Offset: 0x00112D47
		public ItemFilter_ClothingSlot(EClothingSlot slot)
		{
			this.SlotType = slot;
		}

		// Token: 0x06004192 RID: 16786 RVA: 0x00114B58 File Offset: 0x00112D58
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			ClothingInstance clothingInstance = instance as ClothingInstance;
			if (clothingInstance == null)
			{
				return false;
			}
			ClothingDefinition clothingDefinition = clothingInstance.Definition as ClothingDefinition;
			return !(clothingDefinition == null) && clothingDefinition.Slot == this.SlotType && base.DoesItemMatchFilter(instance);
		}
	}
}
