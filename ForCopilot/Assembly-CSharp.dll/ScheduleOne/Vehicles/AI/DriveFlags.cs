using System;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x02000827 RID: 2087
	[Serializable]
	public class DriveFlags
	{
		// Token: 0x060038AB RID: 14507 RVA: 0x000EED34 File Offset: 0x000ECF34
		public void ResetFlags()
		{
			this.OverrideSpeed = false;
			this.OverriddenSpeed = 50f;
			this.OverriddenReverseSpeed = 10f;
			this.SpeedLimitMultiplier = 1f;
			this.IgnoreTrafficLights = false;
			this.UseRoads = true;
			this.StuckDetection = true;
			this.ObstacleMode = DriveFlags.EObstacleMode.Default;
			this.AutoBrakeAtDestination = true;
			this.TurnBasedSpeedReduction = true;
		}

		// Token: 0x04002894 RID: 10388
		public bool OverrideSpeed;

		// Token: 0x04002895 RID: 10389
		public float OverriddenSpeed = 50f;

		// Token: 0x04002896 RID: 10390
		public float OverriddenReverseSpeed = 10f;

		// Token: 0x04002897 RID: 10391
		public float SpeedLimitMultiplier = 1f;

		// Token: 0x04002898 RID: 10392
		public bool IgnoreTrafficLights;

		// Token: 0x04002899 RID: 10393
		public bool UseRoads = true;

		// Token: 0x0400289A RID: 10394
		public bool StuckDetection = true;

		// Token: 0x0400289B RID: 10395
		public DriveFlags.EObstacleMode ObstacleMode;

		// Token: 0x0400289C RID: 10396
		public bool AutoBrakeAtDestination = true;

		// Token: 0x0400289D RID: 10397
		public bool TurnBasedSpeedReduction = true;

		// Token: 0x02000828 RID: 2088
		public enum EObstacleMode
		{
			// Token: 0x0400289F RID: 10399
			Default,
			// Token: 0x040028A0 RID: 10400
			IgnoreAll,
			// Token: 0x040028A1 RID: 10401
			IgnoreOnlySquishy
		}
	}
}
