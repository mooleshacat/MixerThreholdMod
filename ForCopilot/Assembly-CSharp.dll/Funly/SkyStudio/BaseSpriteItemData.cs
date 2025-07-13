using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001E5 RID: 485
	public class BaseSpriteItemData
	{
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0002F47B File Offset: 0x0002D67B
		// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x0002F483 File Offset: 0x0002D683
		public Matrix4x4 modelMatrix { get; protected set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0002F48C File Offset: 0x0002D68C
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x0002F494 File Offset: 0x0002D694
		public BaseSpriteItemData.SpriteState state { get; protected set; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x0002F49D File Offset: 0x0002D69D
		// (set) Token: 0x06000ABC RID: 2748 RVA: 0x0002F4A5 File Offset: 0x0002D6A5
		public Vector3 spritePosition { get; set; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x0002F4AE File Offset: 0x0002D6AE
		// (set) Token: 0x06000ABE RID: 2750 RVA: 0x0002F4B6 File Offset: 0x0002D6B6
		public float startTime { get; protected set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x0002F4BF File Offset: 0x0002D6BF
		// (set) Token: 0x06000AC0 RID: 2752 RVA: 0x0002F4C7 File Offset: 0x0002D6C7
		public float endTime { get; protected set; }

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0002F4D0 File Offset: 0x0002D6D0
		public BaseSpriteItemData()
		{
			this.state = BaseSpriteItemData.SpriteState.NotStarted;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0002F4DF File Offset: 0x0002D6DF
		public void SetTRSMatrix(Vector3 worldPosition, Quaternion rotation, Vector3 scale)
		{
			this.spritePosition = worldPosition;
			this.modelMatrix = Matrix4x4.TRS(worldPosition, rotation, scale);
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0002F4F8 File Offset: 0x0002D6F8
		public void Start()
		{
			this.state = BaseSpriteItemData.SpriteState.Animating;
			this.startTime = BaseSpriteItemData.CalculateStartTimeWithDelay(this.delay);
			this.endTime = BaseSpriteItemData.CalculateEndTime(this.startTime, this.spriteSheetData.frameCount, this.spriteSheetData.frameRate);
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0002F544 File Offset: 0x0002D744
		public void Continue()
		{
			if (this.state != BaseSpriteItemData.SpriteState.Animating)
			{
				return;
			}
			if (Time.time > this.endTime)
			{
				this.state = BaseSpriteItemData.SpriteState.Complete;
				return;
			}
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0002F565 File Offset: 0x0002D765
		public void Reset()
		{
			this.state = BaseSpriteItemData.SpriteState.NotStarted;
			this.startTime = -1f;
			this.endTime = -1f;
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0002F584 File Offset: 0x0002D784
		public static float CalculateStartTimeWithDelay(float delay)
		{
			return Time.time + delay;
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x0002F590 File Offset: 0x0002D790
		public static float CalculateEndTime(float startTime, int itemCount, int animationSpeed)
		{
			float num = 1f / (float)animationSpeed;
			float num2 = (float)itemCount * num;
			return startTime + num2;
		}

		// Token: 0x04000B86 RID: 2950
		public SpriteSheetData spriteSheetData;

		// Token: 0x04000B8C RID: 2956
		public float delay;

		// Token: 0x020001E6 RID: 486
		public enum SpriteState
		{
			// Token: 0x04000B8E RID: 2958
			Unknown,
			// Token: 0x04000B8F RID: 2959
			NotStarted,
			// Token: 0x04000B90 RID: 2960
			Animating,
			// Token: 0x04000B91 RID: 2961
			Complete
		}
	}
}
