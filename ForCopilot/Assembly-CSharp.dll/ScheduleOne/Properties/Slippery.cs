using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200032F RID: 815
	[CreateAssetMenu(fileName = "Slippery", menuName = "Properties/Slippery Property")]
	public class Slippery : Property
	{
		// Token: 0x06001201 RID: 4609 RVA: 0x0004E6A0 File Offset: 0x0004C8A0
		public override void ApplyToNPC(NPC npc)
		{
			npc.Movement.SlipperyMode = true;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x0004E6AE File Offset: 0x0004C8AE
		public override void ApplyToPlayer(Player player)
		{
			player.Slippery = true;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0004E6B7 File Offset: 0x0004C8B7
		public override void ClearFromNPC(NPC npc)
		{
			npc.Movement.SlipperyMode = false;
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x0004E6C5 File Offset: 0x0004C8C5
		public override void ClearFromPlayer(Player player)
		{
			player.Slippery = false;
		}
	}
}
