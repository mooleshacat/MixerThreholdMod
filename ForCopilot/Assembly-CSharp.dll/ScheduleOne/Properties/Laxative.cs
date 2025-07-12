using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000323 RID: 803
	[CreateAssetMenu(fileName = "Laxative", menuName = "Properties/Laxative Property")]
	public class Laxative : Property
	{
		// Token: 0x060011C9 RID: 4553 RVA: 0x0004DE6B File Offset: 0x0004C06B
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.EnableLaxative(true);
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0004DE7E File Offset: 0x0004C07E
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.EnableLaxative(true);
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0004DE91 File Offset: 0x0004C091
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.DisableLaxative(true);
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0004DEA4 File Offset: 0x0004C0A4
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.DisableLaxative(true);
		}
	}
}
