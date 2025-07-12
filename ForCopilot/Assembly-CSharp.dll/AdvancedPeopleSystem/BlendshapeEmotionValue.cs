using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x0200020B RID: 523
	[Serializable]
	public class BlendshapeEmotionValue
	{
		// Token: 0x04000C36 RID: 3126
		public CharacterBlendShapeType BlendType;

		// Token: 0x04000C37 RID: 3127
		[Range(-100f, 100f)]
		public float BlendValue;

		// Token: 0x04000C38 RID: 3128
		public AnimationCurve BlendAnimationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 0f)
		});
	}
}
