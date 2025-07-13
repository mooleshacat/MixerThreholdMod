using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000320 RID: 800
	[CreateAssetMenu(fileName = "LongFaced", menuName = "Properties/LongFaced Property")]
	public class LongFaced : Property
	{
		// Token: 0x060011BA RID: 4538 RVA: 0x0004DD1D File Offset: 0x0004BF1D
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetGiraffeActive(true, true);
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0004DD31 File Offset: 0x0004BF31
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetGiraffeActive(true, true);
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.AddOverride(15f, this.Tier, "longfaced");
			}
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0004DD6C File Offset: 0x0004BF6C
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetGiraffeActive(false, true);
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0004DD80 File Offset: 0x0004BF80
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetGiraffeActive(false, true);
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.RemoveOverride("longfaced");
			}
		}
	}
}
