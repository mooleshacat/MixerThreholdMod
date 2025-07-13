using System;
using ScheduleOne.Clothing;
using ScheduleOne.DevUtilities;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A3B RID: 2619
	public class ClothingSlotUI : ItemSlotUI
	{
		// Token: 0x06004660 RID: 18016 RVA: 0x00127165 File Offset: 0x00125365
		private void Start()
		{
			this.SlotTypeImage.sprite = Singleton<ClothingUtility>.Instance.GetSlotData(this.SlotType).Icon;
		}

		// Token: 0x04003320 RID: 13088
		public EClothingSlot SlotType;

		// Token: 0x04003321 RID: 13089
		public Image SlotTypeImage;
	}
}
