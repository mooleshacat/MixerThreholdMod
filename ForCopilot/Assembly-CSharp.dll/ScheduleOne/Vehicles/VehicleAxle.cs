using System;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x0200080E RID: 2062
	public class VehicleAxle : MonoBehaviour
	{
		// Token: 0x06003821 RID: 14369 RVA: 0x000EC849 File Offset: 0x000EAA49
		protected virtual void Awake()
		{
			this.model = base.transform.Find("Model");
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x000EC864 File Offset: 0x000EAA64
		protected virtual void LateUpdate()
		{
			Vector3 position = base.transform.position;
			Vector3 position2 = this.wheel.axleConnectionPoint.position;
			this.model.transform.position = (position + position2) / 2f;
			base.transform.LookAt(position2);
			this.model.transform.localScale = new Vector3(this.model.transform.localScale.x, 0.5f * Vector3.Distance(position, position2), this.model.transform.localScale.z);
		}

		// Token: 0x040027EA RID: 10218
		[Header("References")]
		[SerializeField]
		protected Wheel wheel;

		// Token: 0x040027EB RID: 10219
		private Transform model;
	}
}
