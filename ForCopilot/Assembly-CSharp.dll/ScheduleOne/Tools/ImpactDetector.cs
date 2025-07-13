using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x020008A0 RID: 2208
	public class ImpactDetector : MonoBehaviour
	{
		// Token: 0x06003C03 RID: 15363 RVA: 0x000FD3AE File Offset: 0x000FB5AE
		private void OnCollisionEnter(Collision collision)
		{
			this.onImpact.Invoke();
			if (this.DestroyScriptOnImpact)
			{
				UnityEngine.Object.Destroy(this);
			}
		}

		// Token: 0x04002AD9 RID: 10969
		public bool DestroyScriptOnImpact;

		// Token: 0x04002ADA RID: 10970
		public UnityEvent onImpact = new UnityEvent();
	}
}
