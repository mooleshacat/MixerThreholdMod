using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000339 RID: 825
	[CreateAssetMenu(fileName = "Zombifying", menuName = "Properties/Zombifying Property")]
	public class Zombifying : Property
	{
		// Token: 0x0600122D RID: 4653 RVA: 0x0004ED44 File Offset: 0x0004CF44
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetZombified(true, true);
			npc.VoiceOverEmitter.SetRuntimePitchMultiplier(0.5f);
			npc.VoiceOverEmitter.SetDatabase(this.zombieVODatabase, false);
			npc.PlayVO(EVOLineType.Grunt);
			npc.Movement.SpeedController.SpeedMultiplier = 0.4f;
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x0004EDA2 File Offset: 0x0004CFA2
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetZombified(true, true);
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0004EDB8 File Offset: 0x0004CFB8
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetZombified(false, true);
			npc.VoiceOverEmitter.SetRuntimePitchMultiplier(1f);
			npc.VoiceOverEmitter.ResetDatabase();
			npc.Movement.SpeedController.SpeedMultiplier = 1f;
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0004EE07 File Offset: 0x0004D007
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetZombified(false, true);
		}

		// Token: 0x0400118A RID: 4490
		public VODatabase zombieVODatabase;
	}
}
