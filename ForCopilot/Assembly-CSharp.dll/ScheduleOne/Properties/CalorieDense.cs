using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000319 RID: 793
	[CreateAssetMenu(fileName = "CalorieDense", menuName = "Properties/CalorieDense Property")]
	public class CalorieDense : Property
	{
		// Token: 0x06001196 RID: 4502 RVA: 0x0004D767 File Offset: 0x0004B967
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.AddAdditionalWeightOverride(1f, 6, "calorie dense", true);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x0004D785 File Offset: 0x0004B985
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.AddAdditionalWeightOverride(1f, 6, "calorie dense", true);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0004D7A3 File Offset: 0x0004B9A3
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.RemoveAdditionalWeightOverride("calorie dense", true);
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x0004D7BB File Offset: 0x0004B9BB
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.RemoveAdditionalWeightOverride("calorie dense", true);
		}
	}
}
