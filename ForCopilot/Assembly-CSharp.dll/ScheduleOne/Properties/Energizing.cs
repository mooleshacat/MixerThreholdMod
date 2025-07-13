using System;
using ScheduleOne.AvatarFramework;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200031C RID: 796
	[CreateAssetMenu(fileName = "Energizing", menuName = "Properties/Energizing Property")]
	public class Energizing : Property
	{
		// Token: 0x060011A5 RID: 4517 RVA: 0x0004DA08 File Offset: 0x0004BC08
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.6f,
				topLidOpen = 0.7f
			});
			npc.Avatar.Eyes.ForceBlink();
			npc.Movement.SpeedController.SpeedMultiplier = 1.15f;
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0004DA6C File Offset: 0x0004BC6C
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.6f,
				topLidOpen = 0.7f
			});
			player.Avatar.Eyes.ForceBlink();
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerMovement>.Instance.MoveSpeedMultiplier = 1.15f;
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.AddOverride(5f, this.Tier, "energizing");
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.VolumeController.AddOverride(0.3f, this.Tier, "energizing");
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.PitchController.AddOverride(1.4f, this.Tier, "energizing");
			}
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0004D482 File Offset: 0x0004B682
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Eyes.ResetEyeLids();
			npc.Avatar.Eyes.ForceBlink();
			npc.Movement.SpeedController.SpeedMultiplier = 1f;
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0004DB38 File Offset: 0x0004BD38
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Eyes.ResetEyeLids();
			player.Avatar.Eyes.ForceBlink();
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerMovement>.Instance.MoveSpeedMultiplier = 1f;
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.RemoveOverride("energizing");
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.VolumeController.RemoveOverride("energizing");
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.PitchController.RemoveOverride("energizing");
			}
		}

		// Token: 0x04001162 RID: 4450
		public const float SPEED_MULTIPLIER = 1.15f;
	}
}
