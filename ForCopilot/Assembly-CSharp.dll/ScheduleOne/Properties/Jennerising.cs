using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000322 RID: 802
	[CreateAssetMenu(fileName = "Jennerising", menuName = "Properties/Jennerising Property")]
	public class Jennerising : Property
	{
		// Token: 0x060011C4 RID: 4548 RVA: 0x0004DE1B File Offset: 0x0004C01B
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetGenderInverted(true, true);
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0004DE2F File Offset: 0x0004C02F
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetGenderInverted(true, true);
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0004DE43 File Offset: 0x0004C043
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetGenderInverted(false, true);
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0004DE57 File Offset: 0x0004C057
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetGenderInverted(false, true);
		}
	}
}
