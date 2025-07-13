using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001BE RID: 446
	[Serializable]
	public class TextureKeyframe : BaseKeyframe
	{
		// Token: 0x0600090A RID: 2314 RVA: 0x0002849E File Offset: 0x0002669E
		public TextureKeyframe(Texture texture, float time) : base(time)
		{
			this.texture = texture;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x000284AE File Offset: 0x000266AE
		public TextureKeyframe(TextureKeyframe keyframe) : base(keyframe.time)
		{
			this.texture = keyframe.texture;
			base.interpolationCurve = keyframe.interpolationCurve;
			base.interpolationDirection = keyframe.interpolationDirection;
		}

		// Token: 0x04000993 RID: 2451
		public Texture texture;
	}
}
