using System;
using ScheduleOne.AvatarFramework;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000315 RID: 789
	[CreateAssetMenu(fileName = "Athletic", menuName = "Properties/Athletic Property")]
	public class Athletic : Property
	{
		// Token: 0x06001182 RID: 4482 RVA: 0x0004D324 File Offset: 0x0004B524
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.7f,
				topLidOpen = 0.8f
			});
			npc.Avatar.Eyes.ForceBlink();
			npc.Movement.SpeedController.SpeedMultiplier = 1.8f;
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x0004D388 File Offset: 0x0004B588
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.7f,
				topLidOpen = 0.8f
			});
			player.Avatar.Eyes.ForceBlink();
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerMovement>.Instance.MoveSpeedMultiplier = 1.3f;
				PlayerSingleton<PlayerMovement>.Instance.ForceSprint = true;
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.AddOverride(10f, this.Tier, "athletic");
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.VolumeController.AddOverride(0.5f, this.Tier, "athletic");
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.PitchController.AddOverride(1.7f, this.Tier, "athletic");
				Singleton<PostProcessingManager>.Instance.ColorFilterController.AddOverride(this.TintColor, this.Tier, "athletic");
			}
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x0004D482 File Offset: 0x0004B682
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Eyes.ResetEyeLids();
			npc.Avatar.Eyes.ForceBlink();
			npc.Movement.SpeedController.SpeedMultiplier = 1f;
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x0004D4BC File Offset: 0x0004B6BC
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Eyes.ResetEyeLids();
			player.Avatar.Eyes.ForceBlink();
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerMovement>.Instance.MoveSpeedMultiplier = 1f;
				PlayerSingleton<PlayerMovement>.Instance.ForceSprint = false;
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.RemoveOverride("athletic");
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.VolumeController.RemoveOverride("athletic");
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.PitchController.RemoveOverride("athletic");
				Singleton<PostProcessingManager>.Instance.ColorFilterController.RemoveOverride("athletic");
			}
		}

		// Token: 0x0400115B RID: 4443
		public const float SPEED_MULTIPLIER = 1.3f;

		// Token: 0x0400115C RID: 4444
		public const float NPC_SPEED_MULTIPLIER = 1.8f;

		// Token: 0x0400115D RID: 4445
		[ColorUsage(true, true)]
		[SerializeField]
		public Color TintColor = Color.white;
	}
}
