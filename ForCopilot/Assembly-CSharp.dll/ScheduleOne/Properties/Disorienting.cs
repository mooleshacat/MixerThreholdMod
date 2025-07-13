using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200031A RID: 794
	[CreateAssetMenu(fileName = "Disorienting", menuName = "Properties/Disorienting Property")]
	public class Disorienting : Property
	{
		// Token: 0x0600119B RID: 4507 RVA: 0x0004D7D4 File Offset: 0x0004B9D4
		public override void ApplyToNPC(NPC npc)
		{
			npc.Movement.Disoriented = true;
			npc.Avatar.Eyes.leftEye.AngleOffset = new Vector2(20f, 10f);
			npc.Avatar.EmotionManager.AddEmotionOverride("Concerned", "disoriented", 0f, 0);
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0004D834 File Offset: 0x0004BA34
		public override void ApplyToPlayer(Player player)
		{
			player.Disoriented = true;
			player.Avatar.Eyes.leftEye.AngleOffset = new Vector2(20f, 10f);
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.SmoothLookSmoother.AddOverride(0.8f, this.Tier, "disoriented");
			}
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0004D894 File Offset: 0x0004BA94
		public override void ClearFromNPC(NPC npc)
		{
			npc.Movement.Disoriented = false;
			npc.Avatar.Eyes.leftEye.AngleOffset = Vector2.zero;
			npc.Avatar.Eyes.rightEye.AngleOffset = Vector2.zero;
			npc.Avatar.EmotionManager.RemoveEmotionOverride("disoriented");
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0004D8F8 File Offset: 0x0004BAF8
		public override void ClearFromPlayer(Player player)
		{
			player.Disoriented = false;
			player.Avatar.Eyes.leftEye.AngleOffset = Vector2.zero;
			player.Avatar.Eyes.rightEye.AngleOffset = Vector2.zero;
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.SmoothLookSmoother.RemoveOverride("disoriented");
			}
		}
	}
}
