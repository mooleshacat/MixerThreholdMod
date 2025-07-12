using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001B5 RID: 437
	[Serializable]
	public class TextureKeyframeGroup : KeyframeGroup<TextureKeyframe>
	{
		// Token: 0x060008EF RID: 2287 RVA: 0x00028228 File Offset: 0x00026428
		public TextureKeyframeGroup(string name, TextureKeyframe keyframe) : base(name)
		{
			base.AddKeyFrame(keyframe);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00028238 File Offset: 0x00026438
		public Texture TextureForTime(float time)
		{
			if (this.keyframes.Count == 0)
			{
				Debug.LogError("Can't return texture without any keyframes");
				return null;
			}
			if (this.keyframes.Count == 1)
			{
				return base.GetKeyframe(0).texture;
			}
			int index;
			int num;
			base.GetSurroundingKeyFrames(time, out index, out num);
			return base.GetKeyframe(index).texture;
		}
	}
}
