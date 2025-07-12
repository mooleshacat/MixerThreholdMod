using System;
using ScheduleOne.AvatarFramework;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000328 RID: 808
	[CreateAssetMenu(fileName = "Sedating", menuName = "Properties/Sedating Property")]
	public class Sedating : Property
	{
		// Token: 0x060011E2 RID: 4578 RVA: 0x0004E2F4 File Offset: 0x0004C4F4
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.18f,
				topLidOpen = 0.18f
			});
			npc.Avatar.Eyes.ForceBlink();
			npc.Movement.SpeedController.SpeedMultiplier = 0.6f;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0004E358 File Offset: 0x0004C558
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.18f,
				topLidOpen = 0.18f
			});
			player.Avatar.Eyes.ForceBlink();
			if (player.IsOwner)
			{
				Singleton<EyelidOverlay>.Instance.OpenMultiplier.AddOverride(0.7f, 6, "sedating");
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.AddOverride(-8f, this.Tier, "sedating");
				PlayerSingleton<PlayerCamera>.Instance.SmoothLookSmoother.AddOverride(0.8f, this.Tier, "sedating");
			}
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0004D482 File Offset: 0x0004B682
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Eyes.ResetEyeLids();
			npc.Avatar.Eyes.ForceBlink();
			npc.Movement.SpeedController.SpeedMultiplier = 1f;
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x0004E408 File Offset: 0x0004C608
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Eyes.ResetEyeLids();
			player.Avatar.Eyes.ForceBlink();
			if (player.IsOwner)
			{
				Singleton<EyelidOverlay>.Instance.OpenMultiplier.RemoveOverride("sedating");
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.RemoveOverride("sedating");
				PlayerSingleton<PlayerCamera>.Instance.SmoothLookSmoother.RemoveOverride("sedating");
			}
		}
	}
}
