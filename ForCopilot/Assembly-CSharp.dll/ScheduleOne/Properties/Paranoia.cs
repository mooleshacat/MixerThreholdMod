using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000325 RID: 805
	[CreateAssetMenu(fileName = "Paranoia", menuName = "Properties/Paranoia Property")]
	public class Paranoia : Property
	{
		// Token: 0x060011D3 RID: 4563 RVA: 0x0004E0A2 File Offset: 0x0004C2A2
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.EmotionManager.AddEmotionOverride("Concerned", "paranoia", 0f, 0);
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0004E0C4 File Offset: 0x0004C2C4
		public override void ApplyToPlayer(Player player)
		{
			player.Paranoid = true;
			player.Avatar.EmotionManager.AddEmotionOverride("Concerned", "paranoia", 0f, 0);
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0004E0ED File Offset: 0x0004C2ED
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.EmotionManager.RemoveEmotionOverride("paranoia");
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0004E104 File Offset: 0x0004C304
		public override void ClearFromPlayer(Player player)
		{
			player.Paranoid = false;
			player.Avatar.EmotionManager.RemoveEmotionOverride("paranoia");
		}
	}
}
