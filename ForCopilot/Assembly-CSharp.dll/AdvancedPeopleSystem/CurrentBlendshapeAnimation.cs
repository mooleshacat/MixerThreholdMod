using System;
using System.Collections.Generic;

namespace AdvancedPeopleSystem
{
	// Token: 0x0200020C RID: 524
	public class CurrentBlendshapeAnimation
	{
		// Token: 0x04000C39 RID: 3129
		public CharacterAnimationPreset preset;

		// Token: 0x04000C3A RID: 3130
		public List<BlendshapeEmotionValue> blendShapesTemp = new List<BlendshapeEmotionValue>();

		// Token: 0x04000C3B RID: 3131
		public float timer;
	}
}
