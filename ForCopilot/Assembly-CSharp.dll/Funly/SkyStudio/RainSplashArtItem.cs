using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x0200019B RID: 411
	[CreateAssetMenu(fileName = "rainSplashArtItem.asset", menuName = "Sky Studio/Rain/Rain Splash Art Item")]
	public class RainSplashArtItem : SpriteArtItem
	{
		// Token: 0x0400094D RID: 2381
		[Range(0f, 1f)]
		public float intensityMultiplier = 1f;

		// Token: 0x0400094E RID: 2382
		[Range(0f, 1f)]
		public float scaleMultiplier = 1f;
	}
}
