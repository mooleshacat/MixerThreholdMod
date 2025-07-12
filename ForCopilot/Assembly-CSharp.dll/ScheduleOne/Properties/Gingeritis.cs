using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200031F RID: 799
	[CreateAssetMenu(fileName = "Gingeritis", menuName = "Properties/Gingeritis Property")]
	public class Gingeritis : Property
	{
		// Token: 0x060011B4 RID: 4532 RVA: 0x0004DCA3 File Offset: 0x0004BEA3
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.OverrideHairColor(Gingeritis.Color, true);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0004DCC0 File Offset: 0x0004BEC0
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.OverrideHairColor(Gingeritis.Color, true);
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0004DCDD File Offset: 0x0004BEDD
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.ResetHairColor(true);
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0004DCF0 File Offset: 0x0004BEF0
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.ResetHairColor(true);
		}

		// Token: 0x04001163 RID: 4451
		public static Color32 Color = new Color32(198, 113, 34, byte.MaxValue);
	}
}
