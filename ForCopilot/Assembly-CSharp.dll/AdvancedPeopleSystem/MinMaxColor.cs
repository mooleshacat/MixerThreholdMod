using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000219 RID: 537
	[Serializable]
	public class MinMaxColor
	{
		// Token: 0x06000BBF RID: 3007 RVA: 0x00036524 File Offset: 0x00034724
		public Color GetRandom()
		{
			int index = UnityEngine.Random.Range(0, this.minColors.Count);
			return Color.Lerp(this.minColors[index], this.maxColors[index], UnityEngine.Random.Range(0f, 1f));
		}

		// Token: 0x04000C89 RID: 3209
		public List<Color> minColors = new List<Color>();

		// Token: 0x04000C8A RID: 3210
		public List<Color> maxColors = new List<Color>();
	}
}
