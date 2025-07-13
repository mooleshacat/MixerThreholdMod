using System;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;

namespace ScheduleOne.Tools
{
	// Token: 0x020008AA RID: 2218
	public class PlayerSmoothedVelocityCalculator : SmoothedVelocityCalculator
	{
		// Token: 0x06003C32 RID: 15410 RVA: 0x000FDBF4 File Offset: 0x000FBDF4
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.Player.CurrentVehicle != null)
			{
				this.Velocity = this.Player.CurrentVehicle.GetComponent<LandVehicle>().VelocityCalculator.Velocity;
			}
		}

		// Token: 0x04002AF8 RID: 11000
		public Player Player;
	}
}
