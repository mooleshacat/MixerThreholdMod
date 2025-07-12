using System;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008AE RID: 2222
	public class RoadCracksRandomizer : MonoBehaviour
	{
		// Token: 0x06003C3C RID: 15420 RVA: 0x000FDD0C File Offset: 0x000FBF0C
		[Button]
		private void Randomize()
		{
			List<Transform> list = new List<Transform>(this.Cracks);
			for (int i = 0; i < list.Count; i++)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				Transform value = list[i];
				list[i] = list[index];
				list[index] = value;
			}
			int num = UnityEngine.Random.Range(this.MinCount, this.MaxCount + 1);
			for (int j = 0; j < list.Count; j++)
			{
				list[j].gameObject.SetActive(j < num);
			}
		}

		// Token: 0x04002AFF RID: 11007
		public Transform[] Cracks;

		// Token: 0x04002B00 RID: 11008
		public int MinCount;

		// Token: 0x04002B01 RID: 11009
		public int MaxCount = 4;
	}
}
