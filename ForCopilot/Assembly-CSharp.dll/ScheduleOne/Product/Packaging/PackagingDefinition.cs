using System;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Product.Packaging
{
	// Token: 0x02000950 RID: 2384
	[CreateAssetMenu(fileName = "PackagingDefinition", menuName = "ScriptableObjects/Item Definitions/PackagingDefinition", order = 1)]
	[Serializable]
	public class PackagingDefinition : StorableItemDefinition
	{
		// Token: 0x04002DCB RID: 11723
		public int Quantity = 1;

		// Token: 0x04002DCC RID: 11724
		public EStealthLevel StealthLevel;

		// Token: 0x04002DCD RID: 11725
		public FunctionalPackaging FunctionalPackaging;

		// Token: 0x04002DCE RID: 11726
		public Equippable Equippable_Filled;

		// Token: 0x04002DCF RID: 11727
		public StoredItem StoredItem_Filled;
	}
}
