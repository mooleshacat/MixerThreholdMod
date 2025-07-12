using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000314 RID: 788
	[CreateAssetMenu(fileName = "AntiGravity", menuName = "Properties/AntiGravity Property")]
	public class AntiGravity : Property
	{
		// Token: 0x0600117D RID: 4477 RVA: 0x0004D29D File Offset: 0x0004B49D
		public override void ApplyToNPC(NPC npc)
		{
			npc.Movement.SetGravityMultiplier(0.3f);
			npc.Avatar.Effects.SetAntiGrav(true, true);
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x0004D2C1 File Offset: 0x0004B4C1
		public override void ApplyToPlayer(Player player)
		{
			player.SetGravityMultiplier(0.3f);
			player.Avatar.Effects.SetAntiGrav(true, true);
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x0004D2E0 File Offset: 0x0004B4E0
		public override void ClearFromNPC(NPC npc)
		{
			npc.Movement.SetGravityMultiplier(1f);
			npc.Avatar.Effects.SetAntiGrav(false, true);
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x0004D304 File Offset: 0x0004B504
		public override void ClearFromPlayer(Player player)
		{
			player.SetGravityMultiplier(1f);
			player.Avatar.Effects.SetAntiGrav(false, true);
		}

		// Token: 0x0400115A RID: 4442
		public const float GravityMultiplier = 0.3f;
	}
}
