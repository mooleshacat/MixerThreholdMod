using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000334 RID: 820
	[CreateAssetMenu(fileName = "Toxic", menuName = "Properties/Toxic Property")]
	public class Toxic : Property
	{
		// Token: 0x0600121A RID: 4634 RVA: 0x0004E96A File Offset: 0x0004CB6A
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.TriggerSick(true);
			npc.Avatar.EmotionManager.AddEmotionOverride("Concerned", "toxic", 30f, 1);
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0004E9A0 File Offset: 0x0004CBA0
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.TriggerSick(true);
			player.Avatar.EmotionManager.AddEmotionOverride("Concerned", "toxic", 30f, 1);
			if (player.Owner.IsLocalClient)
			{
				Singleton<PostProcessingManager>.Instance.ColorFilterController.AddOverride(this.TintColor, this.Tier, "Toxic");
			}
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x000045B1 File Offset: 0x000027B1
		public override void ClearFromNPC(NPC npc)
		{
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0004EA0B File Offset: 0x0004CC0B
		public override void ClearFromPlayer(Player player)
		{
			if (player.Owner.IsLocalClient)
			{
				Singleton<PostProcessingManager>.Instance.ColorFilterController.RemoveOverride("Toxic");
			}
		}

		// Token: 0x04001178 RID: 4472
		[ColorUsage(true, true)]
		[SerializeField]
		public Color TintColor = Color.white;
	}
}
