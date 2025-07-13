using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000330 RID: 816
	[CreateAssetMenu(fileName = "Smelly", menuName = "Properties/Smelly Property")]
	public class Smelly : Property
	{
		// Token: 0x06001206 RID: 4614 RVA: 0x0004E6CE File Offset: 0x0004C8CE
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetStinkParticlesActive(true, true);
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x0004E6E2 File Offset: 0x0004C8E2
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetStinkParticlesActive(true, true);
			if (player.Owner.IsLocalClient)
			{
				PlayerSingleton<PlayerCamera>.Instance.Flies.Play();
			}
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x0004E712 File Offset: 0x0004C912
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetStinkParticlesActive(false, true);
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x0004E726 File Offset: 0x0004C926
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetStinkParticlesActive(false, true);
			if (player.Owner.IsLocalClient)
			{
				PlayerSingleton<PlayerCamera>.Instance.Flies.Stop();
			}
		}
	}
}
