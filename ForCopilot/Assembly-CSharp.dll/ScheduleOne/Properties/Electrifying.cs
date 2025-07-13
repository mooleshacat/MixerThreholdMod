using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200031B RID: 795
	[CreateAssetMenu(fileName = "Electrifying", menuName = "Properties/Electrifying Property")]
	public class Electrifying : Property
	{
		// Token: 0x060011A0 RID: 4512 RVA: 0x0004D95C File Offset: 0x0004BB5C
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetZapped(true, true);
			npc.Avatar.Effects.OverrideEyeColor(this.EyeColor, 0.5f, true);
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0004D98C File Offset: 0x0004BB8C
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetZapped(true, true);
			player.Avatar.Effects.OverrideEyeColor(this.EyeColor, 0.5f, true);
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0004D9BC File Offset: 0x0004BBBC
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetZapped(false, true);
			npc.Avatar.Effects.ResetEyeColor(true);
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0004D9E1 File Offset: 0x0004BBE1
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetZapped(false, true);
			player.Avatar.Effects.ResetEyeColor(true);
		}

		// Token: 0x04001161 RID: 4449
		public Color EyeColor;
	}
}
