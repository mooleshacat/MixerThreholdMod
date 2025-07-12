using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001AB RID: 427
	public abstract class FeatureBlender : MonoBehaviour, IFeatureBlender
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060008B6 RID: 2230
		protected abstract string featureKey { get; }

		// Token: 0x060008B7 RID: 2231
		protected abstract void BlendBoth(ProfileBlendingState state, BlendingHelper helper);

		// Token: 0x060008B8 RID: 2232
		protected abstract void BlendIn(ProfileBlendingState state, BlendingHelper helper);

		// Token: 0x060008B9 RID: 2233
		protected abstract void BlendOut(ProfileBlendingState state, BlendingHelper helper);

		// Token: 0x060008BA RID: 2234 RVA: 0x000279E7 File Offset: 0x00025BE7
		protected virtual ProfileFeatureBlendingMode BlendingMode(ProfileBlendingState state, BlendingHelper helper)
		{
			return helper.GetFeatureAnimationMode(this.featureKey);
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x000279F8 File Offset: 0x00025BF8
		public virtual void Blend(ProfileBlendingState state, BlendingHelper helper)
		{
			switch (this.BlendingMode(state, helper))
			{
			case ProfileFeatureBlendingMode.Normal:
				this.BlendBoth(state, helper);
				return;
			case ProfileFeatureBlendingMode.FadeFeatureOut:
				this.BlendOut(state, helper);
				return;
			case ProfileFeatureBlendingMode.FadeFeatureIn:
				this.BlendIn(state, helper);
				return;
			default:
				return;
			}
		}
	}
}
