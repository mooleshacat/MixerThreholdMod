using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000324 RID: 804
	[CreateAssetMenu(fileName = "Lethal", menuName = "Properties/Lethal Property")]
	public class Lethal : Property
	{
		// Token: 0x060011CE RID: 4558 RVA: 0x0004DEB8 File Offset: 0x0004C0B8
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetSicklySkinColor(true);
			npc.Avatar.EmotionManager.AddEmotionOverride("Concerned", "Sickly", 0f, this.Tier);
			npc.Avatar.Effects.TriggerSick(true);
			npc.Health.SetAfflictedWithLethalEffect(true);
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0004DF18 File Offset: 0x0004C118
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetSicklySkinColor(true);
			player.Avatar.EmotionManager.AddEmotionOverride("Concerned", "Sickly", 0f, this.Tier);
			player.Avatar.Effects.TriggerSick(true);
			player.Health.SetAfflictedWithLethalEffect(true);
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.VolumeController.AddOverride(0.7f, this.Tier, "sickly");
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.PitchController.AddOverride(1f, this.Tier, "sickly");
			}
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0004DFC8 File Offset: 0x0004C1C8
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetSicklySkinColor(false);
			npc.Avatar.EmotionManager.RemoveEmotionOverride("Sickly");
			npc.Avatar.Effects.TriggerSick(true);
			npc.Health.SetAfflictedWithLethalEffect(false);
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0004E018 File Offset: 0x0004C218
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetSicklySkinColor(false);
			player.Avatar.EmotionManager.RemoveEmotionOverride("Sickly");
			player.Avatar.Effects.TriggerSick(true);
			player.Health.SetAfflictedWithLethalEffect(false);
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.VolumeController.RemoveOverride("sickly");
				PlayerSingleton<PlayerCamera>.Instance.HeartbeatSoundController.PitchController.RemoveOverride("sickly");
			}
		}

		// Token: 0x04001165 RID: 4453
		public const float HEALTH_DRAIN_PLAYER = 15f;

		// Token: 0x04001166 RID: 4454
		public const float HEALTH_DRAIN_NPC = 15f;
	}
}
