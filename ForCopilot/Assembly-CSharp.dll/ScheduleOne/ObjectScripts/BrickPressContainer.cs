using System;
using ScheduleOne.Packaging;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C12 RID: 3090
	public class BrickPressContainer : MonoBehaviour
	{
		// Token: 0x060053A9 RID: 21417 RVA: 0x00161640 File Offset: 0x0015F840
		public void SetContents(ProductItemInstance product, float fillLevel)
		{
			fillLevel = Mathf.Clamp01(fillLevel);
			if (product == null || fillLevel == 0f)
			{
				this.ContentsContainer.gameObject.SetActive(false);
				return;
			}
			product.SetupPackagingVisuals(this.Visuals);
			this.ContentsContainer.localPosition = Vector3.Lerp(this.Contents_Min.localPosition, this.Contents_Max.localPosition, fillLevel);
			this.ContentsContainer.gameObject.SetActive(true);
		}

		// Token: 0x04003E5E RID: 15966
		public FilledPackagingVisuals Visuals;

		// Token: 0x04003E5F RID: 15967
		public Transform ContentsContainer;

		// Token: 0x04003E60 RID: 15968
		public Transform Contents_Min;

		// Token: 0x04003E61 RID: 15969
		public Transform Contents_Max;
	}
}
