using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001A8 RID: 424
	public class StarBlender : FeatureBlender
	{
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x0002748C File Offset: 0x0002568C
		protected override string featureKey
		{
			get
			{
				return "StarLayer" + this.starLayer.ToString() + "Feature";
			}
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x000274A8 File Offset: 0x000256A8
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor(this.PropertyKeyForLayer("Star1ColorKey"));
			helper.BlendNumber(this.PropertyKeyForLayer("Star1SizeKey"));
			helper.BlendNumber(this.PropertyKeyForLayer("Star1RotationSpeed"));
			helper.BlendNumber(this.PropertyKeyForLayer("Star1TwinkleAmountKey"));
			helper.BlendNumber(this.PropertyKeyForLayer("Star1TwinkleSpeedKey"));
			helper.BlendNumber(this.PropertyKeyForLayer("Star1EdgeFeathering"));
			helper.BlendNumber(this.PropertyKeyForLayer("Star1ColorIntensityKey"));
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0002752C File Offset: 0x0002572C
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn(this.PropertyKeyForLayer("Star1SizeKey"), 0f);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00027544 File Offset: 0x00025744
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut(this.PropertyKeyForLayer("Star1SizeKey"), 0f);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0002755C File Offset: 0x0002575C
		private string PropertyKeyForLayer(string key)
		{
			return key.Replace("Star1", "Star" + this.starLayer.ToString());
		}

		// Token: 0x04000958 RID: 2392
		[Range(1f, 3f)]
		public int starLayer;
	}
}
