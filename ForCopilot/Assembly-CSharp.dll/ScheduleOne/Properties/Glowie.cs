using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000321 RID: 801
	[CreateAssetMenu(fileName = "Glowie", menuName = "Properties/Glowie Property")]
	public class Glowie : Property
	{
		// Token: 0x060011BF RID: 4543 RVA: 0x0004DDB0 File Offset: 0x0004BFB0
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetGlowingOn(this.GlowColor, true);
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0004DDC9 File Offset: 0x0004BFC9
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetGlowingOn(this.GlowColor, true);
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0004DDE2 File Offset: 0x0004BFE2
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetGlowingOff(true);
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0004DDF5 File Offset: 0x0004BFF5
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetGlowingOff(true);
		}

		// Token: 0x04001164 RID: 4452
		[ColorUsage(true, true)]
		[SerializeField]
		public Color GlowColor = Color.white;
	}
}
