using System;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A23 RID: 2595
	public class Flipboard : MonoBehaviour
	{
		// Token: 0x060045DC RID: 17884 RVA: 0x001255D4 File Offset: 0x001237D4
		public void Update()
		{
			this.time += Time.deltaTime * this.SpeedMultiplier;
			if (this.time >= this.FlipTime)
			{
				this.time = 0f;
				this.index = (this.index + 1) % this.Sprites.Length;
				this.Image.sprite = this.Sprites[this.index];
			}
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x00125642 File Offset: 0x00123842
		public void SetIndex(int index)
		{
			this.index = index;
			this.time = 0f;
			this.Image.sprite = this.Sprites[index];
		}

		// Token: 0x0400328B RID: 12939
		public Sprite[] Sprites;

		// Token: 0x0400328C RID: 12940
		public Image Image;

		// Token: 0x0400328D RID: 12941
		public float FlipTime = 0.2f;

		// Token: 0x0400328E RID: 12942
		public float SpeedMultiplier = 1f;

		// Token: 0x0400328F RID: 12943
		private float time;

		// Token: 0x04003290 RID: 12944
		private int index;
	}
}
