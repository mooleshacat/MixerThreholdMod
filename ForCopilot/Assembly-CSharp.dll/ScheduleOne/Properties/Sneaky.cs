using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.Vision;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000331 RID: 817
	[CreateAssetMenu(fileName = "Sneaky", menuName = "Properties/Sneaky Property")]
	public class Sneaky : Property
	{
		// Token: 0x0600120B RID: 4619 RVA: 0x0004E756 File Offset: 0x0004C956
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.FootstepSounds.VolumeMultiplier = 0.4f;
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0004E770 File Offset: 0x0004C970
		public override void ApplyToPlayer(Player player)
		{
			player.Sneaky = true;
			this.visibilityAttribute = new VisibilityAttribute("sneaky", 0f, 0.6f, -1);
			player.Avatar.FootstepSounds.VolumeMultiplier = 0.4f;
			if (player.IsOwner)
			{
				Singleton<EyelidOverlay>.Instance.OpenMultiplier.AddOverride(0.8f, 6, "sneaky");
				PlayerSingleton<PlayerMovement>.Instance.MoveSpeedMultiplier = 0.85f;
			}
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x0004E7E5 File Offset: 0x0004C9E5
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.FootstepSounds.VolumeMultiplier = 1f;
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0004E7FC File Offset: 0x0004C9FC
		public override void ClearFromPlayer(Player player)
		{
			player.Sneaky = true;
			this.visibilityAttribute.Delete();
			player.Avatar.FootstepSounds.VolumeMultiplier = 1f;
			if (player.IsOwner)
			{
				Singleton<EyelidOverlay>.Instance.OpenMultiplier.RemoveOverride("sneaky");
				PlayerSingleton<PlayerMovement>.Instance.MoveSpeedMultiplier = 1f;
			}
		}

		// Token: 0x04001174 RID: 4468
		public const float SPEED_MULTIPLIER = 0.85f;

		// Token: 0x04001175 RID: 4469
		public const float FOOTSTEP_VOL_MULTIPLIER = 0.4f;

		// Token: 0x04001176 RID: 4470
		private VisibilityAttribute visibilityAttribute;
	}
}
