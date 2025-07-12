using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x020006CA RID: 1738
	public class SensorRollerDoors : RollerDoor
	{
		// Token: 0x06002FDC RID: 12252 RVA: 0x000C9230 File Offset: 0x000C7430
		protected virtual void Update()
		{
			if (!this.CanOpen())
			{
				if (base.IsOpen)
				{
					base.Close();
				}
				return;
			}
			if (this.Detector.vehicles.Count <= 0)
			{
				base.Close();
				return;
			}
			if (!this.DetectPlayerOccupiedVehiclesOnly || this.ClipDetector.vehicles.Count > 0)
			{
				base.Open();
				return;
			}
			for (int i = 0; i < this.Detector.vehicles.Count; i++)
			{
				if (this.Detector.vehicles[i].DriverPlayer != null)
				{
					base.Open();
					return;
				}
			}
			base.Close();
		}

		// Token: 0x040021AD RID: 8621
		[Header("References")]
		public VehicleDetector Detector;

		// Token: 0x040021AE RID: 8622
		public VehicleDetector ClipDetector;

		// Token: 0x040021AF RID: 8623
		[Header("Settings")]
		public bool DetectPlayerOccupiedVehiclesOnly = true;
	}
}
