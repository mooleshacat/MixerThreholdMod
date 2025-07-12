using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200032E RID: 814
	[CreateAssetMenu(fileName = "Shrinking", menuName = "Properties/Shrinking Property")]
	public class Shrinking : Property
	{
		// Token: 0x060011FC RID: 4604 RVA: 0x0004E638 File Offset: 0x0004C838
		public override void ApplyToNPC(NPC npc)
		{
			npc.SetScale(0.8f, 1f);
			npc.VoiceOverEmitter.SetRuntimePitchMultiplier(1.5f);
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0004E65A File Offset: 0x0004C85A
		public override void ApplyToPlayer(Player player)
		{
			player.SetScale(0.8f, 1f);
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x0004E66C File Offset: 0x0004C86C
		public override void ClearFromNPC(NPC npc)
		{
			npc.SetScale(1f, 1f);
			npc.VoiceOverEmitter.SetRuntimePitchMultiplier(1f);
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0004E68E File Offset: 0x0004C88E
		public override void ClearFromPlayer(Player player)
		{
			player.SetScale(1f, 1f);
		}

		// Token: 0x04001172 RID: 4466
		public const float Scale = 0.8f;

		// Token: 0x04001173 RID: 4467
		public const float LerpTime = 1f;
	}
}
