using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000722 RID: 1826
	public class LookAt : MonoBehaviour
	{
		// Token: 0x06003167 RID: 12647 RVA: 0x000CE48F File Offset: 0x000CC68F
		private void LateUpdate()
		{
			if (this.Target != null)
			{
				base.transform.LookAt(this.Target);
			}
		}

		// Token: 0x040022CA RID: 8906
		public Transform Target;
	}
}
