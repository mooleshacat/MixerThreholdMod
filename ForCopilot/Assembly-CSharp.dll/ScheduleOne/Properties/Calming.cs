using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000318 RID: 792
	[CreateAssetMenu(fileName = "CalmingProperty", menuName = "Properties/Calming Property")]
	public class Calming : Property
	{
		// Token: 0x06001191 RID: 4497 RVA: 0x0004D6B8 File Offset: 0x0004B8B8
		public override void ApplyToNPC(NPC npc)
		{
			npc.Movement.SpeedController.SpeedMultiplier = 0.8f;
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0004D6D0 File Offset: 0x0004B8D0
		public override void ApplyToPlayer(Player player)
		{
			if (player.IsOwner)
			{
				Singleton<EyelidOverlay>.Instance.OpenMultiplier.AddOverride(0.9f, 6, "calming");
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.AddOverride(-4f, this.Tier, "calming");
			}
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0004D71E File Offset: 0x0004B91E
		public override void ClearFromNPC(NPC npc)
		{
			npc.Movement.SpeedController.SpeedMultiplier = 1f;
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x0004D735 File Offset: 0x0004B935
		public override void ClearFromPlayer(Player player)
		{
			if (player.IsOwner)
			{
				Singleton<EyelidOverlay>.Instance.OpenMultiplier.RemoveOverride("calming");
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.RemoveOverride("calming");
			}
		}
	}
}
