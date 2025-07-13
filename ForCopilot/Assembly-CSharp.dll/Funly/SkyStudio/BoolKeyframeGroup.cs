using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001AF RID: 431
	[Serializable]
	public class BoolKeyframeGroup : KeyframeGroup<BoolKeyframe>
	{
		// Token: 0x060008BF RID: 2239 RVA: 0x00027A74 File Offset: 0x00025C74
		public BoolKeyframeGroup(string name) : base(name)
		{
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00027A7D File Offset: 0x00025C7D
		public BoolKeyframeGroup(string name, BoolKeyframe keyframe) : base(name)
		{
			base.AddKeyFrame(keyframe);
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00027A90 File Offset: 0x00025C90
		public bool BoolForTime(float time)
		{
			if (this.keyframes.Count == 0)
			{
				Debug.LogError("Can't sample bool without any keyframes");
				return false;
			}
			if (this.keyframes.Count == 1)
			{
				return this.keyframes[0].value;
			}
			if (time < this.keyframes[0].time)
			{
				return this.keyframes[this.keyframes.Count - 1].value;
			}
			int index = 0;
			int num = 1;
			while (num < this.keyframes.Count && this.keyframes[num].time <= time)
			{
				index = num;
				num++;
			}
			return this.keyframes[index].value;
		}
	}
}
