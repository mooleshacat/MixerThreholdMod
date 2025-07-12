using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000225 RID: 549
	[Serializable]
	public class CharacterAnimationPreset
	{
		// Token: 0x04000CBE RID: 3262
		public string name;

		// Token: 0x04000CBF RID: 3263
		public List<BlendshapeEmotionValue> blendshapes = new List<BlendshapeEmotionValue>();

		// Token: 0x04000CC0 RID: 3264
		public bool UseGlobalBlendCurve = true;

		// Token: 0x04000CC1 RID: 3265
		public AnimationCurve GlobalBlendAnimationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000CC2 RID: 3266
		[HideInInspector]
		public float AnimationPlayDuration = 1f;

		// Token: 0x04000CC3 RID: 3267
		[HideInInspector]
		public float weightPower = 1f;

		// Token: 0x04000CC4 RID: 3268
		[Header("May decrease performance")]
		public bool applyToAllCharacterMeshes;
	}
}
