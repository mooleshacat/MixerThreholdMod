using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Vehicles
{
	// Token: 0x02000810 RID: 2064
	public class VehicleCollisionDetector : MonoBehaviour
	{
		// Token: 0x0600382F RID: 14383 RVA: 0x000ED066 File Offset: 0x000EB266
		public void OnCollisionEnter(Collision collision)
		{
			if (this.onCollisionEnter != null)
			{
				this.onCollisionEnter.Invoke(collision);
			}
		}

		// Token: 0x04002800 RID: 10240
		public UnityEvent<Collision> onCollisionEnter;
	}
}
