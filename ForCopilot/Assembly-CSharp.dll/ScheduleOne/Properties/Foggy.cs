using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x0200031E RID: 798
	[CreateAssetMenu(fileName = "Foggy", menuName = "Properties/Foggy Property")]
	public class Foggy : Property
	{
		// Token: 0x060011AF RID: 4527 RVA: 0x0004DC0E File Offset: 0x0004BE0E
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.SetFoggy(true, true);
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x0004DC22 File Offset: 0x0004BE22
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.SetFoggy(true, true);
			if (player.IsLocalPlayer)
			{
				Singleton<EnvironmentFX>.Instance.FogEndDistanceController.AddOverride(0.1f, this.Tier, base.name);
			}
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0004DC5E File Offset: 0x0004BE5E
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.SetFoggy(false, true);
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x0004DC72 File Offset: 0x0004BE72
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.SetFoggy(false, true);
			if (player.IsLocalPlayer)
			{
				Singleton<EnvironmentFX>.Instance.FogEndDistanceController.RemoveOverride(base.name);
			}
		}
	}
}
