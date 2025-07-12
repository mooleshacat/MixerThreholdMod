using System;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000317 RID: 791
	[CreateAssetMenu(fileName = "BrightEyed", menuName = "Properties/BrightEyed Property")]
	public class BrightEyed : Property
	{
		// Token: 0x0600118C RID: 4492 RVA: 0x0004D5C4 File Offset: 0x0004B7C4
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Effects.OverrideEyeColor(this.EyeColor, this.Emission, true);
			npc.Avatar.Effects.SetEyeLightEmission(this.LightIntensity, this.EyeColor, true);
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x0004D600 File Offset: 0x0004B800
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Effects.OverrideEyeColor(this.EyeColor, this.Emission, true);
			player.Avatar.Effects.SetEyeLightEmission(this.LightIntensity, this.EyeColor, true);
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x0004D63C File Offset: 0x0004B83C
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Effects.ResetEyeColor(true);
			npc.Avatar.Effects.SetEyeLightEmission(0f, this.EyeColor, true);
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x0004D66B File Offset: 0x0004B86B
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Effects.ResetEyeColor(true);
			player.Avatar.Effects.SetEyeLightEmission(0f, this.EyeColor, true);
		}

		// Token: 0x0400115E RID: 4446
		public Color EyeColor;

		// Token: 0x0400115F RID: 4447
		public float Emission = 0.5f;

		// Token: 0x04001160 RID: 4448
		public float LightIntensity = 1f;
	}
}
