using System;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C1A RID: 3098
	public class CauldronDisplayTub : MonoBehaviour
	{
		// Token: 0x0600544D RID: 21581 RVA: 0x00164590 File Offset: 0x00162790
		public void Configure(CauldronDisplayTub.EContents contentsType, float fillLevel)
		{
			this.CocaLeafContainer.gameObject.SetActive(false);
			Transform transform = null;
			if (contentsType == CauldronDisplayTub.EContents.CocaLeaf)
			{
				transform = this.CocaLeafContainer;
			}
			if (transform != null)
			{
				transform.transform.localPosition = Vector3.Lerp(this.Container_Min.localPosition, this.Container_Max.localPosition, fillLevel);
				transform.gameObject.SetActive(fillLevel > 0f);
			}
		}

		// Token: 0x04003EB3 RID: 16051
		public Transform CocaLeafContainer;

		// Token: 0x04003EB4 RID: 16052
		public Transform Container_Min;

		// Token: 0x04003EB5 RID: 16053
		public Transform Container_Max;

		// Token: 0x02000C1B RID: 3099
		public enum EContents
		{
			// Token: 0x04003EB7 RID: 16055
			None,
			// Token: 0x04003EB8 RID: 16056
			CocaLeaf
		}
	}
}
