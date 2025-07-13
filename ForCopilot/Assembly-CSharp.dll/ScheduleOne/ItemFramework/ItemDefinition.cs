using System;
using ScheduleOne.Equipping;
using ScheduleOne.UI.Items;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000987 RID: 2439
	[CreateAssetMenu(fileName = "ItemDefinition", menuName = "ScriptableObjects/ItemDefinition", order = 1)]
	[Serializable]
	public class ItemDefinition : ScriptableObject
	{
		// Token: 0x060041B4 RID: 16820 RVA: 0x00114F94 File Offset: 0x00113194
		public virtual ItemInstance GetDefaultInstance(int quantity = 1)
		{
			Console.LogError("This should be overridden in the definition class!", null);
			return null;
		}

		// Token: 0x04002ED2 RID: 11986
		public const int DEFAULT_STACK_LIMIT = 10;

		// Token: 0x04002ED3 RID: 11987
		public string Name;

		// Token: 0x04002ED4 RID: 11988
		[TextArea(3, 10)]
		public string Description;

		// Token: 0x04002ED5 RID: 11989
		public string ID;

		// Token: 0x04002ED6 RID: 11990
		public Sprite Icon;

		// Token: 0x04002ED7 RID: 11991
		public EItemCategory Category;

		// Token: 0x04002ED8 RID: 11992
		public string[] Keywords;

		// Token: 0x04002ED9 RID: 11993
		public bool AvailableInDemo = true;

		// Token: 0x04002EDA RID: 11994
		public bool UsableInFilters = true;

		// Token: 0x04002EDB RID: 11995
		public Color LabelDisplayColor = Color.white;

		// Token: 0x04002EDC RID: 11996
		public int StackLimit = 10;

		// Token: 0x04002EDD RID: 11997
		public Equippable Equippable;

		// Token: 0x04002EDE RID: 11998
		public ItemUI CustomItemUI;

		// Token: 0x04002EDF RID: 11999
		public ItemInfoContent CustomInfoContent;

		// Token: 0x04002EE0 RID: 12000
		[Header("Legal Status")]
		public ELegalStatus legalStatus;
	}
}
