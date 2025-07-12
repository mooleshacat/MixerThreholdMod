using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x020008AD RID: 2221
	public class RigidbodyEventBroadcaster : MonoBehaviour
	{
		// Token: 0x06003C3A RID: 15418 RVA: 0x000FDCF6 File Offset: 0x000FBEF6
		private void OnTriggerEnter(Collider other)
		{
			if (this.onTriggerEnter != null)
			{
				this.onTriggerEnter.Invoke(other);
			}
		}

		// Token: 0x04002AFE RID: 11006
		public UnityEvent<Collider> onTriggerEnter;
	}
}
