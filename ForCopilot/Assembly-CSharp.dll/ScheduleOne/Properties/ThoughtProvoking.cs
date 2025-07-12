using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000333 RID: 819
	[CreateAssetMenu(fileName = "ThoughtProvoking", menuName = "Properties/ThoughtProvoking Property")]
	public class ThoughtProvoking : Property
	{
		// Token: 0x06001215 RID: 4629 RVA: 0x0004E91A File Offset: 0x0004CB1A
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetBigHeadActive(true, true);
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0004E92E File Offset: 0x0004CB2E
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetBigHeadActive(true, true);
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0004E942 File Offset: 0x0004CB42
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetBigHeadActive(false, true);
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0004E956 File Offset: 0x0004CB56
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetBigHeadActive(false, true);
		}
	}
}
