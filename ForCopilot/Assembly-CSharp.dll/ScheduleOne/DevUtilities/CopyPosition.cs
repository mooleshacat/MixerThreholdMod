using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000718 RID: 1816
	public class CopyPosition : MonoBehaviour
	{
		// Token: 0x06003130 RID: 12592 RVA: 0x000CDAB5 File Offset: 0x000CBCB5
		private void LateUpdate()
		{
			base.transform.position = this.ToCopy.position;
		}

		// Token: 0x040022A7 RID: 8871
		public Transform ToCopy;
	}
}
