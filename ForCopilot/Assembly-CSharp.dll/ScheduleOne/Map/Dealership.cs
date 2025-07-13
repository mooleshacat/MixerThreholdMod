using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C6F RID: 3183
	public class Dealership : MonoBehaviour
	{
		// Token: 0x06005992 RID: 22930 RVA: 0x0017A59C File Offset: 0x0017879C
		public void SpawnVehicle(string vehicleCode)
		{
			Transform transform = this.SpawnPoints[UnityEngine.Random.Range(0, this.SpawnPoints.Length)];
			NetworkSingleton<VehicleManager>.Instance.SpawnVehicle(vehicleCode, transform.position, transform.rotation, true);
		}

		// Token: 0x040041A8 RID: 16808
		public Transform[] SpawnPoints;
	}
}
