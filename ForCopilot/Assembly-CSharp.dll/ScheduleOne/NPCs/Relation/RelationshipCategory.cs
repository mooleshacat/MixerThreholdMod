using System;
using UnityEngine;

namespace ScheduleOne.NPCs.Relation
{
	// Token: 0x020004C7 RID: 1223
	public class RelationshipCategory
	{
		// Token: 0x06001AD6 RID: 6870 RVA: 0x000748C6 File Offset: 0x00072AC6
		public static ERelationshipCategory GetCategory(float delta)
		{
			if (delta >= 4f)
			{
				return ERelationshipCategory.Loyal;
			}
			if (delta >= 3f)
			{
				return ERelationshipCategory.Friendly;
			}
			if (delta >= 2f)
			{
				return ERelationshipCategory.Neutral;
			}
			if (delta >= 1f)
			{
				return ERelationshipCategory.Unfriendly;
			}
			return ERelationshipCategory.Hostile;
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x000748F4 File Offset: 0x00072AF4
		public static Color32 GetColor(ERelationshipCategory category)
		{
			switch (category)
			{
			case ERelationshipCategory.Hostile:
				return RelationshipCategory.Hostile_Color;
			case ERelationshipCategory.Unfriendly:
				return RelationshipCategory.Unfriendly_Color;
			case ERelationshipCategory.Neutral:
				return RelationshipCategory.Neutral_Color;
			case ERelationshipCategory.Friendly:
				return RelationshipCategory.Friendly_Color;
			case ERelationshipCategory.Loyal:
				return RelationshipCategory.Loyal_Color;
			default:
				Console.LogError("Failed to find relationship category color", null);
				return Color.white;
			}
		}

		// Token: 0x040016C5 RID: 5829
		public static Color32 Hostile_Color = new Color32(173, 63, 63, byte.MaxValue);

		// Token: 0x040016C6 RID: 5830
		public static Color32 Unfriendly_Color = new Color32(227, 136, 55, byte.MaxValue);

		// Token: 0x040016C7 RID: 5831
		public static Color32 Neutral_Color = new Color32(208, 208, 208, byte.MaxValue);

		// Token: 0x040016C8 RID: 5832
		public static Color32 Friendly_Color = new Color32(61, 181, 243, byte.MaxValue);

		// Token: 0x040016C9 RID: 5833
		public static Color32 Loyal_Color = new Color32(63, 211, 63, byte.MaxValue);
	}
}
