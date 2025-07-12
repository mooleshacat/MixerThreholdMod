using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200031D RID: 797
	[CreateAssetMenu(fileName = "Explosive", menuName = "Properties/Explosive Property")]
	public class Explosive : Property
	{
		// Token: 0x060011AA RID: 4522 RVA: 0x0004DBC2 File Offset: 0x0004BDC2
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.TriggerCountdownExplosion(false);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0004DBD5 File Offset: 0x0004BDD5
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.TriggerCountdownExplosion(false);
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0004DBE8 File Offset: 0x0004BDE8
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.StopCountdownExplosion(false);
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0004DBFB File Offset: 0x0004BDFB
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.StopCountdownExplosion(false);
		}
	}
}
