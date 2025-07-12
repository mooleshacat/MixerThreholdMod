using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Clothing
{
	// Token: 0x0200077E RID: 1918
	[CreateAssetMenu(fileName = "ClothingDefinition", menuName = "ScriptableObjects/ClothingDefinition", order = 1)]
	[Serializable]
	public class ClothingDefinition : StorableItemDefinition
	{
		// Token: 0x060033CE RID: 13262 RVA: 0x000D7C80 File Offset: 0x000D5E80
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new ClothingInstance(this, quantity, this.DefaultColor);
		}

		// Token: 0x04002494 RID: 9364
		public EClothingSlot Slot;

		// Token: 0x04002495 RID: 9365
		public EClothingApplicationType ApplicationType;

		// Token: 0x04002496 RID: 9366
		public string ClothingAssetPath = "Path/To/Clothing/Asset";

		// Token: 0x04002497 RID: 9367
		public bool Colorable = true;

		// Token: 0x04002498 RID: 9368
		public EClothingColor DefaultColor;

		// Token: 0x04002499 RID: 9369
		public List<EClothingSlot> SlotsToBlock = new List<EClothingSlot>();
	}
}
