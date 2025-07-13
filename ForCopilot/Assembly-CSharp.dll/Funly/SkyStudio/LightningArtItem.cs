using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x02000198 RID: 408
	[CreateAssetMenu(fileName = "lightningArtItem.asset", menuName = "Sky Studio/Lightning/Lightning Art Item")]
	public class LightningArtItem : SpriteArtItem
	{
		// Token: 0x04000944 RID: 2372
		[Tooltip("Adjust how the lightning bolt is positioned inside the spawn area container.")]
		public LightningArtItem.Alignment alignment;

		// Token: 0x04000945 RID: 2373
		[Tooltip("Thunder sound clip to play when this lighting bolt is rendered.")]
		public AudioClip thunderSound;

		// Token: 0x04000946 RID: 2374
		[Tooltip("Probability adjustment for this specific lightning bolt. This value is multiplied against the global lightning probability.")]
		[Range(0f, 1f)]
		public float strikeProbability = 1f;

		// Token: 0x04000947 RID: 2375
		[Range(0f, 60f)]
		[Tooltip("Size of the lighting bolt.")]
		public float size = 20f;

		// Token: 0x04000948 RID: 2376
		[Range(0f, 1f)]
		[Tooltip("The blending weight of the additive lighting bolt effect")]
		public float intensity = 1f;

		// Token: 0x02000199 RID: 409
		public enum Alignment
		{
			// Token: 0x0400094A RID: 2378
			ScaleToFit,
			// Token: 0x0400094B RID: 2379
			TopAlign
		}
	}
}
