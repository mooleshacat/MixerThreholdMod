using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x02000819 RID: 2073
	public class VehicleRecoveryPoint : MonoBehaviour
	{
		// Token: 0x06003870 RID: 14448 RVA: 0x000EDD99 File Offset: 0x000EBF99
		protected virtual void Awake()
		{
			VehicleRecoveryPoint.recoveryPoints.Add(this);
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x000EDDA8 File Offset: 0x000EBFA8
		public static VehicleRecoveryPoint GetClosestRecoveryPoint(Vector3 pos)
		{
			VehicleRecoveryPoint vehicleRecoveryPoint = null;
			for (int i = 0; i < VehicleRecoveryPoint.recoveryPoints.Count; i++)
			{
				if (vehicleRecoveryPoint == null || Vector3.Distance(VehicleRecoveryPoint.recoveryPoints[i].transform.position, pos) < Vector3.Distance(vehicleRecoveryPoint.transform.position, pos))
				{
					vehicleRecoveryPoint = VehicleRecoveryPoint.recoveryPoints[i];
				}
			}
			return vehicleRecoveryPoint;
		}

		// Token: 0x04002834 RID: 10292
		public static List<VehicleRecoveryPoint> recoveryPoints = new List<VehicleRecoveryPoint>();
	}
}
