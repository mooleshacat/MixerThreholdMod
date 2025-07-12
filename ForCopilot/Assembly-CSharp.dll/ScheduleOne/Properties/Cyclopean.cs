using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000310 RID: 784
	[CreateAssetMenu(fileName = "Cyclopean", menuName = "Properties/Cyclopean Property")]
	public class Cyclopean : Property
	{
		// Token: 0x06001169 RID: 4457 RVA: 0x0004D245 File Offset: 0x0004B445
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetCyclopean(true, true);
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x0004D259 File Offset: 0x0004B459
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetCyclopean(true, true);
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x0004D26D File Offset: 0x0004B46D
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetCyclopean(false, true);
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0004D281 File Offset: 0x0004B481
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetCyclopean(false, true);
		}
	}
}
