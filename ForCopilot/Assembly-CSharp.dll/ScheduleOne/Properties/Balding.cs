using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000316 RID: 790
	[CreateAssetMenu(fileName = "Balding", menuName = "Properties/Balding Property")]
	public class Balding : Property
	{
		// Token: 0x06001187 RID: 4487 RVA: 0x0004D578 File Offset: 0x0004B778
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.VanishHair(true);
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x0004D58B File Offset: 0x0004B78B
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.VanishHair(true);
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x0004D59E File Offset: 0x0004B79E
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.ReturnHair(true);
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x0004D5B1 File Offset: 0x0004B7B1
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.ReturnHair(true);
		}
	}
}
