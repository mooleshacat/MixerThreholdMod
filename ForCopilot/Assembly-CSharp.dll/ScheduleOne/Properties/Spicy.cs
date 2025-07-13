using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000332 RID: 818
	[CreateAssetMenu(fileName = "Spicy", menuName = "Properties/Spicy Property")]
	public class Spicy : Property
	{
		// Token: 0x06001210 RID: 4624 RVA: 0x0004E85B File Offset: 0x0004CA5B
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetFireActive(true, true);
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x0004E870 File Offset: 0x0004CA70
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetFireActive(true, true);
			if (player.Owner.IsLocalClient)
			{
				Singleton<PostProcessingManager>.Instance.ColorFilterController.AddOverride(this.TintColor, this.Tier, base.name);
			}
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0004E8BD File Offset: 0x0004CABD
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetFireActive(false, true);
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0004E8D1 File Offset: 0x0004CAD1
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetFireActive(false, true);
			if (player.Owner.IsLocalClient)
			{
				Singleton<PostProcessingManager>.Instance.ColorFilterController.RemoveOverride(base.name);
			}
		}

		// Token: 0x04001177 RID: 4471
		[ColorUsage(true, true)]
		[SerializeField]
		public Color TintColor = Color.white;
	}
}
