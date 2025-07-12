using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000335 RID: 821
	[CreateAssetMenu(fileName = "TropicThunder", menuName = "Properties/TropicThunder Property")]
	public class TropicThunder : Property
	{
		// Token: 0x0600121F RID: 4639 RVA: 0x0004EA41 File Offset: 0x0004CC41
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetSkinColorInverted(true, true);
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0004EA55 File Offset: 0x0004CC55
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetSkinColorInverted(true, true);
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0004EA69 File Offset: 0x0004CC69
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetSkinColorInverted(false, true);
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0004EA7D File Offset: 0x0004CC7D
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetSkinColorInverted(false, true);
		}
	}
}
